using Notification_Forwarder.ConfigHelper;
using System;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Notification_Forwarder.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LogsPage : Page
    {
        private int _currentLogIndex = 0;
        private bool _requestThreadExit = false;

        public LogsPage()
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
                    LoadLogs();
                    TextBlock_Loading.Visibility = Visibility.Collapsed;
                    TextBox_Logs.Visibility = Visibility.Visible;
                    TextBox_Logs.Select(TextBox_Logs.Text.Length - 1, 0);
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
                        LoadLogs();
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

        private void LoadLogs()
        {
            lock (Conf.Logs)
            {
                var len = Conf.Logs.Count;
                if (len == _currentLogIndex) return;
                for (int i = _currentLogIndex; i < len; i++)
                {
                    var entry = Conf.Logs[i];
                    TextBox_Logs.Text += entry.ToString();
                }
                _currentLogIndex = len;
            }
            TextBox_Logs.Select(TextBox_Logs.Text.Length - 1, 0);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _requestThreadExit = true;
        }
    }
}
