using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Notification_Forwarder.Controls
{
    public sealed partial class NotificationItem : UserControl
    {
        public NotificationItem()
        {
            this.InitializeComponent();
        }
        public NotificationItem(AppDisplayInfo displayInfo, string title, string text, DateTime time)
        {
            this.InitializeComponent();
            //var appLogo = new BitmapImage();
            //var appLogoStream = displayInfo.GetLogo(new Size(44, 44));
            //var thread = new Thread(async () =>
            //{
            //    try
            //    {
            //        await appLogo.SetSourceAsync(await appLogoStream.OpenReadAsync());
            //        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //        {
            //            Image_Icon.Source = appLogo;
            //        });
            //    }
            //    catch { }
            //});
            //thread.Start();
            TextBlock_AppName.Text = title;
            TextBlock_NotificationContent.Text = text;
            if (time.Date == DateTime.Today)
            {
                TextBlock_Time.Text = time.ToShortTimeString();
            }
            else
            {
                TextBlock_Time.Text = time.ToLongTimeString();
            }
        }
    }
}
