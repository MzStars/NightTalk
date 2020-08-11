using Infrastructure.Common;
using NightTalk.Models;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalk.Controllers
{
    [ExceptionHandling]
    public class BaseApi : ApiController
    {
        private string RedisPassword = ConfigHelper.AppSettings("RedisPassword");
        private string RedisHost = ConfigHelper.AppSettings("RedisHost");
        private string RedisPort = ConfigHelper.AppSettings("RedisPort");

        public BaseApi() { }

        public BaseModel GetBaseInfo()
        {
            HttpRequest re = HttpContext.Current.Request;
            var headers = re.Headers;
            var ApiKey = headers.Get("ApiKey");

            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new Exception("秘钥错误,请重新登录");
            }
            var dt = "";
            using (var redisPublisher = new RedisClient(RedisHost, int.Parse(RedisPort), RedisPassword))
            {
                //获取缓存信息
                dt = redisPublisher.Get<string>("NightTalk:WechatUserInfo:" + ApiKey);

                if (string.IsNullOrEmpty(dt))
                {
                    throw new Exception("您的登录信息已过期,请重新登录");
                }
            }
            var baseModel = JsonHelper.DeserializeJsonToObject<BaseModel>(dt);
            baseModel.ApiKey = ApiKey;
            return baseModel;
        }

        public JsonResult<object> JsonNet<T>(T items, int? total = null)
        {
            var objs = new object();
            objs = new
            {
                items,
                status = true,
                code = 1000
            };
            if (total != null)
            {
                objs = new
                {
                    items,
                    status = true,
                    code = 1000,
                    total
                };
            }
            return Json(objs, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings, System.Text.Encoding.UTF8);
        }

        public JsonResult<object> JsonError<T>(T items)
        {
            var objs = new object();
            objs = new
            {
                items,
                status = false,
                code = 1001
            };
            
            return Json(objs, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings, System.Text.Encoding.UTF8);
        }

    }
}