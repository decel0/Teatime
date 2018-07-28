using Teatime.Model;

namespace Teatime.Utils
{
    public class ParticipantFactory
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
    }
}