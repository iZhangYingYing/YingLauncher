using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ying.YingCommand;

namespace Ying.YingCommands
{
    class YingStop : YingYing, YingCommandExecutor
    {
        public Boolean onYCommand(String YSender, YingCommandInfo YCommand, String YLabel, String[] YArgs)
        {
            getYConsole().sendYMessage("Stopping the server");

            getYServer().YStop();

            getYPluginManager().YDisableAll();

            Application.Current.Shutdown();

            //YingApp.Main.Current.Shutdown();

            return true;
        }
    }
}
