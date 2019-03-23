using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ying.YingDataBase.YingDataStructs;
using Ying.YingExceptions;

namespace Ying
{
    public struct YingAuthenticateResult
        {
            public Boolean ysuccess { get; set; }
            public YingExceptionTypes ytype { get; set; }

            public zyy_users yuser { get; set; }
            public zyy_yggdrasil_tokens ytoken { get; set; }
        }

    public class YingAuthenticate : YingYing
    {
        

        public static YingAuthenticateResult yauthenticate(String yemail, String ypassword)
        {
            zyy_users yuser = (from y in getYDataBaseManager().getYConnection().Table<zyy_users>()
                               where y.yemail == yemail
                               select y).FirstOrDefault();
            //if(yuser == null) throw iYingException.Ying(YingExceptionTypes.YingInvalidUsernameOrPassword);
            ypassword = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(ypassword))).Replace("-", "").ToLower();
            ypassword = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(ypassword + yuser.ysalt))).Replace("-", "").ToLower();
            if (yuser.ypassword != ypassword)
            {
                return new YingAuthenticateResult { ysuccess = false, ytype = YingExceptionTypes.YingInvalidUsernameOrPassword, yuser = yuser };
            }
            
            return new YingAuthenticateResult { ysuccess = true, yuser = yuser };
        }

        public static YingAuthenticateResult yrefresh(String yaccessToken, String yclientToken)
        {
            zyy_yggdrasil_tokens ytoken = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_tokens>()
                                           where y.yaccessToken == yaccessToken
                                           select y).FirstOrDefault();

            if (ytoken.yaccessToken == null || !String.IsNullOrWhiteSpace(yclientToken) && ytoken.yclientToken != yclientToken || getYTimeStamp().TotalMilliseconds - ytoken.ytime > TimeSpan.FromDays(14.64).TotalMilliseconds)
            {
                return new YingAuthenticateResult { ysuccess = false, ytype = YingExceptionTypes.YingInvalidToken, ytoken = ytoken };
            }

            /*if (yuser.ypassword != ypassword)
            {
                throw iYingException.Ying(YingExceptionTypes.YingInvalidUsernameOrPassword);
            }*/
            //TODO: Ying updata token
            getYDataBaseManager().getYConnection().Delete<zyy_yggdrasil_tokens>(ytoken.yid);
            ytoken = new zyy_yggdrasil_tokens
            {
                yemail = ytoken.yemail,

                yclientToken = ytoken.yclientToken,
                yaccessToken = Guid.NewGuid().ToString("N"),

                ytime = getYTimeStamp().TotalMilliseconds
            };
            getYDataBaseManager().getYConnection().Insert(ytoken);

            zyy_users yuser = (from y in getYDataBaseManager().getYConnection().Table<zyy_users>()
                               where y.yemail == ytoken.yemail
                               select y).FirstOrDefault();

            return new YingAuthenticateResult { ysuccess = true, yuser = yuser, ytoken = ytoken };
        }

        public static YingAuthenticateResult yvalidate(String yaccessToken, String yclientToken)
        {
            zyy_yggdrasil_tokens ytoken = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_tokens>()
                               where y.yaccessToken == yaccessToken
                               select y).FirstOrDefault();

            if (ytoken.yaccessToken == null || !String.IsNullOrWhiteSpace(yclientToken) && ytoken.yclientToken != yclientToken || getYTimeStamp().TotalMilliseconds - ytoken.ytime > TimeSpan.FromDays(7.64).TotalMilliseconds)
            {
                return new YingAuthenticateResult { ysuccess = false, ytype = YingExceptionTypes.YingInvalidToken, ytoken = ytoken };
            }

            /*if (yuser.ypassword != ypassword)
            {
                throw iYingException.Ying(YingExceptionTypes.YingInvalidUsernameOrPassword);
            }*/

            return new YingAuthenticateResult { ysuccess = true, ytoken = ytoken };
        }

        public static void yinvalidate(String yaccessToken, String yclientToken)
        {
            zyy_yggdrasil_tokens ytoken = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_tokens>()
                                           where y.yaccessToken == yaccessToken
                                           select y).FirstOrDefault();

            getYDataBaseManager().getYConnection().Delete<zyy_yggdrasil_tokens>(ytoken.yid);
        }

        public static Boolean ysignout(String yusername, String ypassword)
        {
            YingAuthenticateResult yresult = yauthenticate(yusername, ypassword);
            if(yresult.ysuccess) {
                IEnumerator<zyy_yggdrasil_tokens> yenumerator = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_tokens>()
                                           where y.yemail == yresult.yuser.yemail
                                           select y).GetEnumerator();
                while(yenumerator.MoveNext())
                {
                    getYDataBaseManager().getYConnection().Delete<zyy_yggdrasil_tokens>(yenumerator.Current.yid);  
                }
                return true;
            }
            return false;
        }

    }
}
