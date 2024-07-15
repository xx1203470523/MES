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
        Task<int> ExecutePushAsync(int limitCount = 1000);

    }
}
