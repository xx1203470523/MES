using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation
{
    /// <summary>
    /// 进站
    /// </summary>
    public interface IManuInStationService
    {
        /// <summary>
        /// 执行（进站）
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        Task<int> InStationAsync(ManuSfcProduceEntity sfcProduceEntity);

    }
}
