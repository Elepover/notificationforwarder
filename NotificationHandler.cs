using Notification_Forwarder.ConfigHelper;
using System;
using System.Linq;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

namespace Notification_Forwarder
{
    public sealed partial class MainPage : Page
    {
        private async void NotificationHandler(object sender, UserNotificationChangedEventArgs e)
        {
            if (!IsListenerActive) return;
            if (e.ChangeKind != UserNotificationChangedKind.Added) return;
            try
            {
                var notifs = await Listener.GetNotificationsAsync(NotificationKinds.Toast);
                var newlyAdded = notifs.Except(Notifications, new NotificationComparer()).ToList();
                NewNotificationPool.AddRange(newlyAdded);
                Notifications.AddRange(newlyAdded);
                foreach (var item in newlyAdded)
                {
                    Conf.CurrentConf.AddApp(new AppInfo(item.AppInfo) { ForwardingEnabled = !Conf.CurrentConf.MuteNewApps });
                    var appIndex = Conf.CurrentConf.FindAppIndex(new AppInfo(item.AppInfo));
                    if (appIndex == -1 && !Conf.CurrentConf.MuteNewApps) continue;
                    if (!Conf.CurrentConf.AppsToForward[appIndex].ForwardingEnabled) continue;
                    UnsentNotificationPool.Add(new Protocol.Notification(item));
                }
                Conf.CurrentConf.NotificationsReceived += newlyAdded.Count;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147024891)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => { await NoPermissionDialog(); });
                }
            }
        }
    }
}