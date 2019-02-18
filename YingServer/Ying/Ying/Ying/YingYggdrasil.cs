using LitJson;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WebSocketSharp;
using WebSocketSharp.Net;
using Ying.YingDataBase.YingDataStructs;
using Ying.YingExceptions;
using Ying.YingUtil;
using static Ying.YingAuthenticate;
using static Ying.YingRouter;

namespace Ying
{

    public struct YingError
    {
        public String error { get; set; }
        public String errorMessage { get; set; }
        public String cause { get; set; }
    }

    public struct iYingMeta
    {
        public String serverName { get; set; }
        public String implementationName { get; set; }
        public String implementationVersion { get; set; }
    }

    public struct iYing
    {
        public iYingMeta meta { get; set; }
        public List<String> skinDomains { get; set; }
        public String signaturePublickey { get; set; }
    }

    public struct YingAuthenticateRequestAgent
    {
        public String name { get; set; }
        public int version { get; set; }
    }

    public struct YingAuthenticateRequest
    {
        public String username { get; set; }
        public String password { get; set; }
        public String clientToken { get; set; }
        public Boolean requestUser { get; set; }
        public YingAuthenticateRequestAgent agent { get; set; }
    }


    /// <summary>
    /// {
    ///     "name":"属性的键",
    ///     "value":"属性的值",
    ///     "signature":"属性值的数字签名（仅在特定情况下需要包含）"
    /// }
    /// </summary>
    public struct YingPropertie
    {
        public String name { get; set; }
        public String value { get; set; }
        public String signature { get; set; }

    }

    public struct YingProfile
    {
        public String id { get; set; }
        public String name { get; set; }
        public List<YingPropertie> properties { get; set; }
    }

    /// <summary>
    /// {
	///     "id":"角色 UUID（无符号）",
	///     "name":"角色名称",
	///     "properties":[ // 角色的属性（数组，每一元素为一个属性）（仅在特定情况下需要包含）
	///         { // 一项属性
	///		        "name":"属性的键",
	///		        "value":"属性的值",
	///		        "signature":"属性值的数字签名（仅在特定情况下需要包含）"
    ///
    ///         }
	///	        // ,...（可以有更多）
	///     ]
    /// }
    /// </summary>
    public struct YingUser
    {
        public String id { get; set; }
        public List<YingPropertie> properties { get; set; }

    }




    public struct YingTextureInfo
    {
        public String url { get; set; }
        public Dictionary<String, String> metadata { get; set; }

    }

    /// <summary>
    /// {
    ///     "timestamp": 该属性值被生成时的时间戳（Java 时间戳格式，即自 1970-01-01 00:00:00 UTC 至今经过的毫秒数）,
    ///     "profileId": "角色 UUID（无符号）",
    ///     "profileName": "角色名称",
    ///     "textures": { // 角色的材质
    ///         "材质类型（如 SKIN）": { // 若角色不具有该项材质，则不必包含
    ///             "url":"材质的 URL",
    ///             "metadata":{ // 材质的元数据，若没有则不必包含
    ///                 "键":"值"
    ///                 // ,...（可以有更多）            
    ///             }
    ///         }
    ///         // ,...（可以有更多）
    ///     }
    /// }        
    /// </summary>
    public struct YingTexture
    {
        public int timestamp { get; set; }
        public String profileId { get; set; }
        public String profileName { get; set; }
        public List<YingTextureInfo> textures { get; set; }
    }


    public struct iYingAuthenticate
    {

        public String accessToken { get; set; }
        public String clientToken { get; set; }

        public List<YingProfile> availableProfiles { get; set; }
        public YingProfile selectedProfile { get; set; }

        public YingUser user { get; set; }

    }

    public struct YingValidate
    {

        public String accessToken { get; set; }
        public String clientToken { get; set; }

    }

    public struct YingRefresh
    {

        public String accessToken { get; set; }
        public String clientToken { get; set; }

        public Boolean requestUser { get; set; }
        public YingProfile selectedProfile { get; set; }

    }

    public struct YingRefreshResponse
    {

        public String accessToken { get; set; }
        public String clientToken { get; set; }

        public YingProfile selectedProfile { get; set; }

        public YingUser user { get; set; }


    }






    /*
     
         {
	"accessToken":"令牌的 accessToken",
	"clientToken":"令牌的 clientToken",
	"availableProfiles":[ // 用户可用角色列表
		// ,... 每一项为一个角色（格式见 §角色信息的序列化）
	],
	"selectedProfile":{
		// ... 绑定的角色，若为空，则不需要包含（格式见 §角色信息的序列化）
	},
	"user":{
		// ... 用户信息（仅当请求中 requestUser 为 true 时包含，格式见 §用户信息的序列化）
	}
}*/

    



    public class YingYggdrasil : YingYing
    {



        private static YingRouter yrouter = getYServer().getYRouter();


        /*public YingYggdrasil()
        {

        }*/

