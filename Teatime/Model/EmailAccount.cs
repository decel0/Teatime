namespace Teatime.Model
{
    public class EmailAccount
    {
        public string Name { get; set; }

        public string DisplayText { get; set; }

        public string EmailAddress { get; set; }

        public string EmailPassword { get; set; }
        
        public string EmailHost { get; set; }

        public int ImapPort { get; set; }

        public int SmtpPort { get; set; }

        public Participant ToParticipant()
        {
            return new Participant
            {
                Name = this.Name,
                DisplayText = this.DisplayText,
                EmailAddress = this.EmailAddress
            };
        }
    }
}