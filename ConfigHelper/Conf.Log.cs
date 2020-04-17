namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static void Log(string content, LogLevel level = LogLevel.Info)
        {
            Logs.Add(new LogEntry(content + '\n', level));
        }
    }
}
