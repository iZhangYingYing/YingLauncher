using System;

namespace Ying.YingWebsocket.YingStructs
{
    class YingStruct
    {

        public enum YingType
        {
            Ying, YUpdata, YLogin, YCode, YRegister, YMessage, YMusic
        }

        public struct YingData
        {
            public int ycode { get; set; }
            public String ysender { get; set; }
            public String ymessage { get; set; }
            public Byte[] ydata { get; set; }
        }

        public String Ying { get; set; } = "颖颖";
        public YingType ytype { get; set; } = YingType.Ying;
        public YingData ydata { get; set; } = new YingData();
        



    }
}
