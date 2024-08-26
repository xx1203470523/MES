namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IOP070Service
    {
        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(int limitCount = 1000, string tableName = "");

    }
}
