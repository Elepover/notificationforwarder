using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class LogEntry
    {
        public string LogContent;
        public DateTime Time;
        public LogLevel Level;
        public Brush Color { get
            {
                switch (Level)
                {
                    case LogLevel.Info: return new SolidColorBrush(Colors.Cyan);
                    case LogLevel.Warning: return new SolidColorBrush(Colors.Yellow);
                    case LogLevel.Error: return new SolidColorBrush(Colors.Red);
                    default: return new SolidColorBrush(Colors.Gray);
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
