using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Un4seen.Bass;
using static Ying.YingYing;

namespace Ying.YingPages
{
    /// <summary>
    /// YingMediaPage.xaml 的交互逻辑
    /// </summary>
    public partial class YingMediaPage : Page
    {

        private Boolean isY = false;

        public YingMediaPage(String yurl, Action yended)
        {
            InitializeComponent();

            yelement.Source = new Uri(yurl);
            yelement.Play();
            yelement.MediaOpened += (ysender, yevent) =>
            {
                yelement.Position = new TimeSpan(20020);
                BASSActive y = getYPlayer().isYActive();
                if (getYPlayer().isYActive() == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    this.isY = true;
                    getYPlayer().YPause();
                }
            };
            yelement.MediaEnded += (ysender, yevent) =>
            {
                if (this.isY) getYPlayer().YPlay();
                yended.Invoke();
                yelement.Close();
            };

            //if (yallowback) ybackButton.Visibility = Visibility.Visible;
            yelement.MouseLeftButtonDown += (y, yy) =>
            {
                if (this.isY) getYPlayer().YPlay();
                yended.Invoke();
                yelement.Close();
            };

        }

    }
}
