using Newtonsoft.Json;
using Redbus.Events;
using SQLite;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using Ying.YingDataBase.YingDataStructs;
using Ying.YingEvents;
using Ying.YingUtil;
using Ying.YingUtils;
using Ying.YingWebsocket.YingStructs;
using Ying.YingWindows;
using static Ying.YingAuthenticate;
using static Ying.YingYing;

namespace Ying.YingWebsocket.YingBehaviors
{
    public class YingBehavior : WebSocketBehavior
    {


        private Action<Boolean> ycompleted = (y) =>
        {
            getYConsole().sendYMessage($"Ying A Message Send {y}");
        };

        private static DispatcherTimer ytimer = new DispatcherTimer
        {
            Interval = new TimeSpan(6040000),
            IsEnabled = true,
            Tag = "Ying Verification Code Clear"
        };


        public YingBehavior()
        {

        }

        //YingYing.getYConsole().sendYMessage("颖: Ying WebSocket Server was run on " + this.Context.ServerEndPoint);




        protected override void OnOpen()
        {



            //getYEvent().Publish<YingLogEvent>(new YingLogEvent("颖: A new Client Connected"));

            YingYing.getYServer().getYClients().Add(this.ID, this);

            getYConsole().sendYMessage("颖: A new Client Connected");


            this.Log.Output += (y, yy) =>
            {
                getYConsole().sendYMessage($"[{y.Caller}][{y.Date}][{y.Level}] {y.Message} | {yy}");

            };

            /*SendAsync(JsonConvert.SerializeObject(new YingStruct()
            {
                YType = YingStruct.YingType.YUpdata,
                YData = new YingStruct.YingData()
                {
                    ycode = 0,
                    ysender = "YingServer",
                    ymessage = "Y1500614670051.jpg",
                    ydata = YingFile.YReadFileAsync(‪new System.IO.FileInfo(@"Z:\张颖颖\颖颖颖颖\1500614670051.jpg"))
                }
            }), ycompleted);*/



            //this.SendAsync(new System.IO.FileInfo("zyy.mp4"), ycompleted);

            ytimer.Tick += (ysender, yevent) =>
            {
                //if(getYDataBaseManager().getYConnection().)
                TableQuery<zyy_verification_code> yquery = getYDataBaseManager().getYConnection().Table<zyy_verification_code>();
                yquery.ToList().ForEach((y) =>
                {
                    if (y.ytime < getYTimeStamp().TotalMilliseconds - new TimeSpan(1, 6, 4).TotalMilliseconds)
                    {
                        getYDataBaseManager().getYConnection().Delete<zyy_verification_code>(y.yid);
                    }
                });
            };
            ytimer.Start();



        }

