using System.Net;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class HttpBasicAuthCredential
    {
        public ICredentials GetNetworkCredential() => new NetworkCredential(User, Password);
    }
}
