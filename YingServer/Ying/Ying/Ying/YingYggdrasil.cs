using LitJson;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Threading;
using WebSocketSharp;
using WebSocketSharp.Net;
using Ying.YingDataBase.YingDataStructs;
using Ying.YingExceptions;
using Ying.YingUtil;
using Ying.YingUtils;
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
        public Double timestamp { get; set; }
        public String profileId { get; set; }
        public String profileName { get; set; }
        public Dictionary<String, YingTextureInfo> textures { get; set; }
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

    public struct YingSignout
    {
        public String username { get; set; }
        public String password { get; set; }
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

    public struct YingJoin
    {
        public String accessToken { get; set; }
        public String selectedProfile { get; set; }
        public String serverId { get; set; }
        public String yip { get; set; }
        public TimeSpan ytime { get; set; }
    }


    public class YingYggdrasil : YingYing
    {



        private static YingRouter yrouter = getYServer().getYRouter();


        /*public YingYggdrasil()
        {

        }*/

        private static DispatcherTimer ytimer = new DispatcherTimer
        {
            Interval = new TimeSpan(6, 0, 4),
            IsEnabled = true,
            Tag = "Ying Token Clear"
        };

        private static Dictionary<String, YingJoin> yjoin = new Dictionary<String, YingJoin>();



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
                    skinDomains = new List<String> { "zyy.com:6040", "iying.top:6040" },
                    signaturePublickey = "-----BEGIN PUBLIC KEY-----\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAwg1953V0NHFuE+vClZPp\nmFerk/xmF5ZvnctBQG9wbchhHHHPAiReBpeAmlLXF3nO8NEWyO9fXcUObv4/n8xi\n+QGE0DWwP6h+e/26A68bJsYBbYcxCEBAM7AxKEXyu++EVt3z5VltvDwT9j5V1RzE\neymeyobeJuOpygYMqTu90H4SwVIDJ5Pojjqq0XHrD76X5A0wkoyacXQ0stjNrOtU\nUAOKs4um7+7vLSCrN0ZrnXyBfMxCphyRxcnjLC+NMud68pyx2AiyaVwvPMTfPXt/\noT5VmLDNqzwCaL+uaQFsGmwcwUU3z5DmOp0hNKPop5YedGlD9H4ntNpjk15L7p/g\n9m9UELN63k95VpFCRxt6LaoQ3yd+crbdpoydtlM2JII7f/fBnhvbMzlVmR4qBcDs\nocxVjTd/iNwseGVK9SnN82fFSmKIDScJw8jkmh6St5VTNACxcw+oNQfppfP1YSTm\nIbQlMyGLCaUNwKkHorCoS/paRa9fYi3cuO3PhIdeaOqcSlQHXlle+3dh46rL84LU\nMUcmQv8BO6X9AU5nXrxbOTXn9ezzEbMJW+BQ6+Ureyp/KtJwQQFTZa/Rx/vdNgIJ\nMnCPUod0GT042lA0gICirgy1oPFGwjZpIMmumprJSwUApIjebvtq5jqwEaX9gdZU\nPQsuIGAvhszLY073Ix8W+akCAwEAAQ==\n-----END PUBLIC KEY-----\n"
                }
            ))).ypost("/yingyggdrasil/authserver/authenticate", (yrequest) =>
            {

                YingAuthenticateRequest yauthinfo = JsonConvert.DeserializeObject<YingAuthenticateRequest>(new StreamReader(yrequest.InputStream).ReadToEnd());
                YingAuthenticateResult yresult = YingAuthenticate.yauthenticate(yauthinfo.username, yauthinfo.password);
                if (!yresult.ysuccess) throw YingException.Ying(yresult.ytype);

                String yaccessToken = Guid.NewGuid().ToString("N");
                if (yauthinfo.clientToken == null) yauthinfo.clientToken = Guid.NewGuid().ToString("N");

                getYDataBaseManager().getYConnection().Insert(new zyy_yggdrasil_tokens
                {
                    yemail = yauthinfo.username,

                    yclientToken = yauthinfo.clientToken,
                    yaccessToken = yaccessToken,

                    ytime = getYTimeStamp().TotalMilliseconds
                });

                String[] yprofiles = JsonConvert.DeserializeObject<String[]>(yresult.yuser.yprofiles);
                String yprofileId = yprofiles.FirstOrDefault();

                zyy_yggdrasil_profile yprofile = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_profile>()
                                                  where y.yyid == yprofileId
                                                  select y).FirstOrDefault();

                zyy_yggdrasil_texture ytexture = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_texture>()
                                                  where y.ytextureid == yprofile.ytextureid
                                                  select y).FirstOrDefault();

                YingProfile yyy = new YingProfile
                {
                    id = yprofile.yyid,
                    name = yprofile.yname,
                    properties = new List<YingPropertie> { new YingPropertie
                        {
                            name = "textures",
                            value = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonMapper.ToJson(new YingTexture
                            {
                                timestamp = ytexture.ytimestamp,
                                profileId = yprofile.yyid,
                                profileName = yprofile.yname,
                                textures = new Dictionary<String, YingTextureInfo> { {"SKIN", new YingTextureInfo() { url = ytexture.yskin, metadata = new Dictionary<String, String> { {"model", ytexture.yskinmetadata } } } } }
                             }))),
                            signature = null
                        }
                    }
                };

                return new YingResponse(".json", JsonConvert.SerializeObject(new iYingAuthenticate
                {
                    accessToken = yaccessToken,
                    clientToken = yauthinfo.clientToken,

                    availableProfiles = new List<YingProfile> { yyy },
                    selectedProfile = yyy,

                    user = new YingUser
                    {
                        id = yresult.yuser.yopenid,
                        properties = new List<YingPropertie>
                        {
                            new YingPropertie {
                                name = "preferredLanguage",
                                value = "zh_CN"
                            }
                        }
                    }
                }));
            }).ypost("/yingyggdrasil/authserver/refresh", (yrequest) =>
            {
                YingRefresh yinfo = JsonMapper.ToObject<YingRefresh>(new StreamReader(yrequest.InputStream).ReadToEnd());
                YingAuthenticateResult yresult = YingAuthenticate.yrefresh(yinfo.accessToken, yinfo.clientToken);
                if (yresult.ysuccess)
                    throw new YingHttpException((int)HttpStatusCode.NoContent, $"Ying {HttpStatusCode.NoContent.GetDescription()}");
                else
                {
                    String[] yprofiles = JsonConvert.DeserializeObject<String[]>(yresult.yuser.yprofiles);
                    String yprofileId = yprofiles.FirstOrDefault();

                    zyy_yggdrasil_profile yprofile = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_profile>()
                                                      where y.yyid == yprofileId
                                                      select y).FirstOrDefault();

                    zyy_yggdrasil_texture ytexture = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_texture>()
                                                      where y.ytextureid == yprofile.ytextureid
                                                      select y).FirstOrDefault();



                    return new YingResponse(".json", JsonConvert.SerializeObject(new YingRefreshResponse
                    {
                        accessToken = yresult.ytoken.yaccessToken,
                        clientToken = yresult.ytoken.yclientToken,

                        selectedProfile = new YingProfile
                        {
                            id = yprofile.yyid,
                            name = yprofile.yname,
                            properties = new List<YingPropertie> { new YingPropertie
                                {
                                name = "textures",
                                value = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonMapper.ToJson(new YingTexture
                                {
                                    timestamp = ytexture.ytimestamp,
                                    profileId = yprofile.yyid,
                                    profileName = yprofile.yname,
                                    textures = new Dictionary<String, YingTextureInfo> { {"SKIN", new YingTextureInfo() { url = ytexture.yskin, metadata = new Dictionary<String, String> { {"model", ytexture.yskinmetadata } } } } }
                                 }))),
                                signature = null
                                }
                            }
                        },

                        user = new YingUser
                        {
                            id = yresult.yuser.yopenid,
                            properties = new List<YingPropertie>
                        {
                            new YingPropertie {
                                name = "preferredLanguage",
                                value = "zh_CN"
                            }
                        }
                        }
                    }));
                }
            }).ypost("/yingyggdrasil/authserver/validate", (yrequest) =>
            {
                YingValidate yinfo = JsonMapper.ToObject<YingValidate>(new StreamReader(yrequest.InputStream).ReadToEnd());

                YingAuthenticateResult yresult = YingAuthenticate.yvalidate(yinfo.accessToken, yinfo.clientToken);
                if (yresult.ysuccess)
                {
                    throw new YingHttpException((int) HttpStatusCode.NoContent, $"Ying {HttpStatusCode.NoContent.GetDescription()}");
                }
                throw YingException.Ying(YingExceptionTypes.YingInvalidToken);

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
                */


            }).ypost("/yingyggdrasil/authserver/signout", (yrequest) => {
                YingSignout yinfo = JsonMapper.ToObject<YingSignout>(new StreamReader(yrequest.InputStream).ReadToEnd());

                if (YingAuthenticate.ysignout(yinfo.username, yinfo.password))
                    throw new YingHttpException((int)HttpStatusCode.NoContent, $"Ying {HttpStatusCode.NoContent.GetDescription()}");
                else
                    throw YingException.Ying(YingExceptionTypes.YingInvalidUsernameOrPassword);
            }).ypost("/yingyggdrasil/authserver/invalidate", (yrequest) => {
                YingValidate yinfo = JsonMapper.ToObject<YingValidate>(new StreamReader(yrequest.InputStream).ReadToEnd());

                YingAuthenticate.yinvalidate(yinfo.accessToken, yinfo.clientToken);
                throw new YingHttpException((int)HttpStatusCode.NoContent, $"Ying {HttpStatusCode.NoContent.GetDescription()}");
            }).ypost("/yingyggdrasil/sessionserver/session/minecraft/join", (yrequest) => {
                YingJoin yyjoin = JsonMapper.ToObject<YingJoin>(new StreamReader(yrequest.InputStream).ReadToEnd());
                yyjoin.yip = yrequest.UserHostAddress;
                yyjoin.ytime = getYTimeStamp();
                yjoin.Add(yyjoin.serverId, yyjoin);
                throw new YingHttpException((int)HttpStatusCode.NoContent, $"Ying {HttpStatusCode.NoContent.GetDescription()}");
            }).yget("/yingyggdrasil/sessionserver/session/minecraft/hasJoined*", (yrequest) => {
                NameValueCollection yquery = yrequest.QueryString;
                String yusername = yquery.Get("username");
                String yserverId = yquery.Get("serverId");
                String yip = yquery.Get("ip");
                if (yjoin.ContainsKey(yserverId)) {
                    String yselectedProfile = yjoin[yserverId].selectedProfile;
                    zyy_yggdrasil_profile yprofile = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_profile>()
                                                      where y.yyid == yselectedProfile
                                                      select y).FirstOrDefault();

                    zyy_yggdrasil_texture ytexture = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_texture>()
                                                      where y.ytextureid == yprofile.ytextureid
                                                      select y).FirstOrDefault();

                    String yvalue = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonMapper.ToJson(new YingTexture
                    {
                        timestamp = ytexture.ytimestamp,
                        profileId = yprofile.yyid,
                        profileName = yprofile.yname,
                        textures = new Dictionary<String, YingTextureInfo> { { "SKIN", new YingTextureInfo() { url = ytexture.yskin, metadata = new Dictionary<String, String> { { "model", ytexture.yskinmetadata } } } } }
                    })));
                    return new YingResponse(".json", JsonConvert.SerializeObject(new YingProfile
                    {
                        id = yprofile.yyid,
                        name = yprofile.yname,
                        properties = new List<YingPropertie> { new YingPropertie
                        {
                            name = "textures",
                            value = yvalue,
                            signature = YingRSA.EncryptString(yvalue, "-----BEGIN RSA PRIVATE KEY-----\n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\n-----END RSA PRIVATE KEY-----\n")
                        }
                    }
                    }));
                }
                else
                {
                    throw new YingHttpException((int)HttpStatusCode.NoContent, $"Ying {HttpStatusCode.NoContent.GetDescription()}");
                }
            }, true).yget("/yingyggdrasil/sessionserver/session/minecraft/profile/([a-zA-Z0-9]+)", (yrequest) =>
            {
                String yuuid = yrequest.RawUrl.Replace("/yingyggdrasil/sessionserver/session/minecraft/profile/", "");

                zyy_yggdrasil_profile yprofile = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_profile>()
                                                  where y.yyid == yuuid
                                                  select y).FirstOrDefault();

                zyy_yggdrasil_texture ytexture = (from y in getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_texture>()
                                                  where y.ytextureid == yprofile.ytextureid
                                                  select y).FirstOrDefault();
                return new YingResponse(".json", JsonConvert.SerializeObject(new YingProfile
                {
                    id = yprofile.yyid,
                    name = yprofile.yname,
                    properties = new List<YingPropertie> { new YingPropertie
                        {
                            name = "textures",
                            value = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonMapper.ToJson(new YingTexture
                            {
                                timestamp = ytexture.ytimestamp,
                                profileId = yprofile.yyid,
                                profileName = yprofile.yname,
                                textures = new Dictionary<String, YingTextureInfo> { {"SKIN", new YingTextureInfo() { url = ytexture.yskin, metadata = new Dictionary<String, String> { {"model", ytexture.yskinmetadata } } } } }
                            }))),
                            signature = null
                        }
                    }
                }));
            }, true).yget("/yingyggdrasil/ytextures/([a-zA-Z0-9]+)", (yrequest) =>
            {
                String yuuid = yrequest.RawUrl.Replace("/yingyggdrasil/ytextures/", "");
                Byte[] ytexture = null;
                try {
                    ytexture = File.ReadAllBytes(Path.Combine(getYFiles().getYTextures().FullName, $"{yuuid}.png"));
                } catch(Exception yexception) {
                    throw new YingHttpException(404, "Ying Textures Not Found");
                }
                
                return new YingResponse(".png", ytexture);
            }, true);


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


                ytimer.Tick += (ysender, yevent) =>
            {
                //if(getYDataBaseManager().getYConnection().)
                TableQuery<zyy_yggdrasil_tokens> yquery = getYDataBaseManager().getYConnection().Table<zyy_yggdrasil_tokens>();
                yquery.ToList().ForEach((y) =>
                {
                    if (y.ytime < getYTimeStamp().TotalMilliseconds - new TimeSpan(14, 6, 0, 4).TotalMilliseconds)
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