        private static DispatcherTimer ytimer = new DispatcherTimer { Interval = new TimeSpan(6, 0, 4),
                                                               IsEnabled = true,
                                                               Tag = "Ying Token Clear"};



        public static void Ying()
        {


            yrouter.yget("/yingyggdrasil/", (yrequest) => new YingResponse(".json", JsonMapper.ToJson(new iYing
            {
                meta = new iYingMeta
                {
                    serverName = "Ying Sakura City",
                    implementationName = "Ying Yggdrasil Server",
                    implementationVersion = "6.0.4.0"
                },
                skinDomains = new List<String> { "zyy.com" },
                signaturePublickey = "-----BEGIN PUBLIC KEY-----\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAnhEHrOIVm2ReCJIIR/1RHAHzkoJjpS8t\nIVfu+JWQKFd4SiVfc/Ym1T2rsmoWVtlppOcWYLSv3XVzpSJS1yk79p6cmASUPU9ENotFMCCi+sdT\nAVo/RKV7mxZHiz8g3H3JdegGMA1nEiYV5l9ue233d4/2MytXjKF8DVskkWZbfN6pRTiaCsRyjt40\nC3XhThBQzVIyjs75fpGNImjCvNlw2Dpy+AvdcLlT75hmGM8UBh8bkshNwBl0JDsE8SL9Qanen2Tk\noXVFoAMKoUsu1u0Kfwjzit52CutU2JXSSxp68KntcKA5d11520Vt51OvRGnNnOtMg6Qqgql56tAL\niy9HJAZpA1In6Kc6epK9vDH7AqZd9QRPBcTsGFdoO8MqegIJZK0zxQY4jCshO0Sxk/EAY5MdwBtB\nr/hGel5JkZByJqanZYV4dKlmNqTZk9hoakv1cb3e8ku2dXRgvtjJvkFayQFqQYpXgLX4ZIx1V87c\nHb1L5RGyAfB5y+UpPW+igMTRgIKN0OWHroTI9W+hSNFjAVj7XPWwp3vRBhdSABiOL7IHsk8e4VcE\nwiAuYGtRi8o/7IJpiZBBh1UkzQ/m1H7ohzjduvc0pYtp053YOx9Tmf0gj8U3dYNiE1gmfBDRnDwJ\nBxCKAYEv9g1uPC0yeJr7EN06Dbb6QQMA+CsbOSS50A0CAwEAAQ==\n-----END PUBLIC KEY-----\n"
            })
            ));



            yrouter.ypost("/yingyggdrasil/authserver/authenticate", (yrequest) =>
            {

                YingAuthenticateRequest yauthinfo = JsonConvert.DeserializeObject<YingAuthenticateRequest>(new StreamReader(yrequest.InputStream).ReadToEnd());              
                YingAuthenticate.yauthenticate(yauthinfo.username, yauthinfo.password);

                String yaccessToken = Guid.NewGuid().ToString("N");
                if (yauthinfo.clientToken == null) yauthinfo.clientToken = Guid.NewGuid().ToString("N");

                getYDataBaseManager().getYConnection().Insert(new zyy_yggdrasil_tokens
                {
                    yemail = yauthinfo.username,

                    yclientToken = yauthinfo.clientToken,
                    yaccessToken = yaccessToken,

                    ytime = getYTimeStamp().TotalMilliseconds
                });

                YingProfile yyy = new YingProfile
                {
                    id = "C7848C4DF2C64049B1823449CB2DEADA".ToLower(),
                    name = "Ying",
                    properties = new List<YingPropertie> { new YingPropertie
                    {
                        name = "textures",
                        value = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonMapper.ToJson(new YingTexture
                        {
                            timestamp = DateTime.Now.Second,
                            profileId = "4F763C75-B060-40AF-8D1C-89F4B364297E".ToLower(),
                            profileName = "YingSkin",
                            textures = new List<YingTextureInfo> { new YingTextureInfo() }

                        }))),
                        signature = null
                    } }
                };

                return new YingResponse(".json", JsonConvert.SerializeObject(new iYingAuthenticate
                {
                    accessToken = yaccessToken,
                    clientToken = yauthinfo.clientToken,

                    availableProfiles = new List<YingProfile> { yyy },
                    selectedProfile = yyy,

                    user = new YingUser
                    {
                        id = Guid.NewGuid().ToString("N"),
                        properties = new List<YingPropertie>
                        {
                            new YingPropertie {
                                name = "preferredLanguage",
                                value = "zh_CN"
                            }
                        }
                    }
                }));






                /*BufferedStream ystream = new BufferedStream(yrequest.InputStream);
                ystream.rE*/

                //getYConsole().sendYMessage(yrequest.InputStream);
            });

