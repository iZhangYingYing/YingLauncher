using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingUtil
{
    class YingSHA1
    {

        public static String Ying(string y)
        {
            try
            {
                FileStream yfile = new FileStream(y, FileMode.Open, FileAccess.Read);
                SHA1 ysha1 = new SHA1CryptoServiceProvider();
                byte[] yretval = ysha1.ComputeHash(yfile);
                yfile.Close();

                StringBuilder ybuilder = new StringBuilder();
                for (int i = 0; i < yretval.Length; i++)
                {
                    ybuilder.Append(yretval[i].ToString("x2"));
                }
                return ybuilder.ToString();
                //Console.WriteLine("文件SHA1：{0}", ybuilder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }



    }
}
