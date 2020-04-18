namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static void Log(string content, LogLevel level = LogLevel.Info, bool ignoreNewLine = true)
        {
            Logs.Add(new LogEntry(ignoreNewLine ? content.Replace("\r", "").Replace("\n", " ") : content, level));
        }
    }
}
