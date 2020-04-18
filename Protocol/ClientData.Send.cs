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
                    int retryCounter = 1;
                    retry:
                    Conf.Log($"[{session}@{retryCounter}/{_maxRetries}] attempting to forward message to endpoint {endPoint}...");
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
                        Conf.Log($"[{session}@{retryCounter}/{_maxRetries}] successfully forwarded message to {endPoint}.");
                        Conf.CurrentConf.LastSuccessfulForward = DateTime.Now;
                        Conf.CurrentConf.NotificationsForwarded++;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Unable to forward messages: {ex.Message}, target endpoint: {endPoint}");
                        Conf.Log($"[{session}@{retryCounter}/{_maxRetries}] unable to forward message to endpoint {endPoint}: {ex.Message}, HRESULT 0x{ex.HResult:x}", LogLevel.Warning);
                        if (retryCounter < _maxRetries)
                        {
                            retryCounter++;
                            goto retry;
                        }
                        else
                        {
                            Conf.Log($"[{session}@{retryCounter}/{_maxRetries}] couldn't send data: all {retryCounter} retries to {endPoint} failed.", LogLevel.Error);
                        }
                    }
                }) { IsBackground = true };
                worker.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to start uploader thread: {ex.Message}, target endpoint: {endPoint}");
                Conf.Log($"[{session}] uploader thread failed: {ex.Message}, HRESULT 0x{ex.HResult:x}", LogLevel.Warning);
            }
        }
    }
}
