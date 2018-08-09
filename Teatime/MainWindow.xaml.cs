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
        public static readonly Participant RobertJohnson = ParticipantFactory.CreateParticipant("Robert Johnson");
        public static readonly Participant JasonSmith = ParticipantFactory.CreateParticipant("Jason Smith");
        public static readonly Participant LisaDavis = ParticipantFactory.CreateParticipant("Lisa Davis");
        public static readonly Participant AmyRobinson = ParticipantFactory.CreateParticipant("Amy Robinson");

        private readonly TextBoxLogger _textBoxLogger;

        public MainWindow()
        {
            InitializeComponent();

            _textBoxLogger = new TextBoxLogger(this.LogTextBox);

//            ShowSampleData();

            List<Participant> participants = new List<Participant>();
            participants.Add(RobertJohnson);
            participants.Add(JasonSmith);
            participants.Add(LisaDavis);
            participants.Add(AmyRobinson);
            this.EmailAccountComboBox.ItemsSource = participants;
        }

        private void ShowSampleData()
        {
            this.GroupsList.ItemsSource = SampleDataGenerator.GetGroups();
            this.GroupsList.SelectedIndex = 0;
            this.GroupsList.Focus();
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
            DebugEmailService.ListInboxFolders(RobertJohnson, _textBoxLogger);
        }

        private void ListMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            DebugEmailService.ListInboxMessages(JasonSmith, _textBoxLogger);
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            DebugEmailService.SendMessage(RobertJohnson, JasonSmith, _textBoxLogger);
        }

        private void EmailAccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reload();
        }

        private void ReloadButton_OnClick(object sender, RoutedEventArgs e)
        {
            Reload();
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
            string caption = "Error";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
}
