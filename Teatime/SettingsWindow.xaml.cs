using System.Windows;

namespace Teatime
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

//            this.ParticipantList.ItemsSource = participants;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
