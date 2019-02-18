using SQLite;
using System;

namespace Ying.YingDataBase.YingDataStructs
{
    struct zyy_settings
    {
        [PrimaryKey, AutoIncrement]
        public int yid { get; set; }

        public String ykey { get; set; }
        public String yvalue { get; set; }

        public String ydescription { get; set; }
    }
}
