using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KMCCC.Launcher;
using KMCCC.Tools;
using Ying.Modules;
using Ying.Helpers;
using System.Windows.Input;
using Ying.YingWebsocket.YingStructs;
using KMCCC.Authentication;
using static Ying.YingYing;
using System.IO;

namespace Ying.Pages
{
    public partial class YingMainPage : Page
    {
        private static bool _isLaunching;
        private YingLoginStruct yaccount = YingConfig.YArgs.YAccount;

        public YingMainPage()
        {
            InitializeComponent();
            this.DataContext = YingConfig.YArgs;


            Loaded += (s, e) =>
            {
                if (!_isLaunching &&
                    (string.IsNullOrWhiteSpace(yaccount.yuser.yusername) || UsefulTools.IsValidEmailAddress(yaccount.yuser.yusername)))
                {
                    _titleBox.Text = KaomojiHelper.GetKaomoji();
                }
                else
                {
                    _titleBox.Text = "Hello " + yaccount.yuser.yusername;
                }
            };
        }

        private async void Launch(object sender, RoutedEventArgs e)
        {
            if (YingConfig.YArgs.JavaPath == null)
            {
                if (MessageBox.Show("好气哦，Java在哪里啊 Σ( ￣□￣||)!!\n需要给您打开下载页面吗？", "吓得我喝了杯82年的Java",
                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://www.java.com/zh_CN/download/manual.jsp");
                }
                return;
            }
            else
            {
                YingApp.YCore.JavaPath = YingConfig.YArgs.JavaPath;
            }

            YingApp.YCore.YGameLaunch += OnGameLaunch;

            var lostEssentials = DownloadHelper.GetLostEssentials(YingConfig.YArgs.SelectedVersion);
            if (lostEssentials.Any())
            {
                var downloadPage = new DownloadPage();
                NavigationService.Navigate(downloadPage);
                bool hasDownloadSucceeded = await downloadPage.StartDownloadAsync(lostEssentials, "下载依赖库");

                if (!hasDownloadSucceeded)
                {
                    if (MessageBox.Show("依赖库未全部下载成功，可能无法正常启动\n是否继续启动", "Σ( ￣□￣||)",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }

            var lostAssets = DownloadHelper.GetLostAssets(YingConfig.YArgs.SelectedVersion);

            if (lostAssets.Any() && MessageBox.Show("资源文件缺失，是否补齐", "(σﾟ∀ﾟ)σ",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var downloadPage = new DownloadPage();
                this.NavigationService?.Navigate(downloadPage);
                bool hasDownloadSucceeded = await downloadPage.StartDownloadAsync(lostAssets, "下载资源文件");

                if (!hasDownloadSucceeded)
                {
                    if (MessageBox.Show("资源文件未全部下载成功，游戏可能没有声效\n是否继续启动", "(´･ᆺ･`)",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }


            //(YingConfig.YArgs.IsOfflineMode) ?
            //(IAuthenticator)new OfflineAuthenticator(YingConfig.YArgs.UserName) : new YggdrasilLogin(YingConfig.YArgs.UserName, YingConfig.YArgs.PassWord, false),

            var Result = YingApp.YCore.Launch(new LaunchOptions
            {
                Version = YingConfig.YArgs.SelectedVersion,
                Mode = new MCLauncherMode(),
                AgentPath = $"{Path.Combine(getYFiles().getYDataFolder().FullName, "authlib-injector.y.jar")}=http://zyy.com:6040/yingyggdrasil/",
                //VersionSplit = YingConfig.YArgs.IsVersionSplit,

                Authenticator = getYAuhenticator()/*new YingAuhenticator()*/,
                MaxMemory = (int)YingConfig.YArgs.MaxMemory,

                Size = new WindowSize
                {
                    Width = YingConfig.YArgs.GameWinWidth,
                    Height = YingConfig.YArgs.GameWinHeight,
                    FullScreen = YingConfig.YArgs.IsFullScreen
                },

                //ServerAddress = YingConfig.YArgs.ServerAddress,
                VersionType = $"Ying-v{YingConfig.LauncherVersion}",

            }, x => x.AdvencedArguments.Add(YingConfig.YArgs.AdvancedArgs));

            MessageBox.Show(Result.Handle.Arguments.ToArguments());


            if (Result.Success)
            {
                _isLaunching = true;
                _launchButton.IsEnabled = false;
                _titleBox.Text = "(。-`ω´-) 启动中...";
                _launchButton.Content = "启动中";
            }
            else
            {
                MessageBox.Show(Result.ErrorMessage, Result.ErrorType.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                _launchButton.IsEnabled = true;
            }
        }

        private void GotoPage(object sender, RoutedEventArgs e)
        {
            var page = "YingPages/" + (sender as Button).Tag + ".xaml";
            NavigationService.Navigate(new Uri(page, UriKind.Relative));
        }

        private void OnGameLaunch(LaunchHandle handle)
        {
            if (!string.IsNullOrWhiteSpace(YingConfig.YArgs.GameWinTitle))
            {
                handle.SetTitle(YingConfig.YArgs.GameWinTitle);
            }

            switch (YingConfig.YArgs.AfterLaunchBehavior)
            {
                case 0:
                    Dispatcher.Invoke(() =>
                    {
                        Application.Current.MainWindow.Hide();
                    });
                    break;
                case 1:
                    Dispatcher.Invoke(() =>
                    {
                        Application.Current.Shutdown();
                    });
                    break;
                case 2:
                    Dispatcher.Invoke(() =>
                    {
                        if (string.IsNullOrWhiteSpace(yaccount.yuser.yusername))
                        {
                            _titleBox.Text = KaomojiHelper.GetKaomoji();
                        }
                        else
                        {
                            _titleBox.Text = "Hello " + yaccount.yuser.yusername;
                        }

                        _launchButton.IsEnabled = true;
                        _isLaunching = false;
                    });
                    break;
            }
        }

        private void YAvatarInfo(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void YAvatarAccountList(object sender, MouseButtonEventArgs e)
        {
            
        }


    }
}
