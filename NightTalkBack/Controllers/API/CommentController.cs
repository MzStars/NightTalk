
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
    public class CommentController : BaseApi
    {
        /// <summary>
        /// 评论
        /// </summary>
        private readonly CommentService _commentService;

        /// <summary>
        /// 构造
        /// </summary>
        public CommentController(CommentService commentService) 
        {
            _commentService = commentService;
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/comment/GetComment")]
        public JsonResult<object> GetComment(GetCommentRequest request)
        {
            var baseInfo = GetBaseInfo();
            if (request == null)
            {
                return JsonError("请求参数错误");
            }
            if (request.ArticleID == Guid.Empty)
            {
                return JsonError("文章ID错误");
            }

            int listCount = 0;
            var list = _commentService.GetData(out listCount, request.ArticleID, request.PageIndex, request.PageSize);

            return JsonNet(list, listCount);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/comment/DeleteComment")]
        public JsonResult<object> DeleteComment(DeleteCommentRequest request) 
        {
            var baseInfo = GetBaseInfo();
            if (request == null)
            {
                return JsonError("请求参数错误");
            }
            _commentService.DeleteComment(request.CommentID);

            return JsonNet("删除成功");
        }

        public JsonResult<object> EditArticle() 
        {
            return JsonNet("修改成功");
        }
    }
}