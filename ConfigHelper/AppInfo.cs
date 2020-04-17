namespace Notification_Forwarder.ConfigHelper
{
    public partial class AppInfo
    {
        public string AppUserModelId;
        public string Id;
        public string DisplayName;
        public bool ForwardingEnabled;
        public AppInfo() { }
        public AppInfo(string appUserModelId, string id, string displayName, bool forwardingEnabled)
        {
            AppUserModelId = appUserModelId;
            Id = id;
            DisplayName = displayName;
            ForwardingEnabled = forwardingEnabled;
        }
        public AppInfo(Windows.ApplicationModel.AppInfo app, bool forwardingEnabled = false)
        {
            AppUserModelId = app.AppUserModelId;
            Id = app.Id;
            DisplayName = app.DisplayInfo.DisplayName;
            ForwardingEnabled = forwardingEnabled;
        }
    }
}
