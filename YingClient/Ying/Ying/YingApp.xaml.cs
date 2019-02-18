using System.Windows;
using System.Windows.Media;
using System.Threading;
using KMCCC.Launcher;
using System.IO;
using Ying.Modules;
using System.Linq;
using System.Collections.ObjectModel;
using static Ying.YingYing;
using Ying.YingUtils;
using Ying.YingWebsocket;

namespace Ying
{
    /// <summary>
    /// YingApp.xaml 的交互逻辑
    /// </summary>
    public partial class YingApp : Application
    {
        public static LauncherCore YCore { get; private set; }

        private static Mutex ymutex;
        private static StreamWriter ylogger;

        protected override void OnStartup(StartupEventArgs e)
        {

            ymutex = new Mutex(true, "Ying Client", out bool ret);
            if (!ret)
            {
                MessageBox.Show("已经有一个我在运行了", "(>ㅂ< )", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Shutdown(0);
            }

            if(!File.Exists("bass.dll"))
            {
                YingTools.YExtractResFile("Ying.yresources.yplugins.bass.dll", "bass.dll");
                File.SetAttributes("bass.dll", FileAttributes.Hidden); //设置为隐藏文件
            }

            YingConfig.Load();
            YingConfig.YArgs.Versions = new ObservableCollection<Version>();

            InitializeLauncherCore();
            InitializeThemeColor();

            //Dispatcher.UnhandledException += UnhandledExceptionHandler;

            base.OnStartup(e);
        }

        private void InitializeLauncherCore()
        {
            YCore = LauncherCore.Create();
            YCore.GameExit += OnYGameExit;
            YCore.GameLog += OnYGameLog;

            LoadVersions();

            var logPath = YCore.GameRootPath + @"\logs\";
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            ylogger = new StreamWriter(new FileStream(logPath + "mcrun.log", FileMode.Create));
        }

        private void InitializeThemeColor()
        {
            Color ThemeColor;

            if (YingConfig.YArgs.IsUseSystemThemeColor || string.IsNullOrEmpty(YingConfig.YArgs.ThemeColor))
            {
                ThemeColor = SystemParameters.WindowGlassColor;
                ThemeColor.A = 150;
            }
            else
            {
                ThemeColor = (Color)ColorConverter.ConvertFromString(YingConfig.YArgs.ThemeColor);
            }
            Resources["ThemeColor"] = ThemeColor;
            UpdateThemeColorBrush(ThemeColor);
        }

        public static void LoadVersions()
        {
            YingConfig.YArgs.Versions.Clear();
            foreach (var ver in YCore.GetVersions())
            {
                YingConfig.YArgs.Versions.Add(ver);
            }

            if (!YingConfig.YArgs.Versions.Any())
            {
                YingConfig.YArgs.VersionIndex = -1;
            }
            else if (YingConfig.YArgs.VersionIndex == -1 || YingConfig.YArgs.VersionIndex >= YingConfig.YArgs.Versions.Count)
            {
                YingConfig.YArgs.VersionIndex = 0;
            }
        }

        public static void UpdateThemeColorBrush(Color _col)
        {
            float Gray = _col.R * 0.299f + _col.G * 0.577f + _col.B * 0.124f;

            if (Gray < 10.0f)
            {
                _col = Color.FromRgb(90, 90, 90);
            }
            else if (Gray < 100.0f)
            {
                _col = Color.Multiply(_col, 23.0f - Gray * 0.22f);
            }
            else if (Gray > 175.0f)
            {
                _col = Color.Multiply(_col, 2.75f - Gray * 0.01f);
            }

            _col.A = 255;
            Current.Resources["ThemeColorBrush"] = new SolidColorBrush(_col);
        }

        private System.Action<LaunchHandle, string> OnYGameLog = (yhandle, yline) =>
        {
            ylogger.WriteLine(yline);

            System.Console.WriteLine(yline);

            getYConsole().sendYMessage(yline);
        };

        private System.Action<LaunchHandle, int> OnYGameExit = (yhandle, ycode) =>
        {

            if (ycode != 0)
            {
                if (MessageBox.Show($"Minecraft异常退出了,Exit Code: {ycode}\n是否查看log文件？", "（/TДT)/",
                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(YCore.GameRootPath + @"\logs\mcrun.log");
                }
            }

            Current.Dispatcher.Invoke(() =>
            {
                if (YingConfig.YArgs.AfterLaunchBehavior == 0)
                {
                    Current.Shutdown();
                }
            });
        };

        protected override void OnExit(ExitEventArgs e)
        {
            YingConfig.YArgs.ThemeColor = Resources["ThemeColor"].ToString();
            YingConfig.Save();
            base.OnExit(e);
        }

        void UnhandledExceptionHandler(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"异常信息：{e.Exception.Message}\n异常源：{e.Exception.StackTrace}", "程序发生了无法处理的异常！", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(-1);
#if !Debug
            e.Handled = true;
#endif
        }

    }
}
