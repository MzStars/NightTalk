using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class CommentExtend
    {
        /// <summary>
        /// 标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }

        /// <summary>
        /// 评论用户的UnionID
        /// </summary>
        public string UnionID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avater { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
    }
}
