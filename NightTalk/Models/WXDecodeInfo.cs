using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk.Models
{
    /// <summary>
    /// 微信解压信息
    /// </summary>
    public class WXDecodeInfo
    {
        /// <summary>
        /// OpenID
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 性别 0 未知 1男 2女
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avatarUrl { get; set; }
        /// <summary>
        /// UnionID
        /// </summary>
        public string unionId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 分享码
        /// </summary>
        public string ShareCode { get; set; }

        /// <summary>
        /// Api秘钥
        /// </summary>
        public string ApiKey { get; set; }
    }
}