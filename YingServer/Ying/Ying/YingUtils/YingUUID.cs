using System;
using System.Security.Cryptography;
using System.Text;

namespace Ying.YingUtils
{
    public class YingUUID : YingYing
    {
        public Guid YUUID { get; set; }

        public YingUUID(String yplayerName)
        {
            MD5CryptoServiceProvider ymd5 = new MD5CryptoServiceProvider();
            byte[] yuuidBytes = ymd5.ComputeHash(Encoding.UTF8.GetBytes("OfflinePlayer:" + yplayerName));
            yuuidBytes[6] &= 0x0f;
            yuuidBytes[6] |= 0x30;
            yuuidBytes[8] &= 0x3f;
            yuuidBytes[8] |= 0x80;
            YUUID = new Guid(ytoUuidString(yuuidBytes));
        }

        private string ytoUuidString(byte[] data)
        {
            long ymsb = 0;
            long ylsb = 0;
            for (int i = 0; i < 8; i++)
                ymsb = (ymsb << 8) | (data[i] & 0xff);
            for (int i = 8; i < 16; i++)
                ylsb = (ylsb << 8) | (data[i] & 0xff);
            return (ydigits(ymsb >> 32, 8) + "-" +
                ydigits(ymsb >> 16, 4) + "-" +
                ydigits(ymsb, 4) + "-" +
                ydigits(ylsb >> 48, 4) + "-" +
                ydigits(ylsb, 12));
        }

        private string ydigits(long yval, int ydigits)
        {
            long yhi = 1L << (ydigits * 4);
            return (yhi | (yval & (yhi - 1))).ToString("X").Substring(1);
        }
    }
}
