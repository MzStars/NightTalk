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
    /// 后台账号
    /// </summary>
    [Table("Account")]
    [Serializable]
    public class Account
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Account()
        {
            Global.InitModel(this);
        }

        /// <summary>
        /// 标识
        /// </summary>
        [Key]
        public int UID { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(30)]
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(30)]
        public string PassWord { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column(TypeName = "int")]
        public AccountStatusEnum AccountStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
