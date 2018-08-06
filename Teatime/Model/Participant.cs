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

        protected bool Equals(Participant other)
        {
            return string.Equals(EmailAddress, other.EmailAddress);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Participant) obj);
        }

        public override int GetHashCode()
        {
            return (EmailAddress != null ? EmailAddress.GetHashCode() : 0);
        }
    }
}