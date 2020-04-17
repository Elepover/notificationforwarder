using System.Collections.Generic;
using Windows.UI.Notifications;

namespace Notification_Forwarder.ConfigHelper
{
    public class NotificationComparer : IEqualityComparer<UserNotification>
    {
        public bool Equals(UserNotification x, UserNotification y)
        {
            if (x.AppInfo.AppUserModelId != y.AppInfo.AppUserModelId) return false;
            if (x.Id != y.Id) return false;
            if (x.CreationTime != y.CreationTime) return false;
            return true;
        }
        public int GetHashCode(UserNotification obj)
        {
            return 0;
        }
    }
}
