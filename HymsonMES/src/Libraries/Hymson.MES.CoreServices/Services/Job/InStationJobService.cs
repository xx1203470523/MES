using Dapper;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using ErrorCode = Hymson.MES.Core.Constants.ErrorCode;

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
            ILocalizationService localizationService)
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
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return;

            // 校验工序和资源是否对应
            var resourceIds = await bo.Proxy.GetValueAsync(_masterDataService.GetProcResourceIdByProcedureIdAsync, bo.ProcedureId);
            if (resourceIds == null || resourceIds.Any(a => a == bo.ResourceId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16317));

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            // 判断条码锁状态
            var sfcProduceBusinessEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.lineUp, _localizationService.GetResource($"{typeof(SfcProduceStatusEnum).FullName}.{nameof(SfcProduceStatusEnum.lineUp)}"));
            sfcProduceBusinessEntities?.VerifyProcedureLock(bo.SFCs, bo.ProcedureId);

            // 验证条码是否被容器包装
            await _manuCommonService.VerifyContainerAsync(bo);

            //（前提：这些条码都是同一工单同一工序）
            var firstProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return;

            // 获取生产工单（附带工单状态校验）
            var planWorkOrderEntity = await bo.Proxy.GetValueAsync(async parameters =>
            {
                long workOrderId = (long)parameters[0];
                bool isVerifyActivation = parameters.Length <= 1 || (bool)parameters[1];
                return await _masterDataService.GetProduceWorkOrderByIdAsync(workOrderId, isVerifyActivation);
            }, new object[] { firstProduceEntity.WorkOrderId, true });

            // 当工单已激活且完工状态，且条码处于工艺路线的首工序，进站时，提示“工单状态为完工，不允许再对工单投入”
            if (planWorkOrderEntity?.Status == PlanWorkOrderStatusEnum.Finish)
            {
                // 检查是否首工序
                var isFirstProcedure = await bo.Proxy.GetValueAsync(async parameters =>
                {
                    var processRouteId = (long)parameters[0];
                    var procedureId = (long)parameters[1];
                    return await _masterDataService.IsFirstProcedureAsync(processRouteId, procedureId);
                }, new object[] { firstProduceEntity.ProcessRouteId, firstProduceEntity.ProcedureId });

                // 因为获取工单方法已经对激活状态做了校验，这里不需要再校验
                if (isFirstProcedure) throw new CustomerValidationException(nameof(ErrorCode.MES16350));
            }

            // 如果工序对应不上
            if (firstProduceEntity.ProcedureId != bo.ProcedureId)
            {
                var processRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(firstProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(firstProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await bo.Proxy.GetValueAsync(async parameters =>
                {
                    var processRouteDetailLinks = (IEnumerable<ProcProcessRouteDetailLinkEntity>)parameters[0];
                    var processRouteDetailNodes = (IEnumerable<ProcProcessRouteDetailNodeEntity>)parameters[1];
                    var processRouteId = (long)parameters[2];
                    var procedureId = (long)parameters[3];
                    return await _masterDataService.IsRandomPreProcedureAsync(processRouteDetailLinks, processRouteDetailNodes, processRouteId, procedureId);
                }, new object[] { processRouteDetailLinks, processRouteDetailNodes, firstProduceEntity.ProcessRouteId, bo.ProcedureId });

                if (IsRandomPreProcedure == false) throw new CustomerValidationException(nameof(ErrorCode.MES16308));
            }
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
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return default;

            // 待执行的命令
            InStationResponseBo responseBo = new();

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);

            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return default;
            var entities = sfcProduceEntities.AsList();

            var firstSFCProduceEntity = entities.FirstOrDefault();
            if (firstSFCProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            // 检查是否首工序
            var isFirstProcedure = await bo.Proxy.GetValueAsync(async parameters =>
            {
                var processRouteId = (long)parameters[0];
                var procedureId = (long)parameters[1];
                return await _masterDataService.IsFirstProcedureAsync(processRouteId, procedureId);
            }, new object[] { firstSFCProduceEntity.ProcessRouteId, firstSFCProduceEntity.ProcedureId });

            // 获取当前工序信息
            var procedureEntity = await _masterDataService.GetProcProcedureEntityWithNullCheck(firstSFCProduceEntity.ProcedureId);

            // 更新工单信息
            var updateQtyCommand = new UpdateQtyCommand
            {
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                WorkOrderId = firstSFCProduceEntity.WorkOrderId
            };

            // 组装（进站数据）
            List<ManuSfcStepEntity> sfcStepEntities = new();
            MultiSfcUpdateIsUsedCommand sfcUpdateIsUsedCommand = new();
            entities.ForEach(sfcProduceEntity =>
            {
                // 检查是否测试工序
                if (procedureEntity.Type == ProcedureTypeEnum.Test)
                {
                    // 超过复投次数，标识为NG
                    if (firstSFCProduceEntity.RepeatedCount > procedureEntity.Cycle) throw new CustomerValidationException(nameof(ErrorCode.MES16036));
                    firstSFCProduceEntity.RepeatedCount++;
                }

                sfcProduceEntity.ProcedureId = bo.ProcedureId;
                sfcProduceEntity.ResourceId = bo.ResourceId;

                // 更新状态，将条码由"排队"改为"活动"
                sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
                sfcProduceEntity.UpdatedBy = bo.UserName;
                sfcProduceEntity.UpdatedOn = HymsonClock.Now();

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
            });

            if (isFirstProcedure == true)
            {
                // 更新工单的 InputQty
                updateQtyCommand.Qty = entities.Count;

                // 修改条码使用状态为"已使用"
                sfcUpdateIsUsedCommand = new MultiSfcUpdateIsUsedCommand
                {
                    SiteId = bo.SiteId,
                    SFCs = entities.Select(s => s.SFC),
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    IsUsed = YesOrNoEnum.Yes
                };
            }

            //  修改在制品状态
            var multiUpdateProduceSFCCommand = new MultiUpdateProduceSFCCommand
            {
                Ids = entities.Select(s => s.Id),
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                Status = SfcProduceStatusEnum.Activity,
                RepeatedCount = firstSFCProduceEntity.RepeatedCount,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            return new InStationResponseBo
            {
                IsFirstProcedure = isFirstProcedure,
                FirstSFC = firstSFCProduceEntity.SFC,
                ManuSfcProduceEntities = entities,
                MultiUpdateProduceSFCCommand = multiUpdateProduceSFCCommand,
                UpdateQtyCommand = updateQtyCommand,
                SFCStepEntities = sfcStepEntities,
                MultiSfcUpdateIsUsedCommand = sfcUpdateIsUsedCommand
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
            //responseBo.Rows += await _manuSfcProduceRepository.UpdateRangeWithStatusCheckAsync(data.SFCProduceEntities);
            responseBo.Rows += await _manuSfcProduceRepository.MultiUpdateRangeWithStatusCheckAsync(data.MultiUpdateProduceSFCCommand);

            // 未更新到数据，事务回滚
            if (responseBo.Rows <= 0)
            {
                // 这里在外层会回滚事务
                responseBo.Rows = -1;

                //throw new CustomerValidationException(nameof(ErrorCode.MES18217)).WithData("SFC", data.FirstSFCProduceEntity.SFC);
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18216), data.FirstSFC);
                return responseBo;
            }

            // 更新数据
            List<Task<int>> tasks = new();

            // 插入 manu_sfc_step 状态为 进站
            var manuSfcStepTask = _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities);
            tasks.Add(manuSfcStepTask);

            // 如果是首工序
            if (data.IsFirstProcedure == true)
            {
                // 修改条码使用状态为"已使用"
                var multiUpdateSfcIsUsedTask = _manuSfcRepository.MultiUpdateSfcIsUsedAsync(data.MultiSfcUpdateIsUsedCommand);
                tasks.Add(multiUpdateSfcIsUsedTask);
            }

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
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return null;
            return await _masterDataService.GetJobRalationJobByProcedureIdOrResourceId(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.AfterStart
            });
        }

    }
}
