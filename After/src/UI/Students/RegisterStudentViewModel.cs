using CSharpFunctionalExtensions;
using UI.Api;
using UI.Common;

namespace UI.Students
{
    public sealed class RegisterStudentViewModel : ViewModel
    {
        public static string[] Courses { get; } = { "", "Calculus", "Chemistry", "Composition", "Literature", "Trigonometry", "Microeconomics", "Macroeconomics" };
        public static string[] Grades { get; } = { "", "A", "B", "C", "D", "F" };

        public NewStudentDto Student { get; }

        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "Register Student";
        public override double Height => 450;

        public RegisterStudentViewModel()
        {
            Student = new NewStudentDto();

            OkCommand = new Command(Save);
            CancelCommand = new Command(() => DialogResult = false);
        }

        private void Save()
        {
            Result result = ApiClient.Register(Student).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
