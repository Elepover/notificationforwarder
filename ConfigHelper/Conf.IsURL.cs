using System;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static bool IsUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var parseTest) && (parseTest.Scheme == Uri.UriSchemeHttp || parseTest.Scheme == Uri.UriSchemeHttps);
        }
    }
}
