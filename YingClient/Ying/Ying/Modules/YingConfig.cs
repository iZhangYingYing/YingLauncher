namespace Ying.Modules
{
    using System.IO;
    using LitJson;
    using System.ComponentModel;
    using KMCCC.Tools;
    using KMCCC.Launcher;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Ying;
    using Ying.YingUtil;
    using System.Threading;
    using System.Linq;
    using Ying.YingUtils;
    using Ying.YingWebsocket.YingStructs;

    /*
     "serverBaseURL": "http://yskin.iying.top:60408/api/yggdrasil/",
      "clientToken": "f42526717b7d441fbf43b740fc696ee1",
      "displayName": "Ying",
      "userProperties": {},
      "accessToken": "a0470cdabafb42a1bdf4e0eb724f781e",
      "type": "authlibInjector",
      "uuid": "1347d91971e3378fb4ed54723819d15a",
      "userid": "8f4a59eef481505c88302485b02e9642",
      "username": "1340761826@qq.com"
         */

    public class YingConfigArgs : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region 私有字段

        private string _javaPath;
        private int _versionIndex;
        private uint _maxMemory;
        //private bool _isOfflineMode;
        private bool _isUseImageBackground;
        private string _imageFilePath;
        private int _downloadSource;

        #endregion

        #region 属性访问器

        public string JavaPath
        {
            get => _javaPath;
            set
            {
                _javaPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(JavaPath)));
            }
        }

        public int VersionIndex
        {
            get => _versionIndex;
            set
            {
                _versionIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VersionIndex)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasAnyVersion)));
            }
        }

        [YingJsonIgnore]
        public ObservableCollection<Version> Versions { get; set; } = new ObservableCollection<Version>();
        [YingJsonIgnore]
        public Version SelectedVersion { get => Versions[_versionIndex]; }

        [YingJsonIgnore]
        public bool HasAnyVersion { get => (_versionIndex != -1); }

        public bool IsVersionSplit { get; set; }

        public uint MaxMemory
        {
            get => _maxMemory;
            set
            {
                if (value < 1024) value = 1024;
                if (value > SystemTools.GetRunmemory()) value = (uint)SystemTools.GetRunmemory();
                _maxMemory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMemory)));
            }
        }

        /*public bool IsOfflineMode
        {
            get => _isOfflineMode;
            set
            {
                _isOfflineMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOfflineMode)));
            }
        }*/

        /*public string UserName { get; set; }

        public string PassWord { get; set; }

        public bool IsRememberPassWord { get; set; }*/

        public ushort GameWinWidth { get; set; }

        public ushort GameWinHeight { get; set; }

        public bool IsFullScreen { get; set; }

        public string ServerAddress { get; set; }

        public bool IsLoginToServer { get; set; }

        public string AdvancedArgs { get; set; }

        public string GameWinTitle { get; set; }

        public string ThemeColor { get; set; }

        public bool IsUseSystemThemeColor { get; set; }

        public bool IsUseImageBackground
        {
            get => _isUseImageBackground;
            set
            {
                if (YingApp.Current.MainWindow != null)
                {
                    if (value)
                    {
                        YingWindow.ChangeImageBackgroundAsync(_imageFilePath);
                    }
                    else
                    {
                        YingApp.Current.MainWindow.Background = null;
                    }
                }

                _isUseImageBackground = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsUseImageBackground)));
            }
        }

        public string ImageFilePath
        {
            get => _imageFilePath;
            set
            {
                if (_isUseImageBackground)
                {
                    YingWindow.ChangeImageBackgroundAsync(value);
                }

                _imageFilePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageFilePath)));
            }
        }

        public int DownloadSource
        {
            get => _downloadSource; set { _downloadSource = value; DownloadHelper.SetDownloadSource(value); }
        }

        public int AfterLaunchBehavior { get; set; }

        public YingLoginStruct YAccount { get; set; } = new YingLoginStruct();
        public string YAuthServers { get; set; }


        #endregion
    }

    public static class YingConfig
    {
        public static YingConfigArgs YArgs { get; set; }

        public static string LauncherVersion { get; } = "6.0.4";

        private static Timer ytimer = new Timer((y) => {
            try {
                Save();
            } catch { }
        }, "YingYing", 0, 60400);

        public static void Load()
        {
            IEnumerator<string> yjavas = SystemTools.FindJava().GetEnumerator();
            yjavas.MoveNext();
            string ypath = yjavas.Current;

            if (File.Exists("Ying.json") && JsonMapper.ToObject<YingConfigArgs>(File.ReadAllText("Ying.json")) != null)
            {
                YArgs = JsonMapper.ToObject<YingConfigArgs>(File.ReadAllText("Ying.json"));
                //YArgs.PassWord = UsefulTools.DecryptString(YArgs.PassWord);
                if (!File.Exists(YArgs.JavaPath) || YArgs.JavaPath == null)
                {                 
                    YArgs.JavaPath = ypath;
                }
            }
            else
            {
                YArgs = new YingConfigArgs
                {
                    MaxMemory = 2048,
                    GameWinWidth = 854,
                    GameWinHeight = 480,
                    JavaPath = ypath,
                    DownloadSource = 1,
                    ThemeColor = "#32FF96AF",
                    IsUseImageBackground = true
                };
            }/*PM> Install-Package Fody
PM> Install-Package Costura.Fody*/


        }

        public static void Save()
        {
            /*// https://www.baidu.com
            if (YArgs.IsRememberPassWord)
            {
                YArgs.PassWord = UsefulTools.EncryptString(YArgs.PassWord);
            }
            else
            {
                YArgs.PassWord = null;
            }*/          
            File.WriteAllText("Ying.json", YingJsonFormat.Ying( JsonMapper.ToJson(YArgs) ));
        }
    }
}
