using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Ying.Controls;
using Ying.Modules;
using Ying.YingEvents;

namespace Ying.YingPages
{
    /// <summary>
    /// YingSettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class YingSettingsPage : Page
    {
       private YingSettingsEvent yevent = new YingSettingsEvent();
        private Dictionary<String, Label> ytitles = new Dictionary<String, Label>();

        public YingSettingsPage()
        {
            InitializeComponent();

            this.DataContext = YingConfig.YArgs;
            /*

            yevent.ysettings.Add(new YingSettingsEvent.YingSettings
            {
                YTag = YingSettingsEvent.YTag_YLauncher,
                YDisplayName = "颖启动设置",
                YImagePath = "/images/launch.png",
                YContent = ylunch
            });

            yevent.ysettings.Add(new YingSettingsEvent.YingSettings
            {
                YTag = YingSettingsEvent.YTag_YLauncher,
                YDisplayName = "颖启动设置",
                YImagePath = "/images/launch.png",
                YContent = ylunch
            });

            yevent.ysettings.Add(new YingSettingsEvent.YingSettings
            {
                YTag = "Ying Test",
                YDisplayName = "颖启动设置",
                YImagePath = "/images/launch.png",
                YContent = ylunch
            });

            getYEvent().Publish<YingSettingsEvent>(yevent);

            yevent.ysettings.ForEach((y) =>
            {


                TabItemButtonOne yone = new TabItemButtonOne();
                yone.Tag = y.YDisplayName;
                yone.Content = y.YDisplayName;
                yone.Background = new SolidColorBrush(Colors.White);
                yone.Checked += this.YingTabSwitch;
                yone.ImagePath = y.YImagePath;
                yone.ImageSize = 16.604;
                yone.Foreground = new SolidColorBrush(Colors.White);
                yone.Opacity = 0.804;
                yone.Height = 24.604;
                yone.HorizontalAlignment = HorizontalAlignment.Stretch;
                yone.Margin = new Thickness(6.604, 0, 6.604, 3.604);





                if (this.ytitles.ContainsKey(y.YTag))
                {
                    //Checked="YingTabSwitch" Tag="YingLaunchSettings" Content="启动设置" ImagePath= ImageSize="16.604" Foreground="White" 

                    this.ylist.Children.Insert(this.ylist.Children.IndexOf(this.ytitles[y.YTag]), yone);
                }
                else
                {
                    Label ytitle = new Label();
                    ytitle.Content = y.YTag;
                    ytitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7A7A7"));
                    ytitle.Padding = new Thickness(25.604, 25.604, 0, 0);
                    ytitle.VerticalAlignment = VerticalAlignment.Center;
                    ytitle.Height = ytitle.MinHeight = 40.604;
                    int ypostion = this.ylist.Children.Add(ytitle);
                    this.ylist.Children.Add(yone);
                    this.ytitles.Add(y.YTag, ytitle);
                    //this.ylist.Children.Insert(ypostion + 1, yone);
                }

    




                this.ycontorls.Children.Add(y.YContent);
            });
*/
            this.ycontorls.Children.Add(new YingLaunchSettings { Tag = "YingLaunchSettings" });
            this.ycontorls.Children.Add(new YingGameDownload { Tag = "YYingLaunchSettings" });
            this.ycontorls.Children.Add(new YingGameDownload { Tag = "YYYingLaunchSettings" });

        }

        private void YingTabSwitch(object ysender, RoutedEventArgs yevent)
        {
            /*IEnumerator yenumerator = this.ycontorls.Children.GetEnumerator();
            while (yenumerator.MoveNext()) { 
                if ((ysender as TabItemButtonOne).Tag.ToString() == ((Grid)yenumerator.Current).Tag.ToString())
                {
                    GeneralTransform ytransform = (ysender as TabItemButtonOne).TransformToVisual(this.ycontent);
                    Double yposition = ytransform.Transform(new Point(0, 0)).Y - (this.ycontent.ActualHeight / 2) + 10;

                    this.ycontent.ScrollToVerticalOffset(yposition);
                }
            } */         
        }
    }
}
