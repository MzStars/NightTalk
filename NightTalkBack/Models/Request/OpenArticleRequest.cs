using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class OpenArticleRequest : PageModel
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string ArticleTitle { get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        public string ArticleAuthor { get; set; }

        /// <summary>
        /// 文章日期
        /// </summary>
        public DateTime? ArticleDate { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public ArticleTypeEnumDomain? ArticleType { get; set; }

        /// <summary>
        /// 后台账号唯一ID
        /// </summary>
        public int? UID { get; set; }
    }
}