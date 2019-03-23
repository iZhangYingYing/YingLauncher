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
    public class YingPackageEvent : EventBase, iYingEvent
    {

        public Boolean isYSend { get; set; }
        public YingStruct yStruct { get; set; }
        

        public Boolean isYCanceled()
        {
            throw new NotImplementedException();
        }
    }
}
