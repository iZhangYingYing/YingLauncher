using Redbus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ying.YingYing;

namespace Ying.YingEvents
{
    class YingLoginEvent : EventBase, iYingEvent
    {

        public Boolean isYSuccess { get; set; }
        public String ymessage { get; set; }

        public Boolean isYCanceled()
        {
            throw new NotImplementedException();
        }
    }
}
