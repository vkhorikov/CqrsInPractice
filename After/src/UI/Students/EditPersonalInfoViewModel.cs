using CSharpFunctionalExtensions;
using UI.Api;
using UI.Common;

namespace UI.Students
{
    public sealed class EditPersonalInfoViewModel : ViewModel
    {
        private readonly long _studentId;
        public string Name { get; set; }
        public string Email { get; set; }

        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "Edit Personal Info";
        public override double Height => 240;

        public EditPersonalInfoViewModel(long studentId, string name, string email)
        {
            _studentId = studentId;
            Name = name;
            Email = email;

            OkCommand = new Command(Save);
            CancelCommand = new Command(() => DialogResult = false);
        }

        private void Save()
        {
            var dto = new PersonalInfoDto
            {
                Id = _studentId,
                Email = Email,
                Name = Name
            };

            Result result = ApiClient.EditPersonalInfo(dto).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
