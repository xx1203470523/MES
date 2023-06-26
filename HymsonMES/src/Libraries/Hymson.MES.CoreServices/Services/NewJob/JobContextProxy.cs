using System.Collections.Concurrent;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 
    /// </summary>
    public class JobContextProxy : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected ConcurrentDictionary<int, object> dictionary;

        /// <summary>
        /// 
        /// </summary>
        public JobContextProxy()
        {
            dictionary = new();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<TResult?> GetValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameter)
        {
            // TODO
            var key = $"{func.Method.Name}{parameter}";

            if (Has(key) == false)
            {
                try
                {
                    var obj = await func(parameter);
                    if (obj == null) return default;

                    Set(key, obj);
                    return obj;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                return default;
            }
            else
            {
                var obj = Get(key);
                if (obj == null) return default;

                return (TResult)obj;
            }
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

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            dictionary.Clear();
            GC.SuppressFinalize(this);
        }

    }
}
