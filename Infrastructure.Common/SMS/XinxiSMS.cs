using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Common
{
    public class XinxiSMS
    {
        private static readonly string smsuid = ConfigurationManager.AppSettings["XinxiUid"];
        private static readonly string smspwd = ConfigurationManager.AppSettings["XinxiPwd"];
        private static readonly string defaultsignature = "智慧展务";
        private static readonly string website = "http://sms.1xinxi.cn/asmx/smsservice.aspx";

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
            if (string.IsNullOrEmpty(signature))
            {
                signature = defaultsignature;
            }
        
            StringBuilder arge = new StringBuilder();
            arge.AppendFormat("name={0}", smsuid);
            arge.AppendFormat("&pwd={0}", smspwd);
            arge.AppendFormat("&content={0}", "【" + signature + "】" + content);
            arge.AppendFormat("&mobile={0}", mobile);
            arge.Append("&type=pt");

            string resp = PushToWeb(website, arge.ToString(), Encoding.UTF8);
            return resp.Split(',')[0];
        }


        /// HTTP POST方式
        /// POST到的网址
        /// POST的参数及参数值
        /// 编码方式
        /// 
        public static string PushToWeb(string weburl, string data, Encoding encode)
        {
            byte[] byteArray = encode.GetBytes(data);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息：
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader aspx = new StreamReader(response.GetResponseStream(), encode);
            return aspx.ReadToEnd();
        }
    }
}
