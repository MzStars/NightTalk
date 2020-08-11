using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// 访问异常
    /// </summary>
    public class AccessException : Exception
    {
        public AccessException(string code = "1003")
        {
            Code = code;
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        private string Code { get; set; }
    }
}
