﻿using Notification_Forwarder.Controls;
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
using Windows.UI.Xaml.Media.Animation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Notification_Forwarder.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void ToggleSwitch_DisplayPackageName_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            Conf.CurrentConf.DisplayPackageName = ToggleSwitch_DisplayPackageName.IsOn;
        }

        private void ToggleSwitch_EnableForwarder_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            Conf.CurrentConf.EnableForwarding = ToggleSwitch_EnableForwarder.IsOn;
            if (ToggleSwitch_EnableForwarder.IsOn && (MainPage.UploadWorkerThread?.IsAlive != true))
            {
                MainPage.StartUploadWorker();
            }
            else MainPage.RequestWorkerExit = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ToggleSwitch_DisplayPackageName.IsOn = Conf.CurrentConf.DisplayPackageName;
            ToggleSwitch_EnableForwarder.IsOn = Conf.CurrentConf.EnableForwarding;
            ToggleSwitch_MuteNewApps.IsOn = Conf.CurrentConf.MuteNewApps;
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
            if (sender == null) return;
            Conf.CurrentConf.MuteNewApps = ToggleSwitch_MuteNewApps.IsOn;
        }
    }
}