            /*yrouter.ypost("/yingyggdrasil/authserver/refresh", (yrequest, yresponse) =>
            {

                YingRefresh yinfo = JsonMapper.ToObject<YingRefresh>(new StreamReader(yrequest.InputStream).ReadToEnd());


                YingResult yresult = YingAuthenticate.yrefresh(yinfo.accessToken, yinfo.clientToken);
                if (yresult.ysuccess)
                {

                    String yaccessToken = Guid.NewGuid().ToString("N");

                    getYDataBaseManager().getYConnection().Insert(new zyy_yggdrasil_tokens
                    {
                        yusername = yresult.ytoken.yusername,

                        yclientToken = yresult.ytoken.yclientToken,
                        yaccessToken = yaccessToken,

                        ytime = getYTimeStamp()
                    });

                    YingRouter.YResponseBuilder(yresponse, new YingResponseInfo
                    {
                        YStatusCode = HttpStatusCode.NoContent,
                        YStatusDescription = $"Ying {HttpStatusCode.NoContent.GetDescription()}",

                        YContentType = YingMimeMapping.YGetMimeType(".json"),
                        YContentEncoding = Encoding.UTF8
                    });


                    yresponse.WriteContent(Encoding.UTF8.GetBytes(JsonMapper.ToJson(new YingRefreshResponse
                    {
                        accessToken = yaccessToken,
                        clientToken = yresult.ytoken.yclientToken,

                        selectedProfile = yyy,

                        user = new YingUser
                        {
                            id = Guid.NewGuid().ToString("N"),
                            properties = new List<YingPropertie>
                                {
                                    new YingPropertie {
                                        name = "preferredLanguage",
                                        value = "zh_CN"
                                    }
                                }
                        }
                    })));



                }




            });

            yrouter.ypost("/yingyggdrasil/authserver/validate", (yrequest, yresponse) =>
            {
                YingValidate yinfo = JsonMapper.ToObject<YingValidate>(new StreamReader(yrequest.InputStream).ReadToEnd());

                YingResult yresult = YingAuthenticate.yvalidate(yinfo.accessToken, yinfo.clientToken);
                if (yresult.ysuccess)
                {
                    YingRouter.YResponseBuilder(yresponse, new YingResponseInfo
                    {
                        YStatusCode = HttpStatusCode.NoContent,
                        YStatusDescription = $"Ying {HttpStatusCode.NoContent.GetDescription()}",

                        YContentType = YingMimeMapping.YGetMimeType(".json"),
                        YContentEncoding = Encoding.UTF8
                    });
                }

                /*
                HttpListenerRequest yrequest = y.Request;
                HttpListenerResponse yresponse = y.Response;
                YingValidate yinfo = JsonMapper.ToObject<YingValidate>(new StreamReader(yrequest.InputStream).ReadToEnd());
                if(yinfo.clientToken == null)
                {
                    if (getYDataBaseManager().getYConnection().Find<zyy_yggdrasil_tokens>(1).yaccessToken == yinfo.accessToken) ;

                }

                YingRouter.YResponseBuilder(y.Response, new YingResponseInfo
                {
                    YStatusCode = HttpStatusCode.Forbidden,
                    YStatusDescription = "Ying Forbidden",

                    YContentType = YingMimeMapping.YGetMimeType(".json"),
                    YContentEncoding = Encoding.UTF8
                });

                yresponse.WriteContent(Encoding.UTF8.GetBytes(JsonMapper.ToJson(new YingError
                {
                    error = "Ying Error",
                    errorMessage = "Ying Invaild Token",
                    cause = "Ying Test Error"
                })));
                *//*


            });

            yrouter.ypost("/yingyggdrasil/authserver/invalidate", (yrequest, yresponse) => { });
            yrouter.ypost("/yingyggdrasil/authserver/signout", (yrequest, yresponse) => { });

            yrouter.ypost("/yingyggdrasil/sessionserver/session/minecraft/join", (yrequest, yresponse) => { });
            yrouter.yget("/yingyggdrasil/sessionserver/session/minecraft/hasJoined?username={username}&serverId={serverId}&ip={ip}", (yrequest, yresponse) => { });
            yrouter.yget("/yingyggdrasil/sessionserver/session/minecraft/profile/{uuid}?unsigned={unsigned}", (yrequest, yresponse) => { });

            yrouter.ypost("/yingyggdrasil/api/profiles/minecraft", (yrequest, yresponse) => { });*/

            //Guid.NewGuid().ToString("N");


            ytimer.Tick += (ysender, yevent) => {
                //if(getYDataBaseManager().getYConnection().)
                TableQuery<zyy_yggdrasil_tokens> yquery = getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_tokens>();
                yquery.ToList().ForEach((y) => {
                    if(y.ytime < getYTimeStamp().TotalMilliseconds - new TimeSpan(14, 6, 0, 4).TotalMilliseconds)
                    {
                        getYDataBaseManager().getYConnection().Delete<zyy_yggdrasil_tokens>(y.yid);
                    }
                });
            };
            ytimer.Start();

        }


        /*public static String getYString(Stream ystream)
        {
            ByteArrayOutputStream
        }*/


    }
}
