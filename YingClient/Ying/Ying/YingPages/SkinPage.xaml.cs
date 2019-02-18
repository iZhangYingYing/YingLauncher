﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Ying.Modules;
using Ying;

namespace Ying.Pages
{
    public partial class SkinPage : Page
    {
        private static Color tempColor;

        public SkinPage()
        {
            InitializeComponent();
            this.DataContext = YingConfig.YArgs;
            PresetColorList.ItemsSource = PresetColors;

            tempColor = (Color)Application.Current.Resources["ThemeColor"];

            _colorASlider.Value = tempColor.A;
            _colorRSlider.Value = tempColor.R;
            _colorGSlider.Value = tempColor.G;
            _colorBSlider.Value = tempColor.B;

            _colorASlider.ValueChanged += (s, e) =>
            {
                if (e.NewValue == 0) return;
                tempColor.A = (byte)_colorASlider.Value; Update_ThemeColor();
            };
            _colorRSlider.ValueChanged += (s, e) => { tempColor.R = (byte)_colorRSlider.Value; Update_ThemeColor(); };
            _colorGSlider.ValueChanged += (s, e) => { tempColor.G = (byte)_colorGSlider.Value; Update_ThemeColor(); };
            _colorBSlider.ValueChanged += (s, e) => { tempColor.B = (byte)_colorBSlider.Value; Update_ThemeColor(); };

            PresetColorList.SelectionChanged += (s, e) =>
             {
                 var SelectedColor = PresetColorList.SelectedItem as MyColor;
                 tempColor = (Color)ColorConverter.ConvertFromString(SelectedColor.Color);
                 _colorASlider.Value = tempColor.A;
                 _colorRSlider.Value = tempColor.R;
                 _colorGSlider.Value = tempColor.G;
                 _colorBSlider.Value = tempColor.B;
                 YingApp.UpdateThemeColorBrush(tempColor);
             };

            _backButton.Click += (s, e) => NavigationService.GoBack();
        }

        private static void Update_ThemeColor()
        {
            Application.Current.Resources["ThemeColor"] = tempColor;
        }

        private void Slider_Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            YingApp.UpdateThemeColorBrush(tempColor);
        }

        private void GetImageFromDisk(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Title = "请选择图片文件",
                Filter = "图像文件| *.png; *.jpg; *.bmp; *.gif",
            };

            if (dialog.ShowDialog() ?? false)
            {
                YingConfig.YArgs.ImageFilePath = dialog.FileName;
            }
        }

        private class MyColor
        {
            public string Name { get; set; }
            public string Color { get; set; }
        }

        private static MyColor[] PresetColors = new MyColor[]
        {
            new MyColor{Name = "土豪金", Color = "#7DEBAF5A"},
            new MyColor{Name = "滑稽黄", Color = "#96F1C40F"},
            new MyColor{Name = "橙子橙", Color = "#96FF7722"},
            new MyColor{Name = "春节红", Color = "#9DE0392B"},
            new MyColor{Name = "桃花粉", Color = "#9DFF96AF"},
            new MyColor{Name = "蟑螂红", Color = "#C8350C14"},
            new MyColor{Name = "鲜草绿", Color =" #582ECC71"},
            new MyColor{Name = "老草绿", Color = "#96649105"},
            new MyColor{Name = "吃土棕", Color = "#D2563C18"},
            new MyColor{Name = "竹林绿", Color = "#AF00320F"},
            new MyColor{Name = "泰瑞绿", Color = "#961ABC9C"},
            new MyColor{Name = "上天蓝", Color = "#8C3498DB"},
            new MyColor{Name = "墨水蓝", Color = "#B400284B"},
            new MyColor{Name = "闪电蓝", Color = "#284BC8EB"},
            new MyColor{Name = "茄子紫", Color = "#B06E1E8C"},
            new MyColor{Name = "雾霾灰", Color = "#4D7F8C8D"},
            new MyColor{Name = "高冷黑", Color = "#B41E2321"},
        };

    }
}
