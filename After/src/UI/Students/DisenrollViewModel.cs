using CSharpFunctionalExtensions;
using UI.Api;
using UI.Common;

namespace UI.Students
{
    public sealed class DisenrollViewModel : ViewModel
    {
        private readonly long _studentId;
        private readonly int _enrollmentNumber;
        public string Comment { get; set; }

        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "Disenroll Student";
        public override double Height => 180;

        public DisenrollViewModel(long studentId, int enrollmentNumber)
        {
            _studentId = studentId;
            _enrollmentNumber = enrollmentNumber;

            OkCommand = new Command(Save);
            CancelCommand = new Command(() => DialogResult = false);
        }

        private void Save()
        {
            var dto = new DisenrollmentDto
            {
                Id = _studentId,
                Comment = Comment,
                EnrollmentNumber = _enrollmentNumber
            };
            Result result = ApiClient.Disenroll(dto).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
