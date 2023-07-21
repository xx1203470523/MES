using Dapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
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
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="localizationService"></param>
        public OutStationJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuFeedingRepository manuFeedingRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            ILocalizationService localizationService)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuFeedingRepository = manuFeedingRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _localizationService = localizationService;
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
            var sfcProduceEntities = await bo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            // 判断条码锁状态
            var sfcProduceBusinessEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.Activity, _localizationService.GetResource($"{typeof(SfcProduceStatusEnum).FullName}.{nameof(SfcProduceStatusEnum.Activity)}"))
                              .VerifyProcedure(bo.ProcedureId)
                              .VerifyResource(bo.ResourceId);

            //（前提：这些条码都是同一工单同一工序）
            var firstProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return;

            // 获取生产工单（附带工单状态校验）
            _ = await bo.Proxy.GetValueAsync(async parameters =>
            {
                long workOrderId = (long)parameters[0];
                bool isVerifyActivation = parameters.Length <= 1 || (bool)parameters[1];
                return await _masterDataService.GetProduceWorkOrderByIdAsync(workOrderId, isVerifyActivation);
            }, new object[] { firstProduceEntity.WorkOrderId, true });

            // 验证BOM主物料数量
            await _manuCommonService.VerifyBomQtyAsync(new ManuProcedureBomBo
            {
                SiteId = bo.SiteId,
                SFCs = bo.SFCs,
                ProcedureId = bo.ProcedureId,
                BomId = firstProduceEntity.ProductBOMId
            });
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return null;
            return await _masterDataService.GetJobRalationJobByProcedureIdOrResourceId(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
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

            // 待执行的命令
            OutStationResponseBo responseBo = new();

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);

            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return default;
            var entities = sfcProduceEntities.AsList();

            responseBo.FirstSFCProduceEntity = entities.FirstOrDefault();
            if (responseBo.FirstSFCProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            // 读取条码信息
            var manuSfcEntitiesTask = _masterDataService.GetManuSFCEntitiesWithNullCheck(bo);

            // 读取产品基础信息
            var procMaterialEntityTask = _masterDataService.GetProcMaterialEntityWithNullCheck(responseBo.FirstSFCProduceEntity.ProductId);

            // 读取当前工艺路线信息
            var procProcessRouteEntityTask = _masterDataService.GetProcProcessRouteEntityWithNullCheck(responseBo.FirstSFCProduceEntity.ProcessRouteId);

            var manuSfcEntities = await manuSfcEntitiesTask;
            var procMaterialEntity = await procMaterialEntityTask;
            var procProcessRouteEntity = await procProcessRouteEntityTask;

            // 合格品出站
            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await bo.Proxy.GetValueAsync(_masterDataService.GetNextProcedureAsync, responseBo.FirstSFCProduceEntity);
            if (nextProcedure != null)
            {
                responseBo.IsCompleted = false;
                responseBo.NextProcedureCode = nextProcedure.Code;
            }

            // 组合物料数据
            var initialMaterials = await bo.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsAsync, responseBo.FirstSFCProduceEntity);
            if (initialMaterials == null) return default;

            // 物料ID集合
            var materialIds = initialMaterials.Select(s => s.MaterialId);

            // 读取物料加载数据（批量）
            var allFeedingEntities = await bo.Proxy.GetValueAsync(_manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync, new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = responseBo.FirstSFCProduceEntity.ResourceId ?? 0,
                MaterialIds = materialIds
            });

            // 通过物料分组
            var manuFeedingsDictionary = allFeedingEntities?.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 过滤扣料集合
            List<UpdateQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            foreach (var materialBo in initialMaterials)
            {
                if (manuFeedingsDictionary == null) continue;

                // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
                decimal residue = materialBo.Usages;
                if (materialBo.Loss.HasValue == true && materialBo.Loss > 0) residue *= materialBo.Loss.Value;
                if (materialBo.ConsumeRatio > 0) residue *= (materialBo.ConsumeRatio / 100);

                // 收集方式是批次
                if (materialBo.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, responseBo.FirstSFCProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialBo.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 进行扣料
                _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, responseBo.FirstSFCProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
            }
            responseBo.UpdateQtyByIdCommands = updates;
            responseBo.ManuSfcCirculationEntities = adds;

            // 组装（出站步骤数据）
            entities.ForEach(sfcProduceEntity =>
            {
                // 初始化步骤
                var stepEntity = new ManuSfcStepEntity
                {
                    // 插入 manu_sfc_step 状态为出站（默认值）
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = SfcProduceStatusEnum.Activity,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    Qty = sfcProduceEntity.Qty,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    ResourceId = sfcProduceEntity.ResourceId,
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
                    stepEntity.CurrentStatus = SfcProduceStatusEnum.Complete;  // TODO 这里的状态？？
                }

                responseBo.SFCStepEntities.Add(stepEntity);
            });

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
                    WorkOrderId = responseBo.FirstSFCProduceEntity.WorkOrderId,
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
                    SFCs = manuSfcEntities.Select(s => s.SFC)
                };

                // 入库
                entities.ForEach(sfcProduceEntity =>
                {
                    // 新增 wh_material_inventory
                    responseBo.WhMaterialInventoryEntities.Add(new WhMaterialInventoryEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SupplierId = 0,//自制品 没有
                        MaterialId = sfcProduceEntity.ProductId,
                        MaterialBarCode = sfcProduceEntity.SFC,
                        Batch = "",//自制品 没有
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
            }

            // 未完工（这么写仅仅是为了减少if-else的缩进）
            if (responseBo.IsCompleted == false)
            {
                var multiUpdateProduceSFCCommand = new MultiUpdateProduceSFCCommand
                {
                    Ids = entities.Select(s => s.Id),
                    ProcedureId = bo.ProcedureId,
                    ResourceId = bo.ResourceId,
                    Status = SfcProduceStatusEnum.lineUp,
                    RepeatedCount = responseBo.FirstSFCProduceEntity.RepeatedCount,
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
                //tasks.Add(_manuFeedingRepository.UpdateQtyByIdAsync(data.UpdateQtyByIdCommands));
                responseBo.Rows += await _manuFeedingRepository.UpdateQtyByIdAsync(data.UpdateQtyByIdCommands);

                // 未更新到全部需更新的数据，事务回滚
                if (data.UpdateQtyByIdCommands.Count() > responseBo.Rows)
                {
                    responseBo.Rows = -1;
                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18216), data.FirstSFCProduceEntity.SFC);
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

                    //// 2023.05.29 克明说不在这里更新完成时间
                    //// 更新工单统计表的 RealEnd
                    //rows += await _planWorkOrderRepository.UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
                    //{
                    //    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    //    UpdatedBy = sfcProduceEntity.UpdatedBy,
                    //    WorkOrderIds = new long[] { sfcProduceEntity.WorkOrderId }
                    //});

                    // 入库
                    tasks.Add(_whMaterialInventoryRepository.InsertsAsync(data.WhMaterialInventoryEntities));
                    tasks.Add(_whMaterialStandingbookRepository.InsertsAsync(data.WhMaterialStandingbookEntities));
                }
            }
            // 未完工
            else
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                //tasks.Add(_manuSfcProduceRepository.UpdateRangeWithStatusCheckAsync(data.SFCProduceEntities));
                var rows = await _manuSfcProduceRepository.MultiUpdateRangeWithStatusCheckAsync(data.MultiUpdateProduceSFCCommand);

                // 未更新到数据，事务回滚
                if (rows <= 0)
                {
                    // 这里在外层会回滚事务
                    responseBo.Rows = -1;

                    //throw new CustomerValidationException(nameof(ErrorCode.MES18217)).WithData("SFC", data.FirstSFCProduceEntity.SFC);
                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18216), data.FirstSFCProduceEntity.SFC);
                    return responseBo;
                }
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
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES16349), data.FirstSFCProduceEntity.SFC);
            }
            else
            {
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES16351), data.FirstSFCProduceEntity.SFC, data.NextProcedureCode);
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
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return null;
            return await _masterDataService.GetJobRalationJobByProcedureIdOrResourceId(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
            });
        }

    }
}
