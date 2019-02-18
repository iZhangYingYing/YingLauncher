using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingPlugin;

namespace YingResolver
{
    public class YingResolver : iYingPlugin
    {
        public override void onYEnable()
        {
            getYConsole().sendYMessage("YingResolver v6.4.0 was loaded");
        }

        public override void onYDisable()
        {
            getYConsole().sendYMessage("YingResolver v6.4.0 was unloaded");
        }
    }
}
