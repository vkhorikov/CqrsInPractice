using System;
using System.Collections.Generic;
using System.Linq;
using Api.Dtos;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = new StudentRepository(unitOfWork);
            _courseRepository = new CourseRepository(unitOfWork);
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number)
        {
            IReadOnlyList<Student> students = _studentRepository.GetList(enrolled, number);
            List<StudentDto> dtos = students.Select(x => ConvertToDto(x)).ToList();
            return Ok(dtos);
        }

        private StudentDto ConvertToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Course1 = student.FirstEnrollment?.Course?.Name,
                Course1Grade = student.FirstEnrollment?.Grade.ToString(),
                Course1Credits = student.FirstEnrollment?.Course?.Credits,
                Course2 = student.SecondEnrollment?.Course?.Name,
                Course2Grade = student.SecondEnrollment?.Grade.ToString(),
                Course2Credits = student.SecondEnrollment?.Course?.Credits,
            };
        }

        [HttpPost]
        public IActionResult Create([FromBody] StudentDto dto)
        {
            var student = new Student(dto.Name, dto.Email);

            if (dto.Course1 != null && dto.Course1Grade != null)
            {
                Course course = _courseRepository.GetByName(dto.Course1);
                student.Enroll(course, Enum.Parse<Grade>(dto.Course1Grade));
            }

            if (dto.Course2 != null && dto.Course2Grade != null)
            {
                Course course = _courseRepository.GetByName(dto.Course2);
                student.Enroll(course, Enum.Parse<Grade>(dto.Course2Grade));
            }

            _studentRepository.Save(student);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found for Id {id}");

            _studentRepository.Delete(student);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] StudentDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found for Id {id}");

            student.Name = dto.Name;
            student.Email = dto.Email;

            Enrollment firstEnrollment = student.FirstEnrollment;
            Enrollment secondEnrollment = student.SecondEnrollment;

            if (HasEnrollmentChanged(dto.Course1, dto.Course1Grade, firstEnrollment))
            {
                if (string.IsNullOrWhiteSpace(dto.Course1)) // Student disenrolls
                {
                    if (string.IsNullOrWhiteSpace(dto.Course1DisenrollmentComment))
                        return Error("Disenrollment comment is required");

                    Enrollment enrollment = firstEnrollment;
                    student.RemoveEnrollment(enrollment);
                    student.AddDisenrollmentComment(enrollment, dto.Course1DisenrollmentComment);
                }

                if (string.IsNullOrWhiteSpace(dto.Course1Grade))
                    return Error("Grade is required");

                Course course = _courseRepository.GetByName(dto.Course1);

                if (firstEnrollment == null)
                {
                    // Student enrolls
                    student.Enroll(course, Enum.Parse<Grade>(dto.Course1Grade));
                }
                else
                {
                    // Student transfers
                    firstEnrollment.Update(course, Enum.Parse<Grade>(dto.Course1Grade));
                }
            }

            if (HasEnrollmentChanged(dto.Course2, dto.Course2Grade, secondEnrollment))
            {
                if (string.IsNullOrWhiteSpace(dto.Course2)) // Student disenrolls
                {
                    if (string.IsNullOrWhiteSpace(dto.Course2DisenrollmentComment))
                        return Error("Disenrollment comment is required");

                    Enrollment enrollment = secondEnrollment;
                    student.RemoveEnrollment(enrollment);
                    student.AddDisenrollmentComment(enrollment, dto.Course2DisenrollmentComment);
                }

                if (string.IsNullOrWhiteSpace(dto.Course2Grade))
                    return Error("Grade is required");

                Course course = _courseRepository.GetByName(dto.Course2);

                if (secondEnrollment == null)
                {
                    // Student enrolls
                    student.Enroll(course, Enum.Parse<Grade>(dto.Course2Grade));
                }
                else
                {
                    // Student transfers
                    secondEnrollment.Update(course, Enum.Parse<Grade>(dto.Course2Grade));
                }
            }

            _unitOfWork.Commit();

            return Ok();
        }

        private bool HasEnrollmentChanged(string newCourseName, string newGrade, Enrollment enrollment)
        {
            if (string.IsNullOrWhiteSpace(newCourseName) && enrollment == null)
                return false;

            if (string.IsNullOrWhiteSpace(newCourseName) || enrollment == null)
                return true;

            return newCourseName != enrollment.Course.Name || newGrade != enrollment.Grade.ToString();
        }
    }
}
