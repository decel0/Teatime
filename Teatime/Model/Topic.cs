using System.Collections.Generic;

namespace Teatime.Model
{
    public class Topic
    {
        public string Name { get; set; }

        public List<Message> Messages = new List<Message>();

        public string DisplayText => $"{Messages[0].Sender.DisplayText}: {this.Name}";
    }
}
