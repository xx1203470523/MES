using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Quality;

namespace Hymson.MES.CoreServices.Services.Common
{
    /// <summary>
    /// 服务接口（主数据）
    /// </summary>
    public partial interface IMasterDataService
    {
        /// <summary>
        /// 根据掩码ID获取掩码规则
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaskCodeRuleEntity>> GetMaskCodeRuleEntitiesByMaskCodeIdAsync(long maskCodeId);

        /// <summary>
        /// 根据Code获取实体（设备）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetEquipmentEntityByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 根据ID获取实体（物料）
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<ProcMaterialEntity> GetMaterialEntityByIdAsync(long materialId);

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
        /// 根据ID获取工单实体
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetWorkOrderEntityByIdAsync(long workOrderId);

        /// <summary>
        /// 根据ID获取工单实体
        /// </summary>
        /// <param name="workOrderIds"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetWorkOrderEntitiesByIdsAsync(IEnumerable<long> workOrderIds);

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
        /// 获取工艺路线集合（连线）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetProcessRouteLinkEntitiesAsync(EntityBySiteIdQuery query);

        /// <summary>
        /// 获取工艺路线集合（节点）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteDetailNodeEntity>> GetProcessRouteNodeEntitiesAsync(EntityBySiteIdQuery query);

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long processRouteId);

        /// <summary>
        /// 根据ID获取工序实体
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity> GetProcedureEntityByIdAsync(long procedureId);

        /// <summary>
        /// 根据ID获取实体（工序）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetProcedureEntitiesByResourceIdAsync(EntityByLinkIdQuery query);

        /// <summary>
        /// 根据ID获取工序实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureRejudgeEntity>> GetProcedureRejudgeEntitiesAsync(EntityByParentIdQuery query);

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
        /// 根据Code获取实体（资源）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcResourceEntity> GetResourceEntityByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 根据设备Code获取实体（资源）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetResourceEntitiesByEquipmentCodeAsync(ProcResourceQuery query);

        /// <summary>
        /// 获取工序关联的资源
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<IEnumerable<long>> GetProcResourceIdByProcedureIdAsync(long procedureId);

        /// <summary>
        /// 获取资源关联的工序
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>?> GetProcProcedureIdByResourceIdAsync(QueryByIdBo query);

        /// <summary>
        /// 获取资源关联的工序
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetLoadPointLinkEntitiesByResourceIdAsync(long resourceId);

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
        /// 获取BOM关联的物料
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomDetailEntity>> GetBomDetailEntitiesByBomIdAsync(long bomId);

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
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetUnqualifiedEntitiesByIdsAsync(IEnumerable<long> unqualifiedIds);

        /// <summary>
        /// 获取不合格代码列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetUnqualifiedEntitiesByCodesAsync(QualUnqualifiedCodeByCodesQuery query);

        /// <summary>
        /// 创建条码步骤数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="type"></param>
        /// <param name="siteId"></param>
        /// <param name="operationProcedureId"></param>
        /// <param name="operationResourceId"></param>
        /// <param name="operationEquipmentId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ManuSfcStepEntity CreateSFCStepEntity(ManuSfcProduceEntity sfc, ManuSfcStepTypeEnum type, long siteId,
              long? operationProcedureId, long? operationResourceId, long? operationEquipmentId,
                string remark = "");

        /// <summary>
        /// 进行扣料（单一物料，包含物料的替代料）
        /// </summary>
        /// <param name="updates">需要更新数量的集合</param>
        /// <param name="adds">需要新增的条码流转集合</param>
        /// <param name="residue">剩余未扣除的数量</param>
        /// <param name="sfcProduceEntity">条码在制信息</param>
        /// <param name="manuFeedingsDictionary">已分组的物料库存集合</param>
        /// <param name="mainMaterialBo">主物料BO对象</param>
        /// <param name="currentBo">当前进行消耗的物料BO对象</param>
        /// <param name="isMain">是否主物料</param>
        [Obsolete("建议用BatchMaterialConsumption", true)]
        void DeductMaterialQty(ref List<UpdateFeedingQtyByIdCommand> updates,
             ref List<ManuSfcCirculationEntity> adds,
             ref decimal residue,
             ManuSfcProduceEntity sfcProduceEntity,
             Dictionary<long, IGrouping<long, ManuFeedingEntity>> manuFeedingsDictionary,
             MaterialDeductResponseBo mainMaterialBo,
             MaterialDeductResponseBo currentBo,
             bool isMain = true);

        /// <summary>
        /// 进行扣料（单一物料，包含物料的替代料）
        /// </summary>
        /// <param name="updates">需要更新数量的集合</param>
        /// <param name="adds">需要新增的条码流转集合</param>
        /// <param name="residue">剩余未扣除的数量</param>
        /// <param name="coreEntryRequestBo">是否主物料</param>
        void BatchMaterialConsumptionCoreEntry(ref List<UpdateFeedingQtyByIdCommand> updates,
            ref List<ManuBarCodeRelationEntity> adds,
            ref decimal residue,
            ConsumptionCoreEntryRequestBo coreEntryRequestBo);

        /// <summary>
        /// 读取分选规则信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleEntity>> GetSortingRulesAsync(ProcSortingRuleQuery query);

        /// <summary>
        /// 获取条码参数列表
        /// </summary>
        /// <param name="parameterBySfcQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<Core.Domain.Parameter.ManuProductParameterEntity>> GetProductParameterBySfcsAsync(ManuProductParameterBySfcQuery parameterBySfcQuery);


        #region 获取基础数据带缓存 主要用于循环加载中使用
        /// <summary>
        /// 通过Id获取物料实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcMaterialEntity?> GetProcMaterialEntityAsync(long siteId, long id);
        /// <summary>
        /// 通过Id获取工单实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<PlanWorkOrderEntity?> GetPlanWorkOrderEntityAsync(long siteId, long id);

        /// <summary>
        /// 通过Id获取工作中心实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteWorkCenterEntity?> GetInteWorkCenterEntityAsync(long siteId, long id);

        /// <summary>
        /// 通过Id获取工艺路线实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcessRouteEntity?> GetProcProcessRouteEntityAsync(long siteId, long id);

        /// <summary>
        /// 通过Id获取BOM实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcBomEntity?> GetProcBomEntityAsync(long siteId, long id);

        /// <summary>
        /// 通过Id获取资源实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceEntity?> GetProcResourceEntityAsync(long siteId, long id);
        /// <summary>
        /// 通过Id获取设备实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity?> GetEquEquipmentEntityAsync(long siteId, long id);


        /// <summary>
        /// 通过Id获取工序实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity?> GetProcProcedureEntityAsync(long siteId,  long id);

        /// <summary>
        /// 通过Id获取参数实体数据 走缓存 主要用于循环加载中使用
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcParameterEntity?> GetProcParameterEntityAsync(long siteId, long id);


        #endregion 获取基础数据带缓存 主要用于循环加载中使用

    }
}
