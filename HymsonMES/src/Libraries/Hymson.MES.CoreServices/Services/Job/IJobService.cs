using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Dtos.Common;

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
        /// 执行前
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo;

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo;

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<JobResponseBo?> ExecuteAsync(object obj);

        /// <summary>
        /// 执行后
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo;
    }
}
