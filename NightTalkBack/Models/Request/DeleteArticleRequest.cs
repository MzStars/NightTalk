using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class DeleteArticleRequest
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public Guid ArticleID { get; set; }
    }
}