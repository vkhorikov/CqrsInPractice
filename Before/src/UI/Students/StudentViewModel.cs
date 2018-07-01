using CSharpFunctionalExtensions;
using UI.Api;
using UI.Common;

namespace UI.Students
{
    public sealed class StudentViewModel : ViewModel
    {
        public static string[] Courses { get; } = { "", "Calculus", "Chemistry", "Composition", "Literature", "Trigonometry", "Microeconomics", "Macroeconomics" };
        public static string[] Grades { get; } = { "", "A", "B", "C", "D", "F" };

        private readonly bool _isNew;
        public StudentDto Student { get; }

        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => (_isNew ? "Add" : "Edit") + " Student";
        public override double Height => 450;

        public StudentViewModel(StudentDto student = null)
        {
            if (student == null)
            {
                Student = new StudentDto();
                _isNew = true;
            }
            else
            {
                Student = student;
                _isNew = false;
            }

            OkCommand = new Command(Save);
            CancelCommand = new Command(() => DialogResult = false);
        }

        private void Save()
        {
            Result result;

            if (_isNew)
            {
                result = ApiClient.Create(Student).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                result = ApiClient.Update(Student).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
