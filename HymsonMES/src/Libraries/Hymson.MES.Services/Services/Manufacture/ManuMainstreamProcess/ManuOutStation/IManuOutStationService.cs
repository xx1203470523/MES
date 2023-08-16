using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation
{
    /// <summary>
    /// 出站
    /// </summary>
    public interface IManuOutStationService
    {
        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        Task<int> OutStationAsync(ManuSfcProduceEntity sfcProduceEntity);

        /// <summary>
        /// 出站(在制维修)
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> OutStationRepiarAsync(ManufactureRepairBo bo);

    }
}
