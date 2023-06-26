namespace Hymson.MES.CoreServices.Services.Job.JobUtility
{
    public class JobContextInterceptor
    {
        /// <summary>
        /// 
        /// </summary>
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
                var cacheObj = _context.Get<TResult>(key);
                if (cacheObj == null) return default;
                return cacheObj;
            }

            var obj = func(parameter);
            if (obj == null) return default;

            _context.Set(key, obj);
            return obj;
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
                var cacheObj = _context.Get<TResult>(key);
                if (cacheObj == null) return default;
                return cacheObj;
            }

            var obj = await func(parameter);
            if (obj == null) return default;
            _context.Set(key, obj);
            return obj;
        }

    }
}
