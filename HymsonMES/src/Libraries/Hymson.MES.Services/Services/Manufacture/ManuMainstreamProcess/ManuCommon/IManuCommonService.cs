using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon
{
    /// <summary>
    /// 生产共用
    /// </summary>
    public interface IManuCommonService
    {

        /// <summary>
        /// 验证条码掩码规则
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<bool> CheckBarCodeByMaskCodeRule(string barCode, long materialId);

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
      Task<(ManuSfcProduceEntity, ManuSfcProduceBusinessEntity)> GetProduceSFCAsync(string sfc);

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
        /// <param name="manuSfcProduce"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity?> GetNextProcedureAsync(ManuSfcProduceEntity manuSfcProduce);

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="manuSfcProduce"></param>
        /// <returns></returns>
        Task<bool> IsRandomPreProcedure(ManuSfcProduceEntity manuSfcProduce);

        /// <summary>
        /// 获取工艺路线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcessRouteDetailDto>> GetProcessRoute(long id);

        /// <summary>
        /// 验证开始工序是否在结束工序之前
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="startProcedureId"></param>
        /// <param name="endProcedureId"></param>
        /// <returns></returns>
        Task<bool> IsProcessStartBeforeEnd(long processRouteId, long startProcedureId, long endProcedureId);
    }
}
