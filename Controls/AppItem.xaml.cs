using Notification_Forwarder.ConfigHelper;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Notification_Forwarder.Controls
{
    public sealed partial class AppItem : UserControl
    {
        public AppInfo CurrentApp
        {
            get => _currentApp;
            set
            {
                TextBlock_AppName.Text = Conf.CurrentConf.DisplayPackageName ? $"{value.DisplayName} ({value.AppUserModelId})" : value.DisplayName;
                _currentApp = value;
            }
        }
        private AppInfo _currentApp;
        private readonly bool _toggleSwitchReady = false;

        public delegate void AppItemToggleStatusChangedEventHandler(object sender, AppItemToggleStatusChangedEventArgs e);
        public event AppItemToggleStatusChangedEventHandler AppItemToggleStatusChanged;

        public AppItem()
        {
            this.InitializeComponent();
        }

        public AppItem(AppInfo app, bool isOn = false)
        {
            this.InitializeComponent();
            CurrentApp = app;
            ToggleSwitch_Forwarding.IsOn = isOn;
            _toggleSwitchReady = true;
        }

        private void ToggleSwitch_Forwarding_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            if (!_toggleSwitchReady) return;
            AppItemToggleStatusChanged?.Invoke(this, new AppItemToggleStatusChangedEventArgs(ToggleSwitch_Forwarding.IsOn, CurrentApp));
        }
    }

    public class AppItemToggleStatusChangedEventArgs
    {
        public bool IsOn;
        public AppInfo CorrespondingApp;
        public AppItemToggleStatusChangedEventArgs() { }
        public AppItemToggleStatusChangedEventArgs(bool isOn, AppInfo correspondingApp)
        {
            IsOn = isOn;
            CorrespondingApp = correspondingApp;
        }
    }
}
