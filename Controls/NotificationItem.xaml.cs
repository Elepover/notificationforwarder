using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

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
