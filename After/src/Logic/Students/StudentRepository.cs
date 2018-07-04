using System.Collections.Generic;
using System.Linq;
using Logic.Utils;

namespace Logic.Students
{
    public sealed class StudentRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public StudentRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Student GetById(long id)
        {
            return _unitOfWork.Get<Student>(id);
        }

        public IReadOnlyList<Student> GetList(string enrolledIn, int? numberOfCourses)
        {
            IQueryable<Student> query = _unitOfWork.Query<Student>();

            if (!string.IsNullOrWhiteSpace(enrolledIn))
            {
                query = query.Where(x => x.Enrollments.Any(e => e.Course.Name == enrolledIn));
            }

            List<Student> result = query.ToList();

            if (numberOfCourses != null)
            {
                result = result.Where(x => x.Enrollments.Count == numberOfCourses).ToList();
            }

            return result;
        }

        public void Save(Student student)
        {
            _unitOfWork.SaveOrUpdate(student);
        }

        public void Delete(Student student)
        {
            _unitOfWork.Delete(student);
        }
    }


    public sealed class CourseRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public CourseRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Course GetByName(string name)
        {
            return _unitOfWork.Query<Course>()
                .SingleOrDefault(x => x.Name == name);
        }
    }
}