        protected override void OnMessage(MessageEventArgs yevent)
        {
            YingStruct ymessage = JsonConvert.DeserializeObject<YingStruct>(yevent.Data);
            if (ymessage.Ying != "颖") return;
            switch (ymessage.ytype)
            {
                case YingStruct.YingType.Ying:
                    this.SendAsync(JsonConvert.SerializeObject(new YingStruct()
                    {
                        ytype = YingStruct.YingType.Ying,
                        ydata = new YingStruct.YingData()
                        {
                            ycode = 0,
                            ysender = "Ying Server",
                            ymessage = JsonConvert.SerializeObject(new iYingStruct()
                            {
                                yservername = "Ying Server Air Plus",
                                yipaddress = "127.0.0.1",
                                ymotd = "so tell me your story."
                            }),
                            ydata = null
                        }
                    }), ycompleted);
                    break;

                case YingStruct.YingType.YUpdata:
                    this.SendAsync(JsonConvert.SerializeObject(new YingStruct()
                    {
                        ytype = YingStruct.YingType.YUpdata,
                        ydata = new YingStruct.YingData()
                        {
                            ycode = 0,
                            ysender = "Ying Server",
                            ymessage = JsonConvert.SerializeObject(new YingUpdataStruct
                            {
                            }),
                            ydata = null
                        }
                    }), ycompleted);
                    break;

                case YingStruct.YingType.YLogin:

                    YingLoginStruct ylogin = JsonConvert.DeserializeObject<YingLoginStruct>(ymessage.ydata.ymessage);
                    YingAuthenticateResult yresult = YingAuthenticate.yauthenticate(ylogin.yemail, ylogin.ypassword);
                    YingLoginStruct yylogin = new YingLoginStruct();
                    if (ylogin.yclientToken == null) ylogin.yclientToken = Guid.NewGuid().ToString("N");

                    if (yresult.ysuccess)
                    {
                        String yaccessToken = Guid.NewGuid().ToString("N");
                        getYDataBaseManager().getYConnection().Insert(new zyy_yggdrasil_tokens
                        {
                            yemail = ylogin.yemail,

                            yclientToken = ylogin.yclientToken,
                            yaccessToken = yaccessToken,

                            ytime = getYTimeStamp().TotalMilliseconds
                        });

                        zyy_users yuser = yresult.yuser;
                        yuser.ypassword = null;
                        yuser.ysalt = null;

                        yylogin = new YingLoginStruct
                        {
                            yemail = ylogin.yemail,
                            yclientToken = ylogin.yclientToken,
                            yaccessToken = yaccessToken,
                            ymessage = "Ying Ok",
                            yuser = yuser
                        };
                    }
                    else
                    {
                        yylogin = new YingLoginStruct
                        {
                            yemail = ylogin.yemail,
                            yclientToken = ylogin.yclientToken,
                            yaccessToken = null,
                            ymessage = "Ying Error"
                        };
                    }

                    this.SendAsync(JsonConvert.SerializeObject(new YingStruct()
                    {
                        Ying = "颖",
                        ytype = YingStruct.YingType.YLogin,
                        ydata = new YingStruct.YingData()
                        {
                            ycode = 0,
                            ysender = "Ying Server",
                            ymessage = JsonConvert.SerializeObject(yylogin),
                            ydata = null
                        }
                    }), ycompleted);
                    break;

                case YingStruct.YingType.YCode:
                    YingCodeStruct ycode = JsonConvert.DeserializeObject<YingCodeStruct>(ymessage.ydata.ymessage);
                    if (ycode.ycode == 20020604)
                    {
                        if (YingAuthenticate.yauthenticate(ycode.yemail, null).yuser.yid == 0)
                        {
                            int yycode = new Random().Next(100000, 1000000);
                            try
                            {
                                getYDataBaseManager().getYConnection().Delete<zyy_verification_code>((from y in getYDataBaseManager().getYConnection().Table<zyy_verification_code>()
                                                                                                      where y.yemail == ycode.yemail
                                                                                                      select y).FirstOrDefault().yid);
                            }
                            catch { }
                            getYDataBaseManager().getYConnection().Insert(new zyy_verification_code
                            {
                                yemail = ycode.yemail,
                                ycode = yycode,
                                ytime = getYTimeStamp().TotalMilliseconds
                            });
                            YingMail.ysend(ycode.yemail, new System.IO.StreamReader(getYFiles().getYResource("Ying.yresources.yhtml.Ying.html")).ReadToEnd().Replace("{{yname}}", "烟雨城").Replace("{{ycode}}", Convert.ToString(yycode)));
                            ycode.isYSuccess = true;
                            ycode.ymessage = "验证码已发送到您的邮箱";
                            this.SendAsync(JsonConvert.SerializeObject(new YingStruct()
                            {
                                Ying = "颖",
                                ytype = YingStruct.YingType.YCode,
                                ydata = new YingStruct.YingData()
                                {
                                    ycode = 0,
                                    ysender = "Ying Server",
                                    ymessage = JsonConvert.SerializeObject(ycode),
                                    ydata = null
                                }
                            }), ycompleted);
                        }
                        else
                        {
                            ycode.isYSuccess = false;
                            ycode.ymessage = "该邮箱地址已经被使用";
                            this.SendAsync(JsonConvert.SerializeObject(new YingStruct()
                            {
                                Ying = "颖",
                                ytype = YingStruct.YingType.YCode,
                                ydata = new YingStruct.YingData()
                                {
                                    ycode = 0,
                                    ysender = "Ying Server",
                                    ymessage = JsonConvert.SerializeObject(ycode),
                                    ydata = null
                                }
                            }), ycompleted);
                        }
                    }
                    else
                    {
                        zyy_verification_code yycode = (from y in getYDataBaseManager().getYConnection().Table<zyy_verification_code>()
                                                        where y.yemail == ycode.yemail
                                                        select y).FirstOrDefault();
                        if (ycode.ycode == yycode.ycode)
                        {
                            ycode.isYSuccess = true;
                            ycode.ymessage = "验证码正确";
                        }
                        else
                        {
                            ycode.isYSuccess = false;
                            ycode.ymessage = "验证码错误";
                        }
                        this.SendAsync(JsonConvert.SerializeObject(new YingStruct()
                        {
                            Ying = "颖",
                            ytype = YingStruct.YingType.YCode,
                            ydata = new YingStruct.YingData()
                            {
                                ycode = 0,
                                ysender = "Ying Server",
                                ymessage = JsonConvert.SerializeObject(ycode),
                                ydata = null
                            }
                        }), ycompleted);
                    }
                    break;
                case YingStruct.YingType.YRegister:
                    YingRegisterStruct yregister = JsonConvert.DeserializeObject<YingRegisterStruct>(ymessage.ydata.ymessage);
                    if((from y in getYDataBaseManager().getYConnection().Table<zyy_verification_code>()
                                                    where y.yemail == yregister.yuser.yemail
                                                    select y).FirstOrDefault().ycode == yregister.ycode)
                    {
                        Guid yguid = Guid.NewGuid();
                        getYDataBaseManager().getYConnection().Insert(new zyy_users {
                            yusername = yregister.yuser.yusername,
                            ypassword = yregister.yuser.ypassword,
                            yopenid = yguid.ToString("N"),
                            ysex = yregister.yuser.ysex,
                            yemail = yregister.yuser.yemail
                        });
                        getYDataBaseManager().getYConnection().Insert(new zyy_yggdrasil_profile
                        {
                            yyid = yguid.ToString("N"),
                            yname = yregister.yuser.yusername
                        });
                        yregister.isYSuccess = true;
                        yregister.ymessage = "注册成功";
                    } else
                    {
                        yregister.isYSuccess = false;
                        yregister.ymessage = "验证码错误";
                        yregister.yuser = new zyy_users();
                    }
                    this.SendAsync(JsonConvert.SerializeObject(new YingStruct()
                    {
                        Ying = "颖",
                        ytype = YingStruct.YingType.YRegister,
                        ydata = new YingStruct.YingData()
                        {
                            ycode = 0,
                            ysender = "Ying Server",
                            ymessage = JsonConvert.SerializeObject(yregister),
                            ydata = null
                        }
                    }), ycompleted);
                    break;

                case YingStruct.YingType.YMessage:
                    this.SendAsync(yevent.Data, ycompleted);
                    break;
                case YingStruct.YingType.YMusic:
                    this.SendAsync(yevent.Data, ycompleted);
                    break;

                default:
                    break;
            }
        }



        protected override void OnClose(CloseEventArgs yevent)
        {
            getYConsole().sendYMessage($"颖: A Client Disconnect, because ({yevent.Code}){yevent.Reason}");
            YingYing.getYServer().getYClients().Remove(this.ID);
        }

        protected override void OnError(ErrorEventArgs yevent)
        {
            //if(yevent.Exception)
        }

    }
}
