using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk.Models
{
    public class AddCommentRequest
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public Guid ArticleID { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }
    }
}