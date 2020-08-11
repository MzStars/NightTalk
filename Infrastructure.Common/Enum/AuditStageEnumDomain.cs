using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Enum
{
    /// <summary>
    /// 审核阶段
    /// </summary>
    public enum AuditStageEnumDomain
    {
        [Description("未提交")]
        未提交,
        [Description("初审")]
        初审,
        [Description("终审")]
        终审,
        [Description("完成")]
        完成
    }
}
