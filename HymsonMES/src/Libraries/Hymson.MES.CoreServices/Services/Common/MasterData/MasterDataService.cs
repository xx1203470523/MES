using Dapper;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuCommon.ManuCommon;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Sequences;
using Hymson.Snowflake;

namespace Hymson.MES.CoreServices.Services.Common.MasterData
{
    /// <summary>
    /// 主数据公用类
    /// @author wangkeming
    /// @date 2023-06-06
    /// </summary>
    public class MasterDataService : IMasterDataService
    {
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
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>sss
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
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sequenceService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        public MasterDataService(ISequenceService sequenceService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository)
        {
            _sequenceService = sequenceService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
        }


        /// <summary>
        /// 获取物料基础信息（带空检查）
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<ProcMaterialEntity> GetProcMaterialEntityWithNullCheck(long materialId)
        {
            // 读取产品基础信息
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(materialId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17103));

            return procMaterialEntity;
        }

        /// <summary>
        /// 获取工序基础信息（带空检查）
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity> GetProcProcedureEntityWithNullCheck(long procedureId)
        {
            // 获取工序信息
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10406));

            return procProcedureEntity;
        }

        /// <summary>
        /// 获取工艺路线基础信息（带空检查）
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteEntity> GetProcProcessRouteEntityWithNullCheck(long processRouteId)
        {
            // 读取当前工艺路线信息
            var processRouteEntity = await _procProcessRouteRepository.GetByIdAsync(processRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18107));

            return processRouteEntity;
        }

        /// <summary>
        /// 获取条码基础信息（带空检查）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetManuSFCEntitiesWithNullCheck(MultiSFCBo bo)
        {
            // 条码信息
            var manuSfcEntities = await _manuSfcRepository.GetManuSfcEntitiesAsync(new ManuSfcQuery
            {
                SiteId = bo.SiteId,
                SFCs = bo.SFCs
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17104));

            return manuSfcEntities;
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
            if (planWorkOrderEntity.IsLocked == YesOrNoEnum.Yes)
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
                //var whMaterialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
                //{
                //    SiteId = sfcBos.SiteId,
                //    BarCodes = sfcBos.SFCs
                //});
                //if (whMaterialInventoryEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES16318));

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
            if (mainMaterials == null || !mainMaterials.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10612));

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
                    if (replaceMaterialsDic.TryGetValue(item.Id, out var replaces))
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



        /// <summary>
        /// 获取即将扣料的物料数据
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MaterialDeductBo>> GetInitialMaterialsAsync(ManuSfcProduceEntity sfcProduceEntity)
        {
            // 获取BOM绑定的物料
            var mainMaterials = await _procBomDetailRepository.GetByBomIdAsync(sfcProduceEntity.ProductBOMId);

            // 未设置物料
            if (mainMaterials == null || mainMaterials.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10612));

            // 取得特定工序的物料
            mainMaterials = mainMaterials.Where(w => w.ProcedureId == sfcProduceEntity.ProcedureId);
            var materialIds = mainMaterials.Select(s => s.MaterialId).AsList();

            // 查询BOM替代料
            var replaceMaterialsForBOM = await _procBomDetailReplaceMaterialRepository.GetByBomIdAsync(sfcProduceEntity.ProductBOMId);
            var replaceMaterialsForBOMDic = replaceMaterialsForBOM.ToLookup(w => w.BomDetailId).ToDictionary(d => d.Key, d => d);

            // 查询物料基础数据的替代料
            var replaceMaterialsForMain = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(sfcProduceEntity.SiteId);
            replaceMaterialsForMain = replaceMaterialsForMain.Where(w => materialIds.Contains(w.MaterialId));
            var replaceMaterialsForMainDic = replaceMaterialsForMain.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);

            // 组合主物料ID和替代料ID
            materialIds.AddRange(replaceMaterialsForBOM.Select(s => s.ReplaceMaterialId));

            // 查询所有主物料和替代料的基础信息（为了读取消耗系数和收集方式）
            var materialEntities = await _procMaterialRepository.GetBySiteIdAsync(sfcProduceEntity.SiteId);
            materialEntities = materialEntities.Where(w => materialIds.Contains(w.Id));

            // 获取初始扣料数据
            List<MaterialDeductBo> initialMaterials = new();
            foreach (var item in mainMaterials)
            {
                var materialEntitiy = materialEntities.FirstOrDefault(f => f.Id == item.MaterialId);
                if (materialEntitiy == null) continue;

                var deduct = new MaterialDeductBo
                {
                    MaterialId = item.MaterialId,
                    Usages = item.Usages,
                    Loss = item.Loss,
                    DataCollectionWay = item.DataCollectionWay,
                    SerialNumber = materialEntitiy.SerialNumber
                };
                if (materialEntitiy.ConsumeRatio.HasValue) deduct.ConsumeRatio = materialEntitiy.ConsumeRatio.Value;

                // 填充BOM替代料
                if (item.IsEnableReplace == false)
                {
                    if (replaceMaterialsForBOMDic.TryGetValue(item.Id, out var replaces) == true)
                    {
                        // 启用的替代物料（BOM）
                        deduct.ReplaceMaterials = replaces.Select(s => new MaterialDeductItemBo
                        {
                            MaterialId = s.ReplaceMaterialId,
                            Usages = s.Usages,
                            Loss = s.Loss,
                            ConsumeRatio = GetConsumeRatio(materialEntities, s.ReplaceMaterialId)
                        });
                    }
                }
                // 填充物料替代料
                else
                {
                    if (replaceMaterialsForMainDic.TryGetValue(item.MaterialId, out var replaces) == true)
                    {
                        // 启用的替代物料（物料维护）
                        deduct.ReplaceMaterials = replaces.Where(w => w.IsEnabled == true).Select(s => new MaterialDeductItemBo
                        {
                            MaterialId = s.MaterialId,
                            Usages = item.Usages,
                            Loss = item.Loss,
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
        /// 取得消耗系数
        /// </summary>
        /// <param name="materialEntities"></param>
        /// <param name="replaceMaterialId"></param>
        /// <returns></returns>
        private static decimal GetConsumeRatio(IEnumerable<ProcMaterialEntity> materialEntities, long replaceMaterialId)
        {
            decimal defaultConsumeRatio = 100;

            if (materialEntities == null || materialEntities.Any() == false) return defaultConsumeRatio;

            var materialEntity = materialEntities.FirstOrDefault(f => f.Id == replaceMaterialId);
            if (materialEntity == null || materialEntity.ConsumeRatio.HasValue == false) return defaultConsumeRatio;

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
        /// <param name="currentBo">替代料BO对象</param>
        /// <param name="isMain">是否主物料</param>
        public void DeductMaterialQty(ref List<UpdateQtyByIdCommand> updates,
            ref List<ManuSfcCirculationEntity> adds,
            ref decimal residue,
            ManuSfcProduceEntity sfcProduceEntity,
            Dictionary<long, IGrouping<long, ManuFeedingEntity>> manuFeedingsDictionary,
            MaterialDeductBo mainMaterialBo,
            MaterialDeductBo currentBo,
            bool isMain = true)
        {
            // 没有剩余需要抵扣时，直接返回
            if (residue <= 0) return;

            // 取得当前物料的库存
            if (manuFeedingsDictionary.TryGetValue(currentBo.MaterialId, out var feedingEntities) == false) return;
            if (feedingEntities.Any() == false) return;

            // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
            decimal originQty = currentBo.Usages;
            if (currentBo.Loss.HasValue == true && currentBo.Loss > 0) originQty *= currentBo.Loss.Value;
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
                        if (replaceBo.Loss.HasValue == true && replaceBo.Loss > 0) targetQty *= replaceBo.Loss.Value;
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
                updates.Add(new UpdateQtyByIdCommand
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
            if (isMain == false || residue <= 0) return;

            // 扣除替代料
            foreach (var replaceFeeding in currentBo.ReplaceMaterials)
            {
                // 递归扣除替代料库存
                DeductMaterialQty(ref updates, ref adds, ref residue,
                    sfcProduceEntity, manuFeedingsDictionary, mainMaterialBo,
                    new MaterialDeductBo
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


    }
}
