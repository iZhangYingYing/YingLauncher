using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingCommand;
using Ying.YingWebsocket.YingStructs;

namespace Ying.YingCommands
{
    class YingSay : YingYing, YingCommandExecutor
    {///tell <player> <private message ...>
        public Boolean onYCommand(String YSender, YingCommandInfo YCommand, String YLabel, String[] YArgs)
        {
            if (YArgs.Length == 0) return false;
            
            getYServer().getYService().BroadcastAsync(JsonMapper.ToJson(new YingStruct()
            {
                ytype = YingStruct.YingType.Ying,
                ydata = new YingStruct.YingData()
                {
                    ycode = 0,
                    ysender = YSender,
                    ymessage = String.Join(" ", YArgs),
                    ydata = null
                }               
            }), () => { });
            
            return true;
        }
    }
}
