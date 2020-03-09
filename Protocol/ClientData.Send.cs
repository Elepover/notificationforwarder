using Newtonsoft.Json;
using Notification_Forwarder.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Notification_Forwarder.Protocol
{
    public partial class ClientData
    {
        public static void Send(string endPoint, ClientData clientData)
        {
            try
            {
                var worker = new Thread(async () => 
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
                        Conf.CurrentConf.LastSuccessfulForward = DateTime.Now;
                        Conf.CurrentConf.NotificationsForwarded++;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Unable to forward messages: {ex.Message}, target endpoint: {endPoint}");
                    }
                }) { IsBackground = true };
                worker.Start();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unable to start uploader thread: {ex.Message}, target endpoint: {endPoint}");
            }
        }
    }
}
