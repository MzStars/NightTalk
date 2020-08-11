using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public interface ILogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="errorCode">Valid range: 0 - 9999</param>
        void LogError(Exception exception, int errorCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        void LogError(Exception ex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void LogInformation(object message);
    }
}
