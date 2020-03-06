using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return parsed;
        }
    }
}
