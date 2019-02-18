using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using Ying.YingCommands;
using System.Windows.Input;
using Ying.YingControls.YingItems;
using System.Windows.Documents;
using System.Threading;
using static Ying.YingYing;
using Ying.YingEvents;
using Ying.YingControls;
using System.Threading.Tasks;

namespace Ying.YingWindows
{
    public enum YingLogType
    {
        YInfo, YSuccess, YWarn, YError
    }

    public partial class YingWindow : Window
    {
        /*private const string _forgeListUrl = "https://bmclapi2.bangbang93.com/forge/minecraft/";

        private string _mcVersion = "YingYing";

        class ForgeInfo
        {
            public string YMessage { get; set; }
        }

        private static List<ForgeInfo> VersionForges = new List<ForgeInfo>();
        */



        private List<String> ycommands = new List<String>();

        private int yindex = 0;

        public YingWindow()
        {
            InitializeComponent();

            getYCommand("help").setYExecutor(new YingHelp()).setYDescribe("A Ying provided command.");
            getYCommand("list").setYExecutor(new YingList()).setYDescribe("A Ying provided command.");
            getYCommand("say").setYExecutor(new YingSay()).setYDescribe("A Ying provided command.").setYUsage("/say <ymessage...>");
            getYCommand("stop").setYExecutor(new YingStop()).setYDescribe("A Ying provided command.");
            getYCommand("restart").setYExecutor(new YingRestart()).setYDescribe("A Ying provided command.");
            getYCommand("ytools").setYExecutor(new YingTools()).setYDescribe("A Ying provided command.");
            getYCommand("tell").setYExecutor(new YingTell()).setYDescribe("A Ying provided command.").setYUsage("/tell <player> <private message...>");
            getYCommand("Ying").setYExecutor(new YingCommands.Ying()).setYDescribe("A Ying provided command.");
            ///tell <player> <private message...>
            
            

            ycommand.KeyDown += (y, yy) =>
            {
                switch (yy.Key)
                {
                    case Key.Enter:
                        this.YRun(null, null);
                        break;
                    case Key.Up:
                        ycommand.Text = ycommands[--yindex];
                        break;
                    case Key.Down:
                        ycommand.Text = ycommands[++yindex];
                        break;
                }
            };


            yloglist.Items.Add(new YingConsoleMessageItem().setYContext(null, new YingLoadingControl()));

            

            this.sendYMessage("§b颖颖颖颖颖颖颖颖颖颖颖颖颖§a颖颖颖");
            this.sendYMessage("Ying Console Init Success");

            this.MouseLeftButtonDown += (ysender, yevent) => DragMove();

        }



        private void YRun(object sender, RoutedEventArgs e)
        {
            ycommands.Add(ycommand.Text);
            this.yindex = ycommands.Count;
            getYCommandManager().runYCommandAsync(ycommand.Text);
            ycommand.Text = "";
        }

        /*
            代码	颜色名称	技术性名称	        前景色	            背景色
                                                R	G	B	Hex	    R	G	B	Hex
            §0	    黑色	    black	        0	0	0	000000	0	0	0	000000
            §1	    深蓝色	    dark_blue	    0	0	170	0000AA	0	0	42	00002A
            §2	    深绿色	    dark_green	    0	170	0	00AA00	0	42	0	002A00
            §3	    湖蓝色	    dark_aqua	    0	170	170	00AAAA	0	42	42	002A2A
            §4	    深红色	    dark_red	    170	0	0	AA0000	42	0	0	2A0000
            §5	    紫色	    dark_purple	    170	0	170	AA00AA	42	0	42	2A002A
            §6	    金色	    gold	        255	170	0	FFAA00	42	42	0	2A2A00
            §7	    灰色	    gray	        170	170	170	AAAAAA	42	42	42	2A2A2A
            §8	    深灰色	    dark_gray	    85	85	85	555555	21	21	21	151515
            §9	    蓝色	    blue	        85	85	255	5555FF	21	21	63	15153F
            §a	    绿色	    green	        85	255	85	55FF55	21	63	21	153F15
            §b	    天蓝色	    aqua	        85	255	255	55FFFF	21	63	63	153F3F
            §c	    红色	    red	            255	85	85	FF5555	63	21	21	3F1515
            §d	    粉红色	    light_purple	255	85	255	FF55FF	63	21	63	3F153F
            §e	    黄色	    yellow	        255	255	85	FFFF55	63	63	21	3F3F15
            §f	    白色	    white	        255	255	255	FFFFFF	63	63	63	3F3F3F
        */

