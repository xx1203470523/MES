namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务接口（蔚来）
    /// </summary>
    public interface IPushNIOService
    {
        /// <summary>
        /// 发送推送
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        Task<int> ExecutePushAsync(int limitCount = 100);

        /// <summary>
        /// 推送已经失败的数据
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        Task<int> ExecutePushFailAsync(int limitCount = 20);
    }
}
