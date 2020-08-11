using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Enum
{
    public enum ConsultTypeEnumDomain
    {
        [Description("最新资讯")]
        Latest,
        [Description("往期精彩")]
        Previous,
    }
}
