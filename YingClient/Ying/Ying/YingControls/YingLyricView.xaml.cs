using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using Ying.YingUtil;
using System.Text;
using Ying.YingWindows;
using System.Threading.Tasks;

namespace Ying.YingControls
{
    /// <summary>
    /// YingLyricView.xaml 的交互逻辑
    /// </summary>
    public partial class YingLyricView : UserControl
    {

        /// <summary>
        /// 歌曲
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 艺术家
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// 专辑
        /// </summary>
        public string Album { get; set; }
        /// <summary>
        /// 歌词作者
        /// </summary>
        public string LrcBy { get; set; }
        /// <summary>
        /// 偏移量
        /// </summary>
        public string Offset { get; set; }

        /// <summary>
        /// 歌词
        /// </summary>
        public Dictionary<double, string> y { get; set; }

        /*
         * 2018年1月12日09:12:47修改思路
         * 将歌词模型的time数据类型改为int，以总秒数为单位，方便判断
         * 
         * 
         * 2018年1月12日09:39:01
         * 歌词定位错误，速度快了一些，精度不高，改用double，以总毫秒为计算单位
         * */
        #region 歌词模型
        public class LrcModel
        {
            /// <summary>
            /// 歌词所在控件
            /// </summary>
            public TextBlock c_LrcTb { get; set; }

            /// <summary>
            /// 歌词字符串
            /// </summary>
            public string LrcText { get; set; }

            /// <summary>
            /// 时间
            /// </summary>
            public double Time { get; set; }
        }
        #endregion
        #region 变量
        //歌词集合
        public Dictionary<double, LrcModel> Lrcs = new Dictionary<double, LrcModel>();

        //添加当前焦点歌词变量
        public LrcModel foucslrc { get; set; }


        //非焦点歌词颜色
        public SolidColorBrush NoramlLrcColor = new SolidColorBrush(Colors.LightGray);
        //焦点歌词颜色
        public SolidColorBrush FoucsLrcColor = new SolidColorBrush(Colors.White);


        #endregion
        public YingLyricView()
        {
            InitializeComponent();           
        }

