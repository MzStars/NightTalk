using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class LoginRequest
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}