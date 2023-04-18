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
        /// <param name="classNames"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        Task<Dictionary<string, int>> ExecuteJobAsync(IEnumerable<string> classNames, string? extra);

    }
}
