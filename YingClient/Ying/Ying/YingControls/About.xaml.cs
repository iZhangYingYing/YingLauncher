using System.Windows.Controls;
using Ying.Modules;

namespace Ying.Controls
{
    public partial class About : Grid
    {
        public About()
        {
            InitializeComponent();
            _aboutBox.Text = $"关于 GBCL V{YingConfig.LauncherVersion}";
        }

        private void Open_Link(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            System.Diagnostics.Process.Start((sender as TextBlock).Text);
        }
    }
}
