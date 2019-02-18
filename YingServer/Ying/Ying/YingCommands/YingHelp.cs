using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingCommand;

namespace Ying
{
    class YingHelp : YingYing, YingCommandExecutor
    {
        public Boolean onYCommand(String YSender, YingCommandInfo YCommand, String YLabel, String[] YArgs)
        {

            getYConsole().sendYMessage("--------- YingHelp: Index ---------");
            foreach (KeyValuePair<String, YingCommandInfo> y in getYCommandManager().getYCommands())
            {
                getYConsole().sendYMessage($"/{y.Key}: {y.Value.getYDescribe()}");
            }
           
            return true;
            //throw new NotImplementedException();
        }
    }
}
