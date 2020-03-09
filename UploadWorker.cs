using Notification_Forwarder.ConfigHelper;
using Notification_Forwarder.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Notification_Forwarder
{
    public sealed partial class MainPage : Page
    {
        private static void UploadWorker()
        {
            while (!Conf.CurrentConf.EnableForwarding && !RequestWorkerExit)
            {
                if (UnsentNotificationPool.Count == 0) goto Skip;
                var pending = new ClientData();
                lock (UnsentNotificationPool)
                {
                    pending.Notifications.AddRange(UnsentNotificationPool);
                    UnsentNotificationPool.Clear();
                }
                foreach (var endPoint in Conf.CurrentConf.APIEndPoints)
                {
                    ClientData.Send(endPoint, pending);
                }
                Skip:
                Thread.Sleep(1000);
            }
            RequestWorkerExit = false;
        }

        public static void StartUploadWorker()
        {
            if (!Conf.CurrentConf.EnableForwarding) return;
            if (UploadWorkerThread?.IsAlive == true) return;
            RequestWorkerExit = false;
            UploadWorkerThread = new Thread(UploadWorker) { IsBackground = true };
            UploadWorkerThread.Start();
        }
    }
}