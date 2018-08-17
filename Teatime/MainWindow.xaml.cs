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

        private readonly TextBoxLogger textBoxLogger;

        public MainWindow()
        {
            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.textBoxLogger = new TextBoxLogger(this.LogTextBox);

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

        private void ListFoldersButton_Click(object sender, RoutedEventArgs e)
        {
            DebugEmailService.ListInboxFolders(RobertJ, this.textBoxLogger);
        }

        private void ListMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            DebugEmailService.ListInboxMessages(JasonS, this.textBoxLogger);
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            DebugEmailService.SendMessage(RobertJ, JasonS, this.textBoxLogger);
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
                ShowErrorMessageBox("No e-mail account selected.");
                return;
            }

            List<Group> groups = EmailService.LoadData(currentUser);
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
                ShowErrorMessageBox("No e-mail account selected.");
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
        }

        private void AddMessageButton_OnClick(object sender, RoutedEventArgs e)
        {
            Participant currentUser = (Participant)this.EmailAccountComboBox.SelectedItem;
            if (currentUser == null)
            {
                ShowErrorMessageBox("No e-mail account selected.");
                return;
            }

            Group g = (Group)this.GroupsList.SelectedItem;
            if (g == null)
            {
                ShowErrorMessageBox("No topic selected.");
                return;
            }

            Topic t = (Topic)this.TopicsList.SelectedItem;
            if (t == null)
            {
                ShowErrorMessageBox("No topic selected.");
                return;
            }

            string messageBody = this.MessageBodyTextBox.Text;
            if (string.IsNullOrWhiteSpace(messageBody))
            {
                ShowErrorMessageBox("No message entered.");
                return;
            }

            EmailService.SendMessage(currentUser, g.Participants.ToList(), t.Name, messageBody);

            Message message = new Message { Sender = currentUser, Body = messageBody };
            t.Messages.Add(message);

            this.MessageBodyTextBox.Clear();

            this.MessagesList.ScrollIntoView(message);
            this.MessagesList.SelectedIndex = this.MessagesList.Items.Count - 1;
        }

        private static void ShowErrorMessageBox(string messageBoxText)
        {
            const string caption = "Error";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void GenerateSampleData_Click(object sender, RoutedEventArgs e)
        {
            SampleDataGenerator.Generate();
        }
    }
}
