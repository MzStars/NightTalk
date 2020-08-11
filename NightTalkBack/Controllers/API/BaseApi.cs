using Infrastructure.Common;
using NightTalkBack.Models;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalkBack.Controllers
{

    [ExceptionHandling]
    public class BaseApi : ApiController
    {
        private string RedisPassword = ConfigHelper.AppSettings("RedisPassword");
        private string RedisHost = ConfigHelper.AppSettings("RedisHost");
        private string RedisPort = ConfigHelper.AppSettings("RedisPort");

        public BaseApi() { }

        public BaseInfo GetBaseInfo()
        {
            HttpRequest re = HttpContext.Current.Request;
            var headers = re.Headers;
            var apiKey = headers.Get("ApiKey");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("密钥错误");
            }
            var dt = "";
            using (var redis = new RedisClient(RedisHost, int.Parse(RedisPort), RedisPassword))
            {
                //获取缓存信息
                dt = redis.Get<string>("NightTalkBack:BaseInfo:" + apiKey);

                if (string.IsNullOrEmpty(dt))
                {
                    throw new Exception("登入信息失效, 请重新登录");
                }

                //var ts = TimeSpan.FromMinutes(15);//30min 后过期
                //redis.ExpireEntryAt("NightTalkBack:BaseInfo:" + apiKey, ts);
            }
            return JsonHelper.DeserializeJsonToObject<BaseInfo>(dt);
        }

        /// <summary>
        /// 返回参数 - 正常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="total"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 返回参数 - 错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
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
