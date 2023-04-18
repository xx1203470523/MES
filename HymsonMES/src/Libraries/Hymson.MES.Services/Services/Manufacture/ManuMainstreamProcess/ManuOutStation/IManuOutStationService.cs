using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;

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
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> OutStationAsync(ManufactureBo bo);

    }
}
