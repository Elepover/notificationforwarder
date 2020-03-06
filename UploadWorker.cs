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
        private static async Task UploadWorker()
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
                    if (await ClientData.SendAsync(endPoint, pending))
                    {
                        Conf.CurrentConf.LastSuccessfulForward = DateTime.Now;
                        Conf.CurrentConf.NotificationsForwarded++;
                    }
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
            UploadWorkerThread = new Thread(async () => await UploadWorker()) { IsBackground = true };
            UploadWorkerThread.Start();
        }
    }
}