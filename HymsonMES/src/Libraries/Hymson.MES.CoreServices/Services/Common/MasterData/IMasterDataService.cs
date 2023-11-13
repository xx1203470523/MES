using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Common.MasterData;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuCommon.ManuCommon;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.CoreServices.Services.Common.MasterData
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMasterDataService
    {
        /// <summary>
        /// 获取物料基础信息（带空检查）
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<ProcMaterialEntity> GetProcMaterialEntityWithNullCheckAsync(long materialId);

        /// <summary>
        /// 获取物料基础信息（带空检查）
        /// </summary>
        /// <param name="materialIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialEntity>> GetProcMaterialEntityWithNullCheckAsync(IEnumerable<long> materialIds);

        /// <summary>
        /// 获取物料基础信息（带空检查）
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<ProcProcessRouteEntity> GetProcProcessRouteEntityWithNullCheckAsync(long processRouteId);

        /// <summary>
        /// 获取条码基础信息（带空检查）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetManuSFCEntitiesWithNullCheckAsync(MultiSFCBo bo);


        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(WorkOrderIdBo bo);

        /// <summary>
        /// 获取生产工单（批量）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetProduceWorkOrderByIdsAsync(WorkOrderIdsBo bo);

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
        Task<IEnumerable<ManuSfcProduceEntity>> GetProduceEntitiesBySFCsWithCheckAsync(MultiSFCBo sfcBos);

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceInfoView>> GetManuSfcProduceInfoEntitiesAsync(MultiSFCBo sfcBos);

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceBusinessEntity>> GetProduceBusinessEntitiesBySFCsAsync(MultiSFCBo sfcBos);

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long processRouteId);

        /// <summary>
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="routeProcedureWithWorkOrderBo"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity?> GetNextProcedureAsync(ManuRouteProcedureWithWorkOrderBo routeProcedureWithWorkOrderBo);

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="routeProcedureWithInfoBo"></param>
        /// <returns></returns>
        Task<bool> IsRandomPreProcedureAsync(ManuRouteProcedureWithInfoBo routeProcedureWithInfoBo);

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="routeProcedureBo"></param>
        /// <returns></returns>
        Task<bool> IsRandomPreProcedureAsync(ManuRouteProcedureBo routeProcedureBo);

        /// <summary>
        /// 比较两个工序之间是否均是随机工序
        /// </summary>
        /// <param name="routeProcedureRandomCompareBo"></param>
        /// <returns></returns>
        Task<bool> IsAllRandomProcedureBetweenAsync(ManuRouteProcedureRandomCompareBo routeProcedureRandomCompareBo);

        /// <summary>
        /// 判断是否首工序
        /// </summary>
        /// <param name="routeProcedureBo"></param>
        /// <returns></returns>
        Task<bool> IsFirstProcedureAsync(ManuRouteProcedureBo routeProcedureBo);

        /// <summary>
        /// 判断当前工序是否在指定工序之前
        /// </summary>
        /// <param name="routeProcedureWithCompareBo"></param>
        /// <returns></returns>
        Task<bool> IsBeforeProcedureAsync(ManuRouteProcedureWithCompareBo routeProcedureWithCompareBo);

        /// <summary>
        /// 获取工序关联的资源
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<IEnumerable<long>> GetProcResourceIdByProcedureIdAsync(long procedureId);

        /// <summary>
        /// 获取生产配置中产品id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<long?> GetProductSetIdAsync(ProductSetBo param);

        /// <summary>
        /// 获取关联的job
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<JobBo>?> GetJobRelationJobByProcedureIdOrResourceIdAsync(JobRelationBo param);

        /// <summary>
        /// 获取即将扣料的物料数据
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        Task<IEnumerable<MaterialDeductResponseBo>> GetInitialMaterialsAsync(MaterialDeductRequestBo requestBo);

        /// <summary>
        /// 获取即将扣料的物料数据（包含半成品信息）
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        Task<MaterialDeductResponseSummaryBo> GetInitialMaterialsWithSmiFinishedAsync(MaterialDeductRequestBo requestBo);

        /// <summary>
        /// 获取流转数据
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetSFCCirculationEntitiesByTypesAsync(SFCCirculationBo bo);

        /// <summary>
        /// 获取不合格代码列表
        /// </summary>
        /// <param name="unqualifiedIds"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetQualUnqualifiedCodesAsync(long[] unqualifiedIds);

        /// <summary>
        /// 创建条码步骤数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ManuSfcStepEntity CreateSFCStepEntity(ManuSfcProduceEntity sfc, ManuSfcStepTypeEnum type, long siteId, string remark = "");

        /// <summary>
        /// 进行扣料（单一物料，包含物料的替代料）
        /// </summary>
        /// <param name="updates">需要更新数量的集合</param>
        /// <param name="adds">需要新增的条码流转集合</param>
        /// <param name="residue">剩余未扣除的数量</param>
        /// <param name="sfcProduceEntity">条码在制信息</param>
        /// <param name="manuFeedingsDictionary">已分组的物料库存集合</param>
        /// <param name="mainMaterialBo">主物料BO对象</param>
        /// <param name="currentBo">替代料BO对象</param>
        /// <param name="isMain">是否主物料</param>
        void DeductMaterialQty(ref List<UpdateFeedingQtyByIdCommand> updates,
             ref List<ManuSfcCirculationEntity> adds,
             ref decimal residue,
             ManuSfcProduceEntity sfcProduceEntity,
             Dictionary<long, IGrouping<long, ManuFeedingEntity>> manuFeedingsDictionary,
             MaterialDeductResponseBo mainMaterialBo,
             MaterialDeductResponseBo currentBo,
             bool isMain = true);

        /// <summary>
        /// 获取当前生产对象
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        Task<ManufactureProcedureBo> GetManufactureEquipmentAsync(ManufactureEquipmentBo param);

        /// <summary>
        /// 读取分选规则信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleEntity>> GetSortingRulesAsync(ProcSortingRuleQuery query);
    }
}
