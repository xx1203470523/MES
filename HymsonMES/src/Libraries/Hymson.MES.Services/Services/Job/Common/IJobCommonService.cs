namespace Hymson.MES.Services.Services.Job.Common
{
    /// <summary>
    /// 生产共用
    /// </summary>
    public interface IJobCommonService
    {
        /// <summary>
        /// 读取挂载的作业并执行
        /// </summary>
        /// <param name="facePlateButtonId"></param>
        /// <returns></returns>
        Task ExecuteJobAsync(long facePlateButtonId);

    }
}
