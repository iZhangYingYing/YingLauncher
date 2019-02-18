using Ying.Helpers;
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
using System.Threading;

namespace Ying.YingWindows
{
    /// <summary>
    /// YingMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class YingMessageBox : Window
    {

        public struct YingMessageType
        {
            public static ImageSource yencrypted {get => YingApp.Current.FindResource("yimages.yicons.yencrypted") as ImageSource;}
            public static ImageSource yface_cry {get => YingApp.Current.FindResource("yimages.yicons.yface.cry") as ImageSource;}
            public static ImageSource yface_smile {get => YingApp.Current.FindResource("yimages.yicons.yface.smile") as ImageSource;}
            public static ImageSource yhelp {get => YingApp.Current.FindResource("yimages.yicons.yhelp") as ImageSource;}
            public static ImageSource yno {get => YingApp.Current.FindResource("yimages.yicons.yno") as ImageSource;}
            public static ImageSource yok {get => YingApp.Current.FindResource("yimages.yicons.yok") as ImageSource;}
            public static ImageSource ytips {get => YingApp.Current.FindResource("yimages.yicons.ytip") as ImageSource;}
        }

        public YingMessageBox(String ymessage, ImageSource yicon, int ytime)
        {
            InitializeComponent();

            if (Environment.OSVersion.Version.Major == 10)
            {
                Win10BlurHelper.EnableBlur(this);
            }
            else
            {
                this.BorderThickness = new Thickness(1);
                this.BorderBrush = Brushes.DarkGray;
                Win7BlurHelper.EnableAeroGlass(this);
            }

            this.yicon.Source = yicon;
            this.ymessage.Text = ymessage;
            this.Topmost = true;
            Task.Run(() => {
                Thread.Sleep(ytime);
                this.Dispatcher.Invoke(() => this.Close());  
            });
 
        }

        public static YingMessageBox Ying(String ymessage, ImageSource yicon, int ytime)
        {
            YingMessageBox ybox = new YingMessageBox(ymessage, yicon, ytime);
            ybox.Show();

            return ybox;
        }
    }
}
