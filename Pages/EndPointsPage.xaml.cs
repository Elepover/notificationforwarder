using Notification_Forwarder.ConfigHelper;
using Notification_Forwarder.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Notification_Forwarder.Pages
{
    public sealed partial class EndPointsPage : Page
    {
        public EndPointsPage()
        {
            this.InitializeComponent();
        }

        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var endPoint in Conf.CurrentConf.ApiEndPoints2)
            {
                ListView_EndPoints.Items.Add(endPoint.Address);
            }
        }

        private void ToggleButton_Multiselect_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleButton_Multiselect.IsChecked == true) ListView_EndPoints.SelectionMode = ListViewSelectionMode.Multiple;
            else ListView_EndPoints.SelectionMode = ListViewSelectionMode.Single;
        }

        private async void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var temp = new List<string>();
            temp.AddRange(ListView_EndPoints.SelectedItems.Select(s => (string)s));
            if (temp.Count == 0) return;
            var dialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("Prompt_DeleteAPIEndPointConfirmation_Title"),
                Content = resourceLoader.GetString("Prompt_DeleteAPIEndPointConfirmation_Content").Replace("%1", temp.Count.ToString()),
                DefaultButton = ContentDialogButton.Secondary,
                PrimaryButtonText = resourceLoader.GetString("Prompt_DeleteAPIEndPointConfirmation_Yes"),
                SecondaryButtonText = resourceLoader.GetString("Prompt_DeleteAPIEndPointConfirmation_No")
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Secondary) return;
            foreach (var item in temp)
            {
                // search for corresponding item
                foreach (var ep in Conf.CurrentConf.ApiEndPoints2)
                {
                    if (ep.Address == item)
                    {
                        Conf.CurrentConf.ApiEndPoints2.Remove(ep);
                        break;
                    }
                }
                ListView_EndPoints.Items.Remove(item);
                Conf.Log($"deleted endpoint {item}.", LogLevel.Complete);
            }
        }

        private async void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var ep = await AddAPIEndPointDialog.GetApiEndPointAsync();
            if (string.IsNullOrEmpty(ep.Address)) return;
            if (!Conf.IsUrl(ep.Address)) return;
            Conf.Log($"a new endpoint {ep.Address} has been added.", LogLevel.Complete);
            ListView_EndPoints.Items.Add(ep.Address);
            Conf.CurrentConf.ApiEndPoints2.Add(ep);
        }
    }
}
