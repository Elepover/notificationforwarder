using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public bool AddApp(AppInfo app)
        {
            foreach (var cApp in AppsToForward)
            {
                if (AppInfo.Compare(app, cApp)) return false;
            }
            AppsToForward.Add(app);
            return true;
        }
    }
}
