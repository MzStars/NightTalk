using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class DeleteCommentRequest
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public Guid CommentID { get; set; }
    }
}