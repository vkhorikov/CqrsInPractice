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
        public Command CreateStudentCommand { get; }
        public Command<StudentDto> UpdateStudentCommand { get; }
        public Command<StudentDto> DeleteStudentCommand { get; }
        public IReadOnlyList<StudentDto> Students { get; private set; }

        public StudentListViewModel()
        {
            SearchCommand = new Command(Search);
            CreateStudentCommand = new Command(CreateStudent);
            UpdateStudentCommand = new Command<StudentDto>(x => x != null, UpdateStudent);
            DeleteStudentCommand = new Command<StudentDto>(x => x != null, DeleteStudent);

            Search();
        }

        private void DeleteStudent(StudentDto dto)
        {
            ApiClient.Delete(dto.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            Search();
        }

        private void UpdateStudent(StudentDto dto)
        {
            var viewModel = new StudentViewModel(dto);
            _dialogService.ShowDialog(viewModel);

            Search();
        }

        private void CreateStudent()
        {
            var viewModel = new StudentViewModel();
            _dialogService.ShowDialog(viewModel);

            Search();
        }

        private void Search()
        {
            Students = ApiClient.GetAll(SelectedCourse, SelectedNumberOfCourses).ConfigureAwait(false).GetAwaiter().GetResult();

            Notify(nameof(Students));
        }
    }
}
