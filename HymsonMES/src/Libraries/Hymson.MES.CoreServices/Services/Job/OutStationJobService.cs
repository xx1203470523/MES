using Dapper;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 出站
    /// </summary>
    [Job("出站", JobTypeEnum.Standard)]
    public class OutStationJobService : IJobService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（降级品继承）
        /// </summary>
        private readonly IManuDegradedProductExtendService _manuDegradedProductExtendService;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（上料信息）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;

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
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（不合格代码）
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuDegradedProductExtendService"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuSfcSummaryRepository"></param>
        public OutStationJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuDegradedProductExtendService manuDegradedProductExtendService,
            IProcProcedureRepository procProcedureRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuFeedingRepository manuFeedingRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            ILocalizationService localizationService,
            IManuSfcSummaryRepository manuSfcSummaryRepository)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuDegradedProductExtendService = manuDegradedProductExtendService;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuFeedingRepository = manuFeedingRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _localizationService = localizationService;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<OutStationRequestBo>();
            if (bo == null) return;

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return;

            // 判断条码锁状态
            await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.Activity, _localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.Activity)}"))
                              .VerifyProcedure(bo.ProcedureId)
                              .VerifyResource(bo.ResourceId);

            //（前提：这些条码都是同一工单同一工序）
            var firstProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return;

            // 获取生产工单（附带工单状态校验）
            _ = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceWorkOrderByIdAsync, new WorkOrderIdBo { WorkOrderId = firstProduceEntity.WorkOrderId });

            /*
            // 验证BOM主物料数量
            await _manuCommonService.VerifyBomQtyAsync(new ManuProcedureBomBo
            {
                SiteId = bo.SiteId,
                SFCs = bo.SFCs,
                ProcedureId = bo.ProcedureId,
                BomId = firstProduceEntity.ProductBOMId
            });
            */
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<OutStationRequestBo>();
            if (bo == null) return null;
            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.BeforeFinish
            });
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<OutStationRequestBo>();
            if (bo == null) return default;

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return default;

            // 如果是不合格出站
            if (bo.IsQualified.HasValue && bo.IsQualified.Value == false)
            {
                return await OutStationForUnQualifiedProcedureAsync(bo, sfcProduceEntities);
            }

            // 合格出站（为了逻辑清晰，跟上面的不合格出站区分开）
            return await OutStationForQualifiedProcedureAsync(bo, sfcProduceEntities);
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not OutStationResponseBo data) return responseBo;

            // 更新物料库存
            if (data.UpdateQtyByIdCommands.Any())
            {
                responseBo.Rows += await _manuFeedingRepository.UpdateQtyByIdAsync(data.UpdateQtyByIdCommands);

                // 未更新到全部需更新的数据，事务回滚
                if (data.UpdateQtyByIdCommands.Count() > responseBo.Rows)
                {
                    responseBo.Rows = -1;
                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18218), data.FirstSFC);
                    return responseBo;
                }
            }

            // 更新数据
            List<Task<int>> tasks = new();

            // 完工
            if (data.IsCompleted)
            {
                // 只有"生产主工艺路线"，出站时才走下面流程
                if (data.ProcessRouteType == ProcessRouteTypeEnum.ProductionRoute)
                {
                    // 删除 manu_sfc_produce
                    tasks.Add(_manuSfcProduceRepository.DeletePhysicalRangeByIdsSqlAsync(data.DeletePhysicalByProduceIdsCommand));

                    // 删除 manu_sfc_produce_business
                    tasks.Add(_manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdsAsync(data.DeleteSfcProduceBusinesssBySfcInfoIdsCommand));

                    // 更新完工数量
                    tasks.Add(_planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderIdAsync(data.UpdateQtyCommand));

                    // manu_sfc_info 修改为完成 且入库
                    tasks.Add(_manuSfcRepository.MultiUpdateSfcStatusAsync(data.MultiSFCUpdateStatusCommand));

                    // 入库
                    tasks.Add(_whMaterialInventoryRepository.InsertsAsync(data.WhMaterialInventoryEntities));
                    tasks.Add(_whMaterialStandingbookRepository.InsertsAsync(data.WhMaterialStandingbookEntities));

                    // 降级品记录
                    tasks.Add(_manuDegradedProductExtendService.CreateManuDowngradingsByConsumesAsync(data.DegradedProductExtendBo, data.DowngradingEntities));
                }
            }
            // 未完工
            else
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                var rows = await _manuSfcProduceRepository.MultiUpdateRangeWithStatusCheckAsync(data.MultiUpdateProduceSFCCommand);

                // 未更新到数据，事务回滚
                if (rows <= 0)
                {
                    // 这里在外层会回滚事务
                    responseBo.Rows = -1;

                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18216), data.FirstSFC);
                    return responseBo;
                }
            }

            // 汇总表
            if (data.MultiUpdateSummaryOutStationCommands.Any())
            {
                tasks.Add(_manuSfcSummaryRepository.UpdateSummaryOutStationRangeAsync(data.MultiUpdateSummaryOutStationCommands));
            }

            // 添加流转记录
            if (data.ManuSfcCirculationEntities.Any())
            {
                tasks.Add(_manuSfcCirculationRepository.InsertRangeAsync(data.ManuSfcCirculationEntities));
            }

            // 插入 manu_sfc_step
            if (data.SFCStepEntities.Any())
            {
                tasks.Add(_manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities));
            }

            // 插入不良记录
            if (data.ProductBadRecordEntities.Any())
            {
                tasks.Add(_manuProductBadRecordRepository.InsertRangeAsync(data.ProductBadRecordEntities));
            }

            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            // 面板需要的参数
            responseBo.Content = new Dictionary<string, string> {
                { "PackageCom", "False" },
                { "BadEntryCom", "False" },
                { "Qty", $"{data.SFCStepEntities.Count}" },
                { "IsLastProcedure", $"{data.IsCompleted}" },
                { "NextProcedureCode", $"{data.NextProcedureCode}" },
            };

            if (data.IsCompleted)
            {
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES16349), data.FirstSFC);
            }
            else
            {
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES16351), data.FirstSFC, data.NextProcedureCode);
            }

            return responseBo;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<OutStationRequestBo>();
            if (bo == null) return null;
            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.AfterFinish
            });
        }


        #region 内部方法（仅仅是为了逻辑清晰）
        /// <summary>
        /// 合格工序出站
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="sfcProduceEntities"></param>
        /// <returns></returns>
        private async Task<OutStationResponseBo?> OutStationForQualifiedProcedureAsync(OutStationRequestBo bo, IEnumerable<ManuSfcProduceEntity> sfcProduceEntities)
        {
            if (bo == null) return default;
            if (bo.Proxy == null) return default;

            // 待执行的命令
            OutStationResponseBo responseBo = new();

            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return default;
            var entities = sfcProduceEntities.AsList();

            // 这里已经不合适了，因为这批条码可能来源于不同的工单
            var firstSFCProduceEntity = entities.FirstOrDefault();
            if (firstSFCProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();
            responseBo.FirstSFC = firstSFCProduceEntity.SFC;

            // 读取产品基础信息
            var procMaterialEntityTask = _masterDataService.GetProcMaterialEntityWithNullCheckAsync(firstSFCProduceEntity.ProductId);

            // 读取当前工艺路线信息
            var procProcessRouteEntityTask = _masterDataService.GetProcProcessRouteEntityWithNullCheckAsync(firstSFCProduceEntity.ProcessRouteId);

            var procMaterialEntity = await procMaterialEntityTask;
            var procProcessRouteEntity = await procProcessRouteEntityTask;

            // 物料消耗明细数据
            MaterialConsumptionBo consumptionBo = new();
            // 指定消耗物料
            if (bo.ConsumeList != null && bo.ConsumeList.Any())
            {
                consumptionBo = await ExecutenMaterialConsumptionWithBarCodeAsync(bo, sfcProduceEntities);
            }
            // 默认BOM清单
            else
            {
                consumptionBo = await ExecutenMaterialConsumptionWithBOMAsync(bo, sfcProduceEntities);
            }
            responseBo.UpdateQtyByIdCommands = consumptionBo.UpdateQtyByIdCommands;
            responseBo.ManuSfcCirculationEntities = consumptionBo.ManuSfcCirculationEntities;

            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await bo.Proxy.GetValueAsync(_masterDataService.GetNextProcedureAsync, firstSFCProduceEntity);
            if (nextProcedure != null)
            {
                responseBo.IsCompleted = false;
                responseBo.NextProcedureCode = nextProcedure.Code;
            }

            // 查询工序信息
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(bo.ProcedureId);
            var cycle = procProcedureEntity.Cycle ?? 1;

            // 组装（出站步骤数据）
            List<MultiUpdateSummaryOutStationCommand> updateSummaryOutStationCommands = new();
            entities.ForEach(sfcProduceEntity =>
            {
                // 初始化步骤
                var stepEntity = new ManuSfcStepEntity
                {
                    // 插入 manu_sfc_step 状态为出站（默认值）
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = SfcStatusEnum.Activity,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    Qty = sfcProduceEntity.Qty,
                    EquipmentId = bo.EquipmentId,
                    VehicleCode = bo.VehicleCode,
                    SiteId = bo.SiteId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                };

                updateSummaryOutStationCommands.Add(new MultiUpdateSummaryOutStationCommand
                {
                    Id = sfcProduceEntity.SfcSummaryId ?? 0,
                    OutputQty = sfcProduceEntity.Qty,
                    EndOn = HymsonClock.Now(),
                    UpdatedBy = updatedBy,
                    UpdatedOn = HymsonClock.Now()
                });

                // 已完工
                if (responseBo.IsCompleted)
                {
                    stepEntity.Operatetype = procProcessRouteEntity.Type == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                    stepEntity.CurrentStatus = SfcStatusEnum.Complete;  // TODO 这里的状态？？
                }

                // 如果超过复投次数
                if (sfcProduceEntity.RepeatedCount > cycle)
                {
                    stepEntity.CurrentStatus = SfcStatusEnum.InProductionComplete;
                }

                responseBo.SFCStepEntities.Add(stepEntity);
            });

            responseBo.MultiUpdateSummaryOutStationCommands = updateSummaryOutStationCommands;

            // 已完工，且只有"生产主工艺路线"，出站时才走下面流程
            responseBo.ProcessRouteType = procProcessRouteEntity.Type;
            if (responseBo.IsCompleted && responseBo.ProcessRouteType == ProcessRouteTypeEnum.ProductionRoute)
            {
                // 删除 manu_sfc_produce_business
                responseBo.DeleteSfcProduceBusinesssBySfcInfoIdsCommand = new DeleteSfcProduceBusinesssBySfcInfoIdsCommand
                {
                    SiteId = bo.SiteId,
                    SfcInfoIds = entities.Select(s => s.Id) //manuSfcEntities.Select(s => s.Id)
                };

                // 更新完工数量
                responseBo.UpdateQtyCommand = new UpdateQtyCommand
                {
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    WorkOrderId = firstSFCProduceEntity.WorkOrderId,
                    Qty = entities.Count
                };

                // 删除 manu_sfc_produce
                responseBo.DeletePhysicalByProduceIdsCommand = new DeletePhysicalByProduceIdsCommand
                {
                    SiteId = bo.SiteId,
                    Ids = entities.Select(s => s.Id)
                };

                // manu_sfc_info 修改为完成 且入库
                responseBo.MultiSFCUpdateStatusCommand = new MultiSFCUpdateStatusCommand
                {
                    SiteId = bo.SiteId,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    Status = SfcStatusEnum.Complete,
                    SFCs = entities.Select(s => s.SFC) //manuSfcEntities
                };

                // 降级品信息
                var degradedProductExtendBo = new DegradedProductExtendBo
                {
                    SiteId = bo.SiteId,
                    UserName = bo.UserName
                };

                // 入库
                entities.ForEach(sfcProduceEntity =>
                {
                    // 添加降级品记录
                    degradedProductExtendBo.KeyValues.AddRange(responseBo.ManuSfcCirculationEntities.Select(s => new DegradedProductExtendKeyValueBo
                    {
                        BarCode = s.CirculationBarCode,
                        SFC = sfcProduceEntity.SFC
                    }));

                    // 新增 wh_material_inventory
                    responseBo.WhMaterialInventoryEntities.Add(new WhMaterialInventoryEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SupplierId = 0,//自制品 没有
                        MaterialId = sfcProduceEntity.ProductId,
                        MaterialBarCode = sfcProduceEntity.SFC,
                        Batch = "",//自制品 没有
                        MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                        QuantityResidue = procMaterialEntity.Batch,
                        Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                        Source = MaterialInventorySourceEnum.ManuComplete,
                        SiteId = bo.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });

                    // 新增 wh_material_standingbook
                    responseBo.WhMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialCode = procMaterialEntity.MaterialCode,
                        MaterialName = procMaterialEntity.MaterialName,
                        MaterialVersion = procMaterialEntity.Version ?? "",
                        MaterialBarCode = sfcProduceEntity.SFC,
                        Batch = "",//自制品 没有
                        Quantity = procMaterialEntity.Batch,
                        Unit = procMaterialEntity.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.ManuComplete,
                        Source = MaterialInventorySourceEnum.ManuComplete,
                        SiteId = bo.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                });

                responseBo.DegradedProductExtendBo = degradedProductExtendBo;
                responseBo.DowngradingEntities = await _manuDegradedProductExtendService.GetManuDownGradingsAsync(degradedProductExtendBo);
            }

            // 未完工（这么写仅仅是为了减少if-else的缩进）
            if (!responseBo.IsCompleted)
            {
                // TODO 这里不能这么写了，因为每个条码的复投次数不一定相同
                var multiUpdateProduceSFCCommand = new MultiUpdateProduceSFCCommand
                {
                    Ids = entities.Select(s => s.Id),
                    ProcedureId = bo.ProcedureId,
                    ResourceId = bo.ResourceId,
                    Status = SfcStatusEnum.lineUp,
                    RepeatedCount = firstSFCProduceEntity.RepeatedCount,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                };

                // 更新时间
                if (nextProcedure != null)
                {
                    multiUpdateProduceSFCCommand.ProcedureId = nextProcedure.Id;
                    // 不置空的话，进站时，可能校验不通过
                    multiUpdateProduceSFCCommand.ResourceId = null;
                }

                responseBo.MultiUpdateProduceSFCCommand = multiUpdateProduceSFCCommand;
            }

            return responseBo;
        }

        /// <summary>
        /// 不合格工序出站
        /// </summary>
        /// <returns></returns>
        private async Task<OutStationResponseBo?> OutStationForUnQualifiedProcedureAsync(OutStationRequestBo bo, IEnumerable<ManuSfcProduceEntity> sfcProduceEntities)
        {
            if (bo == null) return default;
            if (bo.Proxy == null) return default;

            // 待执行的命令
            OutStationResponseBo responseBo = new();

            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return default;
            var entities = sfcProduceEntities.AsList();

            // 这里已经不合适了，因为这批条码可能来源于不同的工单
            var firstSFCProduceEntity = entities.FirstOrDefault();
            if (firstSFCProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();
            responseBo.FirstSFC = firstSFCProduceEntity.SFC;

            // 读取产品基础信息
            var procMaterialEntityTask = _masterDataService.GetProcMaterialEntityWithNullCheckAsync(firstSFCProduceEntity.ProductId);

            // 读取当前工艺路线信息
            var procProcessRouteEntityTask = _masterDataService.GetProcProcessRouteEntityWithNullCheckAsync(firstSFCProduceEntity.ProcessRouteId);

            var procMaterialEntity = await procMaterialEntityTask;
            var procProcessRouteEntity = await procProcessRouteEntityTask;

            // 物料消耗明细数据
            MaterialConsumptionBo consumptionBo = new();
            // 指定消耗物料逻辑
            if (bo.ConsumeList != null && bo.ConsumeList.Any())
            {
                consumptionBo = await ExecutenMaterialConsumptionWithBarCodeAsync(bo, sfcProduceEntities);
            }
            // 默认消耗逻辑
            else
            {
                consumptionBo = await ExecutenMaterialConsumptionWithBOMAsync(bo, sfcProduceEntities);
            }
            responseBo.UpdateQtyByIdCommands = consumptionBo.UpdateQtyByIdCommands;
            responseBo.ManuSfcCirculationEntities = consumptionBo.ManuSfcCirculationEntities;

            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await bo.Proxy.GetValueAsync(_masterDataService.GetNextProcedureAsync, firstSFCProduceEntity);
            if (nextProcedure != null)
            {
                responseBo.IsCompleted = false;
                responseBo.NextProcedureCode = nextProcedure.Code;
            }

            // 查询工序信息
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(bo.ProcedureId);
            var cycle = procProcedureEntity.Cycle ?? 1;

            // 组装（出站步骤数据）
            entities.ForEach(sfcProduceEntity =>
            {
                // 初始化步骤
                var stepEntity = new ManuSfcStepEntity
                {
                    // 插入 manu_sfc_step 状态为出站（默认值）
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = SfcStatusEnum.Activity,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    Qty = sfcProduceEntity.Qty,
                    EquipmentId = bo.EquipmentId,
                    VehicleCode = bo.VehicleCode,
                    SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                    SiteId = bo.SiteId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                };

                // 已完工
                if (responseBo.IsCompleted)
                {
                    stepEntity.Operatetype = procProcessRouteEntity.Type == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                    stepEntity.CurrentStatus = SfcStatusEnum.Complete;  // TODO 这里的状态？？
                }

                // 如果超过复投次数
                if (sfcProduceEntity.RepeatedCount > cycle)
                {
                    // 清空复投次数
                    sfcProduceEntity.RepeatedCount = 0;

                    // 标记条码为"在制-完成"
                    stepEntity.CurrentStatus = SfcStatusEnum.InProductionComplete;
                }

                responseBo.SFCStepEntities.Add(stepEntity);
            });

            // 未完工（这么写仅仅是为了减少if-else的缩进）
            if (!responseBo.IsCompleted)
            {
                // TODO 这里不能这么写了，因为每个条码的复投次数不一定相同
                var multiUpdateProduceSFCCommand = new MultiUpdateProduceSFCCommand
                {
                    Ids = entities.Select(s => s.Id),
                    ProcedureId = bo.ProcedureId,
                    ResourceId = bo.ResourceId,
                    Status = SfcStatusEnum.lineUp,
                    RepeatedCount = firstSFCProduceEntity.RepeatedCount,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                };

                // 更新时间
                if (nextProcedure != null)
                {
                    multiUpdateProduceSFCCommand.ProcedureId = nextProcedure.Id;
                    // 不置空的话，进站时，可能校验不通过
                    multiUpdateProduceSFCCommand.ResourceId = null;
                }

                responseBo.MultiUpdateProduceSFCCommand = multiUpdateProduceSFCCommand;
            }

            // 记录出站不良记录（如果有传不合格代码）
            if (bo.OutStationUnqualifiedList != null && bo.OutStationUnqualifiedList.Any())
            {
                var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery
                {
                    SiteId = bo.SiteId,
                    Codes = bo.OutStationUnqualifiedList.Select(s => s.UnqualifiedCode)
                });

                List<ManuProductBadRecordEntity> productBadRecordEntities = new();
                foreach (var step in responseBo.SFCStepEntities)
                {
                    foreach (var item in qualUnqualifiedCodeEntities)
                    {
                        productBadRecordEntities.Add(new ManuProductBadRecordEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = bo.SiteId,
                            FoundBadOperationId = bo.ProcedureId,
                            FoundBadResourceId = bo.ResourceId,
                            OutflowOperationId = bo.ProcedureId,
                            UnqualifiedId = item.Id,
                            SfcStepId = step.Id,
                            SFC = step.SFC,
                            SfcInfoId = step.SFCInfoId,
                            Qty = step.Qty,
                            Status = step.CurrentStatus == SfcStatusEnum.InProductionComplete ? ProductBadRecordStatusEnum.Close : ProductBadRecordStatusEnum.Open,
                            Source = ProductBadRecordSourceEnum.EquipmentReBad,
                            Remark = step.Remark,
                            //DisposalResult = ProductBadDisposalResultEnum.AutoHandle,
                            CreatedBy = bo.UserName,
                            UpdatedBy = bo.UserName
                        });
                    }
                }

                responseBo.ProductBadRecordEntities = productBadRecordEntities;
            }

            return responseBo;
        }

        /// <summary>
        /// 执行物料消耗（默认BOM清单）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="sfcProduceEntities"></param>
        /// <returns></returns>
        private async Task<MaterialConsumptionBo> ExecutenMaterialConsumptionWithBOMAsync(OutStationRequestBo bo, IEnumerable<ManuSfcProduceEntity> sfcProduceEntities)
        {
            // 物料消耗对象
            MaterialConsumptionBo responseBo = new();

            if (bo == null) return responseBo;
            if (bo.Proxy == null) return responseBo;

            // 说明：默认所有同时刻进站的条码，都是同一BOM
            var firstSFCProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstSFCProduceEntity == null) return responseBo;

            // 组合物料数据
            var initialMaterials = await bo.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsAsync, firstSFCProduceEntity);
            if (initialMaterials == null) return responseBo;

            // 物料ID集合
            var materialIds = initialMaterials.Select(s => s.MaterialId);

            // 读取物料加载数据（批量）
            var allFeedingEntities = await bo.Proxy.GetValueAsync(_manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync, new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = firstSFCProduceEntity.ResourceId ?? 0,
                MaterialIds = materialIds
            });

            // 通过物料分组
            var manuFeedingsDictionary = allFeedingEntities?.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 过滤扣料集合
            List<UpdateQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            List<MultiUpdateSummaryOutStationCommand> updateSummaryOutStationCommands = new();
            foreach (var materialBo in initialMaterials)
            {
                if (manuFeedingsDictionary == null) continue;

                // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100（因为存在多条码同时出站情况,所以直接消耗 * 条码数量）
                materialBo.Usages *= sfcProduceEntities.Count();
                decimal residue = materialBo.Usages;

                if (materialBo.Loss.HasValue && materialBo.Loss > 0) residue *= materialBo.Loss.Value;
                if (materialBo.ConsumeRatio > 0) residue *= (materialBo.ConsumeRatio / 100);

                // 收集方式是批次
                if (materialBo.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, firstSFCProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialBo.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 进行扣料
                _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, firstSFCProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
            }

            responseBo.UpdateQtyByIdCommands = updates;
            responseBo.ManuSfcCirculationEntities = adds;
            return responseBo;
        }

        /// <summary>
        /// 执行物料消耗（指定物料）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="sfcProduceEntities"></param>
        /// <returns></returns>
        private async Task<MaterialConsumptionBo> ExecutenMaterialConsumptionWithBarCodeAsync(OutStationRequestBo bo, IEnumerable<ManuSfcProduceEntity> sfcProduceEntities)
        {
            // 物料消耗对象
            MaterialConsumptionBo responseBo = new();

            if (bo == null) return responseBo;
            if (bo.Proxy == null) return responseBo;
            if (bo.ConsumeList == null) return responseBo;

            // 说明：默认所有同时刻进站的条码，都是同一BOM
            var firstSFCProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstSFCProduceEntity == null) return responseBo;

            // 组合物料数据
            var initialMaterials = await bo.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsAsync, firstSFCProduceEntity);
            if (initialMaterials == null) return responseBo;

            // 如果存在传过来的消耗编码不在BOM清单里面的物料，直接返回异常
            var hasBarCodeNotInBOM = bo.ConsumeList.Select(s => s.BarCode).Except(initialMaterials.Select(s => s.MaterialCode)).Any();
            if (hasBarCodeNotInBOM)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17105));
            }

            // 只保留传过来的消耗编码
            List<MaterialDeductBo> filterMaterials = new();
            foreach (var item in initialMaterials)
            {
                var consume = bo.ConsumeList.FirstOrDefault(f => f.BarCode == item.MaterialCode);
                if (consume == null) continue;

                if (consume.ConsumeQty.HasValue)
                {
                    item.Usages = consume.ConsumeQty.Value;
                    //item.ConsumeRatio = 100;
                    //item.Loss = 0;
                }

                // 如果不保留替代品（如果保留，就删除这句）
                item.ReplaceMaterials = Enumerable.Empty<MaterialDeductItemBo>();

                filterMaterials.Add(item);
            }

            // 重新赋值
            initialMaterials = filterMaterials;

            // 物料ID集合
            var materialIds = initialMaterials.Select(s => s.MaterialId);

            // 读取物料加载数据（批量）
            var allFeedingEntities = await bo.Proxy.GetValueAsync(_manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync, new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = firstSFCProduceEntity.ResourceId ?? 0,
                MaterialIds = materialIds
            });

            // 通过物料分组
            var manuFeedingsDictionary = allFeedingEntities?.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 过滤扣料集合
            List<UpdateQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            List<MultiUpdateSummaryOutStationCommand> updateSummaryOutStationCommands = new();
            foreach (var materialBo in initialMaterials)
            {
                if (manuFeedingsDictionary == null) continue;

                // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100（因为存在多条码同时出站情况,所以直接消耗 * 条码数量）
                materialBo.Usages *= sfcProduceEntities.Count();
                decimal residue = materialBo.Usages;

                if (materialBo.Loss.HasValue && materialBo.Loss > 0) residue *= materialBo.Loss.Value;
                if (materialBo.ConsumeRatio > 0) residue *= (materialBo.ConsumeRatio / 100);

                // 收集方式是批次
                if (materialBo.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, firstSFCProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialBo.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 进行扣料
                _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, firstSFCProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
            }

            responseBo.UpdateQtyByIdCommands = updates;
            responseBo.ManuSfcCirculationEntities = adds;
            return responseBo;
        }
        #endregion

    }
}
