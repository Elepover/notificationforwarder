using MihaZupan;
using System.Net;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class ProxyInfo
    {
        public IWebProxy ToIWebProxy()
        {
            switch (Type)
            {
                case ProxyType.Http:
                    return new WebProxy(ProxyHost, ProxyPort) { Credentials = new NetworkCredential(User, Password) };
                case ProxyType.Socks5:
                    return new HttpToSocks5Proxy(ProxyHost, ProxyPort, User, Password);
                default:
                    return null;
            }
        }
    }
}
