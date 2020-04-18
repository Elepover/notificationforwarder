using Notification_Forwarder.ConfigHelper;
using System.Collections.Generic;
using System.Linq;

namespace Notification_Forwarder.Protocol
{
    public partial class ClientData
    {
        private const int _maxRetries = 5;
        public string ClientVersion;
        public List<Notification> Notifications;
        public ClientData()
        {
            ClientVersion = Conf.GetVersion();
            Notifications = new List<Notification>();
        }
        public ClientData(Notification[] notifications)
        {
            ClientVersion = Conf.GetVersion();
            Notifications = notifications.ToList();
        }
    }
}
