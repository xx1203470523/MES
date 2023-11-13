using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Services.Manufacture
{
    /// <summary>
    /// 过站
    /// </summary>
    public interface IManuPassStationService
    {
        /// <summary>
        /// 批量进站（条码进站）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> InStationRangeBySFCAsync(SFCInStationBo dto);

        /// <summary>
        /// 批量进站（托盘进站）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> InStationRangeByVehicleAsync(VehicleInStationBo dto);


        /// <summary>
        /// 批量出站（条码出站）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> OutStationRangeBySFCAsync(SFCOutStationBo dto);

        /// <summary>
        /// 批量出站（托盘出站）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> OutStationRangeByVehicleAsync(VehicleOutStationBo dto);

    }
}
