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
        public static void Send(ApiEndPoint endPoint, ClientData clientData, string session)
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
                        var jsonMessage = JsonConvert.SerializeObject(clientData);
                        using (var client = new WebClient())
                        {
                            client.Headers.Set(HttpRequestHeader.UserAgent, $"NotificationForwarder/{Conf.GetVersion()}");
                            client.Headers.Set(HttpRequestHeader.ContentType, "application/json");
                            if (endPoint.UseHttpAuth)
                            {
                                Conf.Log($"[{session}@{retryCounter}/{_maxRetries}] using http basic authentication.");
                                client.Credentials = endPoint.Credential.GetNetworkCredential();
                            }
                            if (endPoint.UseProxy)
                            {
                                Conf.Log($"[{session}@{retryCounter}/{_maxRetries}] using proxy, type: {endPoint.Proxy.Type}.");
                                client.Proxy = endPoint.Proxy.ToIWebProxy();
                            }
                            _ = await client.UploadDataTaskAsync(endPoint.Address, "POST", Encoding.UTF8.GetBytes(jsonMessage)).ConfigureAwait(false);
                        }
                        Conf.Log($"[{session}@{retryCounter}/{_maxRetries}] successfully forwarded message to {endPoint}.", LogLevel.Complete);
                        Conf.CurrentConf.LastSuccessfulForward = DateTime.Now;
                        Conf.CurrentConf.NotificationsForwarded++;
                    }
                    catch (Exception ex)
                    {
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
                Conf.Log($"[{session}] uploader thread failed: {ex.Message}, HRESULT 0x{ex.HResult:x}", LogLevel.Warning);
            }
        }
    }
}
