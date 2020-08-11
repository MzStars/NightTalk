using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk.Models
{
    /// <summary>
    /// 分页类
    /// </summary>
    public class PageModel
    {
        /// <summary>
        /// 构造函数 每页默认20
        /// </summary>
        public PageModel()
        {
            PageSize = 20;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数目
        /// </summary>
        public int PageSize { get; set; }
    }
}