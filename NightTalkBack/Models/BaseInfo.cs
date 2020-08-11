using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    /// <summary>
    /// 基本信息
    /// </summary>
    public class BaseInfo
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}