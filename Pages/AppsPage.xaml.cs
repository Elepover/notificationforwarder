using Notification_Forwarder.ConfigHelper;
using Notification_Forwarder.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Notification_Forwarder.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AppsPage : Page
    {
        public AppsPage()
        {
            this.InitializeComponent();
        }

        private List<AppItem> apps = new List<AppItem>();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var app in Conf.CurrentConf.AppsToForward)
            {
                var item = new AppItem(app, app.ForwardingEnabled);
                item.AppItemToggleStatusChanged += AppItemToggleStatusChanged;
                apps.Add(item);
                ListView_Apps.Items.Add(item);
            }
        }

        private void AppItemToggleStatusChanged(object sender, AppItemToggleStatusChangedEventArgs e)
        {
            // find corresponding app in settings
            Conf.CurrentConf.AppsToForward[Conf.CurrentConf.FindAppIndex(e.CorrespondingApp)].ForwardingEnabled = e.IsOn;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListView_Apps.SelectedItem == null) return;
            var app = ((AppItem)ListView_Apps.SelectedItem).CurrentApp;
            Conf.CurrentConf.AppsToForward.Remove(app);
            ListView_Apps.Items.RemoveAt(ListView_Apps.SelectedIndex);
        }
    }
}
