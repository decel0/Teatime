namespace Teatime.Model
{
    public class Participant
    {
        public string Name { get; set; }

        public string DisplayText { get; set; }

        public string EmailAddress { get; set; }

        public string EmailPassword { get; set; }

        //public ??? AvatarThumbnail { get; set; }

        public Participant()
        {
        }

        public Participant(string emailAddress)
        {
            Name = emailAddress.Split('@')[0].ToUpperInvariant();
            DisplayText = Name;
            EmailAddress = emailAddress;
        }
    }
}