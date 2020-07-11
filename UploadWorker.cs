using Notification_Forwarder.ConfigHelper;
using Notification_Forwarder.Protocol;
using System.Diagnostics;
using System.Threading;
using Windows.UI.Xaml.Controls;

namespace Notification_Forwarder
{
    public sealed partial class MainPage : Page
    {
        private static void UploadWorker()
        {
            Debug.WriteLine("Upload master worker is online.");
            Conf.Log("upload worker activated.", LogLevel.Complete);
            while (Conf.CurrentConf.EnableForwarding)
            {
                if (UnsentNotificationPool.Count == 0) goto Skip;
                Debug.WriteLine("Starting data upload...");
                var sessionId = Conf.GetRandomString(10);
                var pending = new ClientData();
                int processedEndPoints = 0;
                lock (UnsentNotificationPool)
                {
                    Conf.Log($"[{sessionId}] starting forwarding {UnsentNotificationPool.Count} notification(s)...");
                    pending.Notifications.AddRange(UnsentNotificationPool);
                    UnsentNotificationPool.Clear();
                }
                foreach (var endPoint in Conf.CurrentConf.ApiEndPoints2)
                {
                    Debug.WriteLine($"Sending data to {endPoint}");
                    Conf.Log($"[{sessionId}] preparing to send data to {endPoint}...");
                    ClientData.Send(endPoint, pending, sessionId);
                    processedEndPoints++;
                }
                Conf.Log($"[{sessionId}] all uploads started, {processedEndPoints} endpoint(s) to go.", LogLevel.Complete);
                Skip:
                Thread.Sleep(1000);
            }
            Debug.WriteLine("Upload master worker is offline.");
            Conf.Log("upload worker exited.");
        }

        public static void StartUploadWorker()
        {
            if (!Conf.CurrentConf.EnableForwarding) return;
            if (IsUploadWorkerActive) return;
            Debug.WriteLine("Starting upload master worker...");
            Conf.Log("upload worker is starting...");
            UploadWorkerThread = new Thread(UploadWorker) { IsBackground = true };
            UploadWorkerThread.Start();
        }
    }
}