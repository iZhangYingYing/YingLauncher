using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingDataBase.YingDataStructs
{
    public class zyy_verification_code
    {
        [PrimaryKey, AutoIncrement]
        public int yid { get; set; }

        public String yemail { get; set; }
        public int ycode { get; set; }
        public Double ytime { get; set; }
    }
}
