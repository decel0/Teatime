namespace Teatime.Model
{
    public class ModelFactory
    {
        public static Participant CreateParticipant(string name)
        {
            Participant p = new Participant();
            p.Name = name;
            string[] parts = name.Split(' ');
            p.DisplayText = parts[0][0].ToString().ToUpperInvariant() + parts[1][0].ToString().ToUpperInvariant();
            p.EmailAddress = p.DisplayText.ToLowerInvariant() + "@hmail.local";
            p.EmailPassword = p.DisplayText.ToLowerInvariant();
            return p;
        }

        public static EmailAccount CreateEmailAccount(Participant p, string emailHost, int smtpPort, int imapPort)
        {
            EmailAccount ea = new EmailAccount();
            ea.Name = p.Name;
            ea.DisplayText = p.DisplayText;
            ea.EmailAddress = p.EmailAddress;
            ea.EmailPassword = p.EmailPassword;
            ea.EmailHost = emailHost;
            ea.SmtpPort = smtpPort;
            ea.ImapPort = imapPort;
            return ea;
        }
    }
}