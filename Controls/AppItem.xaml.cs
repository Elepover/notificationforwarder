using Notification_Forwarder.ConfigHelper;
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
                if (Conf.CurrentConf.DisplayPackageName) TextBlock_AppName.Text = $"{value.DisplayName} ({value.AppUserModelId})";
                else TextBlock_AppName.Text = value.DisplayName;
                _currentApp = value;
            }
        }
        private AppInfo _currentApp;

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
            TextBlock_AppName.Text = $"{app.DisplayName} ({app.AppUserModelId})";
            ToggleSwitch_Forwarding.IsOn = isOn;
        }

        private void ToggleSwitch_Forwarding_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
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
