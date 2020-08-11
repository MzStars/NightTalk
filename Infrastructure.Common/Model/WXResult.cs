using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// 微信解压类
    /// </summary>
    public class WXResult
    {
        /// <summary>
        /// OpenID
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// SessionKey
        /// </summary>
        public string session_key { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errmsg { get; set; }
    }
}
