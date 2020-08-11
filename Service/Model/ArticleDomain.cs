using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleDomain
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ArticleDomain()
        {
            Global.InitModel(this);
        }

        /// <summary>
        /// 标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 后台账号ID
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        [StringLength(50)]
        public string ArticleTitle { get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        [StringLength(50)]
        public string ArticleAuthor { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public ArticleTypeEnumDomain ArticleType { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleStatusEnumDomain ArticleStatus { get; set; }

        /// <summary>
        /// 文章日期
        /// </summary>
        public DateTime ArticleDate { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string ArticleContent { get; set; }

        /// <summary>
        /// 转发次数
        /// </summary>
        public int ArticleForward { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ArticleViews { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
