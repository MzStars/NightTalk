using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Enum
{
    public enum UserEnumDomain
    {
        /// <summary>
        /// 贵宾证
        /// </summary>
        [Description("贵宾证")]
        VIP,

        /// <summary>
        /// 嘉宾证
        /// </summary>
        [Description("嘉宾证")]
        Customer,

        /// <summary>
        /// 参展证
        /// </summary>
        [Description("参展证")]
        Exhibition,

        /// <summary>
        /// 客商证
        /// </summary>
        [Description("客商证")]
        Merchants,

        /// <summary>
        /// 工作人员证
        /// </summary>
        [Description("工作证")]
        Employee,

        /// <summary>
        /// 记者证
        /// </summary>
        [Description("记者证")]
        Reporter,

        /// <summary>
        /// 志愿者证
        /// </summary>
        [Description("志愿者证")]
        Volunteer,

        /// <summary>
        /// 安保证
        /// </summary>
        [Description("安保证")]
        Security
    }
}
