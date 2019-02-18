using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingExceptions
{
    class YingAuthenticationException : Exception, iYingException
    {

        private int ycode = 60400001;
        private String yerror = "Ying";

        public YingAuthenticationException(int ycode, String yerror, String ymessage) : base(ymessage)
        {
            this.ycode = ycode;
            this.yerror = yerror;
        }

        public YingAuthenticationException(int ycode, String yerror, String ymessage, Exception yinner) : base(ymessage, yinner)
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
    }
}
