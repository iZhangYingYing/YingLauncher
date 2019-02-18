using Redbus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingWebsocket.YingStructs;
using static Ying.YingYing;

namespace Ying.YingEvents
{
    class YingMessageEvent : EventBase, iYingEvent
    {

        public Boolean isYSend { get; set; }
        public YingStruct ystruct { get; set; }
        

        public Boolean isYCanceled()
        {
            throw new NotImplementedException();
        }
    }
}
