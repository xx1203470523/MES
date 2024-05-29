using Dapper;
using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteJob.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Common
{
    /// <summary>
    /// 主数据公用类
    /// @author wangkeming
    /// @date 2023-06-06
    /// </summary>
    public partial class MasterDataService : IMasterDataService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<MasterDataService> _logger;

        /// <summary>
        /// 序列号服务
        /// </summary>
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码流转信息）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（工单信息）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（工单激活信息）
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（物料替代料）
        /// </summary>
        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;

        /// <summary>
        /// 仓储接口（掩码规则维护）
        /// </summary>
        private readonly IProcMaskCodeRuleRepository _procMaskCodeRuleRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（工序复投设置）
        /// </summary>
        private readonly IProcProcedureRejudgeRepository _procProcedureRejudgeRepository;

        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（BOM替代料明细）
        /// </summary>
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;

        /// <summary>
        /// 仓储接口（工艺路线）
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 仓储接口（生产配置）
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;

        /// <summary>
        ///  仓储（上料点关联资源）
        /// </summary>
        private readonly IProcLoadPointLinkResourceRepository _procLoadPointLinkResourceRepository;

        /// <summary>
        /// 仓储接口（作业）
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;

        /// <summary>
        /// 仓储接口（作业业务配置）
        /// </summary>
        private readonly IInteJobBusinessRelationRepository _inteJobBusinessRelationRepository;

        /// <summary>
        /// 不合格代码仓储
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        /// 仓储接口（分选规则）
        /// </summary>
        private readonly IProcSortingRuleRepository _sortingRuleRepository;

        /// <summary>
        /// 仓储接口（分选规则详情）
        /// </summary>
        private readonly IProcSortingRuleDetailRepository _sortingRuleDetailRepository;

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        ///产品参数采集
        /// </summary>
        private readonly IManuProductParameterRepository _productParameterRepository;

        /// <summary>
        /// 仓储接口（产品工序时间）
        /// </summary>
        private readonly IProcProductTimecontrolRepository _procProductTimecontrolRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sequenceService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="procMaskCodeRuleRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcedureRejudgeRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="procProductSetRepository"></param>
        /// <param name="procLoadPointLinkResourceRepository"></param>
        /// <param name="inteJobRepository"></param>
        /// <param name="inteJobBusinessRelationRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="sortingRuleRepository"></param>
        /// <param name="sortingRuleDetailRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="productParameterRepository"></param>
        /// <param name="procProductTimecontrolRepository"></param>
        public MasterDataService(ILogger<MasterDataService> logger,
            ISequenceService sequenceService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            IProcMaskCodeRuleRepository procMaskCodeRuleRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcProcedureRejudgeRepository procProcedureRejudgeRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IProcProductSetRepository procProductSetRepository,
            IProcLoadPointLinkResourceRepository procLoadPointLinkResourceRepository,
            IInteJobRepository inteJobRepository,
            IInteJobBusinessRelationRepository inteJobBusinessRelationRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IProcSortingRuleRepository sortingRuleRepository,
            IProcSortingRuleDetailRepository sortingRuleDetailRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IManuProductParameterRepository productParameterRepository,
            IProcProductTimecontrolRepository procProductTimecontrolRepository)
        {
            _logger = logger;
            _sequenceService = sequenceService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _procMaskCodeRuleRepository = procMaskCodeRuleRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcedureRejudgeRepository = procProcedureRejudgeRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _procProductSetRepository = procProductSetRepository;
            _procLoadPointLinkResourceRepository = procLoadPointLinkResourceRepository;
            _inteJobRepository = inteJobRepository;
            _inteJobBusinessRelationRepository = inteJobBusinessRelationRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _sortingRuleRepository = sortingRuleRepository;
            _sortingRuleDetailRepository = sortingRuleDetailRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _productParameterRepository = productParameterRepository;
            _procProductTimecontrolRepository = procProductTimecontrolRepository;
        }

        /// <summary>
        /// 根据掩码ID获取掩码规则
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaskCodeRuleEntity>> GetMaskCodeRuleEntitiesByMaskCodeIdAsync(long maskCodeId)
        {
            return await _procMaskCodeRuleRepository.GetByMaskCodeIdAsync(maskCodeId);
        }

        /// <summary>
        /// 根据Code获取实体（设备）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquEquipmentEntity> GetEquipmentEntityByCodeAsync(EntityByCodeQuery query)
        {
            return await _equEquipmentRepository.GetByCodeAsync(query);
        }

        /// <summary>
        /// 根据ID获取实体（物料）
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<ProcMaterialEntity> GetMaterialEntityByIdAsync(long materialId)
        {
            return await _procMaterialRepository.GetByIdAsync(materialId);
        }

        /// <summary>
        /// 获取物料基础信息（带空检查）
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<ProcMaterialEntity> GetProcMaterialEntityWithNullCheckAsync(long materialId)
        {
            // 读取产品基础信息
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(materialId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17103));

            return procMaterialEntity;
        }

        /// <summary>
        /// 获取物料基础信息（带空检查）
        /// </summary>
        /// <param name="materialIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetProcMaterialEntityWithNullCheckAsync(IEnumerable<long> materialIds)
        {
            // 读取产品基础信息
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(materialIds)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17103));

            // 不在系统中的物料Id
            var notInSystem = materialIds.Except(procMaterialEntities.Select(s => s.Id));
            if (notInSystem.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17106)).WithData("Ids", string.Join(',', notInSystem));
            }

            return procMaterialEntities;
        }

        /// <summary>
        /// 获取工艺路线基础信息（带空检查）
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteEntity> GetProcProcessRouteEntityWithNullCheckAsync(long processRouteId)
        {
            // 读取当前工艺路线信息
            var processRouteEntity = await _procProcessRouteRepository.GetByIdAsync(processRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18107));

            return processRouteEntity;
        }

        /// <summary>
        /// 获取工艺路线基础信息（带空检查）
        /// </summary>
        /// <param name="processRouteIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteEntity>> GetProcProcessRouteEntityWithNullCheckAsync(IEnumerable<long> processRouteIds)
        {
            // 读取当前工艺路线信息
            var processRouteEntities = await _procProcessRouteRepository.GetByIdsAsync(processRouteIds)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18107));

            // 不在系统中的工艺路线Id
            var notInSystem = processRouteIds.Except(processRouteEntities.Select(s => s.Id));
            if (notInSystem.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17111)).WithData("Ids", string.Join(',', notInSystem));
            }

            return processRouteEntities;
        }

        /// <summary>
        /// 获取条码基础信息（带空检查）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetManuSFCEntitiesWithNullCheckAsync(MultiSFCBo bo)
        {
            // 条码信息
            var manuSfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = bo.SiteId,
                SFCs = bo.SFCs,
                Type = SfcTypeEnum.Produce
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17104));

            return manuSfcEntities;
        }

        /// <summary>
        /// 根据ID获取工单实体
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetWorkOrderEntityByIdAsync(long workOrderId)
        {
            return await _planWorkOrderRepository.GetByIdAsync(workOrderId);
        }

        /// <summary>
        /// 根据ID获取工单实体
        /// </summary>
        /// <param name="workOrderIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetWorkOrderEntitiesByIdsAsync(IEnumerable<long> workOrderIds)
        {
            return await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
        }

        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(WorkOrderIdBo bo)
        {
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(bo.WorkOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            // 判断是否被锁定
            if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Pending)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16302)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            if (bo.IsVerifyActivation)
            {
                // 判断是否是激活的工单
                _ = await _planWorkOrderActivationRepository.GetByWorkOrderIdAsync(planWorkOrderEntity.Id)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16410));
            }

            switch (planWorkOrderEntity.Status)
            {
                case PlanWorkOrderStatusEnum.SendDown:
                case PlanWorkOrderStatusEnum.InProduction:
                case PlanWorkOrderStatusEnum.Finish:
                    break;
                case PlanWorkOrderStatusEnum.NotStarted:
                case PlanWorkOrderStatusEnum.Closed:
                default:
                    throw new CustomerValidationException(nameof(ErrorCode.MES16303)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            return planWorkOrderEntity;
        }

        /// <summary>
        /// 获取生产工单（批量）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetProduceWorkOrderByIdsAsync(WorkOrderIdsBo bo)
        {
            bo.WorkOrderIds = bo.WorkOrderIds.Distinct();

            var planWorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(bo.WorkOrderIds)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            // 不在系统中的工单Id
            var notInSystem = bo.WorkOrderIds.Except(planWorkOrderEntities.Select(s => s.Id));
            if (notInSystem.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17112)).WithData("Ids", string.Join(',', notInSystem));
            }

            var validationFailures = new List<ValidationFailure>();
            foreach (var planWorkOrderEntity in planWorkOrderEntities)
            {
                var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", planWorkOrderEntity.OrderCode);

                if (bo.IsVerifyActivation)
                {
                    // 判断是否是激活的工单
                    var workOrderActivationRecord = await _planWorkOrderActivationRepository.GetByWorkOrderIdAsync(planWorkOrderEntity.Id);
                    if (workOrderActivationRecord == null)
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("Code", planWorkOrderEntity.OrderCode);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16416);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                }

                switch (planWorkOrderEntity.Status)
                {
                    // 判断是否被锁定
                    case PlanWorkOrderStatusEnum.Pending:
                        validationFailure.FormattedMessagePlaceholderValues.Add("ordercode", planWorkOrderEntity.OrderCode);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16302);
                        validationFailures.Add(validationFailure);
                        continue;
                    case PlanWorkOrderStatusEnum.SendDown:
                    case PlanWorkOrderStatusEnum.InProduction:
                    case PlanWorkOrderStatusEnum.Finish:
                        break;
                    case PlanWorkOrderStatusEnum.NotStarted:
                    case PlanWorkOrderStatusEnum.Closed:
                    default:
                        validationFailure.FormattedMessagePlaceholderValues.Add("ordercode", planWorkOrderEntity.OrderCode);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16303);
                        validationFailures.Add(validationFailure);
                        continue;
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException("", validationFailures);
            }

            return planWorkOrderEntities;
        }

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceEntity>> GetProduceEntitiesBySFCsAsync(MultiSFCBo sfcBos)
        {
            if (sfcBos.SFCs.Any(a => a.Contains(' '))) throw new CustomerValidationException(nameof(ErrorCode.MES16305));

            // 条码在制表
            var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
            {
                SiteId = sfcBos.SiteId,
                Sfcs = sfcBos.SFCs
            });

            return sfcProduceEntities;
        }

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceEntity>> GetProduceEntitiesBySFCsWithCheckAsync(MultiSFCBo sfcBos)
        {
            if (sfcBos.SFCs.Any(a => a.Contains(' '))) throw new CustomerValidationException(nameof(ErrorCode.MES16305));

            // 条码在制表
            var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
            {
                SiteId = sfcBos.SiteId,
                Sfcs = sfcBos.SFCs
            });

            if (!sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', sfcBos.SFCs));
            }

            // 不存在在制表的话，就去库存查找？？

            return sfcProduceEntities;
        }

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceInfoView>> GetManuSfcProduceInfoEntitiesAsync(MultiSFCBo sfcBos)
        {
            if (sfcBos.SFCs.Any(a => a.Contains(' '))) throw new CustomerValidationException(nameof(ErrorCode.MES16305));

            // 条码在制表
            var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceInfoEntitiesAsync(new ManuSfcProduceQuery
            {
                SiteId = sfcBos.SiteId,
                Sfcs = sfcBos.SFCs
            });

            // 不存在在制表的话，就去库存查找
            if (!sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }

            return sfcProduceEntities;
        }

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceBusinessEntity>> GetProduceBusinessEntitiesBySFCsAsync(MultiSFCBo sfcBos)
        {
            if (sfcBos.SFCs.Any(a => a.Contains(' '))) throw new CustomerValidationException(nameof(ErrorCode.MES16305));

            // 获取锁状态
            var sfcProduceBusinessEntities = await _manuSfcProduceRepository.GetSfcProduceBusinessEntitiesBySFCAsync(new SfcListProduceBusinessQuery
            {
                SiteId = sfcBos.SiteId,
                Sfcs = sfcBos.SFCs,
                BusinessType = ManuSfcProduceBusinessType.Lock
            });

            return sfcProduceBusinessEntities;
        }

        /// <summary>
        /// 获取工艺路线集合（连线）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetProcessRouteLinkEntitiesAsync(EntityBySiteIdQuery query)
        {
            return await _procProcessRouteDetailLinkRepository.GetListAsync(query);
        }

        /// <summary>
        /// 获取工艺路线集合（节点）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeEntity>> GetProcessRouteNodeEntitiesAsync(EntityBySiteIdQuery query)
        {
            return await _procProcessRouteDetailNodeRepository.GetListAsync(query);
        }

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long processRouteId)
        {
            var procProcessRouteDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(processRouteId);
            if (procProcessRouteDetailNodeEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16304));

            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procProcessRouteDetailNodeEntity.ProcedureId);
            if (procProcedureEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES10406));

            return new ProcessRouteProcedureDto
            {
                ProcessRouteId = processRouteId,
                SerialNo = procProcessRouteDetailNodeEntity.SerialNo,
                //ProcedureId = procProcessRouteDetailNodeEntity.ProcedureId,
                CheckType = procProcessRouteDetailNodeEntity.CheckType,
                CheckRate = procProcessRouteDetailNodeEntity.CheckRate,
                IsWorkReport = procProcessRouteDetailNodeEntity.IsWorkReport,
                ProcedureId = procProcedureEntity.Id,
                ProcedureCode = procProcedureEntity.Code,
                ProcedureName = procProcedureEntity.Name,
                Type = procProcedureEntity.Type,
                PackingLevel = procProcedureEntity.PackingLevel,
                ResourceTypeId = procProcedureEntity.ResourceTypeId,
                Cycle = procProcedureEntity.Cycle,
                IsRepairReturn = procProcedureEntity.IsRepairReturn
            };
        }

        /// <summary>
        /// 根据ID获取实体（工序）
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity> GetProcedureEntityByIdAsync(long procedureId)
        {
            return await _procProcedureRepository.GetByIdAsync(procedureId);
        }

        /// <summary>
        /// 根据ID获取实体（工序）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureEntity>> GetProcedureEntitiesByResourceIdAsync(EntityByLinkIdQuery query)
        {
            return await _procProcedureRepository.GetProcProduresByResourceIdAsync(new ProcProdureByResourceIdQuery
            {
                SiteId = query.SiteId,
                ResourceId = query.LinkId
            });
        }

        /// <summary>
        /// 根据ID获取工序实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureRejudgeEntity>> GetProcedureRejudgeEntitiesAsync(EntityByParentIdQuery query)
        {
            return await _procProcedureRejudgeRepository.GetEntitiesAsync(query);
        }

        /// <summary>
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="routeProcedureWithWorkOrderBo"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity?> GetNextProcedureAsync(ManuRouteProcedureWithWorkOrderBo routeProcedureWithWorkOrderBo)
        {
            // 因为可能有分叉，所以返回的下一步工序是集合
            var processRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(routeProcedureWithWorkOrderBo.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

            var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(routeProcedureWithWorkOrderBo.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

            // 数据过滤
            processRouteDetailLinks = processRouteDetailLinks.Where(w => w.PreProcessRouteDetailId == routeProcedureWithWorkOrderBo.ProcedureId);
            processRouteDetailNodes = processRouteDetailNodes.Where(w => processRouteDetailLinks.Select(s => s.ProcessRouteDetailId).Contains(w.ProcedureId));

            // 默认下一工序
            ProcProcessRouteDetailNodeEntity? defaultNextProcedure = null;

            // 有多工序分叉的情况
            if (processRouteDetailNodes.Count() > 1)
            {
                // 随机工序Key
                var cacheKey = $"{routeProcedureWithWorkOrderBo.WorkOrderId}-{routeProcedureWithWorkOrderBo.ProcessRouteId}-{routeProcedureWithWorkOrderBo.ProcedureId}";
                var count = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.None, cacheKey, maxLength: 9);

                // 检查是否有"空值"类型的工序
                defaultNextProcedure = processRouteDetailNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10441));

                // 如果不是第一次走该工序，count是从1开始，不包括0。
                if (count > 1)
                {
                    // 抽检类型不为空值的下一工序
                    var nextProcedureOfNone = processRouteDetailNodes.FirstOrDefault(f => f.CheckType != ProcessRouteInspectTypeEnum.None)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES10447));

                    // 判断工序抽检比例
                    if (nextProcedureOfNone.CheckType == ProcessRouteInspectTypeEnum.FixedScale
                        && nextProcedureOfNone.CheckRate == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10446));

                    // 如果满足抽检次数，就取出一个非"空值"的随机工序作为下一工序
                    if (count > 1 && count % nextProcedureOfNone.CheckRate == 0) defaultNextProcedure = nextProcedureOfNone;
                }
            }
            // 没有分叉的情况
            else
            {
                // 抽检类型不为空值的下一工序
                defaultNextProcedure = processRouteDetailNodes.FirstOrDefault()
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10440));

                switch (defaultNextProcedure.CheckType)
                {
                    case ProcessRouteInspectTypeEnum.FixedScale:
                        // 判断工序抽检比例
                        if (defaultNextProcedure.CheckRate == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10446));
                        break;
                    case ProcessRouteInspectTypeEnum.None:
                    case ProcessRouteInspectTypeEnum.RandomInspection:
                    case ProcessRouteInspectTypeEnum.SpecialSamplingInspection:
                    default:
                        break;
                }
            }

            return await _procProcedureRepository.GetByIdAsync(defaultNextProcedure.ProcedureId);
        }

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="processRouteDetailLinks"></param>
        /// <param name="processRouteDetailNodes"></param>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<bool> IsRandomPreProcedureAsync(ManuRouteProcedureWithInfoBo routeProcedureWithInfoBo)
        {
            routeProcedureWithInfoBo.ProcessRouteDetailLinks = routeProcedureWithInfoBo.ProcessRouteDetailLinks.Where(w => w.ProcessRouteDetailId == routeProcedureWithInfoBo.ProcedureId);
            if (!routeProcedureWithInfoBo.ProcessRouteDetailLinks.Any()) return false;

            routeProcedureWithInfoBo.ProcessRouteDetailNodes = routeProcedureWithInfoBo.ProcessRouteDetailNodes.Where(w => routeProcedureWithInfoBo.ProcessRouteDetailLinks.Select(s => s.PreProcessRouteDetailId).Contains(w.ProcedureId));
            if (!routeProcedureWithInfoBo.ProcessRouteDetailNodes.Any()) return false;

            // 有多工序分叉的情况（取第一个当默认值）
            ProcProcessRouteDetailNodeEntity? defaultPreProcedure = routeProcedureWithInfoBo.ProcessRouteDetailNodes.FirstOrDefault();
            if (routeProcedureWithInfoBo.ProcessRouteDetailLinks.Count() > 1)
            {
                // 下工序找上工序，执照正常流程的工序
                defaultPreProcedure = routeProcedureWithInfoBo.ProcessRouteDetailNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                   ?? throw new CustomerValidationException(nameof(ErrorCode.MES10441));
            }

            // 获取上一工序
            if (defaultPreProcedure == null) throw new CustomerValidationException(nameof(ErrorCode.MES10442));
            if (defaultPreProcedure.CheckType == ProcessRouteInspectTypeEnum.RandomInspection) return true;

            // 继续检查上一工序
            routeProcedureWithInfoBo.ProcedureId = defaultPreProcedure.Id;
            return await IsRandomPreProcedureAsync(routeProcedureWithInfoBo);
        }

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="routeProcedureBo"></param>
        /// <returns></returns>
        public async Task<bool> IsRandomPreProcedureAsync(ManuRouteProcedureBo routeProcedureBo)
        {
            // 因为可能有分叉，所以返回的上一步工序是集合
            var preProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetPreProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = routeProcedureBo.ProcessRouteId,
                ProcedureId = routeProcedureBo.ProcedureId
            });
            if (preProcessRouteDetailLinks == null || !preProcessRouteDetailLinks.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10442));

            // 获取当前工序在工艺路线里面的扩展信息
            var procedureNodes = await _procProcessRouteDetailNodeRepository
                .GetByProcedureIdsAsync(new ProcProcessRouteDetailNodesQuery
                {
                    ProcessRouteId = routeProcedureBo.ProcessRouteId,
                    ProcedureIds = preProcessRouteDetailLinks.Where(w => w.PreProcessRouteDetailId.HasValue).Select(s => s.PreProcessRouteDetailId!.Value)
                }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10442));

            // 有多工序分叉的情况（取第一个当默认值）
            ProcProcessRouteDetailNodeEntity? defaultPreProcedure = procedureNodes.FirstOrDefault();
            if (preProcessRouteDetailLinks.Count() > 1)
            {
                // 下工序找上工序，执照正常流程的工序
                defaultPreProcedure = procedureNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                   ?? throw new CustomerValidationException(nameof(ErrorCode.MES10441));
            }

            // 获取上一工序
            if (defaultPreProcedure == null) throw new CustomerValidationException(nameof(ErrorCode.MES10442));
            if (defaultPreProcedure.CheckType == ProcessRouteInspectTypeEnum.RandomInspection) return true;

            // 继续检查上一工序
            return await IsRandomPreProcedureAsync(new ManuRouteProcedureBo
            {
                ProcessRouteId = routeProcedureBo.ProcessRouteId,
                ProcedureId = defaultPreProcedure.Id
            });
        }

        /// <summary>
        /// 判断是否首工序
        /// </summary>
        /// <param name="routeProcedureBo"></param>
        /// <returns></returns>
        public async Task<bool> IsFirstProcedureAsync(ManuRouteProcedureBo routeProcedureBo)
        {
            var firstProcedureDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(routeProcedureBo.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10435));

            return firstProcedureDetailNodeEntity.ProcedureId == routeProcedureBo.ProcedureId;
        }

        /// <summary>
        /// 判断当前工序是否在指定工序之前
        /// </summary>
        /// <param name="routeProcedureWithCompareBo"></param>
        /// <returns></returns>
        public async Task<bool> IsBeforeProcedureAsync(ManuRouteProcedureWithCompareBo routeProcedureWithCompareBo)
        {
            var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(routeProcedureWithCompareBo.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

            // 找出目标工序
            var targetProcedureNode = processRouteDetailNodes.FirstOrDefault(f => f.ProcedureId == routeProcedureWithCompareBo.ProcedureId);
            if (targetProcedureNode == null) return false;

            // 找出目标工序前面的工序
            var compareProcedureNodes = processRouteDetailNodes.Where(f => f.SerialNo.ParseToInt() <= targetProcedureNode.SerialNo.ParseToInt());

            // 如果满足排序号条件的工序中不包含当前工序，则返回false
            return compareProcedureNodes.Any(a => a.ProcedureId == routeProcedureWithCompareBo.CurrentProcedureId);
        }

        /// <summary>
        /// 根据Code获取实体（资源）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcResourceEntity> GetResourceEntityByCodeAsync(EntityByCodeQuery query)
        {
            return await _procResourceRepository.GetByCodeAsync(query);
        }

        /// <summary>
        /// 根据设备Code获取实体（资源）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetResourceEntitiesByEquipmentCodeAsync(ProcResourceQuery query)
        {
            return await _procResourceRepository.GetByEquipmentCodeAsync(query);
        }

        /// <summary>
        /// 获取工序关联的资源
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<long>> GetProcResourceIdByProcedureIdAsync(long procedureId)
        {
            var resources = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(procedureId);

            if (resources == null || !resources.Any()) return Array.Empty<long>();
            return resources.Select(s => s.Id);
        }

        /// <summary>
        /// 获取资源关联的工序
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureEntity>?> GetProcProcedureIdByResourceIdAsync(QueryByIdBo query)
        {
            var resourceEntity = await _procResourceRepository.GetByIdAsync(query.QueryId);
            if (resourceEntity == null) return null;

            return await _procProcedureRepository.GetProcProduresByResourceIdAsync(new ProcProdureByResourceIdQuery
            {
                SiteId = query.SiteId,
                ResourceId = resourceEntity.Id,
            });
        }

        /// <summary>
        /// 获取资源关联的工序
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetLoadPointLinkEntitiesByResourceIdAsync(long resourceId)
        {
            return await _procLoadPointLinkResourceRepository.GetByResourceIdAsync(resourceId);
        }

        /// <summary>
        /// 获取生产配置中产品id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long?> GetProductSetIdAsync(ProductSetBo param)
        {
            // 读取资源的产出设置（优先）
            var productSetEntity = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new GetByProcedureIdAndProductIdQuery { ProductId = param.ProductId, SetPointId = param.ResourceId, SiteId = param.SiteId });
            if (productSetEntity != null) return productSetEntity.SemiProductId;

            // 读取工序的产出设置
            productSetEntity = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new GetByProcedureIdAndProductIdQuery { ProductId = param.ProductId, SetPointId = param.ProcedureId, SiteId = param.SiteId });
            if (productSetEntity != null) return productSetEntity.SemiProductId;

            return null;
        }

        /// <summary>
        /// 获取关联的job
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> GetJobRelationJobByProcedureIdOrResourceIdAsync(JobRelationBo param)
        {
            var inteJobBusinessRelations = await _inteJobBusinessRelationRepository.GetByJobByBusinessIdAsync(new InteJobBusinessRelationByBusinessIdQuery
            {
                BusinessId = param.ResourceId,
                LinkPoint = param.LinkPoint,
                IsUse = true
            });
            if (inteJobBusinessRelations == null || !inteJobBusinessRelations.Any())
            {
                inteJobBusinessRelations = await _inteJobBusinessRelationRepository.GetByJobByBusinessIdAsync(new InteJobBusinessRelationByBusinessIdQuery
                {
                    BusinessId = param.ProcedureId,
                    LinkPoint = param.LinkPoint,
                    IsUse = true
                });
            }
            if (inteJobBusinessRelations == null || !inteJobBusinessRelations.Any()) return null;

            var jobEntities = await _inteJobRepository.GetByIdsAsync(inteJobBusinessRelations.Select(s => s.JobId));
            return jobEntities.Select(s => new JobBo { Name = s.ClassProgram });
        }

        /// <summary>
        /// 组装工艺路线
        /// </summary>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="procedureId"></param>
        /// <param name="procProcessRouteDetailLinkEntities"></param>
        private void CombinationProcessRoute(ref IList<ProcessRouteDetailDto> list, long procedureId, IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetailLinkEntities, long key = 0)
        {
            if (list == null || !list.Any())
            {
                key = IdGenProvider.Instance.CreateId();
                list = new List<ProcessRouteDetailDto>
                {
                    new ProcessRouteDetailDto
                    {
                        key = key,
                        ProcedureIds = new List<long> { procedureId }
                    }
                };
            }

            var procProcessRouteDetailLinkByprocedureIdList = procProcessRouteDetailLinkEntities.Where(x => x.PreProcessRouteDetailId == procedureId);
            if (procProcessRouteDetailLinkByprocedureIdList != null && procProcessRouteDetailLinkByprocedureIdList.Any())
            {
                var processRouteDetail = list.FirstOrDefault(x => x.key == key);
                if (processRouteDetail != null)
                {
                    var procedureIds = processRouteDetail.ProcedureIds.ToList();
                    int index = 1;
                    foreach (var item in procProcessRouteDetailLinkByprocedureIdList.Select(x => x.ProcessRouteDetailId))
                    {
                        if (item != ProcessRoute.LastProcedureId)
                        {
                            if (index == 1)
                            {
                                processRouteDetail.ProcedureIds.Add(item);
                                CombinationProcessRoute(ref list, item, procProcessRouteDetailLinkEntities, key);
                            }
                            else
                            {
                                var processRouteDetailDto = new ProcessRouteDetailDto()
                                {
                                    key = IdGenProvider.Instance.CreateId(),
                                    ProcedureIds = procedureIds,
                                };
                                processRouteDetailDto.ProcedureIds.Add(item);
                                list.Add(processRouteDetailDto);
                                CombinationProcessRoute(ref list, item, procProcessRouteDetailLinkEntities, processRouteDetailDto.key);
                            }
                        }
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// 获取BOM关联的物料
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailEntity>> GetBomDetailEntitiesByBomIdAsync(long bomId)
        {
            return await _procBomDetailRepository.GetByBomIdAsync(bomId);
        }

        /// <summary>
        /// 获取即将扣料的物料数据
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MaterialDeductResponseBo>> GetInitialMaterialsAsync(MaterialDeductRequestBo requestBo)
        {
            // 获取初始扣料数据
            List<MaterialDeductResponseBo> initialMaterials = new();

            // 获取BOM绑定的物料
            var mainMaterials = await _procBomDetailRepository.GetByBomIdAsync(requestBo.ProductBOMId);

            // 未设置物料（克明说化成和返工的是没有投料的）
            if (mainMaterials == null || !mainMaterials.Any())
            {
                return initialMaterials;
            }

            // 取得特定工序的物料
            mainMaterials = mainMaterials.Where(w => w.ProcedureId == requestBo.ProcedureId);
            var materialIds = mainMaterials.Select(s => s.MaterialId).AsList();

            // 查询BOM替代料
            var replaceMaterialsForBOM = await _procBomDetailReplaceMaterialRepository.GetByBomIdAsync(requestBo.ProductBOMId);
            var replaceMaterialsForBOMDic = replaceMaterialsForBOM.ToLookup(w => w.BomDetailId).ToDictionary(d => d.Key, d => d);

            // 查询物料基础数据的替代料
            var replaceMaterialsForMain = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(requestBo.SiteId);
            replaceMaterialsForMain = replaceMaterialsForMain.Where(w => materialIds.Contains(w.MaterialId));
            var replaceMaterialsForMainDic = replaceMaterialsForMain.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);

            // 组合主物料ID和替代料ID
            materialIds.AddRange(replaceMaterialsForBOM.Select(s => s.ReplaceMaterialId));

            // 查询所有主物料和替代料的基础信息（为了读取消耗系数和收集方式）
            var materialEntities = await _procMaterialRepository.GetBySiteIdAsync(requestBo.SiteId);
            materialEntities = materialEntities.Where(w => materialIds.Contains(w.Id));

            foreach (var item in mainMaterials)
            {
                var materialEntitiy = materialEntities.FirstOrDefault(f => f.Id == item.MaterialId);
                if (materialEntitiy == null) continue;

                var deduct = new MaterialDeductResponseBo
                {
                    MaterialId = item.MaterialId,
                    MaterialCode = materialEntitiy.MaterialCode,
                    Usages = item.Usages,
                    Loss = item.Loss,
                    DataCollectionWay = item.DataCollectionWay,
                    SerialNumber = materialEntitiy.SerialNumber
                };
                if (materialEntitiy.ConsumeRatio.HasValue) deduct.ConsumeRatio = materialEntitiy.ConsumeRatio.Value;

                // 填充BOM替代料
                if (!item.IsEnableReplace)
                {
                    if (replaceMaterialsForBOMDic.TryGetValue(item.Id, out var replaces))
                    {
                        // 启用的替代物料（BOM）
                        deduct.ReplaceMaterials = replaces.Select(s => new MaterialDeductItemBo
                        {
                            MaterialId = s.ReplaceMaterialId,
                            Usages = s.Usages,
                            Loss = s.Loss,
                            DataCollectionWay = materialEntities.FirstOrDefault(f => f.Id == s.ReplaceMaterialId)!.SerialNumber,
                            ConsumeRatio = GetConsumeRatio(materialEntities, s.ReplaceMaterialId)
                        });
                    }
                }
                // 填充物料替代料
                else
                {
                    if (replaceMaterialsForMainDic.TryGetValue(item.MaterialId, out var replaces))
                    {
                        // 启用的替代物料（物料维护）
                        deduct.ReplaceMaterials = replaces.Where(w => w.IsEnabled).Select(s => new MaterialDeductItemBo
                        {
                            MaterialId = s.MaterialId,
                            Usages = item.Usages,
                            Loss = item.Loss,
                            DataCollectionWay = materialEntities.FirstOrDefault(f => f.Id == s.ReplaceMaterialId)!.SerialNumber,
                            ConsumeRatio = GetConsumeRatio(materialEntities, s.MaterialId)
                        });
                    }
                }

                // 添加到初始扣料集合
                initialMaterials.Add(deduct);
            }

            return initialMaterials;
        }

        /// <summary>
        /// 获取即将扣料的物料数据（包含半成品信息）
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        public async Task<MaterialDeductResponseSummaryBo> GetInitialMaterialsWithSmiFinishedAsync(MaterialDeductRequestBo requestBo)
        {
            var responseSummaryBo = new MaterialDeductResponseSummaryBo
            {
                // 初始扣料数据
                InitialMaterials = new()
            };

            // 获取BOM绑定的物料
            var mainMaterials = await _procBomDetailRepository.GetByBomIdAsync(requestBo.ProductBOMId);

            // 半成品清单
            if (requestBo.ProductId > 0) responseSummaryBo.SmiFinisheds = mainMaterials.Where(w => w.MaterialId == requestBo.ProductId);

            // 未设置物料（克明说化成和返工的是没有投料的）
            if (mainMaterials == null || !mainMaterials.Any()) return responseSummaryBo;

            // 取得特定工序的物料（经过此过滤，结果已不存在半成品）
            mainMaterials = mainMaterials.Where(w => w.ProcedureId == requestBo.ProcedureId);
            var materialIds = mainMaterials.Select(s => s.MaterialId).AsList();

            // 查询BOM替代料
            var replaceMaterialsForBOM = await _procBomDetailReplaceMaterialRepository.GetByBomIdAsync(requestBo.ProductBOMId);
            var replaceMaterialsForBOMDic = replaceMaterialsForBOM.ToLookup(w => w.BomDetailId).ToDictionary(d => d.Key, d => d);

            // 查询物料基础数据的替代料
            var replaceMaterialsForMain = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(requestBo.SiteId);
            replaceMaterialsForMain = replaceMaterialsForMain.Where(w => materialIds.Contains(w.MaterialId));
            var replaceMaterialsForMainDic = replaceMaterialsForMain.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);

            // 组合主物料ID和替代料ID
            materialIds.AddRange(replaceMaterialsForBOM.Select(s => s.ReplaceMaterialId));

            // 查询所有主物料和替代料的基础信息（为了读取消耗系数和收集方式）
            var materialEntities = await _procMaterialRepository.GetBySiteIdAsync(requestBo.SiteId);
            materialEntities = materialEntities.Where(w => materialIds.Contains(w.Id));

            foreach (var item in mainMaterials)
            {
                var materialEntitiy = materialEntities.FirstOrDefault(f => f.Id == item.MaterialId);
                if (materialEntitiy == null) continue;

                var deduct = new MaterialDeductResponseBo
                {
                    MaterialId = item.MaterialId,
                    MaterialCode = materialEntitiy.MaterialCode,
                    Usages = item.Usages,
                    Loss = item.Loss,
                    DataCollectionWay = item.DataCollectionWay,
                    SerialNumber = materialEntitiy.SerialNumber
                };
                if (materialEntitiy.ConsumeRatio.HasValue) deduct.ConsumeRatio = materialEntitiy.ConsumeRatio.Value;

                // 填充BOM替代料
                if (!item.IsEnableReplace)
                {
                    if (replaceMaterialsForBOMDic.TryGetValue(item.Id, out var replaces))
                    {
                        // 启用的替代物料（BOM）
                        deduct.ReplaceMaterials = replaces.Select(s => new MaterialDeductItemBo
                        {
                            MaterialId = s.ReplaceMaterialId,
                            Usages = s.Usages,
                            Loss = s.Loss,
                            DataCollectionWay = materialEntities.FirstOrDefault(f => f.Id == s.ReplaceMaterialId)!.SerialNumber,
                            ConsumeRatio = GetConsumeRatio(materialEntities, s.ReplaceMaterialId)
                        });
                    }
                }
                // 填充物料替代料
                else
                {
                    if (replaceMaterialsForMainDic.TryGetValue(item.MaterialId, out var replaces))
                    {
                        // 启用的替代物料（物料维护）
                        deduct.ReplaceMaterials = replaces.Where(w => w.IsEnabled).Select(s => new MaterialDeductItemBo
                        {
                            MaterialId = s.MaterialId,
                            Usages = item.Usages,
                            Loss = item.Loss,
                            DataCollectionWay = materialEntities.FirstOrDefault(f => f.Id == s.ReplaceMaterialId)!.SerialNumber,
                            ConsumeRatio = GetConsumeRatio(materialEntities, s.MaterialId)
                        });
                    }
                }

                // 添加到初始扣料集合
                responseSummaryBo.InitialMaterials.Add(deduct);
            }

            return responseSummaryBo;
        }

        /// <summary>
        /// 获取流转数据
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetSFCCirculationEntitiesByTypesAsync(SFCCirculationBo bo)
        {
            var types = new List<SfcCirculationTypeEnum>();

            if (bo.Type == SFCCirculationReportTypeEnum.Remove || bo.Type == SFCCirculationReportTypeEnum.Whole)
            {
                types.Add(SfcCirculationTypeEnum.Disassembly);
            }

            if (bo.Type == SFCCirculationReportTypeEnum.Activity || bo.Type == SFCCirculationReportTypeEnum.Whole)
            {
                types.Add(SfcCirculationTypeEnum.Consume);
                types.Add(SfcCirculationTypeEnum.ModuleAdd);
                types.Add(SfcCirculationTypeEnum.ModuleReplace);
            }

            var query = new ManuSfcCirculationQuery
            {
                Sfc = bo.SFC,
                SiteId = bo.SiteId,
                CirculationTypes = types
            };

            if (bo.Type == SFCCirculationReportTypeEnum.Remove)
            {
                query.IsDisassemble = TrueOrFalseEnum.Yes;
            }

            if (bo.Type == SFCCirculationReportTypeEnum.Activity)
            {
                query.IsDisassemble = TrueOrFalseEnum.No;
            }

            return await _manuSfcCirculationRepository.GetSfcMoudulesAsync(query);
        }

        /// <summary>
        /// 获取不合格代码列表
        /// </summary>
        /// <param name="unqualifiedIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeEntity>> GetUnqualifiedEntitiesByIdsAsync(IEnumerable<long> unqualifiedIds)
        {
            return await _qualUnqualifiedCodeRepository.GetByIdsAsync(unqualifiedIds);
        }

        /// <summary>
        /// 获取不合格代码列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeEntity>> GetUnqualifiedEntitiesByCodesAsync(QualUnqualifiedCodeByCodesQuery query)
        {
            return await _qualUnqualifiedCodeRepository.GetByCodesAsync(query);
        }

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
        public ManuSfcStepEntity CreateSFCStepEntity(ManuSfcProduceEntity sfc, ManuSfcStepTypeEnum type, long siteId,
          long? operationProcedureId, long? operationResourceId, long? operationEquipmentId,
            string remark = "")
        {
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = sfc.SFC,
                ProductId = sfc.ProductId,
                WorkOrderId = sfc.WorkOrderId,
                WorkCenterId = sfc.WorkCenterId,
                ProductBOMId = sfc.ProductBOMId,
                ProcessRouteId = sfc.ProcessRouteId,
                Qty = sfc.Qty,
                EquipmentId = sfc.EquipmentId,
                ResourceId = sfc.ResourceId,
                ProcedureId = sfc.ProcedureId,
                Operatetype = type,
                CurrentStatus = sfc.Status,
                OperationProcedureId = operationProcedureId,
                OperationResourceId = operationResourceId,
                OperationEquipmentId = operationEquipmentId,
                //Lock = sfc.Lock,
                Remark = remark,
                SiteId = siteId,
                CreatedBy = sfc.CreatedBy,
                UpdatedBy = sfc.UpdatedBy
            };
        }

        /// <summary>
        /// 取得消耗系数
        /// </summary>
        /// <param name="materialEntities"></param>
        /// <param name="replaceMaterialId"></param>
        /// <returns></returns>
        private static decimal GetConsumeRatio(IEnumerable<ProcMaterialEntity> materialEntities, long replaceMaterialId)
        {
            decimal defaultConsumeRatio = 100;

            if (materialEntities == null || !materialEntities.Any()) return defaultConsumeRatio;

            var materialEntity = materialEntities.FirstOrDefault(f => f.Id == replaceMaterialId);
            if (materialEntity == null || !materialEntity.ConsumeRatio.HasValue) return defaultConsumeRatio;

            return materialEntity.ConsumeRatio.Value;
        }

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
        public void DeductMaterialQty(ref List<UpdateFeedingQtyByIdCommand> updates,
            ref List<ManuSfcCirculationEntity> adds,
            ref decimal residue,
            ManuSfcProduceEntity sfcProduceEntity,
            Dictionary<long, IGrouping<long, ManuFeedingEntity>> manuFeedingsDictionary,
            MaterialDeductResponseBo mainMaterialBo,
            MaterialDeductResponseBo currentBo,
            bool isMain = true)
        {
            // 没有剩余需要抵扣时，直接返回
            if (residue <= 0) return;

            // 取得当前物料的库存
            if (!manuFeedingsDictionary.TryGetValue(currentBo.MaterialId, out var feedingEntities)) return;
            if (!feedingEntities.Any()) return;

            // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
            decimal originQty = currentBo.Usages;
            if (currentBo.Loss.HasValue && currentBo.Loss > 0) originQty *= (1 + currentBo.Loss.Value / 100);
            if (currentBo.ConsumeRatio > 0) originQty *= (currentBo.ConsumeRatio / 100);

            // 遍历当前物料的所有的物料库存
            foreach (var feeding in feedingEntities)
            {
                decimal targetQty = originQty;
                var consume = 0m;
                if (residue <= 0) break;
                if (feeding.Qty <= 0) continue;

                // 如果是替代料条码，就将替代料的消耗数值重新算下
                if (currentBo.MaterialId != feeding.MaterialId)
                {
                    var replaceBo = currentBo.ReplaceMaterials.FirstOrDefault(f => f.MaterialId == feeding.MaterialId);
                    if (replaceBo != null)
                    {
                        // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
                        targetQty = replaceBo.Usages;
                        if (replaceBo.Loss.HasValue && replaceBo.Loss > 0) targetQty *= replaceBo.Loss.Value;
                        if (replaceBo.ConsumeRatio > 0) targetQty *= (replaceBo.ConsumeRatio / 100);
                    }
                }

                // 剩余折算成目标数量
                var convertResidue = ToTargetValue(originQty, targetQty, residue);

                // 数量足够
                if (convertResidue <= feeding.Qty)
                {
                    consume = convertResidue;
                    residue = 0;
                    feeding.Qty -= consume;
                }
                // 数量不够，继续下一个
                else
                {
                    consume = feeding.Qty;
                    residue -= ToTargetValue(targetQty, originQty, consume);
                    feeding.Qty = 0;
                }

                // 添加到扣减物料库存
                updates.Add(new UpdateFeedingQtyByIdCommand
                {
                    UpdatedBy = sfcProduceEntity.UpdatedBy ?? sfcProduceEntity.CreatedBy,
                    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    Qty = consume,  // 因为现在是在SQL语句进行的扣减，所以不能用 feeding.Qty,
                    Id = feeding.Id
                });

                // 添加条码流转记录（消耗）
                adds.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = sfcProduceEntity.SiteId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    SFC = sfcProduceEntity.SFC,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    ProductId = sfcProduceEntity.ProductId,
                    CirculationBarCode = feeding.BarCode,
                    CirculationProductId = currentBo.MaterialId,
                    CirculationMainProductId = mainMaterialBo.MaterialId,
                    CirculationQty = consume,
                    CirculationType = SfcCirculationTypeEnum.Consume,
                    CreatedBy = sfcProduceEntity.CreatedBy,
                    UpdatedBy = sfcProduceEntity.UpdatedBy
                });
            }

            // 主物料才扣除检索下级替代料，当还有剩余未扣除的数量时，扣除替代料（替代料不再递归扣除下级替代料库存）
            if (!isMain || residue <= 0) return;

            // 扣除替代料
            foreach (var replaceFeeding in currentBo.ReplaceMaterials)
            {
                // 递归扣除替代料库存
                DeductMaterialQty(ref updates, ref adds, ref residue,
                    sfcProduceEntity, manuFeedingsDictionary, mainMaterialBo,
                    new MaterialDeductResponseBo
                    {
                        MaterialId = replaceFeeding.MaterialId,
                        Usages = replaceFeeding.Usages,
                        Loss = replaceFeeding.Loss,
                        ConsumeRatio = replaceFeeding.ConsumeRatio,
                        DataCollectionWay = mainMaterialBo.DataCollectionWay
                    }, false);
            }
        }

        /// <summary>
        /// 进行扣料（单一物料，包含物料的替代料）
        /// </summary>
        /// <param name="updates">需要更新数量的集合</param>
        /// <param name="adds">需要新增的条码流转集合</param>
        /// <param name="residue">剩余未扣除的数量</param>
        /// <param name="coreEntryRequestBo">是否主物料</param>
        public void BatchMaterialConsumptionCoreEntry(ref List<UpdateFeedingQtyByIdCommand> updates,
            ref List<ManuBarCodeRelationEntity> adds,
            ref decimal residue,
            ConsumptionCoreEntryRequestBo coreEntryRequestBo)
        {
            // 没有剩余需要抵扣时，直接返回
            if (residue <= 0) return;

            // 为了让下面的代码不动，这里做了一个转换
            var sfcProduceEntity = coreEntryRequestBo.SFCProduceEntity;
            var currentMaterialBo = coreEntryRequestBo.CurrentMaterialBo;

            // 取得当前物料的库存
            if (!coreEntryRequestBo.ManuFeedingsDict.TryGetValue(currentMaterialBo.MaterialId, out var feedingEntities)) return;
            if (!feedingEntities.Any()) return;

            // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
            decimal originQty = currentMaterialBo.Usages;
            if (currentMaterialBo.Loss.HasValue && currentMaterialBo.Loss > 0) originQty *= (1 + currentMaterialBo.Loss.Value / 100);
            if (currentMaterialBo.ConsumeRatio > 0) originQty *= (currentMaterialBo.ConsumeRatio / 100);

            // 遍历当前物料的所有的物料库存
            foreach (var feeding in feedingEntities)
            {
                decimal targetQty = originQty;
                var consume = 0m;
                if (residue <= 0) break;
                if (feeding.Qty <= 0) continue;

                // 如果是替代料条码，就将替代料的消耗数值重新算下
                if (currentMaterialBo.MaterialId != feeding.MaterialId)
                {
                    var replaceBo = currentMaterialBo.ReplaceMaterials.FirstOrDefault(f => f.MaterialId == feeding.MaterialId);
                    if (replaceBo != null)
                    {
                        // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
                        targetQty = replaceBo.Usages;
                        if (replaceBo.Loss.HasValue && replaceBo.Loss > 0) targetQty *= replaceBo.Loss.Value;
                        if (replaceBo.ConsumeRatio > 0) targetQty *= (replaceBo.ConsumeRatio / 100);
                    }
                }

                // 剩余折算成目标数量
                var convertResidue = ToTargetValue(originQty, targetQty, residue);

                // 数量足够
                if (convertResidue <= feeding.Qty)
                {
                    consume = convertResidue;
                    residue = 0;
                    feeding.Qty -= consume;
                }
                // 数量不够，继续下一个
                else
                {
                    consume = feeding.Qty;
                    residue -= ToTargetValue(targetQty, originQty, consume);
                    feeding.Qty = 0;
                }

                // 添加到扣减物料库存
                updates.Add(new UpdateFeedingQtyByIdCommand
                {
                    UpdatedBy = sfcProduceEntity.UpdatedBy ?? sfcProduceEntity.CreatedBy,
                    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    Qty = consume,  // 因为现在是在SQL语句进行的扣减，所以不能用 feeding.Qty,
                    Id = feeding.Id
                });

                // 添加条码流转记录（消耗）
                adds.Add(new ManuBarCodeRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    RelationType = ManuBarCodeRelationTypeEnum.SFC_Consumption,

                    InputBarCode = feeding.BarCode,
                    InputBarCodeLocation = "",
                    InputBarCodeWorkOrderId = sfcProduceEntity.WorkOrderId,
                    InputBarCodeMaterialId = feeding.MaterialId,
                    InputQty = consume,

                    OutputBarCode = sfcProduceEntity.SFC,
                    OutputBarCodeWorkOrderId = sfcProduceEntity.WorkOrderId,
                    OutputBarCodeMaterialId = sfcProduceEntity.ProductId,
                    OutputBarCodeMode = ManuBarCodeOutputModeEnum.Normal,

                    BusinessContent = new
                    {
                        InputSFCStepId = coreEntryRequestBo.SFCStepId,
                        OutputSFCStepId = coreEntryRequestBo.SFCStepId,
                        BomId = sfcProduceEntity.ProductBOMId,
                        BomMainMaterialId = coreEntryRequestBo.MainMaterialBo.MaterialId
                    }.ToSerialize(),
                    Remark = "",

                    ProcedureId = coreEntryRequestBo.IdsBo.ProcedureId,
                    ResourceId = coreEntryRequestBo.IdsBo.ResourceId,
                    EquipmentId = coreEntryRequestBo.IdsBo.EquipmentId,

                    SiteId = coreEntryRequestBo.IdsBo.SiteId,
                    CreatedBy = coreEntryRequestBo.IdsBo.UserName,
                    CreatedOn = coreEntryRequestBo.IdsBo.Time,
                    UpdatedBy = coreEntryRequestBo.IdsBo.UserName,
                    UpdatedOn = coreEntryRequestBo.IdsBo.Time
                });
            }

            // 主物料才扣除检索下级替代料，当还有剩余未扣除的数量时，扣除替代料（替代料不再递归扣除下级替代料库存）
            if (!coreEntryRequestBo.IsMainMaterial || residue <= 0) return;

            // 扣除替代料
            foreach (var replaceFeeding in currentMaterialBo.ReplaceMaterials)
            {
                // 递归扣除替代料库存
                BatchMaterialConsumptionCoreEntry(ref updates, ref adds, ref residue, new ConsumptionCoreEntryRequestBo
                {
                    SFCProduceEntity = sfcProduceEntity,
                    ManuFeedingsDict = coreEntryRequestBo.ManuFeedingsDict,
                    MainMaterialBo = coreEntryRequestBo.MainMaterialBo,
                    CurrentMaterialBo = new MaterialDeductResponseBo
                    {
                        MaterialId = replaceFeeding.MaterialId,
                        Usages = replaceFeeding.Usages,
                        Loss = replaceFeeding.Loss,
                        ConsumeRatio = replaceFeeding.ConsumeRatio,
                        DataCollectionWay = coreEntryRequestBo.MainMaterialBo.DataCollectionWay
                    },
                    IsMainMaterial = false
                });
            }
        }

        /// <summary>
        /// 转换数量
        /// </summary>
        /// <param name="originQty"></param>
        /// <param name="targetQty"></param>
        /// <param name="originValue"></param>
        /// <returns></returns>
        private static decimal ToTargetValue(decimal originQty, decimal targetQty, decimal originValue)
        {
            if (originQty == 0) return originValue;
            return targetQty * originValue / originQty;
        }

        /// <summary>
        /// 读取分选规则信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleEntity>> GetSortingRulesAsync(ProcSortingRuleQuery query)
        {
            return await _sortingRuleRepository.GetProcSortingRuleEntitiesAsync(query);
        }

        /// <summary>
        /// 获取条码参数列表
        /// </summary>
        /// <param name="parameterBySfcQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterBySfcsAsync(ManuProductParameterBySfcQuery parameterBySfcQuery)
        {
            //获取到条码的参数信息
            var parameterList = await _productParameterRepository.GetProductParameterBySFCEntitiesAsync(parameterBySfcQuery);
            return parameterList;
        }

    }
}
