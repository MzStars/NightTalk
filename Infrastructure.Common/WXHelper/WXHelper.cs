using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.HttpUtility;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Common
{
    /// <summary>
    /// 微信帮助类
    /// </summary>
    public static class WXHelper
    {
        private static string appId = ConfigHelper.AppSettings("WecharAppid");
        private static string appSecret = ConfigHelper.AppSettings("WecharAppSecret");
        private static string redisPassword = ConfigHelper.AppSettings("RedisPassword");
        private static string redisHost = ConfigHelper.AppSettings("RedisHost");
        private static int redisPort = int.Parse(ConfigHelper.AppSettings("RedisPort"));

        /// <summary>
        /// 获取小程序页面的小程序码
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="stream">储存小程序码的流</param>
        /// <param name="path">不能为空，最大长度 128 字节（如：pages/index?query=1。注：pages/index 需要在 app.json 的 pages 中定义）</param>
        /// <param name="width">小程序码的宽度</param>
        /// <param name="auto_color">自动配置线条颜色</param>
        /// <param name="timeOut">请求超时时间</param>
        /// <returns></returns>
        public static WxJsonResult GetWxaCode(Stream stream, string path, int width = 430, bool auto_color = false, int timeOut = Config.TIME_OUT)
        {

            var accessTokenOrAppId = WXHelper.GetXCXAccessToken(appId, appSecret);

            return ApiHandlerWapper.TryCommonApi(accessToken =>
            {
                const string urlFormat = "https://api.weixin.qq.com/wxa/getwxacode?access_token={0}";
                var url = string.Format(urlFormat, accessToken);

                var data = new { path = path, width = width, is_hyaline = false, auto_color = false };
                Post.Download(url, SerializerHelper.GetJsonString(data), stream);

                return new WxJsonResult()
                {
                    errcode = ReturnCode.请求成功
                };
            }, accessTokenOrAppId);
        }

        public static WxJsonResult Temp(Stream stream, string path, int r = 0, int g = 0, int b = 0, int width = 430, bool auto_color = false, int timeOut = Config.TIME_OUT)
        {

            var accessTokenOrAppId = WXHelper.GetXCXAccessToken(appId, appSecret);

            return ApiHandlerWapper.TryCommonApi(accessToken =>
            {
                const string urlFormat = "https://api.weixin.qq.com/wxa/getwxacode?access_token={0}";
                var url = string.Format(urlFormat, accessToken);

                var data = new { path = path, width = width, auto_color = false, is_hyaline = false };
                Post.Download(url, SerializerHelper.GetJsonString(data), stream);

                return new WxJsonResult()
                {
                    errcode = ReturnCode.请求成功
                };
            }, accessTokenOrAppId);
        }


        /// <summary>
        /// 检查评论
        /// </summary>
        /// <param name="content">评论内容</param>
        /// <returns></returns>
        public static WxJsonResult MsgCheck(string content)
        {

            var accessTokenOrAppId = WXHelper.GetXCXAccessToken(appId, appSecret);
            try
            {
                return ApiHandlerWapper.TryCommonApi(accessToken =>
                {
                    var data = new
                    {
                        content = content
                    };
                    const string urlFormat = "https://api.weixin.qq.com/wxa/msg_sec_check?access_token={0}";
                    var url = string.Format(urlFormat, accessToken);
                    return Senparc.Weixin.CommonAPIs.CommonJsonSend.Send(null, url, data);
                }, accessTokenOrAppId);
            }
            catch (Senparc.Weixin.Exceptions.ErrorJsonResultException e)
            {
                return e.JsonResult;
            }

        }

        /// <summary>
        /// 获取微信小程序码
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="scenes"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetWxImage(string pages, string scenes, string fileName)
        {

            var accessToken = WXHelper.GetXCXAccessToken(appId, appSecret);
            if (accessToken == "error")
            {
                throw new Exception("accessToken出错");
            }
            string url = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + accessToken;
            var parm = new
            {
                page = pages,
                //"pages/index/index",
                scene = scenes
                //= "id=1"
            };
            var DataJson = JsonConvert.SerializeObject(parm);
            var ret = PostMoths(url, DataJson, fileName);
            return ret;//返回图片地址
        }

        /// <summary>
        /// Post请求微信小程序码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pages"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string PostMoths(string url, string pages, string fileName)
        {


            //在文件名前面加上时间，以防重名
            string imgName = fileName + ".jpg";
            //文件存储相对于当前应用目录的虚拟目录
            var path = "/Upload/WecharImg/";
            //获取相对于应用的基目录,创建目录
            WebHelper.DirectoryIsExists(path);
            //物理路径
            var fileUrl = HttpContext.Current.Server.MapPath(path + imgName);//图片地址
            //返回路径
            var returnUrl = path + imgName;
            //是否创建了
            if (File.Exists(fileUrl))
            {
                return returnUrl;
            }

            try
            {
                string ret = string.Empty;
                string strURL = url;
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                string paraUrlCoded = pages;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();//返回图片数据流
                byte[] tt = StreamToBytes(s);//将数据流转为byte[]
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var result = sr.ReadToEnd().Trim();
                if (result.Contains("40001"))
                {
                    WXHelper.GetXCXAccessToken(appId, appSecret, true);
                    Log.Error("生成小程序码失败，失败原因：", "accessToken过期或无效");
                    return "";
                }
                if (result.Contains("errcode"))
                {
                    Log.Error("生成小程序码失败，原因：", result.Substring(result.IndexOf("errcode")));
                    return "";
                }

                System.IO.File.WriteAllBytes(fileUrl, tt);//讲byte[]存储为图片
                return returnUrl;
            }
            catch (Exception ex)
            {

                Log.Error("生成小程序码失败，原因：", ex.Message.ToString());
                return "";
            }
        }

        /// <summary>
        /// 获取小程序AccessToken
        /// </summary>
        /// <param name="error"></param>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <param name="RedisHost"></param>
        /// <param name="RedisPort"></param>
        /// <param name="RedisPassword"></param>
        /// <returns></returns>
        public static string GetXCXAccessToken(string appid, string appsecret, bool error = false)
        {
            try
            {
                var accessToken = "";
                HttpClient httpClient = new HttpClient();
                using (var redisPublisher = new RedisClient(redisHost, redisPort, redisPassword))
                {
                    accessToken = redisPublisher.Get<string>("NightTalk:WXAccessToken");
                    if (string.IsNullOrEmpty(accessToken))//如果缓存没有accessToken,获取一个添加缓存
                    {
                        //var accessToken = AccessTokenContainer.TryGetAccessToken(_appId, _appSecret);//
                        var url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;
                        var outputs = "";
                        var jt_acts = HttpClientHelper(httpClient, url, out outputs);
                        try
                        {
                            accessToken = jt_acts["access_token"].ToString();
                        }
                        catch (Exception)
                        {
                            throw new Exception(jt_acts.ToString());
                        }
                        //accessToken 7200秒过期
                        TimeSpan ts = TimeSpan.FromSeconds(6500);//7000s
                        redisPublisher.Set("NightTalk:WXAccessToken", accessToken, ts);
                    }
                    if (error)
                    {
                        //重新获取
                        var url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;
                        var outputs = "";
                        var jt_acts = HttpClientHelper(httpClient, url, out outputs);
                        try
                        {


                            accessToken = jt_acts["access_token"].ToString();
                        }
                        catch (Exception)
                        {

                            throw new Exception(jt_acts.ToString());
                        }
                        //accessToken 7200秒过期
                        redisPublisher.Del("NightTalk:WXAccessToken");
                        TimeSpan ts = TimeSpan.FromSeconds(6000);//7000s
                        redisPublisher.Set("NightTalk:WXAccessToken", accessToken, ts);
                    }
                }
                return accessToken;
            }
            catch (Exception ex)
            {
                Log.Error(" 获取WXAccessToken出错", ex.Message.ToString());
                throw new Exception("获取WXAccessToken失败");
            }
        }

        /// <summary>
        /// 删除 AccessToken缓存
        /// </summary>
        public static void RemoveAccessToken()
        {
            using (var redisPublisher = new RedisClient(redisHost, redisPort, redisPassword))
            {
                redisPublisher.Del("NightTalk:WXAccessToken");
            }
        }

        /// <summary>
        /// 解密所有消息的基础方法
        /// </summary>
        /// <param name="sessionKey">储存在 SessionBag 中的当前用户 会话 SessionKey</param>
        /// <param name="encryptedData">接口返回数据中的 encryptedData 参数</param>
        /// <param name="iv">接口返回数据中的 iv 参数，对称解密算法初始向量</param>
        /// <returns></returns>
        public static string DecodeEncryptedData(string sessionKey, string encryptedData, string iv)
        {
            var aesCipher = Convert.FromBase64String(encryptedData);
            var aesKey = Convert.FromBase64String(sessionKey);
            var aesIV = Convert.FromBase64String(iv);

            var result = AES_Decrypt(encryptedData, aesIV, aesKey);
            var resultStr = Encoding.UTF8.GetString(result);
            return resultStr;
        }

        public static string GetSessionKey(string code, string iv, string grant_type = "authorization_code")
        {
            //向微信服务端 使用登录凭证 code 获取 session_key 和 openid   
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appId + "&secret=" + appSecret + "&js_code=" + code + "&grant_type=" + grant_type;
            string type = "utf-8";
            string j = GetUsersHelper.GetUrltoHtml(url, type);//获取微信服务器返回字符串  
            WXResult res = new WXResult();
            JObject jo = new JObject();
            try
            {
                //将字符串转换为json格式  
                jo = (JObject)JsonConvert.DeserializeObject(j);
                if (jo.ToString().Contains("40163"))
                {
                    throw new Exception("请勿重复点击");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("神秘消失的" + ex.Message + j);
            }

            try
            {
                var sessionkey = jo["session_key"].ToString();

                return sessionkey;
            }
            catch (Exception)
            {
                //微信服务器验证失败  
                res.errcode = jo["errcode"].ToString();
                res.errmsg = jo["errmsg"].ToString();
                throw new Exception("请重新登录! 在获取openid或者seeion_key 出错，" + res.errmsg);
            }

        }

        #region 私有方法

        private static byte[] AES_Decrypt(String Input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 128;//原始：256
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Key;
            aes.IV = Iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    //        cs.Read(decryptBytes, 0, decryptBytes.Length);
                    //        cs.Close();
                    //        ms.Close();

                    byte[] xXml = Convert.FromBase64String(Input);
                    byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                    Array.Copy(xXml, msg, xXml.Length);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = decode2(ms.ToArray());
            }
            return xBuff;
        }

        private static byte[] decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }

        private static JToken HttpClientHelper(HttpClient httpClient, string url, out string result)
        {
            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            result = response.Content.ReadAsStringAsync().Result;
            return parseResult(result);
        }

        private static JToken parseResult(string result)
        {
            int num1 = result.IndexOf("(");
            if (num1 > -1)
            {
                result = result.Substring(num1 + 1, result.IndexOf(")") - num1 - 1);
            }
            JToken jt = JObject.Parse(result) as JToken;
            return jt;
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }
            return bytes.ToArray();
        }
        #endregion
    }
}
