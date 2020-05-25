namespace Notification_Forwarder.ConfigHelper
{
    public class ApiEndPoint
    {
        public string Address { get; set; }
        public bool UseHttpAuth { get; set; }
        public HttpBasicAuthCredential Credential { get; set; }
        public bool UseProxy { get; set; }
        public ProxyInfo Proxy { get; set; }

        public ApiEndPoint() { }
        public ApiEndPoint(string address)
        {
            Address = address;
            UseHttpAuth = false;
            Credential = new HttpBasicAuthCredential();
            UseProxy = false;
            Proxy = new ProxyInfo();
        }
    }
}
