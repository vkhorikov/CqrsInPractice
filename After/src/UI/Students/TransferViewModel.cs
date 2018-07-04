using CSharpFunctionalExtensions;
using UI.Api;
using UI.Common;

namespace UI.Students
{
    public sealed class TransferViewModel : ViewModel
    {
        public static string[] Courses { get; } = { "", "Calculus", "Chemistry", "Composition", "Literature", "Trigonometry", "Microeconomics", "Macroeconomics" };
        public static string[] Grades { get; } = { "", "A", "B", "C", "D", "F" };

        private readonly long _studentId;
        public string Course { get; set; }
        public string Grade { get; set; }

        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "Transfer Student";
        public override double Height => 230;

        public TransferViewModel(long studentId)
        {
            _studentId = studentId;

            OkCommand = new Command(Save);
            CancelCommand = new Command(() => DialogResult = false);
        }

        private void Save()
        {
            var dto = new TransferDto
            {
                Id = _studentId,
                Course = Course,
                Grade = Grade,
                EnrollmentNumber = 1
            };
            Result result = ApiClient.Transfer(dto).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
