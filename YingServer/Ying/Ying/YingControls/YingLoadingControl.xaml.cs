using MahApps.Metro;
using System.Windows.Controls;
using static Ying.YingYing;
using System;
using System.Collections.Generic;
using System.IO;
using Ying.YingUtil;
using System.Threading.Tasks;

namespace Ying.YingControls
{
    /// <summary>
    /// YingLoadingControl.xaml 的交互逻辑
    /// </summary>
    public partial class YingLoadingControl : UserControl
    {

        private YingGifImage yloadingimage = new YingGifImage(getYFiles().getYResource("Ying.yresources.yimages.yloading.gif"));
        private Dictionary<String, String> yfiles { get; } = new Dictionary<String, String>();


        private String yyinfo { set => this.Dispatcher.Invoke(() => this.yinfo.Text = value); }
        private String yydescription { set => this.Dispatcher.Invoke(() => this.ydescription.Text = value); }
        private int yyprogress { set => this.Dispatcher.Invoke(() => yprogress.Value = value); }
        private int yyprogressmax { set => this.Dispatcher.Invoke(() => yprogress.Maximum = value); }
        private Boolean yfinally { set => this.Dispatcher.Invoke(() => {
            this.ycontent.Children.Remove(yloadingimage);
            this.ysuccess.Visibility = System.Windows.Visibility.Visible;
            this.yinfo.Text = "加载完成了呢~";
            this.ydescription.Text = "";

            getYServer().Ying();
        }); }

        public YingLoadingControl()
        {
            InitializeComponent();

            this.yloadingimage.Width = 132.64;
            this.yloadingimage.Height = 132.64;
            this.yloadingimage.Margin = new System.Windows.Thickness(0, -24.64, 0, 0);
            this.ycontent.Children.Add(yloadingimage);

            ThemeManager.ChangeAppStyle(YingApp.Current,
                                        ThemeManager.GetAccent("Blue"),
                                        ThemeManager.GetAppTheme("BaseLight"));


            this.Loaded += (ysender, yevent) =>
            {
                this.yloadingimage.StartAnimate();

                yinfo.Text = "颖: 准备检查更新";
                yprogress.Value = 0;
                yprogress.Maximum = 20020604;

                new Task(() => {
                    List<String> yfiles = new List<String>();

                    //遍历文件夹

                    DirectoryInfo yfolder = getYFiles().getYResources().ygame;

                    FileInfo[] yfileInfo = yfolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);

                    foreach (FileInfo yfile in yfileInfo) //遍历文件
                    {
                        yfiles.Add(yfile.FullName);
                        this.yyinfo = "颖: 当前已找到文件总数为 " + Convert.ToString(yfiles.Count);
                        //;
                    }

                    //遍历子文件夹

                    DirectoryInfo[] ydirInfo = yfolder.GetDirectories();

                    foreach (DirectoryInfo ydir in ydirInfo)
                    {

                        //yfiles.Add(NextFolder.ToString());

                        FileInfo[] fileInfo = ydir.GetFiles("*.*", SearchOption.AllDirectories);

                        foreach (FileInfo NextFile in fileInfo)  //遍历文件
                        {
                            yfiles.Add(NextFile.FullName);
                            this.yyinfo = "颖: 当前已找到文件总数为 " + Convert.ToString(yfiles.Count);
                        }

                    }

                    this.yyprogressmax = yfiles.Count;

                    //Ying Todo
                    /*for (int y = 0; y < yfiles.Count; y++)
                    {
                        this.yfiles.Add(yfiles[y].Replace(Environment.CurrentDirectory + "\\Ying", ""), YingSHA1.Ying(yfiles[y]));
                        this.yyinfo = "颖: 正在分析文件 " + Convert.ToString(y) + "/" + Convert.ToString(yfiles.Count - 1);
                        this.yydescription = "颖: " + yfiles[y].Replace(Environment.CurrentDirectory, "");
                        this.yyprogress = y;
                    }*/

                    this.yfinally = true;

                }).Start();
            };
        }

        public void Ying()
        {

        }


        
    }
}
