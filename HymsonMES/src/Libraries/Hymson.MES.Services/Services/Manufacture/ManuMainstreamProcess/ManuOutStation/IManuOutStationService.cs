using Hymson.MES.Services.BOs.Manufacture;

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
        Task OutStationAsync(ManufactureBO bo);

    }
}
