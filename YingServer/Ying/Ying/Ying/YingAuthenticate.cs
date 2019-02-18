using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingDataBase.YingDataStructs;
using Ying.YingExceptions;

namespace Ying
{
    public class YingAuthenticate : YingYing
    {
        public struct YingAuthenticateResult
        {
            public Boolean ysuccess { get; set; }
            public YingExceptionTypes ytype { get; set; }

            public zyy_users yuser { get; set; }
            public zyy_yggdrasil_tokens ytoken { get; set; }
        }

        public static YingAuthenticateResult yauthenticate(String yemail, String ypassword)
        {
            zyy_users yuser = (from y in getYDataBaseManager().getYConnection().Table<zyy_users>()
                               where y.yemail == yemail
                               select y).FirstOrDefault();
            //if(yuser == null) throw iYingException.Ying(YingExceptionTypes.YingInvalidUsernameOrPassword);
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

            if(ytoken.yaccessToken == null || !String.IsNullOrWhiteSpace(yclientToken) && ytoken.yclientToken != yclientToken || getYTimeStamp().TotalMilliseconds - ytoken.ytime > TimeSpan.FromDays(14.64).TotalMilliseconds)
            {
                return new YingAuthenticateResult { ysuccess = false, ytype = YingExceptionTypes.YingInvalidToken, ytoken = ytoken };
            }

            /*if (yuser.ypassword != ypassword)
            {
                throw iYingException.Ying(YingExceptionTypes.YingInvalidUsernameOrPassword);
            }*/

            return new YingAuthenticateResult { ysuccess = true, ytoken = ytoken };
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
        public static Boolean yinvalidate;
        public static Boolean ysignout;

    }
}
