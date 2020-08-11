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
    /// 微信用户
    /// </summary>
    public class WeCharUserInfoDomain
    {
        /// <summary>
        /// UnionID
        /// </summary>
        [StringLength(50)]
        public string UnionID { get; set; }

        /// <summary>
        /// OpenID
        /// </summary>
        [StringLength(50)]
        public string OpenID { get; set; }

        /// <summary>
        /// SessionID
        /// </summary>
        [StringLength(200)]
        public string SessionID { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(50)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(255)]
        public string Avater { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [StringLength(30)]
        public string Phone { get; set; }

        /// <summary>
        /// 分享码
        /// </summary>
        [StringLength(10)]
        public string ShareCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WeCharUserInfoStatusEnumDomain WeCharUserInfoStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
