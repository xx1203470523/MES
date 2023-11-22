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
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> InStationRangeBySFCAsync(SFCInStationBo bo);

        /// <summary>
        /// 批量进站（托盘进站）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> InStationRangeByVehicleAsync(VehicleInStationBo bo);


        /// <summary>
        /// 批量出站（条码出站）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> OutStationRangeBySFCAsync(SFCOutStationBo bo);

        /// <summary>
        /// 批量出站（托盘出站）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> OutStationRangeByVehicleAsync(VehicleOutStationBo bo);

    }
}
