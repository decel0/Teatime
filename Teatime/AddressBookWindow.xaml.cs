using System.Collections.Generic;
using System.Windows;
using Teatime.Model;

namespace Teatime
{
    public partial class AddressBookWindow : Window
    {
        public AddressBookWindow(List<Participant> participants)
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
