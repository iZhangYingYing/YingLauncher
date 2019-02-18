using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingDataBase.YingDataStructs
{

    public struct zyy_yggdrasil_tokens
    {
        [PrimaryKey, AutoIncrement]
        public int yid { get; set; }

        public String yemail { get; set; }

        public String yclientToken { get; set; }
        public String yaccessToken { get; set; }

        public Double ytime { get; set; }
    }
}
