using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;

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
        /// <param name="proxy"></param>
        /// <returns></returns>
        Task VerifyParamAsync<T>(T param, JobContextProxy proxy) where T : JobBaseBo;

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<TResult> DataAssemblingAsync<T, TResult>(T param, JobContextProxy proxy) where T : JobBaseBo where TResult : JobBaseBo, new();

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();

    }
}
