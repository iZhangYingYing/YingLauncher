using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingCommand;
using Ying.YingDataBase.YingDataStructs;
using Ying.YingUtils;

namespace Ying.YingCommands
{
    class YingTools : YingYing, YingCommandExecutor
    {
        public bool onYCommand(string YSender, YingCommandInfo YCommand, string YLabel, string[] YArgs)
        {
            YingMail.ysend(new StreamReader(getYFiles().getYResource("Ying.yresources.yhtml.Ying.html")).ReadToEnd().Replace("{{yname}}", "烟雨城").Replace("{{ycode}}", $"zyy{Convert.ToString(getYTimeStamp())}"));
            getYConsole().sendYMessage(new YingResolver.YingQQMusicResolver("https://y.qq.com/n/yqq/song/001yBMfz2G4weP.html").YUrl);


            if (YArgs.Count() > 0)
            {
                getYDataBaseManager().getYConnection().Insert(new zyy_users {
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

                getYDataBaseManager().getYConnection().Insert(new zyy_yggdrasil_texture {
                    yprofileId = "zyy6ea4d76f01544568a486e3c1aa2a5",
                    yprofileName = "Ying Ying",
                    yskin = "zyy.com",
                    yskinmetadata = "STEVE",
                    ytimestamp = getYTimeStamp()
                });



            }

            return true;
        }
    }
}
