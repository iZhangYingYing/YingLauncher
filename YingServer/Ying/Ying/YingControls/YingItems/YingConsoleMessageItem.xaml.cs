using System;
using System.Windows;
using System.Windows.Controls;


namespace Ying.YingControls.YingItems
{
    /// <summary>
    /// YingConsoleMessageItem.xaml 的交互逻辑
    /// </summary>
    public partial class YingConsoleMessageItem : ListViewItem
    {

        public YingConsoleMessageItem()
        {
            InitializeComponent();
        }

        public YingConsoleMessageItem setYContext(String ytext, Control ycontrol)
        {
            if (ytext == null) ymessage.Visibility = Visibility.Collapsed;
            else ymessage.Text = ytext;
            if (ycontrol == null) ycontrols.Visibility = Visibility.Collapsed;
            else ycontrols.Children.Add(ycontrol);
            return this;
        }
    }
}
