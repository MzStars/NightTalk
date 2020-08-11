using Data;
using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Model
{
    public class ArticleExtend
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ArticleExtend()
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
        public string ArticleTitle { get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        public string ArticleAuthor { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public ArticleTypeEnum ArticleType { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleStatusEnum ArticleStatus { get; set; }

        /// <summary>
        /// 文章日期
        /// </summary>
        public DateTime ArticleDate { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string ArticleContent { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 点赞
        /// </summary>
        public int ArticleLike { get; set; }

        /// <summary>
        /// 收藏
        /// </summary>
        public int ArticleComment { get; set; }

        /// <summary>
        /// 转发
        /// </summary>
        public int ArticleForward { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ArticleViews { get; set; }

        /// <summary>
        /// 点赞
        /// </summary>
        public bool Like { get; set; }

        /// <summary>
        /// 收藏
        /// </summary>
        public bool Favorite { get; set; }

        /// <summary>
        /// 转发
        /// </summary>
        public bool Forward { get; set; }
    }
}