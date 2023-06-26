using System.Collections.Concurrent;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility
{
    /// <summary>
    /// 作业上下文
    /// </summary>
    public class JobContext : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConcurrentDictionary<int, object> _data = new ConcurrentDictionary<int, object>();
        private bool _disposed;

        /// <summary>
        /// 读取上下文数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? Get<T>(string key)
        {
            if (!_disposed && _data.TryGetValue(key.GetHashCode(), out object? value))
            {
                if (value == null)
                {
                    return default;
                }

                return (T)value;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 写入上下文数据对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (!_disposed)
            {
                _data[key.GetHashCode()] = value;
            }
        }

        /// <summary>
        /// 删除上下文数据对象
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (!_disposed)
            {
                _data.TryRemove(key.GetHashCode(), out _);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Has(string key)
        {
            return _data.ContainsKey(key.GetHashCode());
        }

        /// <summary>
        /// IDisposable 接口的实现
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                // 释放上下文对象的资源
                _disposed = true;

                // 通知垃圾回收器不再调用终结器
                GC.SuppressFinalize(this);
            }
        }

    }
}

