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
