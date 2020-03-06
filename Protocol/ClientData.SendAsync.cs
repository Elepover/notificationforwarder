using Newtonsoft.Json;
using Notification_Forwarder.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Notification_Forwarder.Protocol
{
    public partial class ClientData
    {
        public static async Task<bool> SendAsync(string endPoint, ClientData clientData)
        {
            try
            {
                var jsonMessage = JsonConvert.SerializeObject(clientData);
                using (var client = new WebClient())
                {
                    client.Headers.Set(HttpRequestHeader.UserAgent, $"NotificationForwarder/{Conf.GetVersion()}");
                    client.Headers.Set(HttpRequestHeader.ContentType, "application/json");
                    _ = await client.UploadDataTaskAsync(endPoint, "POST", Encoding.UTF8.GetBytes(jsonMessage)).ConfigureAwait(false);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
