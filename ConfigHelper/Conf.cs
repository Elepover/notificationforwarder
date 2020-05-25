using System;
using System.Collections.Generic;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static Conf CurrentConf;
        public static MainPage MainPageInstance;
        public static List<LogEntry> Logs = new List<LogEntry>();

        public bool EnableForwarding;
        public bool DisplayPackageName;
        public bool MuteNewApps;
        public int NotificationsReceived;
        public int NotificationsForwarded;
        public DateTime LastSuccessfulForward;
        public List<AppInfo> AppsToForward;
        public List<string> APIEndPoints;
        public List<ApiEndPoint> ApiEndPoints2;

        public Conf() 
        {
            EnableForwarding = false;
            DisplayPackageName = true;
            MuteNewApps = true;
            NotificationsForwarded = 0;
            NotificationsReceived = 0;
            LastSuccessfulForward = DateTime.MinValue;
            AppsToForward = new List<AppInfo>();
            APIEndPoints = new List<string>();
            ApiEndPoints2 = new List<ApiEndPoint>();
        }
    }
}
