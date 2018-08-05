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
        private static readonly Participant RobertJohnson = ParticipantFactory.CreateParticipant("Robert Johnson");
        private static readonly Participant JasonSmith = ParticipantFactory.CreateParticipant("Jason Smith");
        private static readonly Participant LisaDavis = ParticipantFactory.CreateParticipant("Lisa Davis");
        private static readonly Participant AmyRobinson = ParticipantFactory.CreateParticipant("Amy Robinson");

        private readonly TextBoxLogger _textBoxLogger;

        public MainWindow()
        {
            InitializeComponent();

            _textBoxLogger = new TextBoxLogger(this.LogTextBox);

            this.GroupsList.ItemsSource = GetGroups();
            this.GroupsList.SelectedIndex = 0;
            this.GroupsList.Focus();

            List<Participant> participants = new List<Participant>();
            participants.Add(RobertJohnson);
            participants.Add(JasonSmith);
            participants.Add(LisaDavis);
            participants.Add(AmyRobinson);
            this.EmailAccountComboBox.ItemsSource = participants;
        }

        private IEnumerable GetGroups()
        {
            List<Group> groups = new List<Group>();

            {
                {
                    Group g = new Group();
                    g.Participants.Add(RobertJohnson);
                    g.Participants.Add(LisaDavis);
                    {
                        Topic t = new Topic {Name = "Vacation pictures"};
                        t.Messages.Add(new Message {Sender = RobertJohnson, Body = "Check it out!"});
                        t.Messages.Add(new Message {Sender = LisaDavis, Body = "Very nice :)"});
                        g.Topics.Add(t);
                    }
                    {
                        Topic t = new Topic {Name = "Birthday party" };
                        t.Messages.Add(new Message { Sender = RobertJohnson, Body = "The cake was a lie" });
                        t.Messages.Add(new Message { Sender = LisaDavis, Body = "No!" });
                        g.Topics.Add(t);
                    }
                    groups.Add(g);
                }

                {
                    Group g = new Group();
                    g.Participants.Add(AmyRobinson);
                    g.Participants.Add(JasonSmith);
                    {
                        Topic t = new Topic { Name = "Hotel options" };
                        t.Messages.Add(new Message { Sender = AmyRobinson, Body = "Here are three options for hotels." });
                        t.Messages.Add(new Message { Sender = JasonSmith, Body = "I like the first one." });
                        t.Messages.Add(new Message { Sender = AmyRobinson, Body = "Mee too." });
                        t.Messages.Add(new Message { Sender = JasonSmith, Body = "Let's book it." });
                        g.Topics.Add(t);
                    }
                    {
                        Topic t = new Topic {Name = "Flights" };
                        t.Messages.Add(new Message {Sender = AmyRobinson, Body = "It looks like the flights going through Iceland are the best." });
                        t.Messages.Add(new Message {Sender = JasonSmith, Body = "Sounds like fun." });
                        g.Topics.Add(t);
                    }
                    groups.Add(g);
                }
            }


            return groups;
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
            EmailService.ListInboxFolders(RobertJohnson, _textBoxLogger);
        }

        private void ListMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            EmailService.ListInboxMessages(JasonSmith, _textBoxLogger);
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            EmailService.SendMessage(RobertJohnson, JasonSmith, _textBoxLogger);
        }

        private void EmailAccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Participant currentUser = (Participant)this.EmailAccountComboBox.SelectedItem;
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

            Topic topic = (Topic)this.TopicsList.SelectedItem;
            if (topic == null)
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

            Message message = new Message { Sender = currentUser, Body = messageBody };
            topic.Messages.Add(message);

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
