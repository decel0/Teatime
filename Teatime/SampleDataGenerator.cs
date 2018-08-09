using System.Collections;
using System.Collections.Generic;
using Teatime.Model;

namespace Teatime
{
    internal static class SampleDataGenerator
    {
        public static IEnumerable GetGroups()
        {
            List<Group> groups = new List<Group>();

            {
                {
                    Group g = new Group();
                    g.Participants.Add(MainWindow.RobertJohnson);
                    g.Participants.Add(MainWindow.LisaDavis);
                    {
                        Topic t = new Topic {Name = "Vacation pictures"};
                        t.Messages.Add(new Message {Sender = MainWindow.RobertJohnson, Body = "Check it out!"});
                        t.Messages.Add(new Message {Sender = MainWindow.LisaDavis, Body = "Very nice :)"});
                        g.Topics.Add(t);
                    }
                    {
                        Topic t = new Topic {Name = "Birthday party" };
                        t.Messages.Add(new Message { Sender = MainWindow.RobertJohnson, Body = "The cake was a lie" });
                        t.Messages.Add(new Message { Sender = MainWindow.LisaDavis, Body = "No!" });
                        g.Topics.Add(t);
                    }
                    groups.Add(g);
                }

                {
                    Group g = new Group();
                    g.Participants.Add(MainWindow.AmyRobinson);
                    g.Participants.Add(MainWindow.JasonSmith);
                    {
                        Topic t = new Topic { Name = "Hotel options" };
                        t.Messages.Add(new Message { Sender = MainWindow.AmyRobinson, Body = "Here are three options for hotels." });
                        t.Messages.Add(new Message { Sender = MainWindow.JasonSmith, Body = "I like the first one." });
                        t.Messages.Add(new Message { Sender = MainWindow.AmyRobinson, Body = "Mee too." });
                        t.Messages.Add(new Message { Sender = MainWindow.JasonSmith, Body = "Let's book it." });
                        g.Topics.Add(t);
                    }
                    {
                        Topic t = new Topic {Name = "Flights" };
                        t.Messages.Add(new Message {Sender = MainWindow.AmyRobinson, Body = "It looks like the flights going through Iceland are the best." });
                        t.Messages.Add(new Message {Sender = MainWindow.JasonSmith, Body = "Sounds like fun." });
                        g.Topics.Add(t);
                    }
                    groups.Add(g);
                }
            }

            return groups;
        }
    }
}