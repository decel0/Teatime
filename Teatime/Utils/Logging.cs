using System.Windows.Controls;

namespace Teatime.Utils
{
    public interface ILogger
    {
        void Log(string text);
    }

    public class TextBoxLogger : ILogger
    {
        private readonly TextBox logTextBox;

        public TextBoxLogger(TextBox logTextBox)
        {
            this.logTextBox = logTextBox;
        }

        public void Log(string text)
        {
            this.logTextBox.Text += text + "\r\n";
        }
    }
}