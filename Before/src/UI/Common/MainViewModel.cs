using UI.Students;

namespace UI.Common
{
    public sealed class MainViewModel
    {
        public ViewModel ViewModel { get; }

        public MainViewModel()
        {
            ViewModel = new StudentListViewModel();
        }
    }
}
