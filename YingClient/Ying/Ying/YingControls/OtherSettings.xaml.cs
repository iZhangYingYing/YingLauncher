using System.Windows.Controls;
using Ying.Modules;

namespace Ying.Controls
{
    public partial class OtherSettings : Grid
    {
        public OtherSettings()
        {
            InitializeComponent();
            this.DataContext = YingConfig.YArgs;
        }
    }
}
