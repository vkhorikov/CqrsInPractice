using System;
using UI.Common;

namespace UI.Api
{
    public sealed class StudentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Course1 { get; set; }
        public string Course1Grade { get; set; }
        public int? Course1Credits { get; set; }

        public string Course2 { get; set; }
        public string Course2Grade { get; set; }
        public int? Course2Credits { get; set; }

        public Command<long> EnrollCommand { get; set; }
        public Command<long> TransferCommand { get; set; }
        public Command<long> DisenrollCommand { get; set; }
    }

    public sealed class NewStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Course1 { get; set; }
        public string Course1Grade { get; set; }
        public string Course2 { get; set; }
        public string Course2Grade { get; set; }
    }

    public class Envelope<T>
    {
        public T Result { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime TimeGenerated { get; set; }
    }

    public sealed class EnrollmentDto
    {
        public long Id { get; set; }
        public string Course { get; set; }
        public string Grade { get; set; }
    }

    public sealed class TransferDto
    {
        public long Id { get; set; }
        public int EnrollmentNumber { get; set; }
        public string Course { get; set; }
        public string Grade { get; set; }
    }

    public sealed class DisenrollmentDto
    {
        public long Id { get; set; }
        public int EnrollmentNumber { get; set; }
        public string Comment { get; set; }
    }

    public sealed class PersonalInfoDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
