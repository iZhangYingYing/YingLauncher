using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingDataBase.YingDataStructs
{
    public enum YingLevel
    {
        YPlayer,
        YAdmin,
        YServer
    }

    public enum YingSex
    {
        YBoy,
        YGirl
    }

    public struct zyy_users
    {
        [PrimaryKey, AutoIncrement]
        public int yid { get; set; }

        public String yusername { get; set; }
        public String ypassword { get; set; }
        public String ysalt { get; set; }
        public String yopenid { get; set; }
        public YingSex ysex { get; set; }
        public String yemail { get; set; }
        public String yavatar { get; set; }

        public int ymoney { get; set; }

        public String ysignature { get; set; }
        public Double yregtime { get; set; }
        public Double ylastlogin { get; set; }

        public String yqq { get; set; }

        public YingLevel ylevel { get; set; }

        public String yip { get; set; }
        public String yregip { get; set; }

    }
}
