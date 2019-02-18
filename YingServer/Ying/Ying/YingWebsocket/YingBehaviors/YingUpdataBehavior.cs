using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using Ying.YingEvents;
using Ying.YingExceptions;
using static Ying.YingWindows.YingWindow;
using static Ying.YingYing;

namespace Ying.YingWebsocket.YingBehaviors
{
    class YingUpdataBehavior : WebSocketBehavior
    {

        protected override void OnOpen()
        {
            NameValueCollection yquery = this.Context.QueryString;
            String ytoken = yquery.Get("ytoken");
            String yhash = yquery.Get("yhash");

            try
            {
                if (String.IsNullOrWhiteSpace(ytoken) || String.IsNullOrWhiteSpace(yhash) || ytoken != "zyy20020604") throw YingException.Ying(YingExceptionTypes.YingInvalidToken);

            }
            catch (YingAuthenticationException yexception)
            {

            }
            getYConsole().sendYMessage($"Ying: {ytoken} | {yhash}");
        }

        
    }
}
