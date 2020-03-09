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
            while (Conf.CurrentConf.EnableForwarding && !RequestWorkerExit)
            {
                if (UnsentNotificationPool.Count == 0) goto Skip;
                Debug.WriteLine("Starting data upload...");
                var pending = new ClientData();
                lock (UnsentNotificationPool)
                {
                    pending.Notifications.AddRange(UnsentNotificationPool);
                    UnsentNotificationPool.Clear();
                }
                foreach (var endPoint in Conf.CurrentConf.APIEndPoints)
                {
                    Debug.WriteLine($"Sending data to {endPoint}");
                    ClientData.Send(endPoint, pending);
                }
                Skip:
                Thread.Sleep(1000);
            }
            Debug.WriteLine("Upload master worker is offline.");
            RequestWorkerExit = false;
        }

        public static void StartUploadWorker()
        {
            if (!Conf.CurrentConf.EnableForwarding) return;
            if (UploadWorkerThread?.IsAlive == true) return;
            Debug.WriteLine("Starting upload master worker...");
            RequestWorkerExit = false;
            UploadWorkerThread = new Thread(UploadWorker) { IsBackground = true };
            UploadWorkerThread.Start();
        }
    }
}