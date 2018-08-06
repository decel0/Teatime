using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using Newtonsoft.Json;
using Teatime.Model;

namespace Teatime.Service
{
    public class EmailService
    {
        private const string Host = "hmail.local";
        private const int ImapPort = 143;
        private const int SmtpPort = 25;
        private const bool SendToSender = true;

        public static void SendMessage(Participant sender, List<Participant> recipients, string topicName, string messageText)
        {
            List<Participant> actualRecipients = GetActualRecipients(sender, recipients);

            using (var client = new SmtpClient())
            {
                client.Connect(Host, SmtpPort);
                client.Authenticate(sender.EmailAddress, sender.EmailPassword);

                MimeMessage mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(sender.Name, sender.EmailAddress));
                actualRecipients.ForEach(r => mimeMessage.To.Add(new MailboxAddress(r.Name, r.EmailAddress)));
                mimeMessage.Subject = TeatimeEmail.SubjectTag + " " + DateTime.Now.ToString("o");

                TeatimeEmail te = new TeatimeEmail();
                te.FromEmailAddress = sender.EmailAddress;
                te.ToEmailAddresses = actualRecipients.Select(r => r.EmailAddress).ToList();
                te.TopicName = topicName;
                te.MessageText = messageText;

                mimeMessage.Body = new TextPart("plain") { Text = JsonConvert.SerializeObject(te, Formatting.Indented) };

                client.Send(mimeMessage);

                client.Disconnect(quit: true);
            }
        }

        [SuppressMessage("ReSharper", "UnreachableCode")]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        private static List<Participant> GetActualRecipients(Participant sender, List<Participant> recipients)
        {
#pragma warning disable 162
            return SendToSender ? recipients : recipients.Where(r => !r.Equals(sender)).ToList();
#pragma warning restore 162
        }

        public static List<Group> LoadData(Participant inboxOwner)
        {
            Dictionary<string, Group> groups = new Dictionary<string, Group>();

            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Accept all certificates
                client.Connect(Host, ImapPort, useSsl: false);
                client.Authenticate(inboxOwner.EmailAddress, inboxOwner.EmailPassword);

                var inbox = client.Inbox; // Always available
                inbox.Open(FolderAccess.ReadOnly);

                SearchQuery query = SearchQuery
                    .DeliveredAfter(DateTime.Parse("2018-07-28"))
                    .And(SearchQuery.SubjectContains(TeatimeEmail.SubjectTag));

                foreach (UniqueId messageId in inbox.Search(query))
                {
                    MimeMessage mimeMessage = inbox.GetMessage(messageId);
                    TeatimeEmail te = JsonConvert.DeserializeObject<TeatimeEmail>(mimeMessage.TextBody);
                    Participant sender = new Participant(te.FromEmailAddress);
                    Group g = AddOrGetGroup(groups, sender, te.ToEmailAddresses);
                    Topic t = AddOrGetTopic(g, te.TopicName);
                    AddMessage(t, sender, te.MessageText);
                }

                client.Disconnect(true);
            }

            return groups.Values.ToList();
        }

        private static Group AddOrGetGroup(Dictionary<string, Group> groups, Participant sender,
            List<string> toEmailAddresses)
        {
            Group g = new Group();
            AddParticipantsToGroup(g, sender, toEmailAddresses);

            if (groups.ContainsKey(g.DisplayText))
            {
                g = groups[g.DisplayText];
            }
            else
            {
                groups.Add(g.DisplayText, g);
            }

            return g;
        }

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        private static void AddParticipantsToGroup(Group g, Participant sender, List<string> toEmailAddresses)
        {
            if (SendToSender) g.Participants.Add(sender);
            foreach (string toEmailAddress in toEmailAddresses)
            {
                g.Participants.Add(new Participant(toEmailAddress));
            }
        }

        private static Topic AddOrGetTopic(Group g, string topicName)
        {
            Topic t = g.Topics.Find(i => i.Name.Equals(topicName));

            if (t == null)
            {
                t = new Topic();
                t.Name = topicName;
                g.Topics.Add(t);

            }

            return t;
        }

        private static void AddMessage(Topic t, Participant sender, string messageText)
        {
            Message m = new Message();
            m.Sender = sender;
            m.Body = messageText;
            t.Messages.Add(m);
        }
    }
}