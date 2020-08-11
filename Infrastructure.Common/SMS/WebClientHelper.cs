using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Common
{
    /// <summary>
    /// 请求页面并返回信息的操作类。
    /// </summary>
    public sealed class WebClientHelper
    {

        /// <summary>
        /// 构造函数。
        /// </summary>
        private WebClientHelper()
        {
        }

        /// <summary>
        /// 格式化请求传送的数据。
        /// </summary>
        /// <param name="list">数据列表。</param>
        /// <param name="encoding">请求编码。</param>
        /// <returns>数据字符串。</returns>
        public static string FormatRequestData(NameValueCollection list, Encoding encoding)
        {
            if ((list == null) || (list.Count == 0))
            {
                return string.Empty;
            }
            string[] strArray = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                strArray[i] = string.Format("{0}={1}", list.Keys[i], HttpUtility.UrlEncode(list[i], encoding));
            }
            return string.Join("&", strArray);
        }

        /// <summary>
        /// 反格式化请求传送的数据。
        /// </summary>
        /// <param name="str">请求字符串。</param>
        /// <param name="encoding">请求编码。</param>
        /// <returns>数据字符串。</returns>
        public static NameValueCollection GetRequestList(string str, Encoding encoding)
        {
            string[] strArray = str.Split(new char[] { '&' });
            NameValueCollection values = new NameValueCollection(strArray.Length);
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray.Length > 0)
                {
                    int index = strArray[i].IndexOf("=");
                    if (index == -1)
                    {
                        values.Add("", strArray[i]);
                    }
                    else
                    {
                        values.Add(str.Substring(0, index), (str.Length > (index + 1)) ? str.Substring(index + 1, 0) : "");
                    }
                }
            }
            return values;
        }

        /// <summary>
        /// 使用POST方式请求指定的网址并返回响应信息。
        /// </summary>
        /// <param name="url">网址。</param>
        /// <param name="list">请求值。</param>
        /// <param name="method">请求方式。</param>
        /// <param name="encoding">请求编码。</param>
        /// <returns>响应信息内容字符串。</returns>
        public static string GetString(string url, NameValueCollection list, string method, Encoding encoding)
        {
            return GetString(url, FormatRequestData(list, encoding), method, encoding, 60 * 1000);
        }

        /// <summary>
        /// 使用POST方式请求指定的网址并返回响应信息。
        /// </summary>
        /// <param name="url">网址。</param>
        /// <param name="postData">请求值。</param>
        /// <param name="method">请求方式。</param>
        /// <param name="encoding">请求编码。</param>
        /// <returns>响应信息内容字符串。</returns>
        public static string GetString(string url, string postData, string method, Encoding encoding, int timeout)
        {
            string str = string.Empty;
            using (WebResponse response = GetWebResponse(url, postData, method, encoding, timeout))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    str = FileHelp.ReadContent(stream, encoding);
                    stream.Close();
                }
                response.Close();
            }
            return str;
        }

        /// <summary>
        /// 使用POST方式请求指定的网址并返回响应信息。
        /// </summary>
        /// <param name="url">网址。</param>
        /// <param name="list">请求值。</param>
        /// <param name="method">请求方式。</param>
        /// <param name="encoding">请求编码。</param>
        /// <returns></returns>
        public static WebResponse GetWebResponse(string url, NameValueCollection list, string method, Encoding encoding, int timeout)
        {
            return GetWebResponse(url, FormatRequestData(list, encoding), method, encoding, timeout);
        }

        /// <summary>
        /// 使用POST方式请求指定的网址并返回响应信息。
        /// </summary>
        /// <param name="url">网址。</param>
        /// <param name="postData">请求值。</param>
        /// <param name="method">请求方式。</param>
        /// <param name="encoding">请求编码。</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static WebResponse GetWebResponse(string url, string postData, string method, Encoding encoding, int timeout)
        {
            if (System.String.Compare(method, "get", System.StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (url.IndexOf("?", System.StringComparison.Ordinal) == -1)
                {
                    url = url + "?" + postData;
                }
                else
                {
                    url = url + "&" + postData;
                }
                postData = string.Empty;
                method = "GET";
            }
            else
            {
                method = "POST";
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                return null;
            }
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = timeout;
            if (System.String.Compare(method, "post", System.StringComparison.OrdinalIgnoreCase) == 0)
            {
                byte[] bytes = encoding.GetBytes(postData);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
            }
            return request.GetResponse();
        }


        /// <summary>
        /// 使用POST方式请求指定的网址并返回响应信息。
        /// </summary>
        /// <param name="url">网址。</param>
        /// <param name="postData">请求值。</param>
        /// <param name="method">请求方式。</param>
        /// <param name="encoding">请求编码。</param>
        /// <returns></returns>
        public void GetAsyncWebResponse(string url, string postData, string method, Encoding encoding, int timeout, AsyncCallback callback, object state)
        {
            if (string.Compare(method, "get", true) == 0)
            {
                if (url.IndexOf("?") == -1)
                {
                    url = url + "?" + postData;
                }
                else
                {
                    url = url + "&" + postData;
                }
                postData = string.Empty;
                method = "GET";
            }
            else
            {
                method = "POST";
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = timeout;

            if (string.Compare(method, "post", true) == 0)
            {
                byte[] bytes = encoding.GetBytes(postData);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
            }

            request.BeginGetResponse(callback, state);
        }
    }
}
