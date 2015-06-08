using RecEpee.ViewModels;
using System.Windows;

namespace RecEpee.Framework
{
    class DialogService : IDialogService
    {
        public void ShowAboutDialog()
        {
            VvmBinder.GetView<AboutViewModel>().ShowDialog();
        }

        public bool ShowConfirmationDialog()
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;

            return MessageBox.Show("Are you sure?", "Confirm", button, icon) == MessageBoxResult.Yes;
        }
    }
}
