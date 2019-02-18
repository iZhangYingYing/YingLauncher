using Newtonsoft.Json;
using System;
using System.Windows.Threading;
using WebSocketSharp;
using Ying.YingEvents;
using Ying.YingWebsocket.YingStructs;
using Ying.YingWindows;
using static Ying.YingYing;

namespace Ying.YingWebsocket
{
    public class YingBehavior
    {

        private static DispatcherTimer ytimer = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 64),
            IsEnabled = true,
            Tag = "Ying Reconnect"
        };

        private static WebSocket yclient = new WebSocket("ws://iying.top:6040/Ying");

        private Action<Boolean> ycompleted = (y) =>
        {
            YingYing.getYConsole().sendYMessage($"Ying A Message Send {y}");
        };

        public YingBehavior()
        {
            yclient.OnOpen += (sender, yevent) =>
            {
                Console.WriteLine("YingOpen");
            };

            yclient.OnMessage += (sender, yevent) =>
            {

                Console.WriteLine("颖: " + yevent.Data);

                YingStruct ymessage = JsonConvert.DeserializeObject<YingStruct>(yevent.Data);
                if (ymessage.Ying != "颖") return;
                switch (ymessage.ytype)
                {
                    case YingStruct.YingType.Ying:
                        /*yclient.SendAsync(JsonConvert.SerializeObject(new YingStruct()
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
                        }), ycompleted);*/
                        break;

                    case YingStruct.YingType.YUpdata:
                       /*this.SendAsync(JsonConvert.SerializeObject(new YingStruct()
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
                        }), ycompleted);*/
                        break;

                    case YingStruct.YingType.YLogin:

                        YingLoginStruct ylogin = JsonConvert.DeserializeObject<YingLoginStruct>(ymessage.ydata.ymessage);
                        getYEvent().Publish<YingLoginEvent>(new YingLoginEvent {
                            isYSuccess = ylogin.yaccessToken != null,
                            ymessage = ylogin.ymessage
                        });
                        getYEvent().Publish<YingMessageEvent>(new YingMessageEvent
                        {
                            isYSend = false,
                            ystruct = ymessage
                        });

                        break;
                    case YingStruct.YingType.YCode:
                        getYEvent().Publish<YingMessageEvent>(new YingMessageEvent
                        {
                            isYSend = false,
                            ystruct = ymessage
                        });

                        break;
                    case YingStruct.YingType.YRegister:
                        getYEvent().Publish<YingMessageEvent>(new YingMessageEvent
                        {
                            isYSend = false,
                            ystruct = ymessage
                        });

                        break;

                    case YingStruct.YingType.YMessage:
                        //this.SendAsync(yevent.Data, ycompleted);
                        break;
                    case YingStruct.YingType.YMusic:
                        //this.SendAsync(yevent.Data, ycompleted);
                        break;

                    default:
                        break;
                }

            };



            yclient.OnClose += (sender, yevent) =>
            {
                Console.WriteLine("YingClose");
            };

            yclient.OnError += (sender, yevent) =>
            {
                Console.WriteLine("YingError");
            };

            yclient.ConnectAsync();

            getYEvent().Subscribe<YingMessageEvent>((yevent) => {
                if (!yevent.isYSend) return;
                if(yclient.IsAlive)
                {
                    yclient.SendAsync(JsonConvert.SerializeObject(yevent.ystruct), this.ycompleted);
                }
            });

            ytimer.Tick += (ysender, yevent) =>
            {
                try
                {
                    if (!yclient.IsAlive) yclient.ConnectAsync();
                }
                catch (Exception yexception)
                {
                    YingMessageBox.Ying($"Ying: {yexception.Message}", YingMessageBox.YingMessageType.yface_cry, 6040);
                }

            };
            ytimer.Start();

        }
    }
}
