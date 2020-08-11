using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// 微信用户状态
    /// </summary>
    public enum WeCharUserInfoStatusEnum
    {
        正常 = 0,
        禁止评论 = -1,
        禁止登录 = -2
    }
}
