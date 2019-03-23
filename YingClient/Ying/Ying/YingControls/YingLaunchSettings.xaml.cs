using System.Windows;
using System.Windows.Controls;
using KMCCC.Tools;
using Ying.Modules;
using System.Linq;
using Ying;
using System;

namespace Ying.Controls
{
    public partial class YingLaunchSettings : Grid
    {
        public YingLaunchSettings()
        {
            InitializeComponent();
        }

        private void LaunchSettings_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = YingConfig.YArgs;
            //_passWordBox.Password = YingConfig.YArgs.YAccount..ypassword;
        }

        private void ShowVersionOptions(object sender, RoutedEventArgs e)
        {
            VersionOptionsMenu.PlacementTarget = (sender as Button);
            VersionOptionsMenu.IsOpen = true;
        }

        private void RefreshVersion(object sender, RoutedEventArgs e)
        {
            YingApp.LoadVersions();
        }

        private void OpenVersionFolder(object sender, RoutedEventArgs e)
        {
            string DirPath = $"{YingApp.YCore.GameRootPath}\\versions\\{YingConfig.YArgs.SelectedVersion.Id}\\";
            System.Diagnostics.Process.Start("explorer.exe", DirPath);
        }

        private void OpenVersionJson(object sender, RoutedEventArgs e)
        {
            string JsonPath = $"{YingApp.YCore.GameRootPath}\\versions\\{YingConfig.YArgs.SelectedVersion.Id}\\{YingConfig.YArgs.SelectedVersion.Id}.json";
            try
            {
                System.Diagnostics.Process.Start(JsonPath);
            }
            catch { }
        }

        private void DeleteVersion(object sender, RoutedEventArgs e)
        {
            string DirPath = $"{YingApp.YCore.GameRootPath}\\versions\\{YingConfig.YArgs.SelectedVersion.Id}\\";
            UsefulTools.DeleteDirectoryAsync(DirPath);

            if (YingConfig.YArgs.SelectedVersion.Id.Contains("forge"))
            {
                var forgeDir = $"{YingApp.YCore.GameRootPath}\\libraries\\{System.IO.Path.GetDirectoryName(YingConfig.YArgs.SelectedVersion.Libraries[0].YDownloadInfo.Artifact.Path)}";
                UsefulTools.DeleteDirectoryAsync(forgeDir);
            }

            YingConfig.YArgs.Versions.RemoveAt(YingConfig.YArgs.VersionIndex);
            YingConfig.YArgs.VersionIndex = YingConfig.YArgs.Versions.Any() ? 0 : -1;          
        }

        private void Update_CurrentAvailableMemory(object sender, ToolTipEventArgs e)
        {
            (sender as TextBox).ToolTip = $"当前可用物理内存：{SystemTools.GetRunmemory()} MB";
        }

        private void GetJavaPathFromDisk(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Title = "请选择Java路径",
                Filter = "Java运行环境| javaw.exe; java.exe",
            };

            if (dialog.ShowDialog() ?? false)
            {
                YingConfig.YArgs.JavaPath = dialog.FileName;
            }
        }

        private void UpdatePassWordToConfig(object sender, RoutedEventArgs e)
        {
            //YingConfig.YArgs.YSelectedAccount.ypassword = _passWordBox.Password;
        }
    }
}