        private Func<ListView, YingLogType, String, long, DateTime, int> ysendMessage = (ylist, ytype, ymessage, yspend, ytime) =>
        {
            
            if (ymessage == null) ymessage = "";

            YingConsoleMessageItem yitem = new YingConsoleMessageItem();

            TextBlock y = yitem.ymessage;

            String[] yymessage = ymessage.Split('§');
            if (yymessage.Length > 1)
            {
                y.Inlines.Add(new Run("Ying > "));

                foreach (String yy in yymessage)
                {

                    if (String.IsNullOrWhiteSpace(yy)) continue;
                    Run yrun = new Run(yy.Substring(1));
                    Color ycolor = Colors.White; //前景色
                    Color yycolor = Colors.Transparent; //背景色
                    //SolidColorBrush ybrush = new SolidColorBrush(Colors.White);
                    switch (yy.Substring(0, 1))
                    {
                        case "0": //§0 黑色 black
                            ycolor = (Color)ColorConverter.ConvertFromString("#000000");
                            yycolor = (Color)ColorConverter.ConvertFromString("#000000");
                            break;
                        case "1": //§1深蓝色 dark_blue
                            ycolor = (Color)ColorConverter.ConvertFromString("#0000AA");
                            yycolor = (Color)ColorConverter.ConvertFromString("#00002A");
                            break;
                        case "2": //§2 深绿色 dark_green
                            ycolor = (Color)ColorConverter.ConvertFromString("#00AA00");
                            yycolor = (Color)ColorConverter.ConvertFromString("#002A00");
                            break;
                        case "3": //§3 湖蓝色 dark_aqua
                            ycolor = (Color)ColorConverter.ConvertFromString("#00AAAA");
                            yycolor = (Color)ColorConverter.ConvertFromString("#002A2A");
                            break;
                        case "4"://§4 深红色 dark_red
                            ycolor = (Color)ColorConverter.ConvertFromString("#AA0000");
                            yycolor = (Color)ColorConverter.ConvertFromString("#2A0000");
                            break;
                        case "5": //§5 紫色 dark_purple
                            ycolor = (Color)ColorConverter.ConvertFromString("#AA00AA");
                            yycolor = (Color)ColorConverter.ConvertFromString("#2A002A");
                            break;
                        case "6": //§6 金色 gold
                            ycolor = (Color)ColorConverter.ConvertFromString("#FFAA00");
                            yycolor = (Color)ColorConverter.ConvertFromString("#2A2A00");
                            break;
                        case "7": //§7 灰色 gray
                            ycolor = (Color)ColorConverter.ConvertFromString("#AAAAAA");
                            yycolor = (Color)ColorConverter.ConvertFromString("#2A2A2A");
                            break;
                        case "8": //§8 深灰色 dark_gray
                            ycolor = (Color)ColorConverter.ConvertFromString("#555555");
                            yycolor = (Color)ColorConverter.ConvertFromString("#151515");
                            break;
                        case "9": //§9 蓝色 blue
                            ycolor = (Color)ColorConverter.ConvertFromString("#5555FF");
                            yycolor = (Color)ColorConverter.ConvertFromString("#15153F");
                            break;
                        case "a": //§a 绿色 green
                            ycolor = (Color)ColorConverter.ConvertFromString("#55FF55");
                            yycolor = (Color)ColorConverter.ConvertFromString("#153F15");
                            break;
                        case "b": //§b 天蓝色 aqua
                            ycolor = (Color)ColorConverter.ConvertFromString("#55FFFF");
                            yycolor = (Color)ColorConverter.ConvertFromString("#153F3F");
                            break;
                        case "c": //§c 红色 red
                            ycolor = (Color)ColorConverter.ConvertFromString("#FF5555");
                            yycolor = (Color)ColorConverter.ConvertFromString("#3F1515");
                            break;
                        case "d": //§d 粉红色 light_purple
                            ycolor = (Color)ColorConverter.ConvertFromString("#FF55FF");
                            yycolor = (Color)ColorConverter.ConvertFromString("#3F153F");
                            break;
                        case "e": //§e 黄色 yellow
                            ycolor = (Color)ColorConverter.ConvertFromString("#FFFF55");
                            yycolor = (Color)ColorConverter.ConvertFromString("#3F3F15");
                            break;
                        case "f": //§f 白色 white
                            ycolor = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                            yycolor = (Color)ColorConverter.ConvertFromString("#3F3F3F");
                            break;
                        default:
                            yrun.Text = yy;
                            break;
                    }

                    yrun.Foreground = new SolidColorBrush(ycolor);

                    //yrun.Background = Brushes.Red;
                    y.Inlines.Add(yrun);
                }


            }
            else
            {
                y.Text = "Ying > " + ymessage;
                y.Foreground = new SolidColorBrush(Colors.White);
            }

            switch (ytype)
            {
                case YingLogType.YInfo:
                    yitem.ystatus.Foreground = new SolidColorBrush(Colors.Aqua);
                    break;
                case YingLogType.YSuccess:
                    yitem.ystatus.Foreground = new SolidColorBrush(Colors.LightGreen);
                    break;
                case YingLogType.YWarn:
                    yitem.ystatus.Foreground = new SolidColorBrush(Colors.Yellow);
                    break;
                case YingLogType.YError:
                    yitem.ystatus.Foreground = new SolidColorBrush(Colors.Red);
                    break;
            }

            yitem.ToolTip = $"操作耗时: {yspend}ms" + Environment.NewLine + $"时间: {ytime.Date.ToLocalTime()}";
            yitem.FontFamily = new FontFamily("yfzxkjt");

            yitem.Margin = new Thickness
            {
                Left = -5.64
            };

            return ylist.Items.Add(yitem);
        };

