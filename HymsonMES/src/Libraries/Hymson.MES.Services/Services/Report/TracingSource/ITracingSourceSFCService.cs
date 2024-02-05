using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services
{
    /// <summary>
    /// 条码追溯服务接口
    /// </summary>
    public interface ITracingSourceSFCService
    {
        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<NodeSourceDto> SourceAsync(string sfc);

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<NodeSourceDto> DestinationAsync(string sfc);

    }
}