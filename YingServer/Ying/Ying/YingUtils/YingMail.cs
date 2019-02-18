using Mafly.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingUtils
{
    class YingMail : YingYing
    {

        private static Mail ymail = new Mail(new MailConfig
        {
            Host = "smtp.qq.com",
            Port = 25,
            User = "1519367710",
            Password = "fbtyxgfuyihwfgjh",
            From = "iZhangYingYing@qq.com",
            DisplayName = "一只小颖儿",
            IsHtml = true,
            EnableSsl = false
        });

        public static void ysend(String yreceiver, String ybody) { 
            try
            {
                ymail.Send(new MailInfo
                {
                    Receiver = yreceiver,
                    ReceiverName = null,
                    Replay = "1519367710@qq.com",
                    CC = null,
                    Subject = $"Ying · {getYSettings().yname} · 注册验证码",
                    Body = ybody
                });
            }
            catch (Exception y)
            {
                getYConsole().sendYMessage(y.Message);
                getYConsole().sendYMessage(y.StackTrace);
            }
        }
    }
}
