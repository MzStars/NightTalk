using NightTalkBack.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalkBack.Controllers
{
    public class ArticleController : BaseApi
    {

        /// <summary>
        /// 开放时间
        /// </summary>
        private TimeSpan OpeningHours = DateTime.MinValue.AddHours(22).TimeOfDay;
        private TimeSpan ClosingTime = DateTime.MinValue.AddHours(4).TimeOfDay;

        /// <summary>
        /// 文章
        /// </summary>
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/article/AddArticle")]
        public JsonResult<object> AddArticle(ArticleRequest request)
        {
            if (request == null)
            {
                return JsonError("请求数据错误");
            }
            var baseInfo = GetBaseInfo();
            var domain = request.ToDomainModel();
            domain.ID = Guid.NewGuid();
            domain.UID = baseInfo.UID;
            domain.CreateTime = DateTime.Now;
            _articleService.NewCreate(domain);

            return JsonNet("添加成功");
        }

        /// <summary>
        /// 修改文章状态
        /// </summary>
        [HttpPost]
        [Route("api/article/UpdateArticleStatus")]
        public JsonResult<object> UpdateArticleStatus(UpdateArticleStatusRequest request)
        {
            if (request == null)
            {
                return JsonError("请求数据错误");
            }
            var baseInfo = GetBaseInfo();

            _articleService.UpdateArticleStatus(request.ArticleID, request.ArticleStatus);

            return JsonNet("修改成功");
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/article/OpenArticle")]
        public JsonResult<object> OpenArticle(OpenArticleRequest request)
        {
            var baseInfo = GetBaseInfo();

            //当前时间
            var time = DateTime.Now;
            //是否开放
            if (time.TimeOfDay > OpeningHours || time.TimeOfDay < ClosingTime)
            {
                int listCount = 0;
                var list = _articleService.GetData(out listCount, request.PageIndex, request.PageSize,request.ArticleTitle,request.ArticleAuthor,request.ArticleDate,request.ArticleType,request.UID);

                return JsonNet(list, listCount);
            }

            return JsonError("当前未开开放,开放时间：" + OpeningHours.ToString() + " - " + ClosingTime.ToString());

        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/article/DeleteArticle")]
        public JsonResult<object> DeleteArticle(DeleteArticleRequest request)
        {
            var baseInfo = GetBaseInfo();
            if (request == null)
            {
                return JsonError("请求数据错误");
            }

            _articleService.DeleteArticle(request.ArticleID);

            return JsonNet("删除文章成功");
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/article/EditArticle")]
        public JsonResult<object> EditArticle(ArticleRequest request) 
        {
            var baseInfo = GetBaseInfo();

            if (request == null)
            {
                throw new Exception("请求数据不能为空");
            }

            _articleService.Edit(request.ToDomainModel());

            return JsonNet("修改成功");
        }
    }
}