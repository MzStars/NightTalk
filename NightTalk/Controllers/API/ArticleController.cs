using Infrastructure.Common;
using NightTalk.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalk.Controllers
{
    /// <summary>
    /// 文章API
    /// </summary>
    public class ArticleController : BaseApi
    {
        /// <summary>
        /// 开放时间
        /// </summary>
        private TimeSpan OpeningHours = DateTime.MinValue.AddHours(22).TimeOfDay;

        private TimeSpan ClosingTime = DateTime.MinValue.AddHours(4).TimeOfDay;

        private static bool IsShowArticle = ConfigHelper.AppSettings("IsShowArticle").ToLower() == "true";

        /// <summary>
        /// 文章Service
        /// </summary>
        private readonly ArticleService _articleService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="articleService"></param>
        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// 打开文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/article/OpenArticle")]
        public JsonResult<object> OpenArticle()
        {
            BaseModel baseInfo = new BaseModel();
            HttpRequest re = HttpContext.Current.Request;
            var headers = re.Headers;
            var ApiKey = headers.Get("ApiKey");
            if (!string.IsNullOrEmpty(ApiKey))
            {
                baseInfo = GetBaseInfo();
            }

            //当前时间
            var time = DateTime.Now;
            //是否开放
            if (time.TimeOfDay > OpeningHours || time.TimeOfDay < ClosingTime || IsShowArticle)
            {
                var model = _articleService.GetData(baseInfo?.UnionID);
                if (model != null)
                {
                    _articleService.AddViews(model.ID);
                }
                return JsonNet(model);
            }

            return JsonError("当前未开开放,开放时间：" + OpeningHours.ToString() + " - " + ClosingTime.ToString());
            
        }
    }
}