using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 作业上下文
    /// </summary>
    public class JobContext : IDisposable
    {
        private readonly ConcurrentDictionary<int, object> _data = new ConcurrentDictionary<int, object>();
        private bool _disposed;

        // 读取上下文数据对象
        public T Get<T>(string key) 
        {
            if (!_disposed && _data.TryGetValue(key.GetHashCode(), out object value))
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }

        // 写入上下文数据对象
        public void Set(string key, object value)
        {
            if (!_disposed)
            {
                _data[key.GetHashCode()] = value;
            }
        }

        // 删除上下文数据对象
        public void Remove(string key)
        {
            if (!_disposed)
            {
                _data.TryRemove(key.GetHashCode(), out object value);
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

        // IDisposable 接口的实现
        public void Dispose()
        {
            if (!_disposed)
            {
                // 释放上下文对象的资源
                _disposed = true;

                //通知垃圾回收器不再调用终结器
                GC.SuppressFinalize(this);
            }
        }
    }
}

