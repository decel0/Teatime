using System.Collections.ObjectModel;

namespace Teatime.Model
{
    public class Topic
    {
        public string Name { get; set; }
        
        public Participant Starter { get; set; }

        public ObservableCollection<Message> Messages = new ObservableCollection<Message>();

        public string DisplayText => $"{this.Starter.DisplayText}: {this.Name}";
    }
}
