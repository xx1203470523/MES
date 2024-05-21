using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
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
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

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
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

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

        private readonly ILocalizationService _localizationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="localizationService"></param>
        public ManuOutStationService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonOldService manuCommonOldService,
            IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuFeedingRepository manuFeedingRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonOldService = manuCommonOldService;
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuFeedingRepository = manuFeedingRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        [Obsolete("这个方法可能有点问题，已经跟出站作业逻辑不一致了", false)]
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
                ProcessRouteId = sfcProduceEntity?.ProcessRouteId,
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
            var initialMaterials = await _masterDataService.GetInitialMaterialsAsync(new MaterialDeductRequestBo
            {
                SiteId = sfcProduceEntity.SiteId,
                ProcedureId = sfcProduceEntity.ProcedureId,
                ProductBOMId = sfcProduceEntity.ProductBOMId
            });

            // 物料ID集合
            var materialIds = initialMaterials.Select(s => s.MaterialId);

            // 读取物料加载数据（批量）
            var allFeedingEntities = await _manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync(new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = sfcProduceEntity.ResourceId ?? 0,
                MaterialIds = materialIds
            });

            // 通过物料分组
            var manuFeedingsDictionary = allFeedingEntities.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 过滤扣料集合
            List<UpdateFeedingQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            foreach (var materialBo in initialMaterials)
            {
                // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
                decimal residue = materialBo.Usages;
                if (materialBo.Loss.HasValue && materialBo.Loss > 0) residue *= materialBo.Loss.Value;
                if (materialBo.ConsumeRatio > 0) residue *= (materialBo.ConsumeRatio / 100);

                // 收集方式是批次
                if (materialBo.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, sfcProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialBo.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 进行扣料
                _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, sfcProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
            }

            // manu_sfc_info 修改为完成 且入库
            // 条码信息
            var sfcInfo = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfcProduceEntity.SFC,
                Type = SfcTypeEnum.Produce
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17102)).WithData("SFC", sfcProduceEntity.SFC);

            // 读取产品基础信息
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(sfcProduceEntity.ProductId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17103));

            // 读取当前工艺路线信息
            var currentProcessRoute = await _procProcessRouteRepository.GetByIdAsync(sfcProduceEntity.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18104)).WithData("sfc", sfcProduceEntity.SFC);

            // 更新数据
            using var trans = TransactionHelper.GetTransactionScope();
            List<Task> tasks = new();

            // 更新物料库存
            if (updates.Any())
            {
                rows += await _manuFeedingRepository.UpdateFeedingQtyByIdAsync(updates);

                // 未更新到全部需更新的数据，事务回滚
                if (updates.Count > rows)
                {
                    trans.Dispose();
                    return 0;
                }
            }

            // 添加流转记录
            if (adds.Any())
            {
                var manuSfcCirculationInsertRangeTask = _manuSfcCirculationRepository.InsertRangeAsync(adds);
                tasks.Add(manuSfcCirculationInsertRangeTask);
            }

            // 完工
            if (nextProcedure == null)
            {
                // 插入 manu_sfc_step 状态为 完成
                sfcStep.Operatetype = currentProcessRoute.Type == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                sfcStep.CurrentStatus = SfcStatusEnum.Complete;  // TODO 这里的状态？？

                var manuSfcStepInsertTask = _manuSfcStepRepository.InsertAsync(sfcStep);
                tasks.Add(manuSfcStepInsertTask);

                // 只有"生产主工艺路线"，出站时才走下面流程
                if (currentProcessRoute.Type == ProcessRouteTypeEnum.ProductionRoute)
                {
                    // 删除 manu_sfc_produce
                    var manuSfcProduceDeletePhysicalTask = _manuSfcProduceRepository.DeletePhysicalAsync(new DeletePhysicalBySfcCommand()
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        Sfc = sfcProduceEntity.SFC
                    });
                    tasks.Add(manuSfcProduceDeletePhysicalTask);

                    // 删除 manu_sfc_produce_business
                    var manuSfcProduceDeleteSfcProduceBusinessBySfcInfoIdTask = _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdAsync(new DeleteSfcProduceBusinesssBySfcInfoIdCommand
                    {
                        SiteId = sfcProduceEntity.SiteId,
                        SfcInfoId = sfcInfo.Id
                    });
                    tasks.Add(manuSfcProduceDeleteSfcProduceBusinessBySfcInfoIdTask);

                    // 更新完工数量
                    var planWorkOrderUpdateFinishProductQuantityByWorkOrderIdTask = _planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderIdAsync(new UpdateQtyByWorkOrderIdCommand
                    {
                        UpdatedBy = sfcProduceEntity.UpdatedBy,
                        UpdatedOn = sfcProduceEntity.UpdatedOn,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        Qty = 1,
                    });
                    tasks.Add(planWorkOrderUpdateFinishProductQuantityByWorkOrderIdTask);

                    // 更新状态
                    sfcInfo.Status = SfcStatusEnum.Complete;
                    sfcInfo.UpdatedBy = sfcProduceEntity.UpdatedBy;
                    sfcInfo.UpdatedOn = sfcProduceEntity.UpdatedOn;
                    var manuSfcUpdateTask = _manuSfcRepository.UpdateAsync(sfcInfo);
                    tasks.Add(manuSfcUpdateTask);

                    //// 2023.05.29 克明说不在这里更新完成时间
                    //// 更新工单统计表的 RealEnd

                    // 入库
                    SaveToWarehouseAsync(ref tasks, sfcProduceEntity, procMaterialEntity);
                }
            }
            // 未完工
            else
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                sfcProduceEntity.Status = SfcStatusEnum.lineUp;
                sfcProduceEntity.ProcedureId = nextProcedure.Id;
                // 不置空的话，可能进站时，可能校验不通过
                sfcProduceEntity.ResourceId = null;

                var manuSfcProduceUpdateTask = _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);
                tasks.Add(manuSfcProduceUpdateTask);


                // 插入 manu_sfc_step 状态为 进站
                sfcStep.Operatetype = ManuSfcStepTypeEnum.OutStock;

                var manuSfcStepInsertTask = _manuSfcStepRepository.InsertAsync(sfcStep);
                tasks.Add(manuSfcStepInsertTask);
            }

            await Task.WhenAll(tasks);
            trans.Complete();

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
            sfcProduceEntity.VerifySFCStatus(SfcStatusEnum.Activity, _localizationService)
                            .VerifyProcedure(bo.ProcedureId);

            // 出站
            return await OutStationAsync(sfcProduceEntity);
        }


        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="manuSfcProduceEntity"></param>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        private void SaveToWarehouseAsync(ref List<Task> tasks, ManuSfcProduceEntity manuSfcProduceEntity, ProcMaterialEntity procMaterialEntity)
        {
            // 新增 wh_material_inventory
            tasks.Add(_whMaterialInventoryRepository.InsertAsync(new WhMaterialInventoryEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SupplierId = 0,//自制品 没有
                MaterialId = manuSfcProduceEntity.ProductId,
                MaterialBarCode = manuSfcProduceEntity.SFC,
                //Batch = "",//自制品 没有
                MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                QuantityResidue = procMaterialEntity.Batch ?? 0,
                Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                Source = MaterialInventorySourceEnum.ManuComplete,
                SiteId = manuSfcProduceEntity.SiteId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = manuSfcProduceEntity.UpdatedBy,
                UpdatedOn = manuSfcProduceEntity.UpdatedOn
            }));

            // 新增 wh_material_standingbook
            tasks.Add(_whMaterialStandingbookRepository.InsertAsync(new WhMaterialStandingbookEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                MaterialCode = procMaterialEntity.MaterialCode,
                MaterialName = procMaterialEntity.MaterialName,
                MaterialVersion = procMaterialEntity.Version ?? "",
                MaterialBarCode = manuSfcProduceEntity.SFC,
                //Batch = "",//自制品 没有
                Quantity = procMaterialEntity.Batch ?? 0,
                Unit = procMaterialEntity.Unit ?? "",
                Type = WhMaterialInventoryTypeEnum.ManuComplete,
                Source = MaterialInventorySourceEnum.ManuComplete,
                SiteId = manuSfcProduceEntity.SiteId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = manuSfcProduceEntity.UpdatedBy,
                UpdatedOn = manuSfcProduceEntity.UpdatedOn
            }));
        }

    }
}
