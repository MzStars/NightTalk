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
    /// 文章操作类 微信用户的收藏 点赞 转发
    /// </summary>
    [Table("ArticleOperating")]
    [Serializable]
    public class ArticleOperating
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ArticleOperating()
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
        /// 微信用户的UnionID
        /// </summary>
        [StringLength(50)]
        public string UnionID { get; set; }

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

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CraeteTime { get; set; }
    }
}
