using Notification_Forwarder.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Notification_Forwarder
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
        public static UserNotificationListener Listener = UserNotificationListener.Current;
        public static Type PreviousPage = null;
        public static bool IsListenerActive { get; set; } = false;
        public static bool IsPermissionGranted { get; private set; } = false;
        public static List<UserNotification> Notifications = new List<UserNotification>();
        public static List<UserNotification> NewNotificationPool = new List<UserNotification>();
        public static List<Protocol.Notification> UnsentNotificationPool = new List<Protocol.Notification>();
        public static Thread UploadWorkerThread;
        public static bool IsUploadWorkerActive => UploadWorkerThread?.IsAlive == true;

        private string GetString(string key)
        {
            return resourceLoader.GetString(key);
        }

        public MainPage()
        {
            this.InitializeComponent();
            Conf.MainPageInstance = this;
        }

        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected == true)
            {
                Navigation_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                Navigation_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void Navigation_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type _page;
            switch (navItemTag)
            {
                case "logs":
                    _page = typeof(Pages.LogsPage);
                    break;
                case "settings":
                    _page = typeof(Pages.SettingsPage);
                    break;
                case "home":
                    _page = typeof(Pages.HomePage);
                    break;
                case "notifications":
                    _page = typeof(Pages.NotificationsPage);
                    break;
                default:
                    // cancel navigation
                    return;
            }
            var preNavPageType = ContentFrame.CurrentSourcePageType;
            if (!(_page is null) && !Equals(preNavPageType, _page))
            {
                Navigation.IsBackEnabled = false;
                _ = ContentFrame.Navigate(_page, null, transitionInfo);
            }
        }

        private async Task NoPermissionDialog(string errorContent = "")
        {
            IsListenerActive = false;
            IsPermissionGranted = false;
            var dialog = new ContentDialog()
            {
                Title = GetString("Prompt_NoPermission_Title"),
                Content = GetString("Prompt_NoPermission_Text") + (string.IsNullOrEmpty(errorContent) ? string.Empty : $"\n\nError: {errorContent}"),
                DefaultButton = ContentDialogButton.Close,
                CloseButtonStyle = Resources["ButtonRevealStyle"] as Style,
                CloseButtonText = GetString("Prompt_NoPermission_OK")
            };
            await dialog.ShowAsync();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // go to loading page
            Navigation.IsEnabled = false;
            _ = ContentFrame.Navigate(typeof(Pages.LoadingPage), null);
            // check permissions
            try
            {
                Conf.Log("detecting permissions...");
                var accessStatus = await Listener.RequestAccessAsync();
                switch (accessStatus)
                {
                    case UserNotificationListenerAccessStatus.Allowed:
                        Conf.Log("permission granted!", LogLevel.Complete);
                        IsListenerActive = true;
                        IsPermissionGranted = true;
                        var initialList = await Listener.GetNotificationsAsync(NotificationKinds.Toast);
                        Conf.Log($"loading {initialList.Count} notification(s)...");
                        foreach (var notif in initialList)
                        {
                            Conf.CurrentConf.AddApp(new AppInfo(notif.AppInfo) {ForwardingEnabled = !Conf.CurrentConf.MuteNewApps});
                        }
                        Notifications.AddRange(initialList);
                        Listener.NotificationChanged += NotificationHandler;
                        Conf.Log("notification listener activated.", LogLevel.Complete);
                        StartUploadWorker();
                        break;
                    default:
                        Conf.Log("permission not granted, no exceptions thrown.", LogLevel.Warning);
                        await NoPermissionDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Conf.Log($"notification listener failed: {ex.Message}, HRESULT 0x{ex.HResult:x}", LogLevel.Error);
                await NoPermissionDialog(ex.Message);
            }
            Navigation.SelectedItem = HomePageItem;
            Navigation.IsEnabled = true;
            // wait 1 sec
            var paneCloser = new Thread(async () =>
            {
                Thread.Sleep(1000);
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Navigation.IsPaneOpen = false;
                });
            });
            paneCloser.Start();
        }

        private void Navigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            ContentFrame.GoBack(new DrillInNavigationTransitionInfo());
            Navigation.IsBackEnabled = false;
        }

        public void ToggleBackButton(bool isEnabled)
        {
            Navigation.IsBackEnabled = isEnabled;
        }

        private async void AboutPageItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await new Controls.AboutDialog().ShowAsync();
        }
    }
}
