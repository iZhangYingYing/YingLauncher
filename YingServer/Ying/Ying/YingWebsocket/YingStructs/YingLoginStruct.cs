﻿using System;
using Ying.YingDataBase.YingDataStructs;

namespace Ying.YingWebsocket.YingStructs
{
    public struct YingLoginStruct
    {
        public String yemail { get; set; }
        public String ypassword { get; set; }
        public String yclientToken { get; set; }
        public String yaccessToken { get; set; }
        public String ymessage { get; set; }
        public zyy_users yuser { get; set; }
    }
}
