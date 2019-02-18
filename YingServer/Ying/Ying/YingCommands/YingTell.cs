using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingCommand;

namespace Ying.YingCommands
{
    class YingTell : YingYing, YingCommandExecutor
    {
        public Boolean onYCommand(String YSender, YingCommandInfo YCommand, String YLabel, String[] YArgs)
        {
            return false;
        }
    }
}
