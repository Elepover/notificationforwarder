namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static string GetVersion()
        {
            var packageVersion = Windows.ApplicationModel.Package.Current.Id.Version;
            return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}";
        }
    }
}
