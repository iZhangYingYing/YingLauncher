using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using Ying.YingEvents;
using Ying.YingExceptions;
using Ying.YingUtil;

namespace Ying
{
    public class YingResponse
    {
        public String ytype { get; }
        public Byte[] ycontext { get; }

        public YingResponse(String ytype, Byte[] ycontext)
        {
            this.ytype = ytype;
            this.ycontext = ycontext;
        }

        public YingResponse(String ytype, String ycontext)
        {
            this.ytype = ytype;
            this.ycontext = Encoding.UTF8.GetBytes(ycontext);
        }
    }

    public class YingRouter : YingYing
    {

        public enum YingRouteType
        {
            YConnect,
            YGet,
            YPost,
            YPut,
            YDelete,
            YHead,
            YOptions, 
            YTrace
        }


        private struct YingRouteInfo
        {
            public String YUri { get; set; }
            public Func<HttpListenerRequest, YingResponse> YAction { get; set; }
            public Boolean isYMatch { get; set; }
        }

        

        private Dictionary<YingRouteType, List<YingRouteInfo>> yroutes = new Dictionary<YingRouteType, List<YingRouteInfo>>();


        public YingRouter()
        {
            yroutes.Add(YingRouteType.YConnect, new List<YingRouteInfo>());
            yroutes.Add(YingRouteType.YGet, new List<YingRouteInfo>());
            yroutes.Add(YingRouteType.YPost, new List<YingRouteInfo>());
            yroutes.Add(YingRouteType.YPut, new List<YingRouteInfo>());
            yroutes.Add(YingRouteType.YDelete, new List<YingRouteInfo>());
            yroutes.Add(YingRouteType.YHead, new List<YingRouteInfo>());
            yroutes.Add(YingRouteType.YOptions, new List<YingRouteInfo>());
            yroutes.Add(YingRouteType.YTrace, new List<YingRouteInfo>());
        }


        public YingRouter yadd(YingRouteType ytype, String yuri, Func<HttpListenerRequest, YingResponse> yaction)
        {
            yroutes[ytype].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction
            });
            return this;
        }


        public YingRouter yconnect(String yuri, Func<HttpListenerRequest, YingResponse> yaction)
        {
            yroutes[YingRouteType.YConnect].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction
            });
            return this;
        }

        public YingRouter yget(String yuri, Func<HttpListenerRequest, YingResponse> yaction, Boolean isYMatch = false)
        {
            yroutes[YingRouteType.YGet].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction,
                isYMatch = isYMatch
            });
            return this;
        }

        public YingRouter ypost(String yuri, Func<HttpListenerRequest, YingResponse> yaction, Boolean isYMatch = false)
        {
            yroutes[YingRouteType.YPost].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction,
                isYMatch = isYMatch
            });
            return this;
        }

        public YingRouter yput(String yuri, Func<HttpListenerRequest, YingResponse> yaction)
        {
            yroutes[YingRouteType.YPut].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction
            });
            return this;
        }

        public YingRouter ydelete(String yuri, Func<HttpListenerRequest, YingResponse> yaction)
        {
            yroutes[YingRouteType.YDelete].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction
            });
            return this;
        }

        public YingRouter yhead(String yuri, Func<HttpListenerRequest, YingResponse> yaction)
        {
            yroutes[YingRouteType.YHead].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction
            });
            return this;
        }

        public YingRouter yoptions(String yuri, Func<HttpListenerRequest, YingResponse> yaction)
        {
            yroutes[YingRouteType.YOptions].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction
            });
            return this;
        }

        public YingRouter ytrace(String yuri, Func<HttpListenerRequest, YingResponse> yaction)
        {
            yroutes[YingRouteType.YTrace].Add(new YingRouteInfo
            {
                YUri = yuri,
                YAction = yaction
            });
            return this;
        }




        public void YRoute(YingRouteType ytype, HttpRequestEventArgs yargs)
        {

            getYConsole().sendYMessage($"{ytype.ToString()} > {yargs.Request.RawUrl}");

            //getYConsole().sendYMessage($"{ytype.ToString()} > {yargs.Request.RawUrl}");

            HttpListenerRequest yrequest = yargs.Request;
            HttpListenerResponse yresponse = yargs.Response;

            yroutes[ytype].ForEach((y) =>
            { 
                if (y.isYMatch || yargs.Request.RawUrl == y.YUri)
                {
                    int ycount = Regex.Matches(yargs.Request.RawUrl, y.YUri).Count;
                    if (y.isYMatch && Regex.Matches(yargs.Request.RawUrl, y.YUri).Count == 0) return;
                    try
                    {
                        
                        YingResponse yyresponse = y.YAction.Invoke(yrequest);
                        YResponseBuilder(yresponse, HttpStatusCode.OK, yyresponse.ytype, Encoding.UTF8);
                        yresponse.WriteContent(yyresponse.ycontext);

                        /*try
                        {
                            Regex yregex = new Regex("", RegexOptions.Compiled);
                            yregex.Match(Convert.ToString());
                        }
                        catch (FormatException yexception) { }
                        catch (InvalidCastException yexception) { }
                        catch (OverflowException yexception) { }
                        catch (Exception yexception) { }*/

                    }
                    catch (YingAuthenticationException yexception)
                    {
                        YResponseBuilder(yresponse, (HttpStatusCode)yexception.getYCode(), ".json", Encoding.UTF8);

                        yresponse.WriteContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new YingError
                        {
                            error = yexception.getYError(),
                            errorMessage = yexception.Message,
                            cause = null
                        })));
                    }
                    catch (YingHttpException yexception)
                    {
                        YResponseBuilder(yresponse, (HttpStatusCode)yexception.getYCode(), ".html", Encoding.UTF8);
                    }
                    catch (Exception yexception)
                    {
                        YResponseBuilder(yresponse, HttpStatusCode.InternalServerError, ".html", Encoding.UTF8);
                    }
                    return;
                }
                
            });


            /*YResponseBuilder(yresponse, new YingResponseInfo
            {
                YStatusCode = HttpStatusCode.NotFound,
                YStatusDescription = "Ying",

                YContentType = YingMimeMapping.YGetMimeType(".json"),
                YContentEncoding = Encoding.UTF8
            });*/

            /*yresponse.WriteContent(Encoding.UTF8.GetBytes("{\"Ying\": \"颖\"}" /*JsonMapper.ToJson()*/ /*));*/
            //y.Response.WriteContent


        }

        public static void YResponseBuilder(HttpListenerResponse yresponse, HttpStatusCode YStatusCode, String YContentType, Encoding YContentEncoding)
        {
            yresponse.StatusCode = (int)YStatusCode;
            yresponse.StatusDescription = $"Ying {YStatusCode.GetDescription()}";

            yresponse.ContentType = YingMimeMapping.YGetMimeType(YContentType);
            yresponse.ContentEncoding = YContentEncoding;

            yresponse.Headers.Set(HttpResponseHeader.Server, "Ying Server 6.0.4.0");
        }

        public static String YQuery(String yurl, String ykey)
        {
            return new Regex($@"(?:^|\?|&){ykey}=(\d*)(?:&|$)").Match(yurl).Groups[0].Value;
        }

    }
}
