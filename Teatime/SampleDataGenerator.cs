using System.Collections.Generic;
using Teatime.Model;
using Teatime.Service;

namespace Teatime
{
    internal static class SampleDataGenerator
    {
        public static void Generate()
        {
            EmailService.SendMessage(MainWindow.RobertJ, new List<Participant> { MainWindow.LisaD } , "Vacation pictures", "Check it out!");
            EmailService.SendMessage(MainWindow.RobertJ, new List<Participant> { MainWindow.LisaD }, "Birthday party", "The cake is a lie");
        }
    }
}