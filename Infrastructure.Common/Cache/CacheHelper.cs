using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class CacheHelper
    {
        public static T Get<T>(object id, Func<T> fetch = null, long minute = 120)
        {
            var type = typeof(T);
            var key = string.Format("urn:{1}:{2}", type.Name, id.ToString()); 
            return Get(key, fetch, minute);
        }

        public static T Get<T>(string key, Func<T> fetch = null, long second = 60)
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

        public static void Set<T>(string key, T obj, long Seconds)
        {
            Cache.Set(key, obj, Seconds);
        }

        public static void Remove<T>(object id)
        {
            var type = typeof(T);
            var key = string.Format("urn:{1}:{2}", type.Name, id.ToString());
            Cache.Remove(key);
        }

        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        public static bool RemoveStartWithKeyWord(string KeyWord)
        {
            bool rtnVal = false;
            System.Web.Caching.Cache webCacheList = new System.Web.Caching.Cache();
            System.Collections.IDictionaryEnumerator enumerator = webCacheList.GetEnumerator();
            if (enumerator != null)
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.ToString().StartsWith(KeyWord))
                    {
                        CacheHelper.Remove(enumerator.Key.ToString());
                        rtnVal = true;
                    }
                }
            }
            return rtnVal;
        }

        /// <summary>    
        /// 清除所有缓存  
        /// </summary>    
        public static void RemoveAllCache()
        {
           Cache.RemoveAllCache();
        }
    }
}
