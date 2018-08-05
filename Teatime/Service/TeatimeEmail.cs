using System.Collections.Generic;

namespace Teatime.Service
{
    public class TeatimeEmail
    {
        public string FromEmailAddress { get; set; }

        public List<string> ToEmailAddresses { get; set; }

        public string TopicName { get; set; }

        public string MessageText { get; set; }
    }
}