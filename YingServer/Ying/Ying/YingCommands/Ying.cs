using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingCommand;
using Ying.YingDataBase.YingDataStructs;
using Ying.YingUtils;

namespace Ying.YingCommands
{
    class Ying : YingYing, YingCommandExecutor
    {
        public bool onYCommand(String YSender, YingCommandInfo YCommand, String YLabel, String[] YArgs)
        {
            /*YingMail.ysend("1340761826@qq.com", new StreamReader(getYFiles().getYResource("Ying.yresources.yhtml.Ying.html")).ReadToEnd().Replace("{{yname}}", "烟雨城").Replace("{{ycode}}", $"zyy{Convert.ToString(getYTimeStamp())}"));
            getYConsole().sendYMessage(new YingResolver.YingQQMusicResolver("https://y.qq.com/n/yqq/song/001yBMfz2G4weP.html").YUrl);


            if (YArgs.Count() > 0)
            {
                getYDataBaseManager().getYConnection().Insert(new zyy_users
                {
                    yusername = "Ying",
                    ypassword = "zyy20020604",
                    yopenid = "zyy6ea4d76f01544568a486e3c1aa2a5",
                    yemail = "3282418539@qq.com"
                });

                getYDataBaseManager().getYConnection().Insert(new zyy_yggdrasil_profile
                {
                    yyid = "zyy6ea4d76f01544568a486e3c1aa2a5",
                    yname = "Zhang Ying Ying"
                });

                getYDataBaseManager().getYConnection().Insert(new zyy_yggdrasil_texture
                {
                    yprofileId = "zyy6ea4d76f01544568a486e3c1aa2a5",
                    yprofileName = "Ying Ying",
                    yskin = "zyy.com",
                    yskinmetadata = "STEVE",
                    ytimestamp = getYTimeStamp()
                });



            }*/

            if(YArgs.Length < 1)
            {
                return false;
            }
            else
            {

                switch(YArgs[0])
                {                   //          0       1   2       3
                    case "ysettings": // Ying ysettings set ykey yvalue
                        if (YArgs.Length == 1) {
                            getYConsole().sendYMessage("可用的属性列表");
                            getYConsole().sendYMessage("键           值                       描述");
                            getYDataBaseManager().getYConnection().Table<zyy_settings>().ToList().ForEach((y) => {
                                getYConsole().sendYMessage($"{y.ykey}     {y.yvalue}               {y.ydescription}");
                            });
                        } else if(YArgs.Length > 1)
                        {
                            if(YArgs[1] == "set")
                            {
                                if(YArgs.Length > 2)
                                {
                                    zyy_settings ysetting = (from y in getYDataBaseManager().getYConnection().Table<zyy_settings>()
                                                       where y.ykey == YArgs[2]
                                                       select y).FirstOrDefault();
                                    ysetting.yvalue = YArgs[3];
                                    getYSettings(true);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                /*else
                    getYConsole().sendYMessage("正确用法", YingWindows.YingLogType.YWarn);*/
            }

            return false;
        }

    }
}
