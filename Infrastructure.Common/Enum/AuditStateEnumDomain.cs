using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Enum
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditStateEnumDomain
    {

        [Description("未提交")]
        未提交,
        [Description("初审中")]
        初审中,
        [Description("初审拒绝")]
        初审拒绝,
        [Description("终审中")]
        终审中,
        [Description("终审拒绝")]
        终审拒绝,
        [Description("终审成功")]
        终审成功
    }
}
