using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon
{
    /// <summary>
    /// 生产共用
    /// </summary>
    public interface IManuCommonService
    {
        /// <summary>
        /// 获取生产条码信息（附带条码合法性校验 + 工序活动状态校验）
        /// </summary>
        /// <param name="spc"></param>
        /// <param name="procedureId"></param>
        /// <param name="allowStatus"></param>
        /// <returns></returns>
        Task<ManuSfcProduceEntity> GetProduceSPCWithCheckAsync(string spc, long procedureId, SfcProduceStatusEnum[] allowStatus);

        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(long workOrderId);

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long processRouteId);

        /// <summary>
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity?> GetNextProcedureAsync(long processRouteId, long procedureId);


    }
}
