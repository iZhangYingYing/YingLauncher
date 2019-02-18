using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingDataBase.YingDataStructs;

namespace Ying
{
    public class YingDataBaseManager : YingYing
    {

        private SQLiteConnection yconnection = new SQLiteConnection("Ying.db");


        public YingDataBaseManager()
        {
            yconnection.CreateTable<zyy_users>();
            CreateTableResult yresult = yconnection.CreateTable<zyy_settings>();
            yconnection.CreateTable<zyy_yggdrasil_profile>();
            yconnection.CreateTable<zyy_yggdrasil_texture>();
            yconnection.CreateTable<zyy_yggdrasil_tokens>();
            yconnection.CreateTable<zyy_verification_code>();

            if (yresult == CreateTableResult.Created)
            {
                this.yconnection.Insert(new zyy_settings { ykey = "yid", yvalue = Guid.NewGuid().ToString("N"), ydescription = "服务器唯一标识符 (请不要随意修改)" });
                this.yconnection.Insert(new zyy_settings { ykey = "yname", yvalue = "Ying", ydescription = "服务器名称"  });
                this.yconnection.Insert(new zyy_settings { ykey = "ydescription", yvalue = "一个Minecraft服务器", ydescription = "服务器简介" });
                this.yconnection.Insert(new zyy_settings { ykey = "yip", yvalue = "0.0.0.0", ydescription = "服务器ip" });
                this.yconnection.Insert(new zyy_settings { ykey = "yport", yvalue = "6040", ydescription = "服务器端口 (默认 6040)" });
          }
        }

        public SQLiteConnection getYConnection()
        {
            return this.yconnection;
        }

        /*public void YGet<T>()
        {
            yconnection.Get<YingType>(1);
        }*/
    }
}
