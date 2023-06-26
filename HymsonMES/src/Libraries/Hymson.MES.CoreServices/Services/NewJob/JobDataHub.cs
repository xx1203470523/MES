using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 
    /// </summary>
    public class JobDataHub
    {
        /// <summary>
        /// 
        /// </summary>
        protected ConcurrentDictionary<int, object> dictionary = new();

        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache _memoryCache;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryCache"></param>
        public JobDataHub(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetValue(string key, object value)
        {
            Set(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? GetValue<T>(string key)
        {
            if (Has(key) == false)
            {
                //var isExistsCache = _memoryCache.TryGetValue(key, out var cache);
                //if (isExistsCache == false) return default;

                //Set(key, cache);
                //return (T)cache;

                // TODO
                return default;
            }
            else
            {
                var obj = Get(key);
                if (obj == null) return default;

                return (T)obj;
            }
        }


        /// <summary>
        /// 存放
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object Set(string key, object value)
        {
            //var valueStr = value.ToSerialize();
            return dictionary.AddOrUpdate(key.GetHashCode(), value, (k, v) => value);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object? Get(string key)
        {
            return dictionary.TryGetValue(key.GetHashCode(), out var value) ? value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool Has(string key)
        {
            return dictionary.ContainsKey(key.GetHashCode());
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool Remove(string key)
        {
            return dictionary.TryRemove(key.GetHashCode(), out _);
        }



    }
}
