namespace Notification_Forwarder.ConfigHelper
{
    public partial class ProxyInfo
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string ProxyHost { get; set; }
        public int ProxyPort { get; set; }
        public ProxyType Type { get; set; }

        public ProxyInfo() { }
        public ProxyInfo(string host, int port, ProxyType type)
        {
            ProxyHost = host;
            ProxyPort = port;
            Type = type;
        }
    }
}
