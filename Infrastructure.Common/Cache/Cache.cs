using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Common
{
    /// <summary>
    /// 缓存相关的操作类
    /// Copyright (C) Maticsoft
    /// </summary>
    public static class Cache
    {
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object Get(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void Set(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设定绝对的过期时间
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="seconds">超过多少秒后过期</param>
        public static void Set(string CacheKey, object objObject, long Seconds)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, System.DateTime.Now.AddSeconds(Seconds), TimeSpan.Zero);
        }

        /// <summary>
        /// 移除指定缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        public static void Remove(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Remove(CacheKey);
        }

        /// <summary>    
        /// 清除所有缓存  
        /// </summary>    
        public static void RemoveAllCache()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}
