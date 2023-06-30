using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Sequences;
using System.Data;
using System.Text.Json;

namespace Hymson.MES.CoreServices.Services.Common.ManuCommon
{
    /// <summary>
    /// 生产公共类
    /// </summary>
    public class ManuCommonService : IManuCommonService
    {
        /// <summary>
        /// 序列号服务
        /// </summary>
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

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
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sequenceService"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        public ManuCommonService(ISequenceService sequenceService, ILocalizationService localizationService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuContainerPackRepository manuContainerPackRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IProcResourceRepository procResourceRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository)
        {
            _sequenceService = sequenceService;
            _localizationService = localizationService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _procResourceRepository = procResourceRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
        }


        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBo"></param>
        /// <returns></returns>
        public async Task<(ManuSfcProduceEntity, ManuSfcProduceBusinessEntity)> GetProduceSFCAsync(SingleSFCBo sfcBo)
        {
            if (string.IsNullOrWhiteSpace(sfcBo.SFC)
                || sfcBo.SFC.Contains(' ')) throw new CustomerValidationException(nameof(ErrorCode.MES16305));

            // 条码在制表
            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery
            {
                SiteId = sfcBo.SiteId,
                Sfc = sfcBo.SFC
            });

            // 不存在在制表的话，就去库存查找
            if (sfcProduceEntity == null)
            {
                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                {
                    SiteId = sfcBo.SiteId,
                    BarCode = sfcBo.SFC
                });
                if (whMaterialInventoryEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES16318));

                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }

            // 获取锁状态
            var sfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                SiteId = sfcBo.SiteId,
                Sfc = sfcProduceEntity.SFC,
                BusinessType = ManuSfcProduceBusinessType.Lock
            });

            return (sfcProduceEntity, sfcProduceBusinessEntity);
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

            // 不存在在制表的话，就去库存查找
            if (sfcProduceEntities.Any() == false)
            {
                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
                {
                    SiteId = sfcBos.SiteId,
                    BarCodes = sfcBos.SFCs
                });
                if (whMaterialInventoryEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES16318));

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
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="procedureBo"></param>
        /// <returns></returns>
        public async Task VerifySfcsLockAsync(ManuProcedureBo procedureBo)
        {
            var sfcProduceBusinesss = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery
            {
                SiteId = procedureBo.SiteId,
                Sfcs = procedureBo.SFCs,
                BusinessType = ManuSfcProduceBusinessType.Lock
            });

            if (sfcProduceBusinesss == null || !sfcProduceBusinesss.Any()) return;

            List<ValidationFailure> validationFailures = new();
            foreach (var item in sfcProduceBusinesss)
            {
                var sfcProduceLockBo = JsonSerializer.Deserialize<SfcProduceLockBo>(item.BusinessContent);
                if (sfcProduceLockBo == null) continue;

                // 即时锁
                if (sfcProduceLockBo.Lock != QualityLockEnum.InstantLock) continue;

                // 将来锁
                if (sfcProduceLockBo.Lock != QualityLockEnum.FutureLock) continue;

                // 如果锁的不是目标工序，就跳过
                if (procedureBo.ProcedureId.HasValue && sfcProduceLockBo.LockProductionId != procedureBo.ProcedureId) continue;

                validationFailures.Add(new ValidationFailure
                {
                    FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", item.Sfc } },
                    ErrorCode = nameof(ErrorCode.MES18010)
                });
            }

            // 是否存在错误
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
        }

        /// <summary>
        /// 批量验证条码是否被容器包装
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        public async Task VerifyContainerAsync(MultiSFCBo sfcBos)
        {
            var manuContainerPackEntities = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery
            {
                SiteId = sfcBos.SiteId,
                LadeBarCodes = sfcBos.SFCs.ToArray(),
            });

            if (manuContainerPackEntities != null && manuContainerPackEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18015)).WithData("SFCs", string.Join(",", sfcBos.SFCs));
                //throw new CustomerValidationException(nameof(ErrorCode.MES18019)).WithData("SFC", string.Join(",", sfcs));
            }
        }

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="procedureBomBo"></param>
        /// <returns></returns>
        public async Task VerifyBomQtyAsync(ManuProcedureBomBo procedureBomBo)
        {
            var procBomDetailEntities = await _procBomDetailRepository.GetByBomIdAsync(procedureBomBo.BomId);
            if (procBomDetailEntities == null) return;

            // 过滤出当前工序对应的物料（数据收集方式为内部和外部）
            procBomDetailEntities = procBomDetailEntities.Where(w => w.ProcedureId == procedureBomBo.ProcedureId && w.DataCollectionWay != MaterialSerialNumberEnum.Batch);
            if (procBomDetailEntities == null) return;

            // 流转信息（多条码）
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery
            {
                SiteId = procedureBomBo.SiteId,
                Sfc = procedureBomBo.SFCs,
                CirculationTypes = new SfcCirculationTypeEnum[] {
                    SfcCirculationTypeEnum.Consume ,
                    SfcCirculationTypeEnum.ModuleAdd,
                    SfcCirculationTypeEnum.ModuleReplace
                },
                ProcedureId = procedureBomBo.ProcedureId,
                IsDisassemble = TrueOrFalseEnum.No
            });
            if (sfcCirculationEntities == null || !sfcCirculationEntities.Any())
            {
                // TODO 这里存在需要组装却未组装的漏网之鱼
                return;
                //throw new CustomerValidationException(nameof(ErrorCode.MES16323));
            }

            // 根据物料分组
            var procBomDetailDictionary = procBomDetailEntities.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);
            foreach (var item in procBomDetailDictionary)
            {
                // 检查每个物料是否已经满足BOM用量要求（这里可以优化下）
                var currentQty = sfcCirculationEntities.Where(w => w.CirculationMainProductId == item.Key)
                    .Sum(s => s.CirculationQty);

                // 目标用量
                var targetQty = item.Value.Sum(s => s.Usages);

                if (currentQty < targetQty)
                {
                    var materialEntity = await _procMaterialRepository.GetByIdAsync(item.Key);
                    if (materialEntity == null) continue;

                    throw new CustomerValidationException(nameof(ErrorCode.MES16321)).WithData("Code", materialEntity.MaterialCode);
                }
            }
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
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<bool> IsRandomPreProcedureAsync(IsRandomPreProcedureBo randomPreProcedureBo)
        {
            // 因为可能有分叉，所以返回的上一步工序是集合
            var preProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetPreProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = randomPreProcedureBo.ProcessRouteId,
                ProcedureId = randomPreProcedureBo.ProcedureId
            });
            if (preProcessRouteDetailLinks == null || preProcessRouteDetailLinks.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10442));

            // 获取当前工序在工艺路线里面的扩展信息
            var procedureNodes = await _procProcessRouteDetailNodeRepository
                .GetByProcedureIdsAsync(new ProcProcessRouteDetailNodesQuery
                {
                    ProcessRouteId = randomPreProcedureBo.ProcessRouteId,
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
            return await IsRandomPreProcedureAsync(new IsRandomPreProcedureBo { ProcessRouteId = randomPreProcedureBo.ProcessRouteId, ProcedureId = defaultPreProcedure.Id });
        }



        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="isVerifyActivation"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(GetProduceWorkOrderByIdBo produceWorkOrderByIdBo)
        {
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(produceWorkOrderByIdBo.WorkOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            // 判断是否被锁定
            if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Pending)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16302)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            if (produceWorkOrderByIdBo.IsVerifyActivation)
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
        #region 内部方法

        #endregion
    }
}
