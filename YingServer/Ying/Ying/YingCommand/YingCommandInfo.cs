using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingCommand
{
    public class YingCommandInfo
    {

        private YingCommandExecutor _yexecutor = null;
        private String _yusage = null;
        private String _ydescribe = null;

        /*public YingCommandExecutor setYExecutor { get => _yexecutor; set => _yexecutor = value; }
        public String yusage { get => _yusage; set => _yusage = value; }
        public String ydescribe { get => _ydescribe; set => _ydescribe = value; }*/

        public YingCommandInfo setYExecutor(YingCommandExecutor yexecutor) {
            this._yexecutor = yexecutor;
            return this;
        }

        public YingCommandExecutor getYExecutor()
        {
            return this._yexecutor;
        }

        public YingCommandInfo setYUsage(String yusage)
        {
            this._yusage = yusage;
            return this;
        }

        public String getYUsage()
        {
            return this._yusage;
        }

        public YingCommandInfo setYDescribe(String ydescribe)
        {
            this._ydescribe = ydescribe;
            return this;
        }

        public String getYDescribe()
        {
            return this._ydescribe;
        }



    }
}
