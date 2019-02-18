namespace KMCCC.Tools
{
    
    #region

    using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.VisualBasic.FileIO;

    #endregion

    /// <summary>
    ///     有用的东西
    /// </summary>
    public static class UsefulTools
	{

        const string encryptKey = "zyy20020604";

        public static string EncryptString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

            byte[] key = Encoding.Unicode.GetBytes(encryptKey);
            byte[] data = Encoding.Default.GetBytes(str);

            using (var ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static string DecryptString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

            byte[] key = Encoding.Unicode.GetBytes(encryptKey);
            byte[] data = Convert.FromBase64String(str);

            using (var ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
                cs.Write(data, 0, data.Length);
                try
                {
                    cs.FlushFinalBlock();
                }
                catch
                {
                    return null;
                }
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        public static void DeleteDirectoryAsync(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            Task.Run(() => {
                FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            });
        }


        public static bool IsValidEmailAddress(string emailAddress)
        {
            var regex = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            return (!string.IsNullOrEmpty(emailAddress) && regex.IsMatch(emailAddress));
        }

        public static string DoReplace(this string source, IDictionary<string, string> dic)
		{
			return dic.Aggregate(source, (current, pair) => current.Replace("${" + pair.Key + "}", pair.Value));
		}

		public static string GoString(this Guid guid)
		{
			return guid.ToString().Replace("-", "");
		}

		public static void Dircopy(string source, string target)
		{
			var sourceDir = new DirectoryInfo(source);
			if (!Directory.Exists(target))
			{
				Directory.CreateDirectory(target);
			}
			foreach (var file in sourceDir.GetFiles())
			{
				File.Copy(file.FullName, target + "\\" + file.Name, true);
			}
			foreach (var subdir in sourceDir.GetDirectories())
			{
				Dircopy(subdir.FullName, target + "\\" + subdir.Name);
			}
		}

        public static Guid GetPlayerUuid(string playerName)
        {
            MD5CryptoServiceProvider md5csp = new MD5CryptoServiceProvider();
            byte[] uuidBytes = md5csp.ComputeHash(System.Text.Encoding.UTF8.GetBytes("OfflinePlayer:" + playerName));
            uuidBytes[6] &= 0x0f;
            uuidBytes[6] |= 0x30;
            uuidBytes[8] &= 0x3f;
            uuidBytes[8] |= 0x80;
            Guid uuid = new Guid(toUuidString(uuidBytes));
            return uuid;
        }

        static string toUuidString(byte[] data)
        {
            long msb = 0;
            long lsb = 0;
            for (int i = 0; i < 8; i++)
                msb = (msb << 8) | (data[i] & 0xff);
            for (int i = 8; i < 16; i++)
                lsb = (lsb << 8) | (data[i] & 0xff);
            return (digits(msb >> 32, 8) + "-" +
                digits(msb >> 16, 4) + "-" +
                digits(msb, 4) + "-" +
                digits(lsb >> 48, 4) + "-" +
                digits(lsb, 12));
        }
        static string digits(long val, int digits)
        {
            long hi = 1L << (digits * 4);
            return (hi | (val & (hi - 1))).ToString("X").Substring(1);
        }

        public static string PrintfArray(object[] obj)
        {
            StringBuilder printf = new StringBuilder();
            obj.ToList().ForEach(a => { if (a.GetType() == typeof(string)) { printf.Append(" "+a); } });
            return printf.ToString();
        }

#if DEBUG
		public static string Print(this string str)
		{
			Console.WriteLine(str);
			return str;
		}
#endif
	}
}