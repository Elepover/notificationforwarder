using System;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class LogEntry
    {
        public string LogContent;
        public DateTime Time;
        public LogLevel Level;
        public LogEntry(string logContent, LogLevel level = LogLevel.Info)
        {
            LogContent = logContent;
            Time = DateTime.Now;
            Level = level;
        }
    }
}
