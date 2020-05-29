using Notification_Forwarder.ConfigHelper;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Security.Credentials.UI;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Notification_Forwarder.Controls
{
    public sealed partial class AddAPIEndPointDialog : ContentDialog
    {
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();

        public string Url { get; private set; } = "";
        public bool UseHttpAuth { get; private set; } = false;
        public HttpBasicAuthCredential HttpBasicAuthCredential { get; private set; } = new HttpBasicAuthCredential();
        public bool UseProxy { get; private set; } = false;
        public ProxyInfo ProxyInfo { get; private set; } = new ProxyInfo();

        public AddAPIEndPointDialog()
        {
            this.InitializeComponent();
        }

        private void TextBox_EndPointURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            Url = TextBox_EndPointURL.Text;
            if (!Conf.IsUrl(Url))
                TextBox_EndPointURL.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                TextBox_EndPointURL.BorderBrush = new SolidColorBrush(Colors.Lime);
        }

        public static async Task<string> GetUrlAsync()
        {
            var prompt = new AddAPIEndPointDialog();
            _ = await prompt.ShowAsync();
            return prompt.Url;
        }

        public static async Task<ApiEndPoint> GetApiEndPointAsync()
        {
            var prompt = new AddAPIEndPointDialog();
            _ = await prompt.ShowAsync();
            return new ApiEndPoint(prompt.Url)
            {
                Credential = prompt.HttpBasicAuthCredential,
                Proxy = prompt.ProxyInfo,
                UseHttpAuth = prompt.UseHttpAuth,
                UseProxy = prompt.UseProxy
            };
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Url = string.Empty;
        }

        private async void ToggleSwitch_UseHttpAuth_Toggled(object sender, RoutedEventArgs e)
        {
            if (ToggleSwitch_UseHttpAuth.IsOn)
            {
                var options = new CredentialPickerOptions()
                {
                    TargetName = resourceLoader.GetString("Prompt_HttpAuth_Title"),
                    Caption = resourceLoader.GetString("Prompt_HttpAuth_Title"),
                    Message = resourceLoader.GetString("Prompt_HttpAuth_Message"),
                    CredentialSaveOption = CredentialSaveOption.Hidden,
                    AuthenticationProtocol = AuthenticationProtocol.Basic
                };

                var result = await CredentialPicker.PickAsync(options);
                if (string.IsNullOrEmpty(result.CredentialUserName) || string.IsNullOrWhiteSpace(result.CredentialUserName))
                {
                    ToggleSwitch_UseHttpAuth.IsOn = false;
                    return;
                }
                else
                {
                    UseHttpAuth = true;
                    HttpBasicAuthCredential = new HttpBasicAuthCredential(result.CredentialUserName, result.CredentialPassword);
                }
            }
            else
            {
                UseHttpAuth = false;
                HttpBasicAuthCredential = new HttpBasicAuthCredential();
            }
        }

        private void ToggleSwitch_UseProxy_Toggled(object sender, RoutedEventArgs e)
        {
            if (ToggleSwitch_UseProxy.IsOn)
            {
                UseProxy = true;
                ProxyPanel.Visibility = Visibility.Visible;
            }
            else
            {
                UseProxy = false;
                ProxyPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!Conf.IsUrl(Url))
            {
                TextBox_EndPointURL.BorderBrush = new SolidColorBrush(Colors.Red);
                args.Cancel = true;
            }
            if (UseProxy)
            {
                if (string.IsNullOrEmpty(ProxyPanel.Host) || string.IsNullOrWhiteSpace(ProxyPanel.Host))
                {
                    ProxyPanel.TextBox_ProxyHost.BorderBrush = new SolidColorBrush(Colors.Red);
                    args.Cancel = true;
                    return;
                }
                if (ProxyPanel.Port == 0)
                {
                    args.Cancel = true;
                    return;
                }
                ProxyInfo = new ProxyInfo(ProxyPanel.Host, ProxyPanel.Port, ProxyPanel.ProxyType)
                {
                    User = ProxyPanel.Credential?.User ?? "",
                    Password = ProxyPanel.Credential?.Password ?? ""
                };
            }
            else
            {
                ProxyInfo = new ProxyInfo();
            }
        }
    }
}
