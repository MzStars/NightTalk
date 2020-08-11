using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// 评论
    /// </summary>
    [Table("Comment")]
    [Serializable]
    public class Comment
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Comment()
        {
            Global.InitModel(this);
        }

        /// <summary>
        /// 标识
        /// </summary>
        [Key]
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
