using LitJson;

using Redbus.Events;
using System;
using System.Collections.Generic;

using WebSocketSharp;
using WebSocketSharp.Server;
using Ying.YingWebsocket.YingStructs;
using Ying.YingWindows;

namespace Ying.YingWebsocket.YingBehaviors
{
    class YingPlayerBehavior : WebSocketBehavior
    {



        private Action<Boolean> ycompleted = (y) =>
        {
            YingYing.getYConsole().sendYMessage($"Ying A Message Send {y}");
        };


        public YingPlayerBehavior()
        {

        }

        //YingYing.getYConsole().sendYMessage("颖: Ying WebSocket Server was run on " + this.Context.ServerEndPoint);




        protected override void OnOpen()
        {



            YingYing.getYConsole().sendYMessage("颖: A new Client Connected");

            //YingYing.getYServer().getYClients().Add(this.ID, this);

            //this.Context.WebSocket.SendAsync();

            //this.Sessions.

            //Action<LogData, String> ytest2 = (y, yy) => { Console.WriteLine("void method(t1)"); };


            this.Log.Output += (y, yy) =>
            {
                YingYing.getYConsole().sendYMessage($"[{y.Caller}][{y.Date}][{y.Level}] {y.Message} | {yy}");

            };




        }

        protected override void OnMessage(MessageEventArgs yevent)
        {
            YingYing.getYConsole().sendYMessage("颖: " + yevent.Data);

            try
            {
                YingStruct ymessage = LitJson.JsonMapper.ToObject<YingStruct>(yevent.Data);

                if (ymessage.Ying == "颖")
                {
                    /*switch (ymessage.YType)
                    {
                        case YingStruct.YingType.Ying:
                            SendAsync(JsonMapper.ToJson(new YingStruct()
                            {
                                YType = YingStruct.YingType.Ying,
                                YData = new YingStruct.YingData()
                                {
                                    ycode = 0,
                                    ysender = "YingServer",
                                    ymessage = JsonMapper.ToJson(new iYingStruct()
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
                            SendAsync(JsonMapper.ToJson(new YingStruct()
                            {
                                YType = YingStruct.YingType.YUpdata,
                                YData = new YingStruct.YingData()
                                {
                                    ycode = 0,
                                    ysender = "YingServer",
                                    //ymessage = JsonMapper.ToJson(YingWindow.yy),
                                    ydata = null
                                }
                            }), ycompleted);

                            SendAsync(new System.IO.FileInfo(@"Z:\Ying\YingServer\Ying\Ying\YingApp.xaml.cs"), ycompleted);


                            break;

                        case YingStruct.YingType.YLogin:
                            SendAsync(JsonMapper.ToJson(new YingStruct()
                            {
                                YType = YingStruct.YingType.YLogin,
                                YData = new YingStruct.YingData()
                                {
                                    ycode = 0,
                                    ysender = "YingServer",
                                    ymessage = "Login Success",
                                    ydata = null
                                }
                            }), ycompleted);
                            break;

                        case YingStruct.YingType.YMessage:
                            YingYing.getYServer().getYService().BroadcastAsync(yevent.Data, () => { });
                            break;

                        default:
                            break;
                    }*/
                }
                else
                {
                    SendAsync(JsonMapper.ToJson(new YingStruct()
                    {
                        ytype = YingStruct.YingType.Ying,
                        ydata = new YingStruct.YingData()
                        {
                            ycode = 1,
                            ysender = "YingServer",
                            ymessage = "Wrong package",
                            ydata = null
                        }
                    }), ycompleted);
                }

                /*/
                    YingYing.getYEvent().Subscribe<PayloadEvent<YingStruct>>((y) =>
                    {
                        
                    });


                    YingYing.getYEvent().Publish(new PayloadEvent<YingUpdataStruct>(new YingUpdataStruct()
                    {
                        YUpdata = new Dictionary<String, Object> {
                            { "ydata" ,null },
                            { "yurl" , null }
                        }
                    }));
                /*/

            }
            catch (Exception yexception)
            {
                YingYing.getYConsole().sendYMessage("Recived a wrong package. Its content is " + yevent.Data);
                YingYing.getYConsole().sendYMessage(yexception.Message + Environment.NewLine + yexception.StackTrace);
            }


            /*JObject ymessage = (JObject)JsonConvert.DeserializeObject(yevent.Data);
            if (ymessage["Ying"].ToString() == "颖") {
                if (ymessage.ContainsKey("YUpdata")) {
                    /*Dictionary<String, Object> y = new Dictionary<String, Object>();
                    y.Add("Ying", "颖");
                    Dictionary<String, String> yy = new Dictionary<String, String>();
                    yy.Add("ydata", YingWindow.yy);
                    yy.Add("yurl", "http://222.136.107.157:60408/yclient/");
                    y.Add("YUpdata", yy);*/
            /*SendAsync(JsonConvert.SerializeObject(new YingStruct() {
                YUpdata = new Dictionary<String, Object> {
                    {"ydata", YingWindow.yy}, {"yurl", "http://222.136.107.157:60408/yclient/"}
                }
            }), null);*//*

        }
    }
    else
    {
        //Sessions.CloseSession(yevent.);
    }*/
                        //{"Ying": "颖", "YUpdata": "Ying"}
        }


        protected override void OnClose(CloseEventArgs yevent)
        {
            YingYing.getYConsole().sendYMessage($"颖: A Client Disconnect, because ({yevent.Code}){yevent.Reason}");
            YingYing.getYServer().getYClients().Remove(this.ID);
        }

        protected override void OnError(ErrorEventArgs yevent)
        {
            YingYing.getYConsole().sendYMessage($"颖: A Client Disconnect, because ({yevent.Message}){yevent.Exception.Message} {yevent.Exception.StackTrace}");
            YingYing.getYServer().getYClients().Remove(this.ID);
        }


    }
}
