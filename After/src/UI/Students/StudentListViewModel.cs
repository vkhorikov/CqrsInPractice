using System.Collections.Generic;
using UI.Api;
using UI.Common;

namespace UI.Students
{
    public sealed class StudentListViewModel : ViewModel
    {
        public static string[] Courses { get; } = { "", "Calculus", "Chemistry", "Composition", "Literature", "Trigonometry", "Microeconomics", "Macroeconomics" };
        public static string[] NumberOfCourses { get; } = { "", "0", "1", "2" };

        public string SelectedCourse { get; set; } = "";
        public string SelectedNumberOfCourses { get; set; } = "";

        public Command SearchCommand { get; }
        public Command RegisterStudentCommand { get; }
        public Command<StudentDto> EditPersonalInfoCommand { get; }
        public Command<StudentDto> UnregisterStudentCommand { get; }
        public Command<long> EnrollCommand { get; }
        public Command<long> TransferCommand { get; }
        public Command<long> DisenrollCommand { get; }
        public IReadOnlyList<StudentDto> Students { get; private set; }

        public StudentListViewModel()
        {
            SearchCommand = new Command(Search);
            RegisterStudentCommand = new Command(RegisterStudent);
            EditPersonalInfoCommand = new Command<StudentDto>(x => x != null, EditPersonalInfo);
            UnregisterStudentCommand = new Command<StudentDto>(x => x != null, UnregisterStudent);
            EnrollCommand = new Command<long>(Enroll);
            TransferCommand = new Command<long>(Transfer);
            DisenrollCommand = new Command<long>(Disenroll);

            Search();
        }

        private void Disenroll(long studentId)
        {
            var viewModel = new DisenrollViewModel(studentId, 1);
            _dialogService.ShowDialog(viewModel);

            Search();
        }

        private void Transfer(long studentId)
        {
            var viewModel = new TransferViewModel(studentId);
            _dialogService.ShowDialog(viewModel);

            Search();
        }

        private void Enroll(long studentId)
        {
            var viewModel = new EnrollViewModel(studentId);
            _dialogService.ShowDialog(viewModel);

            Search();
        }

        private void UnregisterStudent(StudentDto dto)
        {
            ApiClient.Unregister(dto.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            Search();
        }

        private void EditPersonalInfo(StudentDto dto)
        {
            var viewModel = new EditPersonalInfoViewModel(dto.Id, dto.Name, dto.Email);
            _dialogService.ShowDialog(viewModel);

            Search();
        }

        private void RegisterStudent()
        {
            var viewModel = new RegisterStudentViewModel();
            _dialogService.ShowDialog(viewModel);

            Search();
        }

        private void Search()
        {
            Students = ApiClient.GetAll(SelectedCourse, SelectedNumberOfCourses).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (StudentDto student in Students)
            {
                student.EnrollCommand = EnrollCommand;
                student.TransferCommand = TransferCommand;
                student.DisenrollCommand = DisenrollCommand;
            }

            Notify(nameof(Students));
        }
    }
}
