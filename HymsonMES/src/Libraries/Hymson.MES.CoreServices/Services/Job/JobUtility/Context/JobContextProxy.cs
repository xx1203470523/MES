using Google.Protobuf.WellKnownTypes;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Tls.Crypto;
using System.Collections.Concurrent;
using System.Linq;

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
        protected ConcurrentDictionary<uint, object> dictionary = new();

        /// <summary>
        /// 
        /// </summary>
        private static SemaphoreSlim[] _semaphores;

        /// <summary>
        /// 构造函数
        /// </summary>
        public JobContextProxy()
        {
            // dictionary = new();

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
        /// 设置作业中入库数据缓存
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TResult?> SetDataBaseValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameters)
        {
            var obj = await GetValueAsync<T, TResult>(func, parameters);
            if (obj == null) return default;

            foreach (var po in obj.GetType().GetProperties())
            {
                var cacheKey = (uint)$"{po.PropertyType}".GetHashCode();
                var value = po.GetValue(obj);
                if (value != null)
                {
                    Set(cacheKey, value);
                }
            }
            return (TResult)obj;
        }

        /// <summary>
        /// 获取作业中入库的数据缓存
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>?> GetDataBaseValueAsync<T, TResult>(Func<T, Task<IEnumerable<TResult>>> func, T parameters, int expectCount = 0) where TResult : BaseEntity
        {
            var name = typeof(IEnumerable<TResult>);
            var cacheKey = (uint)$"{typeof(IEnumerable<TResult>)}".GetHashCode();

            if (Has(cacheKey))
            {
                var cacheObj = Get(cacheKey);
                if (cacheObj == null) return default;
                var cacheResult = (IEnumerable<TResult>)cacheObj;

                if (expectCount != 0 && cacheResult.Count() < expectCount)
                {
                    var obj = await GetValueAsync<T, IEnumerable<TResult>>(func, parameters);
                    if (obj != null)
                    {
                        cacheResult.Concat(obj.Where(x => !cacheResult.Any(o => o.Id == x.Id)));
                        Set(cacheKey, cacheResult);
                    }
                }
                return (IEnumerable<TResult>)cacheResult;
            }

            uint hash = cacheKey % (uint)_semaphores.Length;
            _semaphores[hash].Wait();
            try
            {
                var obj = await GetValueAsync<T, IEnumerable<TResult>>(func, parameters);
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
        /// <param name="parameters"></param>
        /// <returns></returns>
        public JobContextData<T> GtContextDataValue<T>(T parameters)
        {
            var cacheKey = (uint)$"{parameters?.GetType()}".GetHashCode();

            if (Has(cacheKey))
            {
                var cacheObj = Get(cacheKey);
                if (cacheObj == null) return default;

                return new JobContextData<T>(true, (T)cacheObj);
            }

            uint hash = cacheKey % (uint)_semaphores.Length;
            _semaphores[hash].Wait();
            try
            {
                if (parameters != null)
                {
                    Set(cacheKey, parameters);
                    return new JobContextData<T>(true, parameters);
                }
                else
                {
                    return new JobContextData<T>(false, parameters);
                }
            }
            finally
            {
                _semaphores[hash].Release();
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T? SetValue<T>(T parameters) where T : new()
        {
            var cacheKey = (uint)$"{parameters?.GetType()}".GetHashCode();
            uint hash = cacheKey % (uint)_semaphores.Length;
            _semaphores[hash].Wait();
            try
            {
                if (parameters != null)
                {
                    Set(cacheKey, parameters);
                    return parameters;
                }
                else
                {
                    return new T();
                }
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
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TResult? GetValue<T, TResult>(Func<T, TResult> func, T parameters)
        {
            var cacheKey = (uint)$"{func.Method.DeclaringType?.FullName}.{func.Method.Name}{parameters}".GetHashCode();

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
                var obj = func(parameters);
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
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TResult?> GetValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameters)
        {
            var cacheKey = (uint)$"{func.Method.DeclaringType?.FullName}.{func.Method.Name}{parameters}".GetHashCode();

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
                var obj = await func(parameters);
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

    public class JobContextData<T>
    {
        public JobContextData(bool hasKey, T value)
        {
            this.HasKey = hasKey;
            this.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Value { get; set; }
    }
}
