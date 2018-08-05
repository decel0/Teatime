using System;
using System.Collections;
using System.Collections.Generic;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using Newtonsoft.Json;
using Teatime.Model;
using Teatime.Utils;

namespace Teatime.Service
{
    public class EmailService
    {
        private const string Host = "hmail.local";
        private const int ImapPort = 143;
        private const int SmtpPort = 25;

        public static void ListInboxFolders(Participant inboxOwner, ILogger logger)
        {
            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Accept all certificates
                client.Connect(Host, ImapPort, useSsl: false);
                client.Authenticate(inboxOwner.EmailAddress, inboxOwner.EmailPassword);

                IMailFolder personal = client.GetFolder(client.PersonalNamespaces[0]);
                foreach (IMailFolder folder in personal.GetSubfolders(false))
                {
                    logger.Log($"[folder] {folder.Name}");
                }

                if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
                {
                    IMailFolder drafts = client.GetFolder(SpecialFolder.Drafts);
                    logger.Log($"[special folder] {drafts.Name}");
                }
                else
                {
                    logger.Log("unable to get special folders");
                }

                client.Disconnect(true);
            }
        }

        public static void ListInboxMessages(Participant inboxOwner, ILogger logger)
        {
            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Accept all certificates
                client.Connect(Host, ImapPort, useSsl: false);
                client.Authenticate(inboxOwner.EmailAddress, inboxOwner.EmailPassword);

                var inbox = client.Inbox; // Always available
                inbox.Open(FolderAccess.ReadOnly);

                logger.Log($"Total messages: {inbox.Count}");
                logger.Log($"Recent messages: {inbox.Recent}");

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    logger.Log($"Subject: {message.Subject}");
                    string body = message.TextBody;
                    TeatimeEmail tm = JsonConvert.DeserializeObject<TeatimeEmail>(body);
                    //TeatimeEmail tm = XmlConvert.DeserializeObject<TeatimeEmail>(body);
                    logger.Log($"From: {tm.FromEmailAddress}");
                    foreach (string toEmailAddress in tm.ToEmailAddresses)
                    {
                        logger.Log($"To: {toEmailAddress}");
                    }
                    logger.Log($"Topic: {tm.TopicName}");
                    logger.Log($"Message: {tm.MessageText}");
                }

                foreach (var summary in inbox.Fetch(0, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId))
                {
                    logger.Log($"[summary] {summary.Index:D2}: {summary.Envelope.Subject}");
                }

                client.Disconnect(true);
            }
        }

        public static void SendMessage(Participant sender, Participant recipient, ILogger logger)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(Host, SmtpPort);
                client.Authenticate(sender.EmailAddress, sender.EmailPassword);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(sender.Name, sender.EmailAddress));
                message.To.Add(new MailboxAddress(recipient.Name, recipient.EmailAddress));
                message.Subject = TeatimeEmail.SubjectTag + " " + DateTime.Now.ToString("o");

                TeatimeEmail m = new TeatimeEmail();
                m.FromEmailAddress = sender.EmailAddress;
                m.ToEmailAddresses = new List<string>(new [] { recipient.EmailAddress });
                m.TopicName = "Topic1";
                m.MessageText = "Message1";

                message.Body = new TextPart("plain") { Text = JsonConvert.SerializeObject(m, Formatting.Indented) };
                //message.Body = new TextPart("plain") { Text = XmlConvert.SerializeObject(m) };

                client.Send(message);
                logger.Log($"Message sent.");

                client.Disconnect(quit: true);
            }
        }
    }
}