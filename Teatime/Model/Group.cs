using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Teatime.Model
{
    public class Group
    {
        public SortedSet<Participant> Participants = new SortedSet<Participant>(Comparer<Participant>.Create((a, b) => string.Compare(a.EmailAddress, b.EmailAddress, StringComparison.CurrentCultureIgnoreCase)));

        public ObservableCollection<Topic> Topics = new ObservableCollection<Topic>();

        public string DisplayText => string.Join(", ", this.Participants.Select(p => p.DisplayText));
    }
}
