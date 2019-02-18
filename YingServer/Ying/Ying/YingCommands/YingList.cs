using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Ying.YingCommand;
using Ying.YingWebsocket;
using Ying.YingWebsocket.YingBehaviors;

namespace Ying.YingCommands
{
    class YingList : YingYing, YingCommandExecutor
    {
        public Boolean onYCommand(String YSender, YingCommandInfo YCommand, String YLabel, String[] YArgs)
        {
            getYConsole().sendYMessage($"There are {getYServer().getYClients().Count}/20020604 players online:");
            foreach (KeyValuePair<String, YingBehavior> y in getYServer().getYClients())
            {
                getYConsole().sendYMessage(y.Key + " | " + y.Value.StartTime);
            }
            return true;
        }
    }
}
