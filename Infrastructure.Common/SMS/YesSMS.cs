using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace Infrastructure.Common
{
    public class YesSMS
    {
        private static readonly string smsuid = ConfigurationManager.AppSettings["YesCodeName"];
        private static readonly string smspwd = ToMD5(ConfigurationManager.AppSettings["YesCodePswd"].ToString()).ToUpper();
        private static readonly string defaultsignature = "智慧展务";
        private static readonly string sms_website = "http://122.152.192.131:28107";

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
                content = "【" + signature + "】" + content;
            }
            else
            {
                content = "【" + defaultsignature + "】" + content;
            }

            string encodecontent = Base64Encode(Encoding.UTF8, content);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"Action\":" + "\"sendsms\",");
            sb.Append("\"UserName\":" + "\"" + smsuid + "\",");
            sb.Append("\"Password\":" + "\"" + smspwd + "\",");
            sb.Append("\"Mobile\":" + "\"" + mobile + "\",");
            sb.Append("\"Message\":" + "\"" + encodecontent + "\"");
            sb.Append("}");
            var str = sb.ToString();
            string res = HttpPostData(sms_website, sb.ToString());
            var yesReturn =JsonHelper.DeserializeJsonToObject<YesReturn>(res);
            return yesReturn.Status.ToString();
        }

        #region post请求
        public static string HttpPostData(string url, string param)
        {
            var result = string.Empty;
            //注意提交的编码 这边是需要改变的 这边默认的是Default：系统当前编码
            byte[] postData = Encoding.UTF8.GetBytes(param);
            // 设置提交的相关参数 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            Encoding myEncoding = Encoding.UTF8;
            request.Method = "POST";
            request.KeepAlive = false;
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Host = "120.24.241.49";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3236.0 Safari/537.36";

            request.ContentLength = postData.Length;

            // 提交请求数据 
            System.IO.Stream outputStream = request.GetRequestStream();
            outputStream.Write(postData, 0, postData.Length);
            outputStream.Close();

            HttpWebResponse response;
            Stream responseStream;
            StreamReader reader;
            string srcString;
            response = request.GetResponse() as HttpWebResponse;
            responseStream = response.GetResponseStream();
            reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
            srcString = reader.ReadToEnd();
            result = srcString;   //返回值赋值
            reader.Close();

            return result;
        }
        #endregion

        #region  Base64加密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
        #endregion

        #region MD5加密

        public static string ToMD5(string encryptString)
        {
            byte[] result = Encoding.Default.GetBytes(encryptString);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string encryptResult = BitConverter.ToString(output).Replace("-", "");
            return encryptResult;
        }
        #endregion
    }
}
