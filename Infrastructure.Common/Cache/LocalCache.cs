using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class LocalCache : ICache
    {
        public T Get<T>(string key, Func<T> fetch = null, long second = 60)
        {
            T result = default(T);
            var obj = Cache.Get(key);
            if (obj is T)
            {
                result = (T)obj;
            }

            if (result == null || obj == null)
            {
                result = fetch();
                if (result != null)
                {
                    Set(key, result, second);
                }
            }

            return result;
        }

        public T Get<T>(string key)
        {
            T result = default(T);
            var obj = Cache.Get(key);
            if (obj is T)
            {
                result = (T)obj;
            }

            return result;
        }

        public void Set<T>(string key, T obj, long Seconds)
        {
            Cache.Set(key, obj, Seconds);
        }
    }
}
