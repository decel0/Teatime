using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Teatime.Utils
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
    }

    public class TextBlockLogger : ILogger
    {
        private readonly TextBlock logTextBlock;
        private readonly ScrollViewer logScrollViewer;

        public TextBlockLogger(TextBlock logTextBlock, ScrollViewer logScrollViewer)
        {
            this.logTextBlock = logTextBlock;
            this.logScrollViewer = logScrollViewer;
        }

        public void LogInfo(string message)
        {
            this.Log("Info", Brushes.Black, message);
        }

        public void LogError(string message)
        {
            this.Log("Error", Brushes.Red, message);
        }

        private void Log(string caption, SolidColorBrush textColor, string message)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH\\:mm\\:ss");
            string output = $"[{timestamp}] {caption}: {message}{Environment.NewLine}";

            Run run = new Run(output);
            run.Foreground = textColor;
            this.logTextBlock.Inlines.Add(run);
            this.logScrollViewer.ScrollToBottom();
        }
    }
}