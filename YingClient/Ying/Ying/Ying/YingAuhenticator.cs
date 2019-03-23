using KMCCC.Authentication;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ying.YingEvents;
using Ying.YingWebsocket.YingStructs;

namespace Ying
{
    public class YingAuhenticator : YingYing, IAuthenticator
    {

        /// <summary>
        ///     电子邮件地址
        /// </summary>
        public string YEmail { get; }

        /// <summary>
        ///     密码
        /// </summary>
        public string YPassword { get; }

        /// <summary>
        /// </summary>
        public string YAccessToken { get; }

        /// <summary>
        /// </summary>
        public string YClientToken { get; }

        /// <summary>
        ///     返回Yggdrasil验证器类型
        /// </summary>

        public string Type => "Ying Auhenticator";

        public YingLoginStruct YResult { get; set; } = new YingLoginStruct();

        public YingAuhenticator()
        {
        }

        public AuthenticationInfo Do()
        {
            if (this.YResult.yaccessToken == null)
            {
                return new AuthenticationInfo
                {
                    Error = this.YResult.ymessage
                };
            }
            else
            {
                return new AuthenticationInfo
                {
                    AccessToken = Guid.Parse(this.YResult.yaccessToken),
                    UserType = "Mojang",
                    DisplayName = "Ying",
                    Properties = "{}",
                    UUID = Guid.Parse(this.YResult.yuser.yopenid)
                };
            }
        }

        public Task<AuthenticationInfo> DoAsync(CancellationToken ytoken)
        {
            return Task<AuthenticationInfo>.Factory.StartNew(Do, ytoken);
        }
    }
}
