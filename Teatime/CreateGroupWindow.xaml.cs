using System.Windows;
using System.Windows.Controls;

namespace Teatime
{
    public partial class CreateGroupWindow : Window
    {
        public CreateGroupWindow()
        {
            InitializeComponent();
        }

        private void ParticipantList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
