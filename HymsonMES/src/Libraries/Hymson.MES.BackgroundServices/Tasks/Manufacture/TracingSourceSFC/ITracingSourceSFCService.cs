namespace Hymson.MES.BackgroundServices.Tasks.Manufacture.TracingSourceSFC
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITracingSourceSFCService
    {
        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        Task ExecuteAsync(int limitCount = 1000);
    }
}
