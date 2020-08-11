using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk.Models
{
    public class BaseModel
    {
        /// <summary>
        /// 微信UnionId
        /// </summary>
        public string UnionID { get; set; }

        /// <summary>
        /// 微信小程序Id
        /// </summary>
        public string OpenID { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// ApiKey
        /// </summary>
        public string ApiKey { get; set; }
    }
}