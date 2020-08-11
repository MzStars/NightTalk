using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Enum
{
    /// <summary>
    /// 证件类型
    /// </summary>
    public enum CertificateEnumDomain
    {
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        IDCard,

        /// <summary>
        /// 军官证
        /// </summary>
        [Description("军官证")]
        Officer,

        /// <summary>
        /// 护照
        /// </summary>
        [Description("护照")]
        Passport,

        /// <summary>
        /// 港澳通行证
        /// </summary>
        [Description("港澳通行证")]
        HongKongMacao,

        /// <summary>
        /// 台胞证
        /// </summary>
        [Description("台胞证")]
        Taiwan,

        /// <summary>
        /// 回乡证
        /// </summary>
        [Description("回乡证")]
        Home
    }
}
