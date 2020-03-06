using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Notification_Forwarder.Controls
{
    public sealed partial class AddAPIEndPointDialog : ContentDialog
    {
        public string Url { get => _url; }
        private string _url;
        public AddAPIEndPointDialog()
        {
            this.InitializeComponent();
        }

        private void TextBox_EndPointURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            _url = TextBox_EndPointURL.Text;
        }

        public static async Task<string> GetUrl()
        {
            var prompt = new AddAPIEndPointDialog();
            _ = await prompt.ShowAsync();
            return prompt.Url;
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _url = string.Empty;
        }
    }
}
