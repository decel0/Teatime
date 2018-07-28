using System.Collections.Generic;

namespace Teatime.Model
{
    public class Message
    {
        public Participant Sender { get; set; }

        public string Body { get; set; }

        public string DisplayText => $"{Sender.DisplayText}: {this.Body}";

        //public List<object> Images { get; }
    }
}
