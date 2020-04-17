using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        public static async Task DelayAsync(int interval, CoreDispatcher dispatcher)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Thread.Sleep(interval); });
        }
    }
}
