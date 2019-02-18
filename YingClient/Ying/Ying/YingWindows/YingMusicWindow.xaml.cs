using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Ying.YingControls;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Media;
using Ying.YingUtil;
using Redbus.Interfaces;
using Redbus;
using Redbus.Events;
using static Ying.YingYing;
using System.Drawing;
using Un4seen.Bass;

namespace Ying.YingWindows
{
    /// <summary>
    /// YingMusicWindow.xaml 的交互逻辑
    /// </summary>
    public partial class YingMusicWindow : Window
    {

        private Boolean isyfavorite = false;

        private DispatcherTimer ytimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(604) };
        private WindowInteropHelper yhelper = null;

        private Boolean isY = false;
        

        public YingMusicWindow()
        {
            InitializeComponent();


            yhelper = new WindowInteropHelper(this);
            yhelper.EnsureHandle();

            this.isY = getYPlayer().YInit(yhelper.Handle);

            if(this.isY) getYPlayer().YPrepare("https://www.kzwr.com/kzwrfs?fid=c01e443f031534scdt.mp3");
            ylyric.yload("https://www.kzwr.com/kzwrfs?fid=d86f3e0f9694w2s1tw.lrc");

            //getYPlayer().YPlay();

            yname.Text = "流光记";
            yartist.Text = "银临";

            //this.YPlayToggle(this, null);


            ytimer.Tick += (y1, y2) =>
            {
                if (this.isY) ylyric.LrcRoll(TimeSpan.FromSeconds(getYPlayer().getYCurrentTime()).TotalMilliseconds);
                if (this.IsFocused && !getYMusic().IsFocused) getYMusic().Focus();
            };
            ytimer.Start();


            this.MouseLeftButtonDown += (y, yy) => DragMove();
            this.yplaytoggle.Click += (y, yy) => YPlayToggle(y, yy);
            this.yfavorite.Click += (y, yy) => YFavoriteToggle(y, yy);

            getYMagnetic().addChild(this, MagneticLocation.Right);
        }

        private void YPlayToggle(object ysender, RoutedEventArgs yevent)
        {
            if (!this.isY) return;
            if (getYPlayer().isYActive() == BASSActive.BASS_ACTIVE_PLAYING)
            {
                this.yplaytoggle.Background = new ImageBrush(this.FindResource("yimages.ymusic.yplay") as ImageSource); ;
                if(ysender != this) getYPlayer().YPause();
            }
            else
            {
                this.yplaytoggle.Background = new ImageBrush(this.FindResource("yimages.ymusic.ypause") as ImageSource); ;
                if (ysender != this) getYPlayer().YPlay();
            }
            //MessageBox.Show("Ying");
        }

        private void YFavoriteToggle(object ysender, RoutedEventArgs yevent)
        {
            if (isyfavorite)
            {
                yfavorite.Background = new ImageBrush(this.FindResource("yimages.ymusic.ylike") as ImageSource);
                isyfavorite = false;
            }
            else
            {
                yfavorite.Background = new ImageBrush(this.FindResource("yimages.ymusic.ylike.selected") as ImageSource);
                isyfavorite = true;
            }
            //MessageBox.Show("Ying");
        }
        
    }
}
