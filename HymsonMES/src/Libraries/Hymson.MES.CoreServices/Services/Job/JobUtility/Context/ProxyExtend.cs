using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.Context
{
    /// <summary>
    /// 代理扩展
    /// </summary>
    public static class ProxyExtend
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static JobContextData<T> GetProxy<T>(this T param, JobContextProxy proxy)
        {
            return proxy.GtContextDataValue<T>(param);
        }

        /// <summary>
        /// 设定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static T? SetProxy<T>(this T param, JobContextProxy proxy) where T : new()
        {
            return proxy.SetValue<T>(param);
        }
    }
}
