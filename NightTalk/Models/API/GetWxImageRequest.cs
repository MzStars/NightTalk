using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk.Models
{
    public class GetWxImageRequest
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Pages { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
    }
}