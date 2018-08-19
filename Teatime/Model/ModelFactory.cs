namespace Teatime.Model
{
    public class ModelFactory
    {
        public static EmailAccount CreateEmailAccount(string name, string emailHost, int smtpPort, int imapPort)
        {
            EmailAccount ea = new EmailAccount();
            ea.Name = name;
            ea.DisplayText = GetInitials(name);
            ea.EmailAddress = ea.DisplayText.ToLower() + "@" + emailHost;
            ea.EmailPassword = ea.DisplayText.ToLower();
            ea.EmailHost = emailHost;
            ea.SmtpPort = smtpPort;
            ea.ImapPort = imapPort;
            return ea;
        }

        private static string GetInitials(string name)
        {
            string[] parts = name.Split(' ');
            string firstName = parts[0];
            string lastName = parts[1];
            string initials = $"{firstName[0]}{lastName[0]}";
            return initials.ToUpper();
        }
    }
}