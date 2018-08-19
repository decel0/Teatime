using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Teatime.Model;
using Teatime.Service;
using Teatime.Utils;

namespace Teatime
{
    public partial class MainWindow : Window
    {
        public static readonly Participant RobertJ = ParticipantFactory.CreateParticipant("Robert Johnson");
        public static readonly Participant JasonS = ParticipantFactory.CreateParticipant("Jason Smith");
        public static readonly Participant LisaD = ParticipantFactory.CreateParticipant("Lisa Davis");
        public static readonly Participant AmyR = ParticipantFactory.CreateParticipant("Amy Robinson");
        public static readonly List<Participant> Participants = new List<Participant>();

        private readonly TextBlockLogger logger;

        public MainWindow()
        {
            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.logger = new TextBlockLogger(this.LogTextBlock, this.LogScrollViewer);

            Participants.Add(RobertJ);
            Participants.Add(JasonS);
            Participants.Add(LisaD);
            Participants.Add(AmyR);

            this.EmailAccountComboBox.ItemsSource = Participants;
        }

        private void GroupsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group group = (Group) this.GroupsList.SelectedItem;
            if (group != null)
            {
                this.TopicsList.ItemsSource = group.Topics;
                this.TopicsList.SelectedIndex = 0;
            }
            else
            {
                this.TopicsList.ItemsSource = null;
            }
        }

        private void TopicsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Topic topic = (Topic) this.TopicsList.SelectedItem;
            if (topic != null)
            {
                this.MessagesList.ItemsSource = topic.Messages;
                if (topic.Messages.Count > 0)
                {
                    this.MessagesList.SelectedIndex = 0;
                    this.MessageBodyTextBox.Focus();
                }
            }
            else
            {
                this.MessagesList.ItemsSource = null;
            }
        }

        private void EmailAccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Reload();
        }

        private void ReloadButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Reload();
        }

        private void Reload()
        {
            Participant currentUser = (Participant) this.EmailAccountComboBox.SelectedItem;
            if (currentUser == null)
            {
                this.ShowErrorMessage("No e-mail account selected.");
                return;
            }

            List<Group> groups = EmailService.LoadData(currentUser, this.logger);
            this.GroupsList.ItemsSource = groups;
            if (groups.Count > 0)
            {
                this.GroupsList.SelectedIndex = 0;
                this.MessageBodyTextBox.Focus();
            }
        }

        private void CreateGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            Participant currentUser = (Participant)this.EmailAccountComboBox.SelectedItem;
            if (currentUser == null)
            {
                this.ShowErrorMessage("No e-mail account selected.");
                return;
            }

            List<Participant> participants = Participants.Where(p => p.EmailAddress != currentUser.EmailAddress).ToList();
            CreateGroupWindow w = new CreateGroupWindow(participants);
            w.ShowDialog();
            List<Participant> selectedParticipants = w.SelectedParticipants;

            if (selectedParticipants == null) return;

            Group newGroup = new Group();
            selectedParticipants.ForEach(p => newGroup.Participants.Add(p));

            List<Group> newGroups = new List<Group>();
            bool groupAlreadyExists = false;
            if (this.GroupsList.ItemsSource != null)
            {
                foreach (object i in this.GroupsList.ItemsSource)
                {
                    Group g = (Group) i;
                    newGroups.Add(g);
                    if (g.DisplayText.Equals(newGroup.DisplayText, StringComparison.CurrentCultureIgnoreCase)) groupAlreadyExists = true;
                }
            }

            if (!groupAlreadyExists)
            {
                newGroups.Add(newGroup);
                this.GroupsList.ItemsSource = newGroups;
                this.GroupsList.SelectedItem = newGroup;
                this.GroupsList.Focus();
            }
        }

        private void AddTopicButton_OnClick(object sender, RoutedEventArgs e)
        {
            Participant currentUser = (Participant)this.EmailAccountComboBox.SelectedItem;
            if (currentUser == null)
            {
                this.ShowErrorMessage("No e-mail account selected.");
                return;
            }

            Group currentGroup = (Group)this.GroupsList.SelectedItem;
            if (currentGroup == null)
            {
                this.ShowErrorMessage("No group selected.");
                return;
            }

            string newTopicName = this.TopicNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(newTopicName))
            {
                this.ShowErrorMessage("No topic name entered.");
                return;
            }

            Topic newTopic = new Topic { Name = newTopicName, Starter = currentUser };
            currentGroup.Topics.Add(newTopic);
            
            this.TopicNameTextBox.Clear();
            this.TopicsList.ScrollIntoView(newTopic);
            this.TopicsList.SelectedIndex = this.TopicsList.Items.Count - 1;
        }

        private void AddMessageButton_OnClick(object sender, RoutedEventArgs e)
        {
            Participant currentUser = (Participant)this.EmailAccountComboBox.SelectedItem;
            if (currentUser == null)
            {
                this.ShowErrorMessage("No e-mail account selected.");
                return;
            }

            Group g = (Group)this.GroupsList.SelectedItem;
            if (g == null)
            {
                this.ShowErrorMessage("No group selected.");
                return;
            }

            Topic t = (Topic)this.TopicsList.SelectedItem;
            if (t == null)
            {
                this.ShowErrorMessage("No topic selected.");
                return;
            }

            string messageBody = this.MessageBodyTextBox.Text;
            if (string.IsNullOrWhiteSpace(messageBody))
            {
                this.ShowErrorMessage("No message entered.");
                return;
            }

            EmailService.SendMessage(currentUser, g.Participants.ToList(), t.Name, messageBody, this.logger);

            Message newMessage = new Message { Sender = currentUser, Body = messageBody };
            t.Messages.Add(newMessage);

            this.MessageBodyTextBox.Clear();
            this.MessagesList.ScrollIntoView(newMessage);
            this.MessagesList.SelectedIndex = this.MessagesList.Items.Count - 1;
        }

        private void ShowErrorMessage(string message)
        {
//            MessageBoxButton button = MessageBoxButton.OK;
//            MessageBoxImage icon = MessageBoxImage.Error;
//            MessageBox.Show(message, "Error", button, icon);
            this.logger.LogError(message);
        }

        private void ClearLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.LogTextBlock.Inlines.Clear();
        }
    }
}
