using System.Collections.Concurrent;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility
{
    /// <summary>
    /// 
    /// </summary>
    public class JobContextProxy : IDisposable, IJobContextProxy
    {
        /// <summary>
        /// 
        /// </summary>
        protected ConcurrentDictionary<uint, object> dictionary;

        /// <summary>
        /// 
        /// </summary>
        private static SemaphoreSlim[] _semaphores;

        /// <summary>
        /// 构造函数
        /// </summary>
        public JobContextProxy()
        {
            dictionary = new();

            int num = Math.Max(Environment.ProcessorCount * 8, 32);
            _semaphores = new SemaphoreSlim[num];
            for (int i = 0; i < _semaphores.Length; i++)
            {
                _semaphores[i] = new SemaphoreSlim(1);
            }
        }


        /// <summary>
        /// 获取字典Key
        /// </summary>
        /// <returns></returns>
        public ICollection<uint> GetKeys()
        {
            return dictionary.Keys;
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public TResult? GetValue<T, TResult>(Func<T, TResult> func, T parameter)
        {
            var cacheKey = (uint)$"{func.Method.DeclaringType?.FullName}.{func.Method.Name}{parameter}".GetHashCode();

            if (Has(cacheKey))
            {
                var cacheObj = Get(cacheKey);
                if (cacheObj == null) return default;

                return (TResult)cacheObj;
            }

            uint hash = cacheKey % (uint)_semaphores.Length;
            _semaphores[hash].Wait();
            try
            {
                var obj = func(parameter);
                if (obj == null) return default;

                Set(cacheKey, obj);
                return obj;
            }
            finally
            {
                _semaphores[hash].Release();
            }
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<TResult?> GetValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameter)
        {
            var cacheKey = (uint)$"{func.Method.DeclaringType?.FullName}.{func.Method.Name}{parameter}".GetHashCode();

            if (Has(cacheKey))
            {
                var cacheObj = Get(cacheKey);
                if (cacheObj == null) return default;

                return (TResult)cacheObj;
            }

            uint hash = cacheKey % (uint)_semaphores.Length;
            await _semaphores[hash].WaitAsync();
            try
            {
                var obj = await func(parameter);
                if (obj == null) return default;

                Set(cacheKey, obj);
                return obj;
            }
            finally
            {
                _semaphores[hash].Release();
            }
        }


        /// <summary>
        /// 存放
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object Set(uint key, object value)
        {
            //var valueStr = value.ToSerialize();
            return dictionary.AddOrUpdate(key, value, (k, v) => value);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object? Get(uint key)
        {
            return dictionary.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool Has(uint key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool Remove(uint key)
        {
            return dictionary.TryRemove(key, out _);
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
