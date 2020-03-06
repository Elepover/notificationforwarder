using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public int GetActiveAppsCount()
        {
            int total = 0;
            foreach (var item in AppsToForward)
            {
                if (item.ForwardingEnabled) total++;
            }
            return total;
        }
    }
}