        private Action<ListView, int, YingLogType, String, long, DateTime> ysetMessage = (ylist, yline, ytype, ymessage, yspend, ytime) =>
        {
            YingConsoleMessageItem yitem = ylist.Items.GetItemAt(yline) as YingConsoleMessageItem;
            TextBlock y = yitem.ymessage;
            //y.Text = "·Ying > " + ymessage;
            y.ToolTip = $"操作耗时: {yspend}ms" + Environment.NewLine + $"时间: {ytime.Date.ToLocalTime()}";

            y.Foreground = new SolidColorBrush(Colors.White);
            if (yspend > 100) yitem.ystatus.Foreground = new SolidColorBrush(Colors.Red);
        };

        public int sendYMessage(String ymessage = "", YingLogType ytype = YingLogType.YInfo, long yspend = 0, DateTime ytime = new DateTime())
        {
            return (int)this.Dispatcher.Invoke(this.ysendMessage, yloglist, ytype, ymessage, yspend, ytime);
            /*//
            YingLogEvent yevent = new YingLogEvent(ymessage);
            getYEvent().Publish<YingLogEvent>(yevent);
            return yevent.YLine;*/

            //ytime = DateTime.N ow;

        }

        public YingWindow setYMessage(int yline, String ymessage = "", YingLogType ytype = YingLogType.YInfo, long yspend = 0, DateTime ytime = new DateTime())
        {
            this.Dispatcher.Invoke(this.ysetMessage, yloglist, yline, ytype, ymessage, yspend, ytime);
            return this;
        }

    }
}
