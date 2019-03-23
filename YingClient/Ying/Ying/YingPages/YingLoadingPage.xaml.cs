﻿using CsharpHttpHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Ying.Modules;
using Ying.Pages;
using Ying.YingUtil;
using static Ying.YingYing;

namespace Ying.YingPages
{
    /// <summary>
    /// YingLoadingPage.xaml 的交互逻辑
    /// </summary>
    public partial class YingLoadingPage : Page
    {
        private Frame yframe = (YingApp.Current.MainWindow as YingWindow).YFrame;

        public YingLoadingPage()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                String yauthlib = Path.Combine(getYFiles().getYDataFolder().FullName, "authlib-injector.y.jar");
                if (!File.Exists(yauthlib))
                {
                    HttpHelper yhttp = new HttpHelper();
                    HttpItem yitem = new HttpItem()
                    {
                        URL = "https://bmclapi2.bangbang93.com/mirrors/authlib-injector/artifact/latest.json",    //URL这里都是测试     必需项
                        Method = "get",                                                            //URL     可选项 默认为Get
                    };
                    String ydownload = ((JObject)JsonConvert.DeserializeObject(yhttp.GetHtml(yitem).Html))["download_url"].ToString();

                    new YingDownload(ydownload, yauthlib);
                }
            });
        }
    }
}
