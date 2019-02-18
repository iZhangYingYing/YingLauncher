using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ying.YingUtil;

namespace Ying
{
    public class YingLyric
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
        /// <summary>
        /// 获得歌词信息
        /// </summary>
        /// <param name="ypath">歌词路径</param>
        /// <returns>返回歌词信息(Lrc实例)</returns>
        public YingLyric(string ypath)
        {
            Regex regex = new Regex(@"(?<time>\[[0-9.:\]\[\s]*\])(?<value>.*)", RegexOptions.Compiled);
            Regex timeRegex = new Regex(@"\[(?<time>[0-9.:]*)\]\s*", RegexOptions.Compiled);

            y = new Dictionary<double, string>();
            //var tempDic = new Dictionary<double, string>();
            //FileStream fs = new FileStream(ypath, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream fs = new FileStream(ypath, FileMode.Open, FileAccess.Read, FileShare.Read);
            string line;
            StreamReader sr = new StreamReader(fs, YingEncodingType.YGetType(ypath));

            while ((line = sr.ReadLine()) != null)
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
                        MatchCollection timeMatch = timeRegex.Matches(ymatch.Groups["time"].Value);//分割多个时间
                        foreach (var i in timeMatch)
                        {
                            if (yword == "") continue;
                            double time = TimeSpan.Parse("00:" + (i as Match).Groups["time"].Value).TotalSeconds;
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

            y = y.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);//将歌词排序
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
    }
}
