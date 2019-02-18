using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingExceptions
{
    class YingHttpException : Exception, iYingException
    {

        private int ycode = 60400001;

        public YingHttpException(int ycode, String ymessage) : base(ymessage)
        {
            this.ycode = ycode;
        }

        public YingHttpException(int ycode, String ymessage, Exception yinner) : base(ymessage, yinner)
        {
            this.ycode = ycode;
        }

        public int getYCode()
        {
            return this.ycode;
        }

        public String getYError()
        {
            throw new NotImplementedException();
        }
    }
}
