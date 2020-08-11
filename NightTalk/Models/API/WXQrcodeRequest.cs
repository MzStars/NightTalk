using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk.Models
{
    public class WXQrcodeRequest
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Pages { get; set; } = "pages/index/index";

        /// <summary>
        /// 分享码
        /// </summary>
        public string ShareCode { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; } = 430;
    }
}