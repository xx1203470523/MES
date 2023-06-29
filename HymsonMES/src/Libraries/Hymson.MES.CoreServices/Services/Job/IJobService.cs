using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 作业模版
    /// </summary>
    public interface IJobService
    {
        /// <summary>
        /// 参数校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        Task VerifyParamAsync<T>(T param) where T : JobBaseBo;

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<TResult?> DataAssemblingAsync<T, TResult>(T param) where T : JobBaseBo where TResult : JobResultBo, new();

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(object obj);

    }
}
