using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Services.NewJob.Execute
{
    /// <summary>
    /// 执行作业
    /// </summary>
    public interface IExecuteJobService<T> where T : JobBaseBo
    {
        /// <summary>
        /// 执行作业
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync(IEnumerable<JobBo> jobBos, T param);

    }
}
