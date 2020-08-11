using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class UpdatePWDRequest
    {
        /// <summary>
        /// 账号
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassWord { get; set; }
    }
}