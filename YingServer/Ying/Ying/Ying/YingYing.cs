using Redbus;
using SQLite;
using System;
using System.Net;
using Ying.YingCommand;
using Ying.YingDataBase.YingDataStructs;
using Ying.YingPlugin;
using Ying.YingUtils;
using Ying.YingWebsocket;
using Ying.YingWindows;

namespace Ying
{
    public struct YingSettings
    {
        //private TableQuery<zyy_settings> yquery { get => getYDataBaseManager().getYConnection().Table<zyy_settings>(); }
        public String yid { get; set; }
        public String yname { get; set; }
        public String ydescription { get; set; }
        public IPAddress yip { get; set; }
        public int yport { get; set; }
        public int ymail_yaddress { get; set; }
    }

    public class YingYing
    {
        private static YingCommandManager YCommandManager = new YingCommandManager();
        private static YingPluginManager ypluginmanager = new YingPluginManager();
        private static YingServer yserver = new YingServer();
        private static EventBus yevent = new EventBus();
        private static YingDataBaseManager ydatabasemanager = new YingDataBaseManager();
        private static YingFileHandle yfiles = new YingFileHandle();
        private static YingSettings ysettings = new YingSettings();


        public YingYing()
        {

        }

        public static void Ying()
        {

        }

        public static YingServer getYServer()
        {
            return yserver;
        }

        public static YingCommandInfo getYCommand(String YCommand)
        {
            return YCommandManager.getYCommand(YCommand);
        }

        public static YingCommandManager getYCommandManager()
        {
            return YCommandManager;
        }

        public static YingPluginManager getYPluginManager()
        {
            return ypluginmanager;
        }

        public static YingFileHandle getYFiles()
        {
            return yfiles;
        }

        public static YingWindow getYConsole()
        {
            try
            {
                
                return YingApp.Current.Dispatcher.Invoke(() => YingApp.Current.MainWindow as YingWindow);
            }
            catch(Exception yexception)
            {
                getYConsole().sendYMessage(yexception.Message);
                getYConsole().sendYMessage(yexception.StackTrace);
                return null;
            }
        }

        public static EventBus getYEvent()
        {
            return yevent;
        }

        public static YingDataBaseManager getYDataBaseManager()
        {
            return ydatabasemanager;
        }

        public static YingSettings getYSettings()
        {
            if(ysettings.yid == null)
            {
                getYDataBaseManager().getYConnection().Table<zyy_settings>().ToList().ForEach((y) => {
                    switch(y.ykey)
                    {
                        case "yid": ysettings.yid = y.yvalue; break;
                        case "yname": ysettings.yname = y.yvalue; break;
                        case "ydescription": ysettings.ydescription = y.yvalue; break;
                        case "yip": ysettings.yip = IPAddress.Parse(y.yvalue); break;
                        case "yport": ysettings.yport = int.Parse(y.yvalue); break;
                    }
                });
            }
            return ysettings;
        }



        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static TimeSpan getYTimeStamp()
        {
            return DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64(yts.TotalSeconds).ToString();
        }




    }
}
