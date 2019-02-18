using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingDataBase.YingDataStructs;

namespace Ying.YingWebsocket.YingStructs
{
    public class YingRegisterStruct
    {
        public Boolean isYSuccess { get; set; }
        public String ymessage { get; set; }
        public int ycode { get; set; }
        public zyy_users yuser { get; set; }
    }
}
