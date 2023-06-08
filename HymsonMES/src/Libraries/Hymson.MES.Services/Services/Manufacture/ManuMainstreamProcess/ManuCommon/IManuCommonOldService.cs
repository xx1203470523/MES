using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon
{
    /// <summary>
    /// 生产共用
    /// </summary>
    public interface IManuCommonOldService
    {
        /// <summary>
        /// 验证条码掩码规则
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<bool> CheckBarCodeByMaskCodeRuleAsync(string barCode, long materialId);

        /// <summary>
        /// 检查条码是否可以执行某流程
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="sfcCirculationType"></param>
        /// <returns></returns>
        Task<bool> CheckSFCIsCanDoneStepAsync(ManufactureBo bo, SfcCirculationTypeEnum sfcCirculationType);

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
        /// 获取工艺路线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcessRouteDetailDto>> GetProcessRouteAsync(long id);

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

        /*
        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task VerifySfcsLockAsync(IEnumerable<string> sfcs);
        */

        /// <summary>
        /// 批量判断条码是否锁定
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task VerifySfcsLockAsync(string[] sfcs, long procedureId);

        /// <summary>
        /// 批量验证条码是否被容器包装
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task VerifyContainerAsync(string[] sfcs);

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="procedureId"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task VerifyBomQtyAsync(long bomId, long procedureId, string sfc);
    }
}
