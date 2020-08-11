using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack.Models
{
    public class GetListDataRequest :PageModel
    {
        /// <summary>
        /// 文件類型
        /// </summary>
        public int? FileType { get; set; }

        /// <summary>
        /// 文件名稱
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 後台賬號唯一ID
        /// </summary>
        public int? UID { get; set; }
    }
}