namespace Hymson.MES.BackgroundServices.Tasks.Manufacture.WorkOrderStatistic
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWorkOrderStatisticService
    {
        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        Task ExecuteAsync(int limitCount = 1000);
    }
}
