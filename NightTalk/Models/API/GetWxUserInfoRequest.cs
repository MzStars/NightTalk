using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk.Models
{
    /// <summary>
    /// 获取微信授权信息请求类
    /// </summary>
    public class GetWxUserInfoRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Iv { get; set; }

        /// <summary>
        /// 解压数据
        /// </summary>
        public string EncryptedData { get; set; }
    }
}