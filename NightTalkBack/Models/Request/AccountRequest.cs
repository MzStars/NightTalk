using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class AccountRequest
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}