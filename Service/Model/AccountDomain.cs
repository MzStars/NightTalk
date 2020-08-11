using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class AccountDomain
    {
        /// <summary>
        /// 账号唯一ID
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public AccountStatusEnumDomain AccountStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; }
    }
}
