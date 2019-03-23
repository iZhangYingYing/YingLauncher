using Ying;
using Ying.Modules;
using Ying.Pages;
using KMCCC.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ying.YingWindows;
using static Ying.YingYing;
using Ying.YingEvents;
using Ying.YingWebsocket.YingStructs;
using Newtonsoft.Json;
using System.Threading;

namespace Ying.YingPages
{
    /// <summary>
    /// YingLoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class YingLoginPage : Page
    {
        private int ystep = 0;

        public YingLoginPage()
        {
            InitializeComponent();

            yyregister.MouseLeftButtonDown += (ysender, yevent) =>
            {
                ylogin.Visibility = Visibility.Collapsed;
                yregister.Visibility = Visibility.Visible;
            };

            yylogin.MouseLeftButtonDown += (ysender, yevent) =>
            {
                ylogin.Visibility = Visibility.Visible;
                yregister.Visibility = Visibility.Collapsed;
            };


            /*
            YingConfig.YArgs.YAuthServers.Add(new YingAuthServer { ytype = YingAuthType.YAuthLib, yname = "Ying" });
            YingConfig.YArgs.YAuthServers.Add(new YingAuthServer { ytype = YingAuthType.Ying, yname = "Ying Ying" });

            YingConfig.YArgs.YAuthServers.Add(new YingAuthServer { ytype = YingAuthType.YAuthLib, yname = "Ying Sakura City Ying", yurl = "https://skin.prinzeugen.net/api/yggdrasil" });
            /*YingConfig.YArgs.YAuthServers.Add(new YingAuthServer { yname =  });
            //YingConfig.YArgs.YAuthServers.CollectionChanged += (y, yy) => {
                YServerList.ItemsSource = YingConfig.YArgs.YAuthServers;
            //};
            YingConfig.YArgs.YAuthServers.Add(new YingAuthServer { });*/

            getYEvent().Subscribe<YingPackageEvent>((yevent) =>
            {
                if (!yevent.isYSend)
                {
                    switch (yevent.yStruct.ytype)
                    {
                        case YingStruct.YingType.YLogin:
                            getYAuhenticator().YResult = JsonConvert.DeserializeObject<YingLoginStruct>(yevent.yStruct.ydata.ymessage);

                            this.Dispatcher.Invoke(() =>
                            {
                                if (getYAuhenticator().YResult.yaccessToken == null)
                                {
                                    YLoginButton.IsEnabled = true;
                                    YLoginButton.Content = "  〉";
                                    YLoading.Visibility = Visibility.Collapsed;
                                    YingMessageBox.Ying($"Ying: {getYAuhenticator().YResult.ymessage}", YingMessageBox.YingMessageType.yface_cry, 6040);
                                }
                                else
                                {
                                    YingConfig.YArgs.YAccount = getYAuhenticator().YResult;
                                    YingMessageBox.Ying($"Ying: {getYAuhenticator().YResult.ymessage}", YingMessageBox.YingMessageType.yface_smile, 6040);
                                    (YingApp.Current.MainWindow as YingWindow).yframe.Navigate(new YingMainPage());
                                    /*return new AuthenticationInfo
                                    {
                                        AccessToken = Guid.Parse(getYAuhenticator().YResult.yaccessToken),
                                        UserType = "Mojang",
                                        DisplayName = "Ying",
                                        Properties = "{}",
                                        UUID = Guid.Parse(getYAuhenticator().YResult.yopenid)
                                    };*/
                                }
                            });
                            break;
                        case YingStruct.YingType.YCode:
                            //if (this.ywait.Status == TaskStatus.Running) this.ywait.Dispose();
                            YingCodeStruct ycode = JsonConvert.DeserializeObject<YingCodeStruct>(yevent.yStruct.ydata.ymessage);

                            this.Dispatcher.Invoke(() =>
                            {
                                YRegisterButton.IsEnabled = true;
                                YRegisterButton.Content = "  〉";
                                YYLoading.Visibility = Visibility.Collapsed;
                                if (!ycode.isYSuccess)
                                {
                                    YingMessageBox.Ying($"Ying: {ycode.ymessage}", YingMessageBox.YingMessageType.yface_cry, 6040);
                                }
                                else
                                {
                                    this.ystep++;
                                    if (this.ystep == 2)
                                    {
                                        this.ystepf.Visibility = Visibility.Collapsed;
                                        this.ysteps.Visibility = Visibility.Visible;
                                    }
                                    YingMessageBox.Ying($"Ying: {ycode.ymessage}", YingMessageBox.YingMessageType.yface_smile, 6040);
                                }
                            });
                            break;
                        case YingStruct.YingType.YRegister:
                            //if (this.ywait.Status == TaskStatus.Running) this.ywait.Dispose();
                            YingRegisterStruct yregister = JsonConvert.DeserializeObject<YingRegisterStruct>(yevent.yStruct.ydata.ymessage);

                            this.Dispatcher.Invoke(() =>
                            {
                                YRegisterButton.IsEnabled = true;
                                YRegisterButton.Content = "  〉";
                                YYLoading.Visibility = Visibility.Collapsed;
                                if (!yregister.isYSuccess)
                                {
                                    YingMessageBox.Ying($"Ying: {yregister.ymessage}", YingMessageBox.YingMessageType.yface_cry, 6040);
                                }
                                else
                                {
                                    YingConfig.YArgs.YAccount = new YingLoginStruct { yaccessToken = yregister.yaccessToken, yclientToken = yregister.yclientToken, yuser = yregister.yuser };
                                    YingMessageBox.Ying($"Ying: {yregister.ymessage}", YingMessageBox.YingMessageType.yface_smile, 6040);
                                    (YingApp.Current.MainWindow as YingWindow).yframe.Navigate(new YingMainPage());
                                }
                            });


                            break;
                        default:
                            break;
                    }



                }


            });





            /*YAuthType.SelectionChanged += (y, yy) =>
            {
                switch(YAuthType.SelectedIndex)
                {
                    case (int)YingAuthType.YLegacy:
                        YServerList.IsEnabled = false;
                        YServerList.Items.Clear();
                        YServerList.Items.Add(ydefault);
                        YServerList.SelectedIndex = 0;

                        YPassword.IsEnabled = false;
                        break;

                    case (int)YingAuthType.YMojang:
                        YServerList.IsEnabled = false;
                        YServerList.Items.Clear();
                        YServerList.Items.Add(ydefault);
                        YServerList.SelectedIndex = 0;

                        YPassword.IsEnabled = true;
                        break;

                    case (int)YingAuthType.YAuthLib:
                        YServerList.IsEnabled = true;
                        YServerList.Items.Clear();
                        
                        YingConfig.YArgs.YAuthServers.ForEach((yserver) => {
                            if (yserver.ytype == YingAuthType.YAuthLib)
                            {
                                YServerList.Items.Add(yserver);
                            }
                        });
                        if (YServerList.Items.Count != 0) YServerList.SelectedIndex = 0;
                        
                        YPassword.IsEnabled = true;
                        break;

                    case (int)YingAuthType.Ying:
                        YServerList.IsEnabled = true;
                        YServerList.Items.Clear();                   

                        YingConfig.YArgs.YAuthServers.ForEach((yserver) => {
                            if(yserver.ytype == YingAuthType.Ying)
                            {
                                YServerList.Items.Add(yserver);
                            }
                        });
                        if(YServerList.Items.Count != 0) YServerList.SelectedIndex = 0;


                        YPassword.IsEnabled = true;
                        break;

                    default:
                        break;
                }
            };*/


        }

        private void YLogin(object ysender, RoutedEventArgs yevent)
        {

            Guid yclientToken = Guid.NewGuid();

            YLoginButton.IsEnabled = false;
            YLoginButton.Content = "";
            YLoading.Visibility = Visibility.Visible;

            getYEvent().Publish<YingPackageEvent>(new YingPackageEvent
            {
                isYSend = true,
                yStruct = new YingStruct()
                {
                    Ying = "颖",
                    ytype = YingStruct.YingType.YLogin,
                    ydata = new YingStruct.YingData()
                    {
                        ycode = 0,
                        ysender = YEamil.Text,
                        ymessage = JsonConvert.SerializeObject(new YingLoginStruct
                        {
                            yaccessToken = null,
                            yclientToken = yclientToken.ToString("N"),
                            yuser = new zyy_users { yemail = YEamil.Text, ypassword = YPassword.Password }
                        }),
                        ydata = null
                    }
                },
            });

            Task.Run(() =>
            {Thread.Sleep(60400);
                this.Dispatcher.Invoke(() =>
                {
                    
                    YLoginButton.IsEnabled = true;
                    YLoginButton.Content = "  〉";
                    YLoading.Visibility = Visibility.Collapsed;
                    YingMessageBox.Ying("Ying: 登录超时", YingMessageBox.YingMessageType.yface_cry, 6040);
                });
                
            });


            /*YingAuhenticator ylogin = new YingAuhenticator(, , null, );
            AuthenticationInfo yinfo = ylogin.Do();
            if (yinfo.Error == null)
            {
                YingConfig.YArgs.YAccounts.Add(new YingAccount
                {
                    ytype = YingAuthType.YMojang,
                    yusername = YEamil.Text,
                    ypassword = YPassword.Password,
                    yaccessToken = yinfo.AccessToken.ToString("N"),
                    yclientToken = yclientToken.ToString("N"),
                    ydisplayName = yinfo.DisplayName,
                    yuuid = yinfo.UUID.ToString()
                });
            }
            else
            {
                YingMessageBox.Ying(yinfo.Error, null, 6040, (y) => { });
                //(YingApp.Current.MainWindow as YingWindow)._frame.Navigate(new MainPage());
            }*/

        }

        private Task ywaiter = null;
        private String yyeamil = String.Empty;
        private void YRegister(object ysender, RoutedEventArgs yevent)
        {
            ywaiter = new Task(() =>
            {Thread.Sleep(60400);
                this.Dispatcher.Invoke(() =>
                {
                    
                YRegisterButton.IsEnabled = true;
                YRegisterButton.Content = "  〉";
                YYLoading.Visibility = Visibility.Collapsed;
                YingMessageBox.Ying("Ying: 数据接收超时", YingMessageBox.YingMessageType.yface_cry, 6040);
                });
            });
            switch (this.ystep)
            {
                case 0:
                    getYEvent().Publish<YingPackageEvent>(new YingPackageEvent
                    {
                        isYSend = true,
                        yStruct = new YingStruct
                        {
                            Ying = "颖",
                            ytype = YingStruct.YingType.YCode,
                            ydata = new YingStruct.YingData
                            {
                                ycode = 0,
                                ysender = YYEamil.Text,
                                ymessage = JsonConvert.SerializeObject(new YingCodeStruct() { yemail = YYEamil.Text }),
                                ydata = null
                            }
                        }
                    });

                    this.yyeamil = YYEamil.Text;

                    YRegisterButton.IsEnabled = false;
                    YRegisterButton.Content = "";
                    YYLoading.Visibility = Visibility.Visible;
                    ywaiter.Start();
                    break;
                case 1:

                    if (this.YYEamil.Text != this.yyeamil)
                    {
                        this.ystep--;
                        this.YRegister(null, null);
                        return;
                    }
                    getYEvent().Publish<YingPackageEvent>(new YingPackageEvent
                    {
                        isYSend = true,
                        yStruct = new YingStruct
                        {
                            Ying = "颖",
                            ytype = YingStruct.YingType.YCode,
                            ydata = new YingStruct.YingData
                            {
                                ycode = 0,
                                ysender = YEamil.Text,
                                ymessage = JsonConvert.SerializeObject(new YingCodeStruct() { yemail = YYEamil.Text, ycode = Convert.ToInt32(YCode.Text) }),
                                ydata = null
                            }
                        }
                    });

                    YRegisterButton.IsEnabled = false;
                    YRegisterButton.Content = "";
                    YYLoading.Visibility = Visibility.Visible;
                    ywaiter.Start();
                    break;
                case 2:
                    if (this.YYPassword.Password == this.YYYPassword.Password)
                    {
                        this.ystep = 3;
                        //this.ylabel.Text = "";
                        //this.yylabel.Text = "性别";
                        this.ystepf.Visibility = Visibility.Collapsed;
                        this.ysteps.Visibility = Visibility.Collapsed;
                        this.ystept.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        YingMessageBox.Ying("Ying: 两次密码不一样", YingMessageBox.YingMessageType.yface_cry, 6040);
                    }
                    break;
                case 3:
                    //if(!this.YBoy.IsChecked && !this.YGirl.IsChecked) this.YBoy.clic
                    YingSex ysex = YingSex.YBoy;
                    if (this.YGirl.IsChecked == true) ysex = YingSex.YGirl;
                    getYEvent().Publish<YingPackageEvent>(new YingPackageEvent
                    {
                        isYSend = true,
                        yStruct = new YingStruct
                        {
                            Ying = "颖",
                            ytype = YingStruct.YingType.YRegister,
                            ydata = new YingStruct.YingData
                            {
                                ycode = 0,
                                ysender = YYEamil.Text,
                                ymessage = JsonConvert.SerializeObject(new YingRegisterStruct()
                                {
                                    ycode = Convert.ToInt32(YCode.Text),
                                    yuser = new zyy_users
                                    {
                                        yemail = this.yyeamil,
                                        yusername = this.YNickName.Text,
                                        ypassword = this.YYPassword.Password,
                                        ysex = ysex
                                    }
                                }),
                                ydata = null
                            }
                        }
                    });

                    this.yyeamil = YYEamil.Text;

                    YRegisterButton.IsEnabled = false;
                    YRegisterButton.Content = "";
                    YYLoading.Visibility = Visibility.Visible;
                    ywaiter.Start();

                    break;
                default:
                    break;
            }



            /*YingAuhenticator ylogin = new YingAuhenticator(, , null, );
            AuthenticationInfo yinfo = ylogin.Do();
            if (yinfo.Error == null)
            {
                YingConfig.YArgs.YAccounts.Add(new YingAccount
                {
                    ytype = YingAuthType.YMojang,
                    yusername = YEamil.Text,
                    ypassword = YPassword.Password,
                    yaccessToken = yinfo.AccessToken.ToString("N"),
                    yclientToken = yclientToken.ToString("N"),
                    ydisplayName = yinfo.DisplayName,
                    yuuid = yinfo.UUID.ToString()
                });
            }
            else
            {
                YingMessageBox.Ying(yinfo.Error, null, 6040, (y) => { });
                //(YingApp.Current.MainWindow as YingWindow)._frame.Navigate(new MainPage());
            }*/

        }
    }
}
