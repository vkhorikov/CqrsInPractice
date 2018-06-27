using System.Collections.Generic;
using UI.Api;
using UI.Common;

namespace UI.Students
{
    public sealed class StudentListViewModel : ViewModel
    {
        public Command RefreshCommand { get; }
        public Command CreateStudentCommand { get; }
        public Command<StudentDto> UpdateStudentCommand { get; }
        public Command<StudentDto> DeleteStudentCommand { get; }
        public IReadOnlyList<StudentDto> Students { get; private set; }

        public StudentListViewModel()
        {
            RefreshCommand = new Command(Refresh);
            CreateStudentCommand = new Command(CreateStudent);
            UpdateStudentCommand = new Command<StudentDto>(x => x != null, UpdateStudent);
            DeleteStudentCommand = new Command<StudentDto>(x => x != null, DeleteStudent);

            Refresh();
        }

        private void DeleteStudent(StudentDto dto)
        {
            ApiClient.Delete(dto.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            Refresh();
        }

        private void UpdateStudent(StudentDto dto)
        {
            var viewModel = new StudentViewModel(dto);
            _dialogService.ShowDialog(viewModel);

            Refresh();
        }

        private void CreateStudent()
        {
            var viewModel = new StudentViewModel();
            _dialogService.ShowDialog(viewModel);

            Refresh();
        }

        private void Refresh()
        {
            Students = ApiClient.GetAll().ConfigureAwait(false).GetAwaiter().GetResult();

            Notify(nameof(Students));
        }
    }
}
