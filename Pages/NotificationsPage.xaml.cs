using Notification_Forwarder.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Notification_Forwarder.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NotificationsPage : Page
    {
        private bool _requestThreadExit = false;

        public NotificationsPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock_Loading.Visibility = Visibility.Visible;
            var initialWorker = new Thread(async () =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    LoadNotifications(MainPage.Notifications);
                    TextBlock_Loading.Visibility = Visibility.Collapsed;
                });
                var worker = new Thread(UpdateThread) { IsBackground = true };
                worker.Start();
            });
            initialWorker.Start();
        }

        public async void UpdateThread()
        {
            while (!_requestThreadExit)
            {
                try
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        if (MainPage.Notifications.Count == 0)
                        {
                            TextBlock_NoNotificationsAvailable.Visibility = Visibility.Visible;
                            ListView_Notifications.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            TextBlock_NoNotificationsAvailable.Visibility = Visibility.Collapsed;
                            ListView_Notifications.Visibility = Visibility.Visible;
                            lock (MainPage.NewNotificationPool)
                            {
                                LoadNotifications(MainPage.NewNotificationPool);
                                MainPage.NewNotificationPool.Clear();
                            }
                        }
                    });
                }
                catch
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            _requestThreadExit = false;
        }

        private string GetNotificationBody(Notification notif)
        {
            string text = "No text";
            var toastBinding = notif.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);
            if (toastBinding != null)
            {
                IReadOnlyList<AdaptiveNotificationText> textElements = toastBinding.GetTextElements();
                string titleText = textElements.FirstOrDefault()?.Text;
                string bodyText = string.Join("\n", textElements.Skip(1).Select(t => t.Text));
                text = $"{titleText}\n{bodyText}";
            }
            return text;
        }

        private void LoadNotifications(List<UserNotification> notifs)
        {
            foreach (var notif in notifs)
            {
                ListView_Notifications.Items.Insert(
                    0,
                    new Controls.NotificationItem(
                        notif.AppInfo.DisplayInfo,
                        (Conf.CurrentConf.DisplayPackageName ? $"{notif.AppInfo.DisplayInfo.DisplayName} ({notif.AppInfo.AppUserModelId})" : notif.AppInfo.DisplayInfo.DisplayName),
                        GetNotificationBody(notif.Notification),
                        notif.CreationTime.DateTime));
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _requestThreadExit = true;
        }

        private void ListView_Notifications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView_Notifications.SelectedItem = null;
        }
    }
}
