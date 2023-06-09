using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.EquipmentServices.Dtos.Common;
using Hymson.MES.EquipmentServices.Dtos.ManuCommonDto;

namespace Hymson.MES.EquipmentServices.Services.Common
{
    /// <summary>
    /// 生产共用
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 验证条码掩码规则
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<bool> CheckBarCodeByMaskCodeRuleAsync(string barCode, long materialId);

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long processRouteId);

        /// <summary>
        /// 判断是否首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<bool> IsFirstProcedureAsync(long processRouteId, long procedureId);

        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(long workOrderId);

        /// <summary>
        /// 读取并执行Job
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ReadAndExecuteJobAsync(InStationRequestDto dto);
    }
}
