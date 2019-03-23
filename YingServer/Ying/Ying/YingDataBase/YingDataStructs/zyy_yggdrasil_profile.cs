using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingDataBase.YingDataStructs
{
    public struct zyy_yggdrasil_profile
    {
        [PrimaryKey, AutoIncrement]
        public int yid { get; set; }

        public String yyid { get; set; }
        public String yname { get; set; }

        public String ytextureid { get; set; }
    }
}
