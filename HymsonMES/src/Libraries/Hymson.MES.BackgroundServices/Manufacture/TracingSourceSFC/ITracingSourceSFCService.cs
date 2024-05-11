namespace Hymson.MES.BackgroundServices.Manufacture
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

        /// <summary>
        /// 执行统计（达梦）
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        Task ExecuteForDMAsync(int limitCount = 1000);

    }
}
