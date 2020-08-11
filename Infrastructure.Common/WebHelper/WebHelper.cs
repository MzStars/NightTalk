using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Infrastructure.Common
{
    public static class WebHelper
    {
        #region Post
        public static string Post(string para, string url, int timeout, string contentType = "application/x-www-form-urlencoded")
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;


            //设置最大连接数
            ServicePointManager.DefaultConnectionLimit = 200;
            //设置https验证方式


            /***************************************************************
            * 下面设置HttpWebRequest的相关属性
            * ************************************************************/
            request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.Timeout = timeout * 1000;

            //设置代理服务器
            //WebProxy proxy = new WebProxy();                          //定义一个网关对象
            //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
            //request.Proxy = proxy;

            //设置POST的数据类型和长度
            request.ContentType = contentType;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(para);
            request.ContentLength = data.Length;


            //往服务器写入数据
            reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            //获取服务端返回
            response = (HttpWebResponse)request.GetResponse();

            //获取服务端返回数据
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            result = sr.ReadToEnd().Trim();
            sr.Close();
            return result;

        }
        #endregion

        #region 将http路径图片转为byte字节数据
        public static byte[] Url_To_Byte(string filePath)
        {
            //第一步：读取图片到byte数组
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filePath);

            byte[] bytes;
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                using (MemoryStream mstream = new MemoryStream())
                {
                    int count = 0;
                    byte[] buffer = new byte[1024];
                    int readNum = 0;
                    while ((readNum = stream.Read(buffer, 0, 1024)) > 0)
                    {
                        count = count + readNum;
                        mstream.Write(buffer, 0, readNum);
                    }
                    mstream.Position = 0;
                    using (BinaryReader br = new BinaryReader(mstream))
                    {
                        bytes = br.ReadBytes(count);
                    }
                }
            }
            return bytes;


        }

        #endregion

        /// <summary>
        /// 将对象序列化为JSON字符串，不支持存在循环引用的对象
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="value">动态类型对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJsonString<T>(this T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static Tuple<Stream, string, string> HttpGet(string action)
        {
            HttpWebRequest myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "GET";
            myRequest.Timeout = 20 * 1000;
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            var stream = myResponse.GetResponseStream();
            var ct = myResponse.ContentType;
            if (ct.IndexOf("json") >= 0 || ct.IndexOf("text") >= 0)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    var json = sr.ReadToEnd();
                    return new Tuple<Stream, string, string>(null, ct, json);
                }
            }
            else
            {
                Stream MyStream = new MemoryStream();
                byte[] buffer = new Byte[4096];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    MyStream.Write(buffer, 0, bytesRead);
                MyStream.Position = 0;
                return new Tuple<Stream, string, string>(MyStream, ct, string.Empty);
            }
        }
        /// <summary>
        /// 创建新的文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void DirectoryIsExists(string path)
        {

            if (Directory.Exists(HostingEnvironment.MapPath(path)) == false)
            {
                Directory.CreateDirectory(HostingEnvironment.MapPath(path));
            }
        }
    }
}
