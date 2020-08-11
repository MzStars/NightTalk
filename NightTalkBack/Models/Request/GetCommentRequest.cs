using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    /// <summary>
    /// 获取评论API请求类
    /// </summary>
    public class GetCommentRequest : PageModel
    {
        /// <summary>
        /// 当前文章ID
        /// </summary>
        public Guid ArticleID { get; set; }
    }
}