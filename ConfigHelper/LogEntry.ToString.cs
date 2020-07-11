using System.Text;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class LogEntry
    {
        public override string ToString()
        {
            var result = new StringBuilder($"[{Time:o}]");
            string level;
            switch (Level)
            {
                case LogLevel.Info:
                    level = "[INFO] "; break;
                case LogLevel.Warning:
                    level = "[WARN] "; break;
                case LogLevel.Error:
                    level = "[FAIL] "; break;
                case LogLevel.Complete:
                    level = "[DONE] "; break;
                default:
                    level = "[UKNN] "; break;
            }
            result.Append(level);
            result.Append(LogContent);
            return result.ToString();
        }
    }
}
