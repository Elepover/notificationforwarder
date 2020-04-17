using Newtonsoft.Json;
using Windows.Storage;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static void Save(Conf conf)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            var composite = new ApplicationDataCompositeValue
            {
                ["json"] = JsonConvert.SerializeObject(conf, Formatting.None)
            };
            roamingSettings.Values["main"] = composite;
        }
    }
}
