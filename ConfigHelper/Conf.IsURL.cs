using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static bool IsUrl(string url)
        {
            Uri parseTest;
            return Uri.TryCreate(url, UriKind.Absolute, out parseTest) && (parseTest.Scheme == Uri.UriSchemeHttp || parseTest.Scheme == Uri.UriSchemeHttps);
        }
    }
}
