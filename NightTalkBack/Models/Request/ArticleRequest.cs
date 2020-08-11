using Infrastructure.Common;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class ArticleRequest
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ArticleRequest()
        {
            Global.InitModel(this);
        }

        /// <summary>
        /// 标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string ArticleTitle { get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        public string ArticleAuthor { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public ArticleTypeEnumDomain ArticleType { get; set; }

        /// <summary>
        /// 文章日期
        /// </summary>
        public DateTime ArticleDate { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string ArticleContent { get; set; }
    }
}