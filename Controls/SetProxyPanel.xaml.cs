using Microsoft.UI.Xaml.Controls;
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
    public sealed partial class SetProxyPanel : UserControl
    {
        public SetProxyPanel() => this.InitializeComponent();

        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
        public string Host { get; private set; }
        public int Port { get; private set; } = 0;
        public ProxyType ProxyType { get; private set; }
        public bool RequireAuthentication { get; private set; }
        public HttpBasicAuthCredential Credential { get; private set; } = new HttpBasicAuthCredential();

        private async void ToggleSwitch_RequireAuth_Toggled(object sender, RoutedEventArgs e)
        {
            if (ToggleSwitch_RequireAuth.IsOn)
            {
                var options = new CredentialPickerOptions()
                {
                    TargetName = resourceLoader.GetString("Prompt_HttpAuth_Title"),
                    Caption = resourceLoader.GetString("Prompt_HttpAuth_Title"),
                    Message = resourceLoader.GetString("Prompt_ProxyAuth_Message"),
                    CredentialSaveOption = CredentialSaveOption.Hidden,
                    AuthenticationProtocol = AuthenticationProtocol.Basic
                };

                var result = await CredentialPicker.PickAsync(options);
                if (string.IsNullOrEmpty(result.CredentialUserName) || string.IsNullOrWhiteSpace(result.CredentialUserName))
                {
                    ToggleSwitch_RequireAuth.IsOn = false;
                    return;
                }
                else
                {
                    RequireAuthentication = true;
                    Credential = new HttpBasicAuthCredential(result.CredentialUserName, result.CredentialPassword);
                }
            }
            else
            {
                RequireAuthentication = false;
                Credential = new HttpBasicAuthCredential();
            }
        }

        private void TextBox_ProxyHost_TextChanged(object sender, TextChangedEventArgs e)
        {
            Host = TextBox_ProxyHost.Text;
            if (string.IsNullOrEmpty(Host) || string.IsNullOrWhiteSpace(Host))
                TextBox_ProxyHost.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                TextBox_ProxyHost.BorderBrush = new SolidColorBrush(Colors.Lime);
        }

        private void NumberBox_ProxyPort_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        {
            Port = (int)Math.Truncate(NumberBox_ProxyPort.Value);
            NumberBox_ProxyPort.BorderBrush = new SolidColorBrush(Colors.Lime);
        }

        private void ComboBox_ProxyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProxyType = (ProxyType)ComboBox_ProxyType.SelectedIndex;
        }
    }
}
