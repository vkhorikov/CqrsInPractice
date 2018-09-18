using System;
using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;

namespace Logic.AppServices
{
    public sealed class TransferCommand : ICommand
    {
        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Course { get; }
        public string Grade { get; }

        public TransferCommand(long id, int enrollmentNumber, string course, string grade)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Course = course;
            Grade = grade;
        }

        internal sealed class TransferCommandHandler : ICommandHandler<TransferCommand>
        {
            private readonly SessionFactory _sessionFactory;

            public TransferCommandHandler(SessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Result Handle(TransferCommand command)
            {
                var unitOfWork = new UnitOfWork(_sessionFactory);
                var courseRepository = new CourseRepository(unitOfWork);
                var studentRepository = new StudentRepository(unitOfWork);
                Student student = studentRepository.GetById(command.Id);
                if (student == null)
                    return Result.Fail($"No student found with Id '{command.Id}'");

                Course course = courseRepository.GetByName(command.Course);
                if (course == null)
                    return Result.Fail($"Course is incorrect: '{command.Course}'");

                bool success = Enum.TryParse(command.Grade, out Grade grade);
                if (!success)
                    return Result.Fail($"Grade is incorrect: '{command.Grade}'");

                Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollment == null)
                    return Result.Fail($"No enrollment found with number '{command.EnrollmentNumber}'");

                enrollment.Update(course, grade);

                unitOfWork.Commit();

                return Result.Ok();
            }
        }
    }
}