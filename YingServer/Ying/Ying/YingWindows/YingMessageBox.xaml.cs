
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ying.YingWindows
{
    /// <summary>
    /// YingMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class YingMessageBox : Window
    {
        public YingMessageBox(String ymessage, Object ytype, long ytime)
        {
            InitializeComponent();

            this.Topmost = true;

            

            

            DispatcherTimer ytimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(ytime)
            };

            ytimer.Tick += (y, yy) =>
            {
                ytimer.Stop();
                this.Close();
            };

            this.ymessage.Text = ymessage;
            ytimer.Start();
 
        }

        public static YingMessageBox Ying(String ymessage, Object ytype, long ytime, Action<Boolean> y)
        {
            YingMessageBox ybox = new YingMessageBox(ymessage, ytype, ytime);
            ybox.ShowDialog();

            y.Invoke(true);

            return ybox;
        }
    }
}
