using NightTalk.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalk.Controllers
{
    [RoutePrefix("api/articleOperating")]
    public class ArticleOperatingController : BaseApi
    {
        /// <summary>
        /// 文章操作Service
        /// </summary>
        private readonly ArticleOperatingService _articleOperatingService;

        /// <summary>
        /// 文章
        /// </summary>
        private readonly ArticleService _articleService;

        /// <summary>
        /// 微信用户
        /// </summary>
        private readonly WeCharUserInfoService _weCharUserInfoService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ArticleOperatingController(ArticleOperatingService articleOperatingService, ArticleService articleService, WeCharUserInfoService weCharUserInfoService) 
        {
            _articleOperatingService = articleOperatingService;
            _articleService = articleService;
            _weCharUserInfoService = weCharUserInfoService;
        }

        /// <summary>
        /// 点赞操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("LikeOperating")]
        public JsonResult<object> LikeOperating(ArticleOperatingRequest request) 
        {
            if (request == null)
            {
                return JsonError("请求数据不能为空");
            }
            if (request.ArticleID == Guid.Empty || !_articleService.ExistData(request.ArticleID))
            {
                return JsonError("文章不存在");
            }

            var baseInfo = GetBaseInfo();
            if (!_weCharUserInfoService.Exist(baseInfo.UnionID))
            {
                return JsonError("用户不存在");
            }

            _articleOperatingService.LikeArticle(baseInfo.UnionID, request.ArticleID);
            return JsonNet("点赞操作成功");
        }

        /// <summary>
        /// 收藏操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("FavoriteOperating")]
        public JsonResult<object> FavoriteOperating(ArticleOperatingRequest request)
        {
            var baseInfo = GetBaseInfo();
            _articleOperatingService.FavoriteArticle(baseInfo.UnionID, request.ArticleID);
            return JsonNet("收藏操作成功");
        }

        /// <summary>
        /// 转发操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ForwardOperating")]
        public JsonResult<object> ForwardOperating(ArticleOperatingRequest request)
        {
            if (request == null)
            {
                return JsonError("请求数据不能为空");
            }
            if (request.ArticleID == Guid.Empty || !_articleService.ExistData(request.ArticleID))
            {
                return JsonError("文章不存在");
            }
            var baseInfo = GetBaseInfo();

            if (!_weCharUserInfoService.Exist(baseInfo.UnionID))
            {
                return JsonError("用户不存在");
            }

            _articleOperatingService.AddForward(request.ArticleID);


            return JsonNet("转发操作成功");
        }
    }
}