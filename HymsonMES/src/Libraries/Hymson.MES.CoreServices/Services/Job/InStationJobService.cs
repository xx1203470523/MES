﻿using Dapper;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 进站
    /// </summary>
    [Job("进站", JobTypeEnum.Standard)]
    public class InStationJobService : IJobService
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
        /// 仓储接口（工单信息）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="localizationService"></param>
        public InStationJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            ILocalizationService localizationService,
            IProcProcedureRepository procProcedureRepository)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _localizationService = localizationService;
            _procProcedureRepository = procProcedureRepository;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo bo) return;
            if (bo == null) return;
            if (bo.InStationRequestBos == null || bo.InStationRequestBos.Any() == false) return;

            // 校验工序和资源是否对应
            var resourceIds = await bo.Proxy!.GetValueAsync(_masterDataService.GetProcResourceIdByProcedureIdAsync, bo.ProcedureId);
            if (resourceIds == null || !resourceIds.Any(a => a == bo.ResourceId)) throw new CustomerValidationException(nameof(ErrorCode.MES16317));

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = bo.SiteId, SFCs = bo.InStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return;

            // 判断条码锁状态
            var sfcProduceBusinessEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, multiSFCBo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.lineUp, _localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.lineUp)}"));

            // 进站工序信息
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(bo.ProcedureId) ??
                throw new CustomerValidationException(nameof(ErrorCode.MES16352));

            // 循环次数验证（复投次数）
            sfcProduceEntities?.VerifySFCRepeatedCount(procedureEntity.Cycle ?? 1);
            sfcProduceBusinessEntities?.VerifyProcedureLock(multiSFCBo.SFCs, bo.ProcedureId);

            // 验证条码是否被容器包装
            await _manuCommonService.VerifyContainerAsync(multiSFCBo);

            // 获取生产工单（附带工单状态校验）
            var planWorkOrderEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceWorkOrderByIdsAsync, new WorkOrderIdsBo { WorkOrderIds = sfcProduceEntities!.Select(s => s.WorkOrderId) });
            if (planWorkOrderEntities!.Any(a => a.Status == PlanWorkOrderStatusEnum.Finish))
            {
                // 完工的工单，不允许再投入（不管哪个工序都不允许再投入，之前逻辑是会读取工艺路线，只对首工序进行校验）
                throw new CustomerValidationException(nameof(ErrorCode.MES16350));
            }

            // 如果工序对应不上
            var sfcProduceEntitiesOfNoMatchProcedure = sfcProduceEntities!.Where(a => a.ProcedureId != bo.ProcedureId);
            if (sfcProduceEntitiesOfNoMatchProcedure != null && sfcProduceEntitiesOfNoMatchProcedure.Any())
            {
                var query = new EntityBySiteIdQuery { SiteId = bo.SiteId };
                var allProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetListAsync(query);
                var allProcessRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetListAsync(query);

                foreach (var sfcProduce in sfcProduceEntitiesOfNoMatchProcedure)
                {
                    // 如果有性能问题，可以考虑将这个两个集合先分组，然后再进行判断
                    var processRouteDetailLinks = allProcessRouteDetailLinks.Where(w => w.ProcessRouteId == sfcProduce.ProcessRouteId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                    var processRouteDetailNodes = allProcessRouteDetailNodes.Where(w => w.ProcessRouteId == sfcProduce.ProcessRouteId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                    // 判断条码应进站工序和实际进站工序之间是否全部都是随机工序（因为随机工序可以跳过）
                    var IsAllRandomProcedureBetween = await bo.Proxy.GetValueAsync(_masterDataService.IsAllRandomProcedureBetweenAsync, new ManuRouteProcedureRandomCompareBo
                    {
                        ProcessRouteDetailLinks = processRouteDetailLinks,
                        ProcessRouteDetailNodes = processRouteDetailNodes,
                        ProcessRouteId = sfcProduce.ProcessRouteId,
                        BeginProcedureId = sfcProduce.ProcedureId,
                        EndProcedureId = bo.ProcedureId
                    });
                    if (!IsAllRandomProcedureBetween) throw new CustomerValidationException(nameof(ErrorCode.MES16308));
                }
            }
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo bo) return default;
            if (bo == null) return default;

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.BeforeStart
            });
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>00
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo bo) return default;
            if (bo == null) return default;
            if (bo.InStationRequestBos == null || bo.InStationRequestBos.Any() == false) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = bo.SiteId, SFCs = bo.InStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, multiSFCBo);

            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return default;
            var entities = sfcProduceEntities.AsList();

            var firstSFCProduceEntity = entities.FirstOrDefault();
            if (firstSFCProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            // 检查是否首工序
            var isFirstProcedure = await bo.Proxy.GetValueAsync(_masterDataService.IsFirstProcedureAsync, new ManuRouteProcedureBo
            {
                ProcessRouteId = firstSFCProduceEntity.ProcessRouteId,
                ProcedureId = firstSFCProduceEntity.ProcedureId
            });

            // 获取当前工序信息
            var procedureEntity = await _masterDataService.GetProcProcedureEntityWithNullCheckAsync(firstSFCProduceEntity.ProcedureId);

            // 更新工单信息
            var updateQtyCommand = new UpdateQtyCommand
            {
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                WorkOrderId = firstSFCProduceEntity.WorkOrderId
            };

            // 组装（进站数据）
            List<ManuSfcStepEntity> sfcStepEntities = new();
            List<ManuSfcSummaryEntity> manuSfcSummaryEntities = new();
            MultiSfcUpdateIsUsedCommand sfcUpdateIsUsedCommand = new();
            List<MultiUpdateProduceInStationSFCCommand> updateProduceInStationSFCCommands = new();
            List<InStationManuSfcByIdCommand> inStationManuSfcByIdCommands = new();
            entities.ForEach(sfcProduceEntity =>
            {
                // 检查是否测试工序
                if (procedureEntity.Type == ProcedureTypeEnum.Test)
                {
                    // 超过复投次数，标识为NG
                    if (sfcProduceEntity.RepeatedCount > procedureEntity.Cycle) throw new CustomerValidationException(nameof(ErrorCode.MES16036));
                }

                // 每次进站都将复投次数+1
                sfcProduceEntity.RepeatedCount++;

                // 初始化步骤
                sfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Operatetype = ManuSfcStepTypeEnum.InStock,  // 状态为 进站
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
                });

                updateProduceInStationSFCCommands.Add(new MultiUpdateProduceInStationSFCCommand
                {
                    Id = sfcProduceEntity.Id,
                    ProcedureId = bo.ProcedureId,
                    ResourceId = bo.ResourceId,
                    Status = SfcStatusEnum.Activity,
                    CurrentStatus = sfcProduceEntity.Status,
                    RepeatedCount = sfcProduceEntity.RepeatedCount,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });

                inStationManuSfcByIdCommands.Add(new InStationManuSfcByIdCommand
                {
                    Id = sfcProduceEntity.SFCId,
                    Status = SfcStatusEnum.Activity,
                    IsUsed = YesOrNoEnum.Yes,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });

                sfcProduceEntity.ProcedureId = bo.ProcedureId;
                sfcProduceEntity.ResourceId = bo.ResourceId;

                // 更新状态，将条码由"排队"改为"活动"
                sfcProduceEntity.Status = SfcStatusEnum.Activity;
                sfcProduceEntity.UpdatedBy = bo.UserName;
                sfcProduceEntity.UpdatedOn = HymsonClock.Now();
            });

            if (isFirstProcedure)
            {
                // 更新工单的 InputQty
                updateQtyCommand.Qty = entities.Count;
            }

            return new InStationResponseBo
            {
                IsFirstProcedure = isFirstProcedure,
                FirstSFC = firstSFCProduceEntity.SFC,
                ManuSfcProduceEntities = entities,
                UpdateQtyCommand = updateQtyCommand,
                SFCStepEntities = sfcStepEntities,
                MultiUpdateProduceInStationSFCCommands = updateProduceInStationSFCCommands,
                InStationManuSfcByIdCommands = inStationManuSfcByIdCommands
            };
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not InStationResponseBo data) return responseBo;

            // 更改状态
            responseBo.Rows += await _manuSfcProduceRepository.MultiUpdateProduceInStationSFCAsync(data.MultiUpdateProduceInStationSFCCommands);

            // 未更新到数据，事务回滚
            if (responseBo.Rows != data?.MultiUpdateProduceInStationSFCCommands?.Count())
            {
                // 这里在外层会回滚事务
                responseBo.Rows = -1;
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18216), data.FirstSFC);
                return responseBo;
            }

            // 更新数据
            List<Task<int>> tasks = new();

            //更新条码表 状态为排队
            var multiUpdateSfcIsUsedTask = _manuSfcRepository.InStationManuSfcByIdAsync(data.InStationManuSfcByIdCommands);
            tasks.Add(multiUpdateSfcIsUsedTask);

            // 插入 manu_sfc_step 状态为 进站
            var manuSfcStepTask = _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities);
            tasks.Add(manuSfcStepTask);

            // 更新工单的 InputQty
            var updateInputQtyByWorkOrderIdTask = _planWorkOrderRepository.UpdateInputQtyByWorkOrderIdAsync(data.UpdateQtyCommand);
            tasks.Add(updateInputQtyByWorkOrderIdTask);

            // 等待所有任务完成
            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            // 面板需要的数据
            responseBo.Content = new Dictionary<string, string> {
                { "PackageCom", "False" },
                { "BadEntryCom", "False" },
            };

            responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18215), data.FirstSFC);
            return responseBo;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo bo) return default;
            if (bo == null) return default;

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.AfterStart
            });
        }

    }
}
