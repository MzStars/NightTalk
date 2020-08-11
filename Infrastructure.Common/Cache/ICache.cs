using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public interface ICache
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fetch"></param>
        /// <param name="second"></param>
        T Get<T>(string key, Func<T> fetch = null, long second=60);

        void Set<T>(string key, T obj, long Seconds);

        T Get<T>(string key);
    }
}
