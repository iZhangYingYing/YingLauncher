using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebSocketSharp.Server;
using Ying.YingWebsocket.YingBehaviors;
using static Ying.YingRouter;
using Newtonsoft.Json;

namespace Ying.YingWebsocket
{
    public class YingServer : YingYing
    {
        //private HttpServer yserver = new HttpServer(6040, true);
        private HttpServer yserver = new HttpServer(getYSettings().yip, getYSettings().yport);

        private Dictionary<String, YingBehavior> YClients = new Dictionary<String, YingBehavior>();

        private static YingRouter yrouter = new YingRouter();


        /*private EventHandler<HttpRequestEventArgs> YGet = (y, yy) =>
        {
            HttpListenerRequest yrequest = yy.Request;
            HttpListenerResponse yresponse = yy.Response;

            getYConsole().sendYMessage("Ying: YGetting");

            yresponse.StatusCode = (int) HttpStatusCode.OK;
            yresponse.StatusDescription = "Ying";
            yresponse.ContentType = YingMimeMapping.YGetMimeType(".html");
            yresponse.ContentEncoding = Encoding.UTF8;

            yresponse.Headers.Set(HttpResponseHeader.Server, "Ying Server 6.0.4.0");


            yresponse.WriteContent(Encoding.UTF8.GetBytes("Ying"));



            /*Byte[] yyy = yy.ReadFile(@"Z:\张颖颖\颖颖颖颖\1500614670051.jpg");//File.ReadAllBytes(@"Z:\张颖颖\颖颖颖颖\1500614670051.jpg");//YingFile.YReadFileAsync(new FileInfo(@"Z:\张颖颖\颖颖颖颖\1500614670051.jpg"));
            yresponse.OutputStream.WriteAsync(yyy, 0, yyy.Length);
            yresponse.OutputStream.FlushAsync();*//*

        };*/

        public void Ying()
        {
            getYSettings();

            yserver.AddWebSocketService<YingBehavior>("/Ying");
            yserver.AddWebSocketService<YingPlayerBehavior>("/YingPlayer");
            yserver.AddWebSocketService<YingAdminBehavior>("/YingAdmin");
            yserver.AddWebSocketService<YingServerBehavior>("/YingServer");
            yserver.AddWebSocketService<YingUpdataBehavior>("/YingUpdata");

            //yserver.DocumentRootPath = "/Ying/";
            yserver.OnConnect += (y, yy) => { yrouter.YRoute(YingRouteType.YConnect, yy); };
            yserver.OnGet += (y, yy) => { yrouter.YRoute(YingRouteType.YGet, yy); };
            yserver.OnPost += (y, yy) => { yrouter.YRoute(YingRouteType.YPost, yy); };
            yserver.OnPut += (y, yy) => { yrouter.YRoute(YingRouteType.YPut, yy); };
            yserver.OnDelete += (y, yy) => { yrouter.YRoute(YingRouteType.YDelete, yy); };
            yserver.OnHead += (y, yy) => { yrouter.YRoute(YingRouteType.YHead, yy); };
            yserver.OnOptions += (y, yy) => { yrouter.YRoute(YingRouteType.YOptions, yy); };
            yserver.OnTrace += (y, yy) => { yrouter.YRoute(YingRouteType.YTrace, yy); };

            //yserver.SslConfiguration.ServerCertificate = new X509Certificate2("Ying.pfx", "zyy20020604");

            //yserver.GetFile();

            yserver.Start();


            getYConsole().sendYMessage($"Ying Status {yserver.IsListening}");

            yrouter.yget("/Ying", (yrequest) => new YingResponse(".html", "YingYingYing"))
                    .yget("/YingYing", (yrequest) => new YingResponse(".jpg", File.ReadAllBytes("zyy.jpg")));


            YingYggdrasil.Ying();

            getYConsole().sendYMessage(JsonConvert.SerializeObject(yserver.WebSocketServices));



            //yserver.WebSocketServices.
            //Console.ReadKey(true);
            //yserver.Stop();

        }

        public WebSocketServiceManager getYService()
        {
            return this.yserver.WebSocketServices;
        }


        public Dictionary<String, YingBehavior> getYClients()
        {
            return this.YClients;
        }

        public YingRouter getYRouter()
        {
            return yrouter;
        }

        public void YStop()
        {
            this.yserver.Stop();
        }



    }
}