        #region 加载歌词
        public void yload(string ypath)
        {
            Regex regex = new Regex(@"(?<ytime>\[[0-9.:\]\[\s]*\])(?<value>.*)", RegexOptions.Compiled);
            Regex timeRegex = new Regex(@"\[(?<ytime>[0-9.:]*)\]\s*", RegexOptions.Compiled);

            y = new Dictionary<double, string>();
            //var tempDic = new Dictionary<double, string>();
            //FileStream ystream = new FileStream(ypath, FileMode.Open, FileAccess.Read, FileShare.Read);

            string line;
            StreamReader yreader = new YingDownload(ypath, Encoding.Default).YReader;//new StreamReader(ystream, YingEncodingType.YGetType(ypath));
            if (yreader == null) { if (y.Count == 0) y.Add(00.20020604, "Ying · 无法找到歌词"); }
            else
            {
                while ((line = yreader.ReadLine()) != null)
                {
                    if (line.StartsWith("[ti:"))
                    {
                        Title = SplitInfo(line);
                    }
                    else if (line.StartsWith("[ar:"))
                    {
                        Artist = SplitInfo(line);
                    }
                    else if (line.StartsWith("[al:"))
                    {
                        Album = SplitInfo(line);
                    }
                    else if (line.StartsWith("[by:"))
                    {
                        LrcBy = SplitInfo(line);
                    }
                    else if (line.StartsWith("[offset:"))
                    {
                        Offset = SplitInfo(line);
                    }
                    else
                    {
                        try
                        {
                            Match ymatch = regex.Match(line);//分割时间和内容
                            string yword = ymatch.Groups["value"].Value;
                            MatchCollection timeMatch = timeRegex.Matches(ymatch.Groups["ytime"].Value);//分割多个时间
                            foreach (var i in timeMatch)
                            {
                                if (yword == "") continue;
                                double time = TimeSpan.Parse("00:" + (i as Match).Groups["ytime"].Value).TotalSeconds;
                                if (y.ContainsKey(time))//如果是双文歌词，两个歌词时间相同
                                {
                                    y[time] += Environment.NewLine + yword;//将原来的歌词下面加一行新的歌词
                                }
                                else//第一次出现这个时间的歌词
                                {
                                    y.Add(time, yword);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            y = y.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);//将歌词排序

            //循环以换行\n切割出歌词
            foreach (KeyValuePair<double, string> yy in y)
            {

                //歌词时间
                TimeSpan ytime = TimeSpan.FromSeconds(yy.Key);
                //Console.WriteLine("Ying: " + ytime.TotalMilliseconds + "|" + yy.Key);
                //歌词取]后面的就行了
                //string lrc = str.Split(']')[1];

                //歌词显示textblock控件
                TextBlock ylyric = new TextBlock();
                ylyric.TextAlignment = TextAlignment.Center;
                ylyric.MaxWidth = 180.64;
                ylyric.TextWrapping = TextWrapping.Wrap;
                ylyric.Foreground = NoramlLrcColor;
                //赋值
                ylyric.Text = yy.Value;
                if (ylyrics.Children.Count > 0)
                {
                    //增加一些行间距，see起来不那么拥挤~
                    ylyric.Margin = new Thickness(0, 3.604, 0, 0);
                }
                //ylyric.TextEffects = new TextEffectCollection()




                //添加到集合，方便日后操作
                Lrcs.Add(ytime.TotalMilliseconds, new LrcModel()
                {
                    c_LrcTb = ylyric,
                    LrcText = yy.Value,
                    Time = ytime.TotalMilliseconds
                });

                //将歌词显示textblock控件添加到界面中显示
                ylyrics.Children.Add(ylyric);

            }

        }

        /// <summary>
        /// 处理信息
        /// </summary>
        /// <param name="line"></param>
        /// <returns>返回基础信息</returns>
        static string SplitInfo(string line)
        {
            return line.Substring(line.IndexOf(":") + 1).TrimEnd(']');
        }

        //正则表达式提取时间

        public TimeSpan GetTime(string str)
        {
            Regex reg = new Regex(@"\[(?<ytime>.*)\]", RegexOptions.IgnoreCase);
            string timestr = reg.Match(str).Groups["ytime"].Value;

            //获得分
            int m = Convert.ToInt32(timestr.Split(':')[0]);
            //判断是否有小数点
            int s = 0, f = 0;
            if (timestr.Split(':')[1].IndexOf(".") != -1)
            {
                //有
                s = Convert.ToInt32(timestr.Split(':')[1].Split('.')[0]);
                //获得毫秒位
                f = Convert.ToInt32(timestr.Split(':')[1].Split('.')[1]);

            }
            else
            {
                //没有
                s = Convert.ToInt32(timestr.Split(':')[1]);

            }
            Debug.WriteLine(m + "-" + s + "-" + f + "->" + new TimeSpan(0, 0, m, s, f).TotalMilliseconds);
            return new TimeSpan(0, 0, m, s, f);

        }

        #endregion

        #region 歌词滚动
        /// <summary>
        /// 歌词滚动、定位焦点
        /// </summary>
        /// <param name="nowtime"></param>
        public void LrcRoll(double nowtime)
        {
            if (foucslrc == null)
            {
                foucslrc = Lrcs.Values.First();
                foucslrc.c_LrcTb.Foreground = FoucsLrcColor;
            }
            else
            {


                //查找焦点歌词
                IEnumerable<KeyValuePair<double, LrcModel>> s = Lrcs.Where(m => nowtime >= m.Key);
                if (s.Count() > 0)
                {
                    LrcModel lm = s.Last().Value;
                    foucslrc.c_LrcTb.Foreground = NoramlLrcColor;

                    foucslrc = lm;
                    foucslrc.c_LrcTb.Foreground = FoucsLrcColor;
                    //定位歌词在控件中间区域
                    ResetLrcviewScroll();
                }
            }

            //if (FoucsLrcLocation < 0)
            //{
            //    //音乐开始时歌词焦点到第一句
            //    FoucsLrcLocation = 0;
            //    Lrcs.Values.ToList()[FoucsLrcLocation].c_LrcTb.Foreground = FoucsLrcColor;
            //}
            //else
            //{
            //    //循环获取歌词
            //    for (int i = FoucsLrcLocation + 1; i < Lrcs.Values.Count; i++)
            //    {
            //        LrcModel lrc = Lrcs.Values.ToList()[i];



            //        //计算当前音乐播放时间与歌词时间的差值

            //        if ()
            //        {
            //            //取消当前焦点歌词
            //            Lrcs.Values.ToList()[FoucsLrcLocation].c_LrcTb.Foreground = NoramlLrcColor;
            //            //给歌词控件设置颜色突出显示
            //            lrc.c_LrcTb.Foreground = FoucsLrcColor;
            //            //重新设置当前歌词位置
            //            FoucsLrcLocation = i;
            //            ResetLrcviewScroll();
            //            //Debug.WriteLine("nowtime:" + nowtime + ",lrctime:" + lrctime + ",s:" + s);
            //            break;
            //        }

            //    }
            //}


        }



        #endregion


        #region 调整歌词控件滚动条位置
        public void ResetLrcviewScroll()
        {
            //获得焦点歌词位置
            GeneralTransform ytransform = foucslrc.c_LrcTb.TransformToVisual(ylyrics);
            Point ypoint = ytransform.Transform(new Point(0, 0));


            //计算滚动位置（p.Y是焦点歌词控件(c_LrcTb)相对于父级控件c_lrc_items(StackPanel)的位置）
            //拿焦点歌词位置减去滚动区域控件高度除以2的值得到的【大概】就是歌词焦点在滚动区域控件的位置
            double yposition = ypoint.Y - (yviewer.ActualHeight / 2) + 10;
            //滚动

            yviewer.ScrollToVerticalOffset(yposition);

        }
        #endregion
    }
}
