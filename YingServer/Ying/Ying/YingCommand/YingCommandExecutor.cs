using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingCommand
{
    public interface YingCommandExecutor
    {
        Boolean onYCommand(String YSender, YingCommandInfo YCommand, String YLabel, String[] YArgs);
    }
}
