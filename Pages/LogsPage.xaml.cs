using Notification_Forwarder.ConfigHelper;
using System;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
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
            Conf.MainPageInstance.GlobalScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            TextBlock_Loading.Visibility = Visibility.Visible;
            var initialWorker = new Thread(async () =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    LoadLogs();
                    TextBlock_Loading.Visibility = Visibility.Collapsed;
                    RichTextBlock_Logs.Visibility = Visibility.Visible;

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
            var paragraph = new Paragraph();
            lock (Conf.Logs)
            {
                var len = Conf.Logs.Count;
                if (len == _currentLogIndex) return;
                for (int i = _currentLogIndex; i < len; i++)
                {
                    var entry = Conf.Logs[i];
                    var run = new Run
                    {
                        Foreground = entry.Color,
                        Text = (i == len - 1) ? entry.ToString() : entry.ToString() + '\n'
                    };
                    paragraph.Inlines.Add(run);
                }
                RichTextBlock_Logs.Blocks.Add(paragraph);
                Conf.MainPageInstance.GlobalScrollViewer.ChangeView(0.0, Grid_Default.ActualHeight, 1.0f);
                _currentLogIndex = len;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Conf.MainPageInstance.GlobalScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            _requestThreadExit = true;
        }
    }
}
