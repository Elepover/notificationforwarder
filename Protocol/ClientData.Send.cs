using Newtonsoft.Json;
using Notification_Forwarder.ConfigHelper;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace Notification_Forwarder.Protocol
{
    public partial class ClientData
    {
        public static void Send(string endPoint, ClientData clientData, string session)
        {
            try
            {
                Debug.WriteLine("Creating uploader thread...");
                var worker = new Thread(async () => 
                {
                    Conf.Log($"[{session}] attempting to forward message to endpoint {endPoint}...");
                    try
                    {
                        Debug.WriteLine($"Attempting to forward message to endpoint {endPoint}...");
                        var jsonMessage = JsonConvert.SerializeObject(clientData);
                        using (var client = new WebClient())
                        {
                            client.Headers.Set(HttpRequestHeader.UserAgent, $"NotificationForwarder/{Conf.GetVersion()}");
                            client.Headers.Set(HttpRequestHeader.ContentType, "application/json");
                            _ = await client.UploadDataTaskAsync(endPoint, "POST", Encoding.UTF8.GetBytes(jsonMessage)).ConfigureAwait(false);
                        }
                        Conf.Log($"[{session}] successfully forwarded message to {endPoint}.");
                        Conf.CurrentConf.LastSuccessfulForward = DateTime.Now;
                        Conf.CurrentConf.NotificationsForwarded++;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Unable to forward messages: {ex.Message}, target endpoint: {endPoint}");
                        Conf.Log($"[{session}] unable to forward message to endpoint {endPoint}: {ex}", LogLevel.Warning);
                    }
                }) { IsBackground = true };
                worker.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to start uploader thread: {ex.Message}, target endpoint: {endPoint}");
                Conf.Log($"[{session}] uploader thread failed: {ex}", LogLevel.Warning);
            }
        }
    }
}
