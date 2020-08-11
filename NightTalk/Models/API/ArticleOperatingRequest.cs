using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk.Models
{
    /// <summary>
    /// 文章操作 提交类
    /// </summary>
    public class ArticleOperatingRequest
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public Guid ArticleID { get; set; }
    }
}