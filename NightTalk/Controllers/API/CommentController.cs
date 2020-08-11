using Infrastructure.Common;
using NightTalk.Models;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalk.Controllers
{
    /// <summary>
    /// 评论API
    /// </summary>
    public class CommentController : BaseApi
    {
        /// <summary>
        /// 评论Service
        /// </summary>
        private readonly CommentService _commentService;

        /// <summary>
        /// 文章
        /// </summary>
        private readonly ArticleService _articleService;

        private readonly WeCharUserInfoService _weCharUserInfoService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommentController(CommentService commentService, ArticleService articleService, WeCharUserInfoService weCharUserInfoService)
        {
            _commentService = commentService;
            _articleService = articleService;
            _weCharUserInfoService = weCharUserInfoService;
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/comment/GetComment")]
        public JsonResult<object> GetComment(GetCommentRequest request)
        {
            if (!_articleService.ExistData(request.ArticleID))
            {
                throw new Exception("文章不存在");
            }

            int listCount = 0;
            var list = _commentService.GetData(out listCount, request.ArticleID, request.PageIndex, request.PageSize);

            return JsonNet(list, listCount);
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/comment/AddComment")]
        public JsonResult<object> AddComment(AddCommentRequest request)
        {
            if (request == null)
            {
                return JsonError("请求参数不能为空");
            }
            if (string.IsNullOrEmpty(request.CommentContent))
            {
                throw new Exception("评论内容不能为空");
            }
            if (!_articleService.ExistData(request.ArticleID))
            {
                throw new Exception("文章不存在");
            }

            var baseInfo = GetBaseInfo();
            if (!_weCharUserInfoService.Exist(baseInfo.UnionID))
            {
                throw new Exception("微信账号不存在");
            }

            var msgObj = WXHelper.MsgCheck(request.CommentContent);
            if (msgObj.errcode == Senparc.Weixin.ReturnCode.请求成功)
            {
                CommentDomain domain = request.ToDomainModel();
                domain.UnionID = baseInfo.UnionID;
                domain.ID = Guid.NewGuid();
                domain.CreateTime = DateTime.Now;
                _commentService.NewCreate(domain);


                return JsonNet("发表成功");
            }
            if (msgObj.errcode == (Senparc.Weixin.ReturnCode)40001)
            {
                WXHelper.RemoveAccessToken();
                //access_token失效 请重新获取
                return JsonNet("发表错误,请重新发送");
            }

            return JsonError(msgObj.errcode.ToString());
        }
    }
}