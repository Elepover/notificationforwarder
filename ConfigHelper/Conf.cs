using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static Conf CurrentConf;
        public static MainPage MainPageInstance;
        public bool EnableForwarding;
        public bool DisplayPackageName;
        public bool MuteNewApps;
        public int NotificationsReceived;
        public int NotificationsForwarded;
        public DateTime LastSuccessfulForward;
        public List<AppInfo> AppsToForward;
        public List<string> APIEndPoints;
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
        }
    }
}
