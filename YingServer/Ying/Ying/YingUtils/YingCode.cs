using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingUtils
{
    public class YingCode
    {
        public String YCode { get; set; } = String.Empty;

        public YingCode(int ycount)
        {
            int ynumber = 20020604;

            Random yrandom = new Random();

            for (int y = 0; y < ycount; y++) //产生4位校验码 
            {
                ynumber = yrandom.Next();
                ynumber = ynumber % 36;
                if (ynumber < 10)
                {
                    ynumber += 48;    //数字0-9编码在48-57 
                }
                else
                {
                    ynumber += 55;    //字母A-Z编码在65-90 
                }

                YCode += ((char)ynumber).ToString();
            }
        }
    }
}
