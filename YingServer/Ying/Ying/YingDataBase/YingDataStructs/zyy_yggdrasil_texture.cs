using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingDataBase.YingDataStructs
{
    public struct zyy_yggdrasil_texture
    {
        [PrimaryKey, AutoIncrement]
        public int yid { get; set; }
        public String ytextureid { get; set; }

        public Double ytimestamp { get; set; }
        public String yskin { get; set; }
        /// <summary>
        /// default 或 slim ，其中 default 代表 STEVE，slim 代表 ALEX。
        /// </summary>
        public String yskinmetadata { get; set; }
    }
}
