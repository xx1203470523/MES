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
        /// 取值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TResult?> GetValueAsync<T, TResult>(Func<T[], Task<TResult>> func, params T[] parameters);

    }
}