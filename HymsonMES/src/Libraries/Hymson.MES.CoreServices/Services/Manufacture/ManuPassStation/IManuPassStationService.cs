using Hymson.MES.Core.Enums;
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
        /// <param name="source"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> InStationRangeBySFCAsync(SFCInStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi);

        /// <summary>
        /// 批量进站（托盘进站）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> InStationRangeByVehicleAsync(VehicleInStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi);


        /// <summary>
        /// 批量出站（条码出站）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> OutStationRangeBySFCAsync(SFCOutStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi);

        /// <summary>
        /// 批量出站（托盘出站）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> OutStationRangeByVehicleAsync(VehicleOutStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi);


        /// <summary>
        /// 批量中止
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> StopStationRangeBySFCAsync(SFCStopStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi);


    }
}
