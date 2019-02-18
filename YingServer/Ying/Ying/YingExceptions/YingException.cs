using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
namespace Ying.YingExceptions
{
    public enum YingExceptionTypes
    {
        Ying = 60400001,
        //YingSystem
        YingYing = 60401001,
        //YingAuthentication
        YingInvalidToken = 60402001,
        YingInvalidCredentials = 60402002,
        YingInvalidUsernameOrPassword = 60402003
        //YingAccesstoekn
    }

    public class iYingException : Exception
    {

        //private YingExceptionTypes ytype = YingExceptionTypes.Ying;

        private int ycode = 60400001;
        private String yerror = "Ying";

        public iYingException()
        {
        }

        public iYingException(String ymessage) : base(ymessage)
        {
        }

        public iYingException(String ymessage, Exception yinner) : base(ymessage, yinner)
        {
        }

        public iYingException(int ycode, String yerror, String ymessage) : base(ymessage)
        {
            this.ycode = ycode;
            this.yerror = yerror;
        }

        public iYingException(int ycode, String yerror, String ymessage, Exception yinner) : base(ymessage, yinner)
        {
            this.ycode = ycode;
            this.yerror = yerror;
        }

        public int getYCode()
        {
            return this.ycode;
        }

        public String getYError()
        {
            return this.yerror;
        }

        public static iYingException Ying(YingExceptionTypes ytype)
        {
            switch (ytype)
            {
                case YingExceptionTypes.YingInvalidToken:
                    return new iYingException(403, "ForbiddenOperationException", "Ying Invalid Token");
                case YingExceptionTypes.YingInvalidUsernameOrPassword:
                    return new iYingException(403, "ForbiddenOperationException", "Ying Invalid Username Or Password");
                //ForbiddenOperation = 60402001,
        //IllegalArguement = 60402002,
                /*
                /*case YingExceptionTypes.YingInvaildToken:
                    return new iYingException("Ying Invaild Token");
                    break;
                //YingAuthentication
                case YingExceptionTypes.YingUnknownAccount:
                    return new iYingException("Ying Unknow Account");
                    break;
                case YingExceptionTypes.YingIncorrectCredentials:
                    return new iYingException("Ying Incorrect Credentials");
                    break;
                case YingExceptionTypes.YingDisableAccount:
                    return new iYingException("Ying Disable Account");
                    break;*//*
                default:
                    return new iYingException();
            }
        }


    }
}
*/


namespace Ying.YingExceptions
{
    public enum YingExceptionTypes
    {
        Ying = 60400001,
        //YingSystem
        YingYing = 60401001,
        //YingAuthentication
        YingInvalidToken = 60402001,
        YingInvalidCredentials = 60402002,
        YingInvalidUsernameOrPassword = 60402003
        //YingAccesstoekn
    }

    public interface iYingException
    {
        int getYCode();
        String getYError();
    }

    public class YingException
    {
        public static Exception Ying(YingExceptionTypes ytype)
        {
            switch (ytype)
            {
                case YingExceptionTypes.YingInvalidToken:
                    return new YingAuthenticationException(403, "ForbiddenOperationException", "Ying Invalid Token");
                case YingExceptionTypes.YingInvalidUsernameOrPassword:
                    return new YingAuthenticationException(403, "ForbiddenOperationException", "Ying Invalid Username Or Password");
                //ForbiddenOperation = 60402001,
                //IllegalArguement = 60402002,
                
                /*case YingExceptionTypes.YingInvaildToken:
                    return new YingException("Ying Invaild Token");
                //YingAuthentication
                case YingExceptionTypes.YingUnknownAccount:
                    return new YingException("Ying Unknow Account");
                case YingExceptionTypes.YingIncorrectCredentials:
                    return new YingException("Ying Incorrect Credentials");
                case YingExceptionTypes.YingDisableAccount:
                    return new YingException("Ying Disable Account");*/
                default:
                    return null;
            }
        }
    }
}
 