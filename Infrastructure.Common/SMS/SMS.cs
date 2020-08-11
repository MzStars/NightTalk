using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Common
{
    public class SMS
    {
        private static readonly string smsuid = ConfigurationManager.AppSettings["SMS_Uid"];
        private static readonly string smspwd = ConfigurationManager.AppSettings["SMS_Pwd"];
        private static readonly string defaultsignature = "壹亿互动";
        private static readonly string sms_website = "http://www.ztsms.cn/sendNSms.do";

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="content">发送内容</param>
        /// <param name="signature">签名</param>
        /// <returns></returns>
        public static bool Send(string mobile, string content, string signature)
        {
            string returnStr = BaseSend(mobile, content, signature);
            return (int.Parse(returnStr) == 0);
        }

        /// <summary>
        /// 发送手机短信返回错误代码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="content">发送内容</param>
        /// <param name="signature">签名</param>
        /// <param name="returnint">短信发送后返回值</param>
        /// <returns></returns>
        public static bool Send(string mobile, string content, string signature, out int returnint)
        {
            string returnStr = BaseSend(mobile, content, signature);
            returnint = int.Parse(returnStr);
            return (returnint == 0);
        }

        private static string BaseSend(string mobile, string content, string signature)
        {
            if (!string.IsNullOrEmpty(signature))
            {
                content += "【" + signature + "】";
            }
            else
            {
                content += "【" + defaultsignature + "】";
            }

            var tkey = DateTime.Now.ToString("yyyyMMddHHmmss");
            var password = MD5(MD5(smspwd) + tkey);
            string encodecontent = HttpUtility.UrlEncode(content, Encoding.GetEncoding("UTF-8"));

            var postData = $"username={smsuid}&password={password}&tkey={tkey}&mobile={mobile}&content={encodecontent}&productid=887362&xh=";

            string returnStr = WebClientHelper.GetString(sms_website, postData, "get", Encoding.GetEncoding("gb2312"), 10000);
            return returnStr.Split(',')[0];
        }

        public static string MD5(string str)
        {
#pragma warning disable CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
#pragma warning restore CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
        }
    }
}
