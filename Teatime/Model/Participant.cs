namespace Teatime.Model
{
    public class Participant
    {
        public string Name { get; set; }

        public string DisplayText { get; set; }

        public string EmailAddress { get; set; }

        //public ??? AvatarThumbnail { get; set; }

        public Participant()
        {
        }

        public Participant(string emailAddress)
        {
            this.Name = emailAddress.Split('@')[0].ToUpperInvariant();
            this.DisplayText = this.Name;
            this.EmailAddress = emailAddress;
        }

        protected bool Equals(Participant other)
        {
            return string.Equals(this.EmailAddress, other.EmailAddress);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((Participant) obj);
        }

        public override int GetHashCode()
        {
            return (this.EmailAddress != null ? this.EmailAddress.GetHashCode() : 0);
        }
    }
}