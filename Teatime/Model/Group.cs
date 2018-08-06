using System;
using System.Collections.Generic;
using System.Linq;

namespace Teatime.Model
{
    public class Group
    {
        public SortedSet<Participant> Participants = new SortedSet<Participant>(Comparer<Participant>.Create((a, b) => string.Compare(a.EmailAddress, b.EmailAddress, StringComparison.CurrentCultureIgnoreCase)));

        public List<Topic> Topics = new List<Topic>();

        public string DisplayText => string.Join(", ", Participants.Select(p => p.DisplayText));
    }
}
