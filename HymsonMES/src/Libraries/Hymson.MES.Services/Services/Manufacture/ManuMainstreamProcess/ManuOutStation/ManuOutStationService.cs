﻿using Dapper;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation
{
    /// <summary>
    /// 出站
    /// </summary>
    public class ManuOutStationService : IManuOutStationService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（上料信息）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

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
        /// 仓储接口（工艺路线）
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;

        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public ManuOutStationService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonOldService manuCommonOldService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuFeedingRepository manuFeedingRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
             IProcProcessRouteRepository procProcessRouteRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonOldService = manuCommonOldService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuFeedingRepository = manuFeedingRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
        }


        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<int> OutStationAsync(ManuSfcProduceEntity sfcProduceEntity)
        {
            var rows = 0;

            // 获取生产工单
            _ = await _manuCommonOldService.GetProduceWorkOrderByIdAsync(sfcProduceEntity.WorkOrderId);

            // 更新时间
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();

            // 初始化步骤
            var sfcStep = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = sfcProduceEntity.SiteId,
                SFC = sfcProduceEntity.SFC,
                ProductId = sfcProduceEntity.ProductId,
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                WorkCenterId = sfcProduceEntity.WorkCenterId,
                ProductBOMId = sfcProduceEntity.ProductBOMId,
                ProcedureId = sfcProduceEntity.ProcedureId,
                Qty = sfcProduceEntity.Qty,
                EquipmentId = sfcProduceEntity.EquipmentId,
                ResourceId = sfcProduceEntity.ResourceId,
                CreatedBy = sfcProduceEntity.UpdatedBy,
                CreatedOn = sfcProduceEntity.UpdatedOn.Value,
                UpdatedBy = sfcProduceEntity.UpdatedBy,
                UpdatedOn = sfcProduceEntity.UpdatedOn.Value,
            };

            // 合格品出站
            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await _manuCommonOldService.GetNextProcedureAsync(sfcProduceEntity);

            // 扣料
            //await func(sfcProduceEntity.ProductBOMId, sfcProduceEntity.ProcedureId);
            var initialMaterials = await GetInitialMaterialsAsync(sfcProduceEntity);

            // 物料ID集合
            var materialIds = initialMaterials.Select(s => s.MaterialId);

            // 读取物料加载数据（批量）
            var allFeedingEntities = await _manuFeedingRepository.GetByResourceIdAndMaterialIdsAsync(new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = sfcProduceEntity.ResourceId ?? 0,
                MaterialIds = materialIds
            });

            // 通过物料分组
            var manuFeedingsDictionary = allFeedingEntities.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 过滤扣料集合
            List<UpdateQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            foreach (var materialBo in initialMaterials)
            {
                // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
                decimal residue = materialBo.Usages;
                if (materialBo.Loss.HasValue == true && materialBo.Loss > 0) residue *= (materialBo.Loss.Value / 100);
                if (materialBo.ConsumeRatio > 0) residue *= (materialBo.ConsumeRatio / 100);

                // 收集方式是批次
                if (materialBo.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    DeductMaterialQty(ref updates, ref adds, ref residue, sfcProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialBo.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 进行扣料
                DeductMaterialQty(ref updates, ref adds, ref residue, sfcProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
            }

            // manu_sfc_info 修改为完成 且入库
            // 条码信息
            var sfcInfo = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery
            {
                SiteId = _currentSite.SiteId,
                SFC = sfcProduceEntity.SFC
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17102)).WithData("SFC", sfcProduceEntity.SFC);

            // 读取产品基础信息
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(sfcProduceEntity.ProductId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17103));

            // 读取当前工艺路线信息
            var currentProcessRoute = await _procProcessRouteRepository.GetByIdAsync(sfcProduceEntity.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18104)).WithData("sfc", sfcProduceEntity.SFC);

            // 更新数据
            using var trans = TransactionHelper.GetTransactionScope();

            // 更新物料库存
            if (updates.Any() == true) rows += await _manuFeedingRepository.UpdateQtyByIdAsync(updates);

            // 添加流转记录
            if (adds.Any() == true) rows += await _manuSfcCirculationRepository.InsertRangeAsync(adds);

            // 完工
            if (nextProcedure == null)
            {
                // 插入 manu_sfc_step 状态为 完成
                sfcStep.Operatetype = currentProcessRoute.Type == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                sfcStep.CurrentStatus = SfcProduceStatusEnum.Complete;  // TODO 这里的状态？？
                rows += await _manuSfcStepRepository.InsertAsync(sfcStep);

                // 只有"生产主工艺路线"，出站时才走下面流程
                if (currentProcessRoute.Type == ProcessRouteTypeEnum.ProductionRoute)
                {
                    // 删除 manu_sfc_produce
                    rows += await _manuSfcProduceRepository.DeletePhysicalAsync(new DeletePhysicalBySfcCommand()
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        Sfc = sfcProduceEntity.SFC
                    });

                    // 删除 manu_sfc_produce_business
                    rows += await _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdAsync(new DeleteSfcProduceBusinesssBySfcInfoIdCommand
                    {
                        SiteId = sfcProduceEntity.SiteId,
                        SfcInfoId = sfcInfo.Id
                    });

                    // 更新完工数量
                    rows += await _planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderId(new UpdateQtyCommand
                    {
                        UpdatedBy = sfcProduceEntity.UpdatedBy,
                        UpdatedOn = sfcProduceEntity.UpdatedOn,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        Qty = 1,
                    });

                    // 更新状态
                    sfcInfo.Status = SfcStatusEnum.Complete;
                    sfcInfo.UpdatedBy = sfcProduceEntity.UpdatedBy;
                    sfcInfo.UpdatedOn = sfcProduceEntity.UpdatedOn;
                    rows += await _manuSfcRepository.UpdateAsync(sfcInfo);

                    /*
                     * 2023.05.29 克明说不在这里更新完成时间
                    // 更新工单统计表的 RealEnd
                    rows += await _planWorkOrderRepository.UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
                    {
                        UpdatedOn = sfcProduceEntity.UpdatedOn,
                        UpdatedBy = sfcProduceEntity.UpdatedBy,
                        WorkOrderIds = new long[] { sfcProduceEntity.WorkOrderId }
                    });
                    */

                    // 入库
                    rows += await SaveToWarehouseAsync(sfcProduceEntity, procMaterialEntity);
                }
            }
            // 未完工
            else
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                sfcProduceEntity.ProcedureId = nextProcedure.Id;
                rows += await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);

                // 插入 manu_sfc_step 状态为 进站
                sfcStep.Operatetype = ManuSfcStepTypeEnum.OutStock;
                rows += await _manuSfcStepRepository.InsertAsync(sfcStep);
            }

            trans.Complete();

            return rows;
        }

        /// <summary>
        /// 出站（批量）
        /// </summary>
        /// <param name="bos"></param>
        /// <returns></returns>
        public async Task<int> OutStationAsync(IEnumerable<ManufactureBo> bos)
        {
            var rows = 0;

            // TODO
            await Task.CompletedTask;
            return rows;
        }

        /// <summary>
        /// 出站(在制维修)
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> OutStationRepiarAsync(ManufactureRepairBo bo)
        {
            // 获取生产条码信息
            var (sfcProduceEntity, _) = await _manuCommonOldService.GetProduceSFCAsync(bo.SFC);

            // 合法性校验
            sfcProduceEntity.VerifySFCStatus(SfcProduceStatusEnum.Activity)
                            .VerifyProcedure(bo.ProcedureId);

            // 出站
            return await OutStationAsync(sfcProduceEntity);
        }


        /// <summary>
        /// 获取即将扣料的物料数据
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        private async Task<IEnumerable<MaterialDeductBo>> GetInitialMaterialsAsync(ManuSfcProduceEntity sfcProduceEntity)
        {
            // 获取BOM绑定的物料
            var mainMaterials = await _procBomDetailRepository.GetByBomIdAsync(sfcProduceEntity.ProductBOMId);

            // 未设置物料
            if (mainMaterials == null || mainMaterials.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10612));

            // 取得特定工序的物料
            mainMaterials = mainMaterials.Where(w => w.ProcedureId == sfcProduceEntity.ProcedureId);

            // 查询BOM替代料
            var replaceMaterialsForBOM = await _procBomDetailReplaceMaterialRepository.GetByBomIdAsync(sfcProduceEntity.ProductBOMId);
            var replaceMaterialsForBOMDic = replaceMaterialsForBOM.ToLookup(w => w.BomDetailId).ToDictionary(d => d.Key, d => d);

            // 查询物料基础数据的替代料
            var replaceMaterialsForMain = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(new ProcReplaceMaterialsQuery
            {
                SiteId = sfcProduceEntity.SiteId,
                MaterialIds = mainMaterials.Select(s => s.MaterialId)
            });
            var replaceMaterialsForMainDic = replaceMaterialsForMain.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);

            // 组合主物料ID和替代料ID
            var materialIds = mainMaterials.Select(s => s.MaterialId).AsList();
            materialIds.AddRange(replaceMaterialsForBOM.Select(s => s.ReplaceMaterialId));

            // 查询所有主物料和替代料的基础信息（为了读取消耗系数和收集方式）
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialIds);

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
        /// 入库
        /// </summary>
        /// <param name="manuSfcProduceEntity"></param>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        private async Task<int> SaveToWarehouseAsync(ManuSfcProduceEntity manuSfcProduceEntity, ProcMaterialEntity procMaterialEntity)
        {
            var rows = 0;

            // 新增 wh_material_inventory
            rows += await _whMaterialInventoryRepository.InsertAsync(new WhMaterialInventoryEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SupplierId = 0,//自制品 没有
                MaterialId = manuSfcProduceEntity.ProductId,
                MaterialBarCode = manuSfcProduceEntity.SFC,
                Batch = "",//自制品 没有
                QuantityResidue = procMaterialEntity.Batch,
                Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                Source = MaterialInventorySourceEnum.ManuComplete,
                SiteId = manuSfcProduceEntity.SiteId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = manuSfcProduceEntity.UpdatedBy,
                UpdatedOn = manuSfcProduceEntity.UpdatedOn
            });

            // 新增 wh_material_standingbook
            rows += await _whMaterialStandingbookRepository.InsertAsync(new WhMaterialStandingbookEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                MaterialCode = procMaterialEntity.MaterialCode,
                MaterialName = procMaterialEntity.MaterialName,
                MaterialVersion = procMaterialEntity.Version ?? "",
                MaterialBarCode = manuSfcProduceEntity.SFC,
                Batch = "",//自制品 没有
                Quantity = procMaterialEntity.Batch,
                Unit = procMaterialEntity.Unit ?? "",
                Type = WhMaterialInventoryTypeEnum.ManuComplete,
                Source = MaterialInventorySourceEnum.ManuComplete,
                SiteId = manuSfcProduceEntity.SiteId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = manuSfcProduceEntity.UpdatedBy,
                UpdatedOn = manuSfcProduceEntity.UpdatedOn
            });

            return rows;
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
        private static void DeductMaterialQty(ref List<UpdateQtyByIdCommand> updates,
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
            if (currentBo.Loss.HasValue == true && currentBo.Loss > 0) originQty *= (currentBo.Loss.Value / 100);
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
                        if (replaceBo.Loss.HasValue == true && replaceBo.Loss > 0) targetQty *= (replaceBo.Loss.Value / 100);
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
                    Qty = feeding.Qty,
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
