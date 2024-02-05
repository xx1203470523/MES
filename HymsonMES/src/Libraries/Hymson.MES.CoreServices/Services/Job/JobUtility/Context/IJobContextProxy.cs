using Hymson.Infrastructure;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.Context
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJobContextProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ICollection<uint> GetKeys();

        /// <summary>
        /// 设置数据库缓存
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TResult?> SetDataBaseValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameters);

        /// <summary>
        /// 数据库缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="parameters"></param>
        /// <param name="expectCount"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>?> GetDataBaseValueAsync<T, TResult>(Func<T, Task<IEnumerable<TResult>>> func, T parameters, int expectCount = 0) where TResult : BaseEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        TResult? GetValue<T, TResult>(Func<T, TResult> func, T parameter);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<TResult?> GetValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameter);

        /// <summary>
        /// 存放
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object Set(uint key, object value);

    }
}