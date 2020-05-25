namespace Notification_Forwarder.ConfigHelper
{
    public partial class HttpBasicAuthCredential
    {
        public string User { get; set; }
        public string Password { get; set; }

        public HttpBasicAuthCredential() { }
        public HttpBasicAuthCredential(string user, string password)
        {
            User = user;
            Password = password;
        }
    }
}
