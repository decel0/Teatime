using System.Collections.Generic;
using System.Windows;
using Teatime.Model;

namespace Teatime
{
    public partial class AddressBookWindow : Window
    {
        public AddressBookWindow(List<Participant> addressBook)
        {
            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.AddressBookDataGrid.ItemsSource = addressBook;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
