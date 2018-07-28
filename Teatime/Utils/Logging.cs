using System.Windows.Controls;

namespace Teatime.Utils
{
    public interface ILogger
    {
        void Log(string text);
    }

    public class TextBoxLogger : ILogger
    {
        private readonly TextBox _logTextBox;

        public TextBoxLogger(TextBox logTextBox)
        {
            _logTextBox = logTextBox;
        }

        public void Log(string text)
        {
            _logTextBox.Text += text + "\r\n";
        }
    }
}