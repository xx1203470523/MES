using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using MySqlX.XDevAPI.Common;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.Context
{
    /// <summary>
    /// 代理扩展
    /// </summary>
    public static class ProxyExtend
    {
        /// <summary>
        /// zzz
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>?>  GetProxy<TParam,T >(this IEnumerable<T> param, IJobContextProxy proxy, Func<TParam, Task<IEnumerable<T>> > func, TParam parameters, int expectCount = 0) where T : BaseEntity
        {
            return await proxy.GetDataBaseValueAsync<TParam,T >(func, parameters, expectCount);
        }
    }
}
