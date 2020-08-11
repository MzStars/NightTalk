using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class UpdateArticleStatusRequest
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public Guid ArticleID { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleStatusEnumDomain ArticleStatus { get; set; }
    }
}