using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YingResolver
{
    public class YingQQMusicResolver
    {
        //http://music.163.com/song/media/outer/url?id=317151.mp3
        public String YUrl { get; }

        /// <summary>
        /// Ying QQ音乐的歌曲真实地址解析
        /// </summary>
        /// <param name="yurl">Ying Like: https://y.qq.com/n/yqq/song/004QxGK535eBMZ.html</param>
        public YingQQMusicResolver(String yurl)
        {
            try
            {
                Match yid = Regex.Matches(yurl, @"/(\w+).html$")[0];
                String ysongmid = yid.Value;
                if (ysongmid.StartsWith("/")) ysongmid = ysongmid.Substring(1);
                if (ysongmid.EndsWith(".html")) ysongmid = ysongmid.Substring(0, ysongmid.Length - 5);
                String yfilename = $"C400{ysongmid}.m4a";
                double yguid = Math.Round(a: new Random().Next() * 2147483647) * (DateTime.UtcNow.Millisecond) % 1e10;
                Dictionary<String, String> yparams = new Dictionary<String, String>();
                yparams.Add("format", "json");
                yparams.Add("cid", "205361747");
                yparams.Add("uin", "0");
                yparams.Add("songmid", ysongmid);
                yparams.Add("filename", yfilename);
                yparams.Add("guid", Convert.ToString(yguid));

                HttpHelper yhttp = new HttpHelper();
                HttpItem yitem = new HttpItem()
                {
                    URL = this.ybuilder("https://c.y.qq.com/base/fcgi-bin/fcg_music_express_mobile3.fcg", yparams),    //URL这里都是测试     必需项
                    Method = "get",                                                            //URL     可选项 默认为Get
                };
                HttpResult yresult = yhttp.GetHtml(yitem);
                String yvkey = ((JObject)JsonConvert.DeserializeObject(yresult.Html))["data"]["items"][0]["vkey"].ToString();
                this.YUrl = String.Format("http://dl.stream.qqmusic.qq.com/{0}?vkey={1}&guid={2}&uin=0&fromtag=66", yfilename, yvkey, yguid);

            }
            catch
            {

            }       
        }

        private String ybuilder(String ybase, Dictionary<String, String> yparams)
        {
            StringBuilder ybuilder = new StringBuilder($"{ybase}?");
            foreach(KeyValuePair<String, String> y in yparams)
            {
                ybuilder.Append($"&{y.Key}={y.Value}");
            }
            return ybuilder.ToString().Replace($"{ybase}?&", $"{ybase}?");
        }
    }
}
