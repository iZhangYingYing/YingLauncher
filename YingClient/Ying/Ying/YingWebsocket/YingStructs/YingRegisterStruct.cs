﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingWebsocket.YingStructs
{
    public class YingRegisterStruct
    {
        public Boolean isYSuccess { get; set; }
        public int ycode { get; set; }
        public String yclientToken { get; set; }
        public String yaccessToken { get; set; }
        public String ymessage { get; set; }
        public zyy_users yuser { get; set; }
    }
}
