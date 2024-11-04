using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
using Hymson.Sequences;
using Hymson.Snowflake;
using System.Data;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon
{
    /// <summary>
    /// 生产通用
    /// </summary>
    public class ManuCommonOldService : IManuCommonOldService
    {
        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 序列号服务
        /// </summary>
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码流转信息）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（容器包装）
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;

        /// <summary>
        /// 仓储接口（工单信息）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（工单激活信息）
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（BOM替代料明细）
        /// </summary>
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;

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
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="sequenceService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="procMaskCodeRuleRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="localizationService"></param>
        public ManuCommonOldService(ICurrentSite currentSite, ISequenceService sequenceService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuContainerPackRepository manuContainerPackRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            IProcMaskCodeRuleRepository procMaskCodeRuleRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService)
        {
            _currentSite = currentSite;
            _sequenceService = sequenceService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _procMaskCodeRuleRepository = procMaskCodeRuleRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 验证条码掩码规则
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<bool> CheckBarCodeByMaskCodeRuleAsync(string barCode, long materialId)
        {
            var material = await _procMaterialRepository.GetByIdAsync(materialId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10204));

            // 物料未设置掩码
            if (material.MaskCodeId.HasValue == false) throw new CustomerValidationException(nameof(ErrorCode.MES16616)).WithData("barCode", barCode);

            // 未设置规则
            var maskCodeRules = await _procMaskCodeRuleRepository.GetByMaskCodeIdAsync(material.MaskCodeId.Value);
            if (maskCodeRules == null || maskCodeRules.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES16616)).WithData("barCode", barCode);

            return barCode.VerifyBarCode(maskCodeRules);
        }

        /// <summary>
        /// 检查条码是否可以执行某流程
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="sfcCirculationType"></param>
        /// <returns></returns>
        public async Task<bool> CheckSFCIsCanDoneStepAsync(ManufactureBo bo, SfcCirculationTypeEnum sfcCirculationType)
        {
            if (bo == null) return false;

            // 读取指定类型的流转信息
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new Data.Repositories.Manufacture.ManuSfcCirculation.Query.ManuSfcCirculationQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
                Sfc = bo.SFC,
                CirculationTypes = new SfcCirculationTypeEnum[] { sfcCirculationType }
            });

            if (manuSfcCirculationEntities == null || manuSfcCirculationEntities.Any() == false) return true;
            return false;
        }

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<(ManuSfcProduceEntity, ManuSfcProduceBusinessEntity)> GetProduceSFCAsync(string sfc)
        {
            if (string.IsNullOrWhiteSpace(sfc) == true
                || sfc.Contains(' ') == true) throw new CustomerValidationException(nameof(ErrorCode.MES16305));

            // 条码在制表
            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
                Sfc = sfc
            });

            // 不存在在制表的话，就去库存查找
            if (sfcProduceEntity == null)
            {
                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                {
                    SiteId = _currentSite.SiteId,
                    BarCode = sfc
                });
                if (whMaterialInventoryEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES16318));

                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }

            // 获取锁状态
            var sfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
                Sfc = sfcProduceEntity.SFC,
                BusinessType = ManuSfcProduceBusinessType.Lock
            });

            return (sfcProduceEntity, sfcProduceBusinessEntity);
        }

        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="isVerifyActivation"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(long workOrderId, bool isVerifyActivation = true)
        {
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            // 判断是否被锁定
            if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Pending)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16302)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            if (isVerifyActivation == true)
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
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetWorkOrderByIdAsync(long workOrderId)
        {
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            // 判断是否被锁定
            if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Pending)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16302)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            switch (planWorkOrderEntity.Status)
            {
                case PlanWorkOrderStatusEnum.SendDown:
                case PlanWorkOrderStatusEnum.InProduction:
                    break;
                case PlanWorkOrderStatusEnum.NotStarted:
                case PlanWorkOrderStatusEnum.Closed:
                case PlanWorkOrderStatusEnum.Finish:
                default:
                    throw new CustomerValidationException(nameof(ErrorCode.MES16303)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            return planWorkOrderEntity;
        }

        /// <summary>
        /// 通过BomId获取物料集合（包含替代料）
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BomMaterialBo>> GetProcMaterialEntitiesByBomIdAndProcedureIdAsync(long bomId, long procedureId)
        {
            // TODO 还未完成，请勿使用

            // 获取BOM绑定的物料
            var mainMaterials = await _procBomDetailRepository.GetByBomIdAsync(bomId);

            // 未设置物料
            if (mainMaterials == null || mainMaterials.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10612));

            // 取得特定工序的物料
            var materialEntities = mainMaterials.Where(w => w.ProcedureId == procedureId).Select(s => new BomMaterialBo
            {
                MaterialId = s.MaterialId,
                ProcedureId = s.ProcedureId
            });

            // 检查是否有BOM替代料
            var replaceMaterialsForBOM = await _procBomDetailReplaceMaterialRepository.GetByBomIdAsync(bomId);
            var replaceMaterialsDic = replaceMaterialsForBOM.ToLookup(w => w.BomDetailId).ToDictionary(d => d.Key, d => d);

            // 获取初始扣料数据
            var initialMaterials = new List<MaterialDeductBo> { };
            foreach (var item in mainMaterials)
            {
                var deduct = new MaterialDeductBo
                {
                    MaterialId = item.MaterialId,
                    Usages = item.Usages,
                    Loss = item.Loss,
                    DataCollectionWay = item.DataCollectionWay
                };

                // 填充BOM替代料
                if (item.IsEnableReplace == false)
                {
                    if (replaceMaterialsDic.TryGetValue(item.Id, out var replaces) == true)
                    {
                        deduct.ReplaceMaterials = replaces.Select(s => new MaterialDeductItemBo
                        {
                            MaterialId = s.ReplaceMaterialId,
                            Usages = s.Usages,
                            Loss = s.Loss,
                        });
                    }
                }
                // 填充物料替代料
                else
                {
                    var replaceMaterialsForMain = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(new ProcReplaceMaterialQuery
                    {
                        SiteId = item.SiteId,
                        MaterialId = item.MaterialId,
                    });

                    // 启用的替代物料
                    deduct.ReplaceMaterials = replaceMaterialsForMain.Where(w => w.IsEnabled == true).Select(s => new MaterialDeductItemBo
                    {
                        MaterialId = s.MaterialId,
                        Usages = item.Usages,
                        Loss = item.Loss
                    });
                }

                // 添加到初始扣料集合
                initialMaterials.Add(deduct);
            }

            return materialEntities;
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
                ProcedureId = procProcessRouteDetailNodeEntity.ProcedureId,
                CheckType = procProcessRouteDetailNodeEntity.CheckType,
                CheckRate = procProcessRouteDetailNodeEntity.CheckRate,
                IsWorkReport = procProcessRouteDetailNodeEntity.IsWorkReport,
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
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="manuSfcProduce"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity?> GetNextProcedureAsync(ManuSfcProduceEntity manuSfcProduce)
        {
            return await GetNextProcedureAsync(manuSfcProduce.ProcessRouteId, manuSfcProduce.ProcedureId, manuSfcProduce.WorkOrderId);
        }

        /// <summary>
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity?> GetNextProcedureAsync(long processRouteId, long procedureId, long workOrderId = 0)
        {
            // 因为可能有分叉，所以返回的下一步工序是集合
            var processRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(processRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

            var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(processRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

            // 数据过滤
            processRouteDetailLinks = processRouteDetailLinks.Where(w => w.PreProcessRouteDetailId == procedureId);
            processRouteDetailNodes = processRouteDetailNodes.Where(w => processRouteDetailLinks.Select(s => s.ProcessRouteDetailId).Contains(w.ProcedureId));

            // 随机工序Key
            //var cacheKey = $"{manuSfcProduce.ProcessRouteId}-{manuSfcProduce.ProcedureId}-{manuSfcProduce.ResourceId}-{manuSfcProduce.WorkOrderId}";
            var cacheKey = $"{workOrderId}-{processRouteId}-{procedureId}";
            var count = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.None, cacheKey, maxLength: 9);

            // 默认下一工序
            ProcProcessRouteDetailNodeEntity? defaultNextProcedure = null;

            // 有多工序分叉的情况
            if (processRouteDetailNodes.Count() > 1)
            {
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

            // 获取下一工序
            if (defaultNextProcedure == null) throw new CustomerValidationException(nameof(ErrorCode.MES10440));
            return await _procProcedureRepository.GetByIdAsync(defaultNextProcedure.ProcedureId);
        }

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<bool> IsRandomPreProcedureAsync(long processRouteId, long procedureId)
        {
            // 因为可能有分叉，所以返回的上一步工序是集合
            var preProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetPreProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = processRouteId,
                ProcedureId = procedureId
            });
            if (preProcessRouteDetailLinks == null || preProcessRouteDetailLinks.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10442));

            // 获取当前工序在工艺路线里面的扩展信息
            var procedureNodes = await _procProcessRouteDetailNodeRepository
                .GetByProcedureIdsAsync(new ProcProcessRouteDetailNodesQuery
                {
                    ProcessRouteId = processRouteId,
                    ProcedureIds = preProcessRouteDetailLinks.Where(w => w.PreProcessRouteDetailId.HasValue).Select(s => s.PreProcessRouteDetailId.Value)
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
            return await IsRandomPreProcedureAsync(processRouteId, defaultPreProcedure.Id);
        }

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="processRouteDetailLinks"></param>
        /// <param name="processRouteDetailNodes"></param>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<bool> IsRandomPreProcedureAsync(IEnumerable<ProcProcessRouteDetailLinkEntity> processRouteDetailLinks, IEnumerable<ProcProcessRouteDetailNodeEntity> processRouteDetailNodes,
            long processRouteId, long procedureId)
        {
            processRouteDetailLinks = processRouteDetailLinks.Where(w => w.ProcessRouteDetailId == procedureId);
            if (processRouteDetailLinks.Any() == false) return false; //throw new CustomerValidationException(nameof(ErrorCode.MES18213));

            processRouteDetailNodes = processRouteDetailNodes.Where(w => processRouteDetailLinks.Select(s => s.PreProcessRouteDetailId).Contains(w.ProcedureId));
            if (processRouteDetailNodes.Any() == false) return false; //throw new CustomerValidationException(nameof(ErrorCode.MES18208));

            // 有多工序分叉的情况（取第一个当默认值）
            ProcProcessRouteDetailNodeEntity? defaultPreProcedure = processRouteDetailNodes.FirstOrDefault();
            if (processRouteDetailLinks.Count() > 1)
            {
                // 下工序找上工序，执照正常流程的工序
                defaultPreProcedure = processRouteDetailNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                   ?? throw new CustomerValidationException(nameof(ErrorCode.MES10441));
            }

            // 获取上一工序
            if (defaultPreProcedure == null) throw new CustomerValidationException(nameof(ErrorCode.MES10442));
            if (defaultPreProcedure.CheckType == ProcessRouteInspectTypeEnum.RandomInspection) return true;

            // 继续检查上一工序
            return await IsRandomPreProcedureAsync(processRouteDetailLinks, processRouteDetailNodes, processRouteId, defaultPreProcedure.Id);
        }

        /// <summary>
        /// 判断是否首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<bool> IsFirstProcedureAsync(long processRouteId, long procedureId)
        {
            var firstProcedureDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(processRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10435));

            return firstProcedureDetailNodeEntity.ProcedureId == procedureId;
        }

        /// <summary>
        /// 验证开始工序是否在结束工序之前
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="startProcedureId"></param>
        /// <param name="endProcedureId"></param>
        /// <returns></returns>
        public async Task<bool> IsProcessStartBeforeEndAsync(long processRouteId, long startProcedureId, long endProcedureId)
        {
            var processRouteDetailList = await GetProcessRouteAsync(processRouteId);
            var processRouteDetails = processRouteDetailList.Where(x => x.ProcedureIds.Contains(startProcedureId) && x.ProcedureIds.Contains(endProcedureId));
            if (processRouteDetails != null && processRouteDetails.Any())
            {
                foreach (var processRouteDetail in processRouteDetails)
                {
                    var startIndex = processRouteDetail.ProcedureIds.ToList().IndexOf(startProcedureId);
                    var endIndex = processRouteDetail.ProcedureIds.ToList().IndexOf(startProcedureId);
                    if (startIndex < endIndex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取工序关联的资源
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<long>> GetProcResourceIdByProcedureIdAsync(long procedureId)
        {
            var resources = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(procedureId);

            if (resources == null || resources.Any() == false) return Array.Empty<long>();
            return resources.Select(s => s.Id);
        }

        /// <summary>
        /// 获取工艺路线
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcessRouteDetailDto>> GetProcessRouteAsync(long processRouteId)
        {
            var processRouteDetailLinkListTask = _procProcessRouteDetailLinkRepository.GetListAsync(new ProcProcessRouteDetailLinkQuery { ProcessRouteId = processRouteId });
            var processRouteDetailNodeListTask = _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(processRouteId);
            var processRouteDetailLinkList = await processRouteDetailLinkListTask;
            var processRouteDetailNodeList = await processRouteDetailNodeListTask;

            IList<ProcessRouteDetailDto> list = new List<ProcessRouteDetailDto>();
            if (processRouteDetailLinkList != null && processRouteDetailLinkList.Any()
                && processRouteDetailNodeList != null && processRouteDetailNodeList.Any())
            {
                var firstProcedure = processRouteDetailNodeList.FirstOrDefault(x => x.IsFirstProcess == 1);
                if (firstProcedure != null)
                {
                    CombinationProcessRoute(ref list, firstProcedure.ProcedureId, processRouteDetailLinkList);
                }
            }
            return list;
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
                    foreach (var item in procProcessRouteDetailLinkByprocedureIdList)
                    {
                        if (item.ProcessRouteDetailId != ProcessRoute.LastProcedureId)
                        {
                            if (index == 1)
                            {
                                processRouteDetail.ProcedureIds.Add(item.ProcessRouteDetailId);
                                CombinationProcessRoute(ref list, item.ProcessRouteDetailId, procProcessRouteDetailLinkEntities, key);
                            }
                            else
                            {
                                var processRouteDetailDto = new ProcessRouteDetailDto()
                                {
                                    key = IdGenProvider.Instance.CreateId(),
                                    ProcedureIds = procedureIds,
                                };
                                processRouteDetailDto.ProcedureIds.Add(item.ProcessRouteDetailId);
                                list.Add(processRouteDetailDto);
                                CombinationProcessRoute(ref list, item.ProcessRouteDetailId, procProcessRouteDetailLinkEntities, processRouteDetailDto.key);
                            }
                        }
                        index++;
                    }
                }
            }
        }

    }
}
