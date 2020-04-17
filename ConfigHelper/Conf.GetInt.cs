using System;
using System.Security.Cryptography;

namespace Notification_Forwarder.ConfigHelper
{
    public partial class Conf
    {
        private static int GetInt(RNGCryptoServiceProvider rnd, int max)
        {
            byte[] r = new byte[4];
            int value;
            do
            {
                rnd.GetBytes(r);
                value = BitConverter.ToInt32(r, 0) & int.MaxValue;
            } while (value >= max * (int.MaxValue / max));
            return value % max;
        }
    }
}
