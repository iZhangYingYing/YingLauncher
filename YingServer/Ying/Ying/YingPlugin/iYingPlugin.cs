using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingPlugin
{
    public abstract class iYingPlugin : YingYing
    {
        public abstract void onYEnable();
        public abstract void onYDisable();
    }
}
