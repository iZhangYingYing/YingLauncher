using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ying.Helpers;
using System.Windows.Controls;
using static Ying.YingYing;
using Ying.YingPages;
using Ying.Pages;
using Ying.Modules;

namespace Ying
{
    public partial class YingWindow : Window
    {
        public Frame YFrame { get => yframe; }

        public YingWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            _minimizeButton.Click += (s, e) => this.WindowState = WindowState.Minimized;
            _shutdownButton.Click += (s, e) => Application.Current.Shutdown();

            this.Ying();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            yframe.Navigate(new YingLoadingPage());

            

            if (Environment.OSVersion.Version.Major == 10)
            {
                Win10BlurHelper.EnableBlur(this);
            }
            else
            {
                this.BorderThickness = new Thickness(1);
                this.BorderBrush = Brushes.DarkGray;
                Win7BlurHelper.EnableAeroGlass(this);
            }
        }

        public static async void ChangeImageBackgroundAsync(string imageFilePath)
        {
            if (!File.Exists(imageFilePath))
            {
                if (!Directory.Exists("bg\\")) return;

                string[] imageFiles = Directory.EnumerateFiles("bg\\")
                .Where(file => file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".bmp")).ToArray();

                if (imageFiles.Any())
                {
                    imageFilePath = AppDomain.CurrentDomain.BaseDirectory + imageFiles[new Random().Next(imageFiles.Length)];
                }
                else return;
            }

            BitmapImage bg = await Task.Run(() =>
            {
                try
                {
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(imageFilePath, UriKind.Absolute);
                    img.DecodePixelWidth = 688;
                    img.DecodePixelHeight = 387;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    img.Freeze();
                    return img;
                }
                catch
                {
                    return null;
                }

            });

            Application.Current.MainWindow.Background = new ImageBrush() { ImageSource = bg };

        }

        private void Ying()
        {
            Application.Current.MainWindow = this;

            getYBehaviors().ybehavior.YConnect();

            getYMusic().Show();

            
            getYConsole().sendYMessage("§b Ying Success Init");

            
            //k_hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);//钩住键按下 
            getYHook().KeyDownEvent += (ysender, yevent) => {
                getYConsole().sendYMessage(yevent.KeyValue.ToString());

                if ((int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control && yevent.KeyValue == (int)System.Windows.Forms.Keys.Y)
                {
                    getYHook().KeyDownEvent += (yysender, yyevent) => {
                        if (yevent.KeyValue == (int)System.Windows.Forms.Keys.C)
                        {
                            if (getYConsole().IsActive) getYConsole().Hide();
                            else getYConsole().Show();
                        }
                    };
                }

                    /*if ((int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control && yevent.KeyValue == (int)System.Windows.Forms.Keys.Y && yevent.KeyValue == (int)System.Windows.Forms.Keys.C)
                    {
                        if (getYConsole().IsActive) getYConsole().Hide();
                        else getYConsole().Show();
                    }*/
                };
            getYHook().Start();//安装键盘钩子

            
        }

    }
}
