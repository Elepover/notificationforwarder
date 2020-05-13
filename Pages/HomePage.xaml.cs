using Notification_Forwarder.ConfigHelper;
using System;
using System.Threading;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Notification_Forwarder.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();

        private string GetString(string key)
        {
            return resourceLoader.GetString(key);
        }

        public HomePage()
        {
            this.InitializeComponent();
        }

        private bool _requestThreadExit = false;

        public async void UpdateThread()
        {
            while (!_requestThreadExit)
            {
                try
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        if (!MainPage.IsPermissionGranted) ToggleSwitch_Listener.IsEnabled = false;
                        if (MainPage.IsListenerActive) ToggleSwitch_Listener.IsOn = true;
                        else ToggleSwitch_Listener.IsOn = false;
                        TextBlock_AppsToForward.Text = $"{Conf.CurrentConf.GetActiveAppsCount()}/{Conf.CurrentConf.AppsToForward.Count}";
                        TextBlock_ForwarderService.Text = (MainPage.UploadWorkerThread?.IsAlive ?? false) ? GetString("On") : GetString("Off");
                        TextBlock_LastSuccessfulForward.Text = Conf.CurrentConf.LastSuccessfulForward == DateTime.MinValue ? "N/A" : Conf.CurrentConf.LastSuccessfulForward.ToString("yyyy/MM/dd\nHH:mm:ss");
                        TextBlock_NotificationsForwarded.Text = Conf.CurrentConf.NotificationsForwarded.ToString();
                        TextBlock_NotificationsReceived.Text = Conf.CurrentConf.NotificationsReceived.ToString();
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var worker = new Thread(UpdateThread)
            {
                IsBackground = true
            };
            worker.Start();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _requestThreadExit = true;
        }

        private void ToggleSwitch_Listener_Toggled(object sender, RoutedEventArgs e)
        {
            // triggered by code
            if (sender == null) return;
            MainPage.IsListenerActive = ToggleSwitch_Listener.IsOn;
        }
    }
}
