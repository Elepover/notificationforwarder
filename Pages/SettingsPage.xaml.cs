using Newtonsoft.Json;
using Notification_Forwarder.ConfigHelper;
using Notification_Forwarder.Controls;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Notification_Forwarder.Pages
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();

        private bool _isToggleSwitchReady = false;

        private void ToggleSwitch_DisplayPackageName_Toggled(object sender, RoutedEventArgs e)
        {
            if (!_isToggleSwitchReady) return;
            Conf.CurrentConf.DisplayPackageName = ToggleSwitch_DisplayPackageName.IsOn;
        }

        private void ToggleSwitch_EnableForwarder_Toggled(object sender, RoutedEventArgs e)
        {
            if (!_isToggleSwitchReady) return;
            Conf.CurrentConf.EnableForwarding = ToggleSwitch_EnableForwarder.IsOn;
            if (ToggleSwitch_EnableForwarder.IsOn && !MainPage.IsUploadWorkerActive)
            {
                MainPage.StartUploadWorker();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ToggleSwitch_DisplayPackageName.IsOn = Conf.CurrentConf.DisplayPackageName;
            ToggleSwitch_EnableForwarder.IsOn = Conf.CurrentConf.EnableForwarding;
            ToggleSwitch_MuteNewApps.IsOn = Conf.CurrentConf.MuteNewApps;
            _isToggleSwitchReady = true;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Conf.MainPageInstance.ToggleBackButton(true);
            Frame.Navigate(typeof(AppsPage), null, new DrillInNavigationTransitionInfo());
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            Conf.MainPageInstance.ToggleBackButton(true);
            Frame.Navigate(typeof(EndPointsPage), null, new DrillInNavigationTransitionInfo());
        }

        private void ToggleSwitch_MuteNewApps_Toggled(object sender, RoutedEventArgs e)
        {
            if (!_isToggleSwitchReady) return;
            Conf.CurrentConf.MuteNewApps = ToggleSwitch_MuteNewApps.IsOn;
        }

        private async void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("Prompt_JsonExportWarning_Title"),
                Content = resourceLoader.GetString("Prompt_JsonExportWarning_Content"),
                DefaultButton = ContentDialogButton.Secondary,
                PrimaryButtonText = resourceLoader.GetString("Prompt_JsonExportWarning_Yes"),
                SecondaryButtonText = resourceLoader.GetString("Prompt_JsonExportWarning_No")
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Secondary) return;
            await DumpJsonDialog.Open(JsonConvert.SerializeObject(Conf.CurrentConf, Formatting.Indented));
        }
    }
}
