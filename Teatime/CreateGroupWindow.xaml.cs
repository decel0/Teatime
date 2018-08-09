using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Teatime.Model;

namespace Teatime
{
    public partial class CreateGroupWindow : Window
    {
        public List<Participant> SelectedParticipants;

        public CreateGroupWindow(List<Participant> participants)
        {
            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.ParticipantList.ItemsSource = participants;
        }

        private void ParticipantList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.SelectedParticipants = new List<Participant>();
            foreach (object selectedItem in this.ParticipantList.SelectedItems)
            {
                this.SelectedParticipants.Add((Participant) selectedItem);
            }

            this.Close();
        }
    }
}
