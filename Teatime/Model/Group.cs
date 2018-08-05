using System.Collections.Generic;
using System.Linq;

namespace Teatime.Model
{
    public class Group
    {
        public List<Participant> Participants = new List<Participant>();

        public List<Topic> Topics = new List<Topic>();

        public string DisplayText => string.Join(", ", Participants.Select(p => p.DisplayText));
    }
}
