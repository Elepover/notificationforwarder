using Notification_Forwarder.ConfigHelper;
using Notification_Forwarder.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Notification_Forwarder.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EndPointsPage : Page
    {
        public EndPointsPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var endPoint in Conf.CurrentConf.APIEndPoints)
            {
                ListView_EndPoints.Items.Add(endPoint);
            }
        }

        private void ToggleButton_Multiselect_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleButton_Multiselect.IsChecked == true) ListView_EndPoints.SelectionMode = ListViewSelectionMode.Multiple;
            else ListView_EndPoints.SelectionMode = ListViewSelectionMode.Single;
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var temp = new List<string>();
            temp.AddRange(ListView_EndPoints.SelectedItems.Select(s => (string)s));
            foreach (var item in temp)
            {
                Conf.CurrentConf.APIEndPoints.Remove(item);
                ListView_EndPoints.Items.Remove(item);
            }
        }

        private async void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var url = await AddAPIEndPointDialog.GetUrl();
            if (string.IsNullOrEmpty(url)) return;
            if (!Conf.IsUrl(url)) return;
            ListView_EndPoints.Items.Add(url);
            Conf.CurrentConf.APIEndPoints.Add(url);
        }
    }
}
