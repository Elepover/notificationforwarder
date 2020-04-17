namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public int FindAppIndex(AppInfo app)
        {
            for (int i = 0; i < AppsToForward.Count; i++)
            {
                var cApp = AppsToForward[i];
                if ((cApp.AppUserModelId == app.AppUserModelId) && (cApp.DisplayName == app.DisplayName) && (cApp.Id == app.Id)) return i;
            }
            return -1;
        }
    }
}
