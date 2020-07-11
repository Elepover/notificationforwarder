using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class LogEntry
    {
        public string LogContent;
        public DateTime Time;
        public LogLevel Level;
        public Brush Color
        {
            get
            {
                switch (Level)
                {
                    case LogLevel.Info:
                        return new SolidColorBrush(Colors.Gray);
                    case LogLevel.Warning:
                        return new SolidColorBrush(Colors.Goldenrod);
                    case LogLevel.Error:
                        return new SolidColorBrush(Colors.Firebrick);
                    case LogLevel.Complete:
                        return new SolidColorBrush(Colors.LimeGreen);
                    default:
                        return new SolidColorBrush(Colors.DarkCyan);
                }
            }
        }
        public LogEntry(string logContent, LogLevel level = LogLevel.Info)
        {
            LogContent = logContent;
            Time = DateTime.Now;
            Level = level;
        }
    }
}
