using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Notification_Forwarder.Controls
{
    public sealed partial class AddAPIEndPointDialog : ContentDialog
    {
        public string Url { get; private set; }

        public AddAPIEndPointDialog()
        {
            this.InitializeComponent();
        }

        private void TextBox_EndPointURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            Url = TextBox_EndPointURL.Text;
        }

        public static async Task<string> GetUrl()
        {
            var prompt = new AddAPIEndPointDialog();
            _ = await prompt.ShowAsync();
            return prompt.Url;
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Url = string.Empty;
        }
    }
}
