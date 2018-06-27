using System.Windows;

namespace UI.Common
{
    public static class CustomMessageBox
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
