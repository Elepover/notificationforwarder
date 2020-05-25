using ColorCode;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace Notification_Forwarder.Controls
{
    public sealed partial class DumpJsonDialog : ContentDialog
    {
        public DumpJsonDialog() => InitializeComponent();
        public DumpJsonDialog(string content)
        {
            InitializeComponent();
            DumpContent = content;
        }
        public string DumpContent { get; private set; }

        public static async Task Open(string content)
        {
            var dialog = new DumpJsonDialog(content);
            var formatter = new RichTextBlockFormatter();
            formatter.FormatRichTextBlock(content, Languages.JavaScript, dialog.TextBox_DumpContent);
            _ = await dialog.ShowAsync();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var pkg = new DataPackage() { RequestedOperation = DataPackageOperation.Copy };
            pkg.SetText(DumpContent);
            Clipboard.SetContent(pkg);
        }
    }
}
