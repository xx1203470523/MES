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
        /// 构造函数
        /// </summary>
        public JobContextProxy()
        {
            dictionary = new();
        }


        /// <summary>
        /// 获取字典Key
        /// </summary>
        /// <returns></returns>
        public ICollection<int> GetKeys()
        {
            return dictionary.Keys;
        }

        /// <summary>
        /// 存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetValue(string key, object value)
        {
            Set(key, value);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? GetValue<T>(string key)
        {
            if (Has(key) == false)
            {
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
        /// 取值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public TResult? GetValue<T, TResult>(Func<T, TResult> func, T parameter)
        {
            var key = $"{func.Method.Name}{parameter}";

            if (Has(key) == false)
            {
                var obj = func(parameter);
                if (obj == null) return default;

                Set(key, obj);
                return obj;
            }
            else
            {
                var obj = Get(key);
                if (obj == null) return default;

                return (TResult)obj;
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
            var key = $"{func.Method.Name}{parameter}";

            if (Has(key) == false)
            {
                var obj = await func(parameter);
                if (obj == null) return default;

                Set(key, obj);
                return obj;
            }
            else
            {
                var obj = Get(key);
                if (obj == null) return default;

                return (TResult)obj;
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

        /// <summary>
        /// 
        /// </summary>
        ~JobContextProxy()
        {
            Dispose();
        }

    }
}
