using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ying.YingCommand;

namespace Ying.YingCommands
{
    class YingRestart : YingYing, YingCommandExecutor
    {
        public Boolean onYCommand(String YSender, YingCommandInfo YCommand, String YLabel, String[] YArgs)
        {
            getYConsole().sendYMessage("Restarting the server");

            getYServer().YStop();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Application.Current.Shutdown();

            return true;
        }
    }
}
