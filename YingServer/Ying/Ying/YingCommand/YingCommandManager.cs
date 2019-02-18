using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Ying.YingWindows;
using static Ying.YingWindows.YingWindow;

namespace Ying.YingCommand
{
    public class YingCommandManager : YingYing
    {

        private Dictionary<String, YingCommandInfo> YCommands = new Dictionary<String, YingCommandInfo>();

        public YingCommandInfo getYCommand(String YCommand) {
            if (YCommands.ContainsKey(YCommand))
            {
                return YCommands[YCommand];
            }
            else
            {
                YCommands.Add(YCommand, new YingCommandInfo());
                return YCommands[YCommand];
            }
        }

        public void runYCommandAsync(String YCommand)
        {
            YingApp.Current.Dispatcher.InvokeAsync(() =>
            {
                
                List<String> YYCommand = YCommand.Split(' ').ToList<String>();

                if (YCommands.ContainsKey(YYCommand[0]))
                {
                    YingCommandInfo ycommand = YCommands[YYCommand[0]];

                    int yline = getYConsole().sendYMessage("Console issued server command: /" + YYCommand[0]);


                    List<String> YArgs = YYCommand.ToList<String>();
                    YArgs.RemoveAt(0);

                    Stopwatch ywatch = new Stopwatch();
                    ywatch.Start();
                    Boolean yresult = ycommand.getYExecutor().onYCommand("Ying Console", ycommand, null, YArgs.ToArray<String>());
                    ywatch.Stop();

                    getYConsole().setYMessage(yline, "", YingLogType.YInfo, ywatch.ElapsedMilliseconds);

                    if (yresult)
                    {

                    }
                    else
                    {
                        getYConsole().sendYMessage(ycommand.getYUsage());
                    }

                    return true;
                }
                else
                {
                    getYConsole().sendYMessage("Unknown command. Type \"/help\" for help.");
                    //
                    return false;
                }
            }) ;
        }


        public Dictionary<String, YingCommandInfo> getYCommands()
        {
            return this.YCommands;
        }



        

    }
}
