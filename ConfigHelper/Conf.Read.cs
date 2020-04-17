using Newtonsoft.Json;
using Windows.Storage;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static Conf Read(bool apply = true)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            var composite = (ApplicationDataCompositeValue) roamingSettings.Values["main"];
            Conf parsed;
            if (composite != null)
            {
                parsed = JsonConvert.DeserializeObject<Conf>((string) composite["json"]);
            }
            else
            {
                parsed = new Conf();
                Save(parsed);
            }
            if (apply) CurrentConf = parsed;
            Log($"configurations loaded, {CurrentConf.AppsToForward.Count} app(s) on record, {CurrentConf.APIEndPoints.Count} target(s) set.");
            return parsed;
        }
    }
}
