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
    /// 评论
    /// </summary>
    public class CommentDomain
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommentDomain()
        {
            Global.InitModel(this);
        }

        /// <summary>
        /// 标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        public Guid ArticleID { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [StringLength(500)]
        public string CommentContent { get; set; }

        /// <summary>
        /// 评论用户的UnionID
        /// </summary>
        [StringLength(50)]
        public string UnionID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
