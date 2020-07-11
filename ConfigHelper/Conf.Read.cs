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
            if (CurrentConf.APIEndPoints.Count != 0)
            {
                Log("legacy API endpoints format detected, converting...", LogLevel.Warning);
                int converted = 0;
                foreach (var apiEp in CurrentConf.APIEndPoints)
                {
                    CurrentConf.ApiEndPoints2.Add(new ApiEndPoint(apiEp));
                    converted++;
                }
                CurrentConf.APIEndPoints.Clear();
                Log($"conversion complete, {converted} entry(entries) converted.", LogLevel.Complete);
            }
            Log($"configurations loaded, {CurrentConf.AppsToForward.Count} app(s) on record, {CurrentConf.ApiEndPoints2.Count} target(s) set.", LogLevel.Complete);
            return parsed;
        }
    }
}
