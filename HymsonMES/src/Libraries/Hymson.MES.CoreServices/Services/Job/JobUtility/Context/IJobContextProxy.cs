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
        void Dispose();
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
        /// 取值
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        JobContextData<T> GtContextDataValue<T>(T parameters);

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T? SetValue<T>(T parameters) where T : new();

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        TResult? SetValue<TResult>(string key, TResult obj);

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object? GetValueOnly(string key);

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        TResult? GetValue<TResult>(string key, TResult obj);

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

    }
}