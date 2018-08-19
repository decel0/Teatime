using System.Collections.Generic;
using System.Windows;
using Teatime.Model;

namespace Teatime
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(List<EmailAccount> emailAccounts)
        {
            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.EmailAccountDataGrid.ItemsSource = emailAccounts;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
