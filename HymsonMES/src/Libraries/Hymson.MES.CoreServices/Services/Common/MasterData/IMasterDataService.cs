using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuCommon.ManuCommon;

namespace Hymson.MES.CoreServices.Services.Common.MasterData
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMasterDataService
    {
        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="isVerifyActivation"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(long workOrderId, bool isVerifyActivation = true);

        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetWorkOrderByIdAsync(long workOrderId);

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBo"></param>
        /// <returns></returns>
        Task<(ManuSfcProduceEntity, ManuSfcProduceBusinessEntity)> GetProduceSFCAsync(SingleSFCBo sfcBo);

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceEntity>> GetProduceEntitiesBySFCsAsync(MultiSFCBo sfcBos);

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceBusinessEntity>> GetProduceBusinessEntitiesBySFCsAsync(MultiSFCBo sfcBos);

        /// <summary>
        /// 通过BomId获取物料集合（包含替代料）
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<IEnumerable<BomMaterialBo>> GetProcMaterialEntitiesByBomIdAndProcedureIdAsync(long bomId, long procedureId);

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
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity?> GetNextProcedureAsync(long processRouteId, long procedureId, long workOrderId = 0);

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="processRouteDetailLinks"></param>
        /// <param name="processRouteDetailNodes"></param>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<bool> IsRandomPreProcedureAsync(IEnumerable<ProcProcessRouteDetailLinkEntity> processRouteDetailLinks, IEnumerable<ProcProcessRouteDetailNodeEntity> processRouteDetailNodes, long processRouteId, long procedureId);

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<bool> IsRandomPreProcedureAsync(long processRouteId, long procedureId);

        /// <summary>
        /// 判断是否首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<bool> IsFirstProcedureAsync(long processRouteId, long procedureId);

        /// <summary>
        /// 验证开始工序是否在结束工序之前
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="startProcedureId"></param>
        /// <param name="endProcedureId"></param>
        /// <returns></returns>
        Task<bool> IsProcessStartBeforeEndAsync(long processRouteId, long startProcedureId, long endProcedureId);

        /// <summary>
        /// 获取工序关联的资源
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<IEnumerable<long>> GetProcResourceIdByProcedureIdAsync(long procedureId);

        /// <summary>
        /// 获取工艺路线
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcessRouteDetailDto>> GetProcessRouteAsync(long processRouteId);

    }
}
