using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    public  class JobContextInterceptor
    {
        private readonly JobContext _context;

        public JobContextInterceptor()
        {
            _context = new JobContext();
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

            if (_context.Has(key))
            {
                var obj = _context.Get<TResult>(key);
                if (obj == null) return default(TResult);
                return obj;
            }
            else
            {
                var obj = func(parameter);
                if (obj == null) return default;

                _context.Set(key, obj);
                return obj;
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

            if (_context.Has(key))
            {
                var obj = _context.Get<TResult>(key);
                if (obj == null) return default;
                return obj;
            }
            else
            {
                var obj = await func(parameter);
                if (obj == null) return default;
                _context.Set(key, obj);
                return obj;
            }
        }
    }
}
