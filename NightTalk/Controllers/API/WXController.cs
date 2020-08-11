using ServiceStack.Redis;
using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using NightTalk.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Service;

namespace NightTalk.Controllers
{
    /// <summary>
    /// 微信 Controller
    /// </summary>
    public class WXController : BaseApi
    {
        private string RedisPassword = ConfigurationManager.AppSettings["RedisPassword"];
        private string RedisHost = ConfigurationManager.AppSettings["RedisHost"];
        private string RedisPort = ConfigurationManager.AppSettings["RedisPort"];
        private string WecharAppid = ConfigurationManager.AppSettings["WecharAppid"];
        private string WecharAppSecret = ConfigurationManager.AppSettings["WecharAppSecret"];
        private static TimeSpan TowDayTime = TimeSpan.FromDays(3);//3天 后过期

        private readonly WeCharUserInfoService _weCharUserInfoService;

        public WXController(WeCharUserInfoService weCharUserInfoService)
        {
            _weCharUserInfoService = weCharUserInfoService;
        }

        /// <summary>
        /// 授权 获取用户信息并保存
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/wx/GetWxUserInfo")]
        public JsonResult<object> GetWxUserInfo(GetWxUserInfoRequest request)
        {
            if (request == null)
            {
                return JsonError("请求数据错误");
            }
            if (string.IsNullOrEmpty(request.Code))
            {
                throw new Exception("Code为空,请重新登录");
            }

            //获取sessionKey
            var sessionkey = WXHelper.GetSessionKey(request.Code, request.Iv);
            //获取解密数据
            //string result = GetUsersHelper.AESDecrypt(encryptedData, sessionkey, iv);
            string result = WXHelper.DecodeEncryptedData(sessionkey, request.EncryptedData, request.Iv);
            //转换数据类型  
            WXDecodeInfo userInfo = JsonConvert.DeserializeObject<WXDecodeInfo>(result);
            var wxUserInfo = _weCharUserInfoService.GetData(userInfo.unionId);
            userInfo.ShareCode = wxUserInfo?.ShareCode;
            if (wxUserInfo == null)
            {
                userInfo.ShareCode = _weCharUserInfoService.CreateGetCode(userInfo.ToDomainModel());
            }
            else
            {
                userInfo.ShareCode = wxUserInfo.ShareCode;
                userInfo.Phone = wxUserInfo.Phone;
            }

            using (var redisPublisher = new RedisClient(RedisHost, int.Parse(RedisPort), RedisPassword))
            {
                string apiKey = Guid.NewGuid().ToString().Replace("-", "");
                //存储session_key

                BaseModel baseInfo = new BaseModel()
                {
                    NickName = userInfo.nickName,
                    OpenID = userInfo.openId,
                    UnionID = userInfo.unionId,
                    Phone = userInfo.Phone

                };
                redisPublisher.Set("NightTalk:WechatUserInfo:" + apiKey, JsonHelper.SerializeObject(baseInfo), TowDayTime);
                userInfo.ApiKey = apiKey;
            }

            //返回解密后的用户数据  
            return JsonNet(userInfo);
        }

        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/wx/GetWxPhone")]
        public JsonResult<object> GetWxPhone(GetWxUserInfoRequest request)
        {
            if (request == null)
            {
                return JsonError("请求数据错误");
            }
            if (string.IsNullOrEmpty(request.Code))
            {
                throw new Exception("Code,请重新登录");
            }
            var baseInfo = GetBaseInfo();

            var sessionkey = WXHelper.GetSessionKey(request.Code, request.Iv);
            //获取解密数据
            string result = WXHelper.DecodeEncryptedData(sessionkey, request.EncryptedData, request.Iv);
            //转为手机号对象
            var phone = JsonConvert.DeserializeObject<WXPhone>(result).phoneNumber;
            _weCharUserInfoService.EditPhone(baseInfo.UnionID, phone);

            baseInfo.Phone = phone;
            using (var redisPublisher = new RedisClient(RedisHost, int.Parse(RedisPort), RedisPassword))
            {
                redisPublisher.Remove("NightTalk:WechatUserInfo:" + baseInfo.ApiKey);
                //存储基本信息
                redisPublisher.Set("NightTalk:WechatUserInfo:" + baseInfo.ApiKey, JsonHelper.SerializeObject(baseInfo), TowDayTime);
            }

            //返回解密后的用户数据 
            return JsonNet(phone);
        }
    }
}