using System.Collections.ObjectModel;

namespace Teatime.Model
{
    public class Topic
    {
        public string Name { get; set; }

        public ObservableCollection<Message> Messages = new ObservableCollection<Message>();

        public string DisplayText => $"{Messages[0].Sender.DisplayText}: {this.Name}";
    }
}
