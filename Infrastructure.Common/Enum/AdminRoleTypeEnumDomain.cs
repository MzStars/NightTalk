using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Enum
{
    public enum AdminRoleTypeEnumDomain
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        Admin,
        /// <summary>
        /// 背审
        /// </summary>
        [Description("背审")]
        Backon,
        /// <summary>
        /// 一级
        /// </summary>
        [Description("一级")]
        FirstLevel,
        /// <summary>
        /// 展览
        /// </summary>
        [Description("展览")]
        Exhibition,
        /// <summary>
        /// 展览主办方
        /// </summary>
        [Description("展览主办方")]
        ExhibitionManage
    }
}
