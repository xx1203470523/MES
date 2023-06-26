using System;
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
        private readonly ConcurrentDictionary<string, object> _data = new ConcurrentDictionary<string, object>();
        private bool _disposed;

        // 读取上下文数据对象
        public T Get<T>(string key) where T : class
        {
            if (!_disposed && _data.TryGetValue(key, out object value))
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
                _data[key] = value;
            }
        }

        // 删除上下文数据对象
        public void Remove(string key)
        {
            if (!_disposed)
            {
                _data.TryRemove(key, out object value);
            }
        }

        // IDisposable 接口的实现
        public void Dispose()
        {
            if (!_disposed)
            {
                // 释放上下文对象的资源
                _disposed = true;
            }
        }
    }
}

