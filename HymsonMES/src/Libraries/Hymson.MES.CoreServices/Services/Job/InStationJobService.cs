﻿using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 进站
    /// </summary>
    [Job("进站", JobTypeEnum.Standard)]
    public class InStationJobService : IJobService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<InStationJobService> _logger;

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
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="localizationService"></param>
        public InStationJobService(ILogger<InStationJobService> logger,
            IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IProcProcedureRepository procProcedureRepository,
            ILocalizationService localizationService)
        {
            _logger = logger;
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _procProcedureRepository = procProcedureRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return;
            if (commonBo == null) return;
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16370));
            }

            // 进站工序信息
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(commonBo.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16358)).WithData("Procedure", commonBo.ProcedureId);

            // 读取工序关联的资源
            var resourceIds = await commonBo.Proxy!.GetValueAsync(_masterDataService.GetProcResourceIdByProcedureIdAsync, commonBo.ProcedureId);
            if (resourceIds == null || !resourceIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16355)).WithData("ProcedureCode", procedureEntity.Code);
            }

            // 校验工序和资源是否对应
            if (!resourceIds.Any(a => a == commonBo.ResourceId))
            {
                _logger.LogWarning($"工序{commonBo.ProcedureId}和资源{commonBo.ResourceId}不对应！");
                throw new CustomerValidationException(nameof(ErrorCode.MES16317));
            }

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            // 是否有不属于在制品表的条码
            var notIncludeSFCs = multiSFCBo.SFCs.Except(sfcProduceEntities.Select(s => s.SFC));
            if (notIncludeSFCs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', notIncludeSFCs));
            }

            // 判断条码锁状态
            var sfcProduceBusinessEntities = await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, multiSFCBo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.lineUp, _localizationService);
            sfcProduceBusinessEntities?.VerifyProcedureLock(sfcProduceEntities, procedureEntity);

            // 验证条码是否被容器包装
            await _manuCommonService.VerifyContainerAsync(multiSFCBo);

            // 获取生产工单（附带工单状态校验）
            var planWorkOrderEntities = await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceWorkOrderByIdsAsync, new WorkOrderIdsBo
            {
                WorkOrderIds = sfcProduceEntities!.Select(s => s.WorkOrderId)
            });
            if (planWorkOrderEntities!.Any(a => a.Status == PlanWorkOrderStatusEnum.Finish))
            {
                // 完工的工单，不允许再投入（不管哪个工序都不允许再投入，之前逻辑是会读取工艺路线，只对首工序进行校验）
                throw new CustomerValidationException(nameof(ErrorCode.MES16350));
            }

            // 如果工序对应不上
            var sfcProduceEntitiesOfNoMatchProcedure = sfcProduceEntities!.Where(a => a.ProcedureId != commonBo.ProcedureId);
            if (sfcProduceEntitiesOfNoMatchProcedure != null && sfcProduceEntitiesOfNoMatchProcedure.Any())
            {
                var query = new EntityBySiteIdQuery { SiteId = commonBo.SiteId };
                var allProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetListAsync(query);
                var allProcessRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetListAsync(query);

                var validationFailures = new List<ValidationFailure>();
                foreach (var sfcProduce in sfcProduceEntitiesOfNoMatchProcedure)
                {
                    var sfcProcedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduce.ProcedureId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES16369))
                        .WithData("SFC", sfcProduce.SFC)
                        .WithData("Procedure", sfcProduce.ProcedureId);

                    // 如果存在工序不一致，且复投次数大于0时，抛出异常
                    if (sfcProduce.RepeatedCount > 0)
                    {
                        var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("SFC", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Procedure", sfcProcedureEntity.Code);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Cycle", sfcProduce.RepeatedCount);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16368);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    // 如果有性能问题，可以考虑将这个两个集合先分组，然后再进行判断
                    var processRouteDetailLinks = allProcessRouteDetailLinks.Where(w => w.ProcessRouteId == sfcProduce.ProcessRouteId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                    var processRouteDetailNodes = allProcessRouteDetailNodes.Where(w => w.ProcessRouteId == sfcProduce.ProcessRouteId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                    // 判断条码应进站工序和实际进站工序之间是否全部都是随机工序（因为随机工序可以跳过）
                    var beginNode = processRouteDetailNodes.FirstOrDefault(f => f.ProcedureId == sfcProduce.ProcedureId);
                    var endNode = processRouteDetailNodes.FirstOrDefault(f => f.ProcedureId == commonBo.ProcedureId);

                    if (beginNode == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES18228))
                            .WithData("SFC", sfcProduce.SFC)
                            .WithData("Procedure", sfcProcedureEntity.Code);
                    }
                    if (endNode == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES18229))
                            .WithData("SFC", sfcProduce.SFC)
                            .WithData("Current", procedureEntity.Code);
                    }

                    var nodesOfOrdered = processRouteDetailNodes.OrderBy(o => o.SerialNo)
                        .Where(w => w.SerialNo.ParseToInt() >= beginNode.SerialNo.ParseToInt() && w.SerialNo.ParseToInt() < endNode.SerialNo.ParseToInt());

                    // 两个工序之间没有工序，即表示当前实际进站的工序，处于条码记录的应进站工序前面
                    if (!nodesOfOrdered.Any())
                    {
                        // 当前工序
                        var currentEntity = await _procProcedureRepository.GetByIdAsync(endNode.ProcedureId);

                        var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("SFC", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Current", procedureEntity.Code);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Procedure", sfcProcedureEntity.Code);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16354);
                        validationFailures.Add(validationFailure);

                        _logger.LogWarning($"工艺路线工序节点数据异常，工艺路线ID：{sfcProduce.ProcessRouteId}，条码工序ID：{beginNode.ProcedureId}，进站工序ID：{endNode.ProcedureId}");
                        continue;
                    }

                    // 如果中间的工序存在不是随机工序的话，就返回false
                    if (nodesOfOrdered.Any(a => a.CheckType != ProcessRouteInspectTypeEnum.RandomInspection))
                    {
                        var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("SFC", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Current", procedureEntity.Code);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Procedure", sfcProcedureEntity.Code);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16357);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                }

                if (validationFailures.Any())
                {
                    throw new ValidationException("", validationFailures);
                }
            }

            // 循环次数验证（复投次数）
            sfcProduceEntities?.VerifySFCRepeatedCount(procedureEntity.Cycle ?? 1, _localizationService);
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = commonBo.ProcedureId,
                ResourceId = commonBo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.BeforeStart
            });
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any()) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            // 进站工序信息
            var currentProcedureEntity = await _procProcedureRepository.GetByIdAsync(commonBo.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16358)).WithData("Procedure", commonBo.ProcedureId);

            // 遍历所有条码
            var responseBos = new List<InStationResponseBo>();
            var responseSummaryBo = new InStationResponseSummaryBo();
            foreach (var requestBo in commonBo.InStationRequestBos)
            {
                var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(s => s.SFC == requestBo.SFC)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", requestBo.SFC);

                // 单条码返回值
                InStationResponseBo responseBo = new();

                // 检查是否首工序
                responseBo.IsFirstProcedure = await commonBo.Proxy.GetValueAsync(_masterDataService.IsFirstProcedureAsync, new ManuRouteProcedureBo
                {
                    ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                    ProcedureId = commonBo.ProcedureId
                });

                // 检查是否测试工序
                if (currentProcedureEntity.Type == ProcedureTypeEnum.Test)
                {
                    // 超过复投次数，标识为NG
                    if (sfcProduceEntity.RepeatedCount > currentProcedureEntity.Cycle)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16047))
                            .WithData("SFC", sfcProduceEntity.SFC)
                            .WithData("Cycle", currentProcedureEntity.Cycle)
                            .WithData("RepeatedCount", sfcProduceEntity.RepeatedCount);
                    }
                }

                // 条码状态（当前状态）
                var currentStatus = sfcProduceEntity.Status;

                // 每次进站都将复投次数+1
                sfcProduceEntity.RepeatedCount++;

                // 更新状态，将条码由"排队"改为"活动"
                sfcProduceEntity.Status = SfcStatusEnum.Activity;
                sfcProduceEntity.ProcedureId = commonBo.ProcedureId;
                sfcProduceEntity.ResourceId = commonBo.ResourceId;
                sfcProduceEntity.UpdatedBy = commonBo.UserName;
                sfcProduceEntity.UpdatedOn = commonBo.Time;
                responseBo.SFCProduceEntitiy = sfcProduceEntity;

                // 初始化步骤
                responseBo.SFCStepEntity = new ManuSfcStepEntity
                {
                    Operatetype = ManuSfcStepTypeEnum.InStock,  // 状态为 进站
                    CurrentStatus = currentStatus,
                    AfterOperationStatus = sfcProduceEntity.Status,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                    Qty = sfcProduceEntity.Qty,
                    VehicleCode = requestBo.VehicleCode,
                    ProcedureId = commonBo.ProcedureId,
                    ResourceId = commonBo.ResourceId,
                    EquipmentId = commonBo.EquipmentId,
                    SiteId = commonBo.SiteId,
                    CreatedBy = commonBo.UserName,
                    CreatedOn = commonBo.Time,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = commonBo.Time
                };

                // 更新条码表
                responseBo.InStationManuSfcByIdCommand = new InStationManuSfcByIdCommand
                {
                    Id = sfcProduceEntity.SFCId,
                    Status = SfcStatusEnum.Activity,
                    IsUsed = YesOrNoEnum.Yes,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = commonBo.Time
                };

                // 更新实体（带状态检查）
                responseBo.UpdateProduceInStationSFCCommand = new UpdateProduceInStationSFCCommand
                {
                    Id = sfcProduceEntity.Id,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    Status = sfcProduceEntity.Status,
                    CurrentStatus = currentStatus,
                    RepeatedCount = sfcProduceEntity.RepeatedCount,
                    UpdatedBy = sfcProduceEntity.UpdatedBy,
                    UpdatedOn = sfcProduceEntity.UpdatedOn
                };

                responseSummaryBo.Code = requestBo.SFC;
                if (commonBo.Type == ManuFacePlateBarcodeTypeEnum.Vehicle) responseSummaryBo.Code = requestBo.VehicleCode ?? "";

                responseBos.Add(responseBo);
            }

            // 归集每个条码的结果
            if (!responseBos.Any()) return responseSummaryBo;
            responseSummaryBo.SFCProduceEntities = responseBos.Select(s => s.SFCProduceEntitiy);
            responseSummaryBo.SFCStepEntities = responseBos.Select(s => s.SFCStepEntity);
            responseSummaryBo.InStationManuSfcByIdCommands = responseBos.Select(s => s.InStationManuSfcByIdCommand);
            responseSummaryBo.UpdateProduceInStationSFCCommands = responseBos.Select(s => s.UpdateProduceInStationSFCCommand);

            responseSummaryBo.Source = commonBo.Source;
            responseSummaryBo.Type = commonBo.Type;
            responseSummaryBo.Count = commonBo.Type == ManuFacePlateBarcodeTypeEnum.Vehicle ? commonBo.InStationRequestBos.Select(s => s.VehicleCode).Distinct().Count() : responseBos.Count;
            responseSummaryBo.ProcedureCode = currentProcedureEntity.Code;
            responseSummaryBo.Status = SfcStatusEnum.Activity;
            return responseSummaryBo;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not InStationResponseSummaryBo data) return responseBo;
            if (data.SFCProduceEntities == null || !data.SFCProduceEntities.Any()) return responseBo;

            // 更改状态（在制品），如果状态一致，这里会直接返回0
            responseBo.Rows += await _manuSfcProduceRepository.UpdateProduceInStationSFCAsync(data.UpdateProduceInStationSFCCommands);

            // 未更新到数据，事务回滚
            if (responseBo.Rows != data.UpdateProduceInStationSFCCommands.Count())
            {
                // 这里在外层会回滚事务
                responseBo.IsSuccess = false;
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18216), string.Join(',', data.SFCProduceEntities.Select(s => s.SFC)));
                return responseBo;
            }

            // 更新数据
            List<Task<int>> tasks = new()
            {
                // 更新条码表 状态为排队
                 _manuSfcRepository.InStationManuSfcByIdAsync(data.InStationManuSfcByIdCommands),

                // 插入 manu_sfc_step 状态为 进站
                _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities)
            };

            // 等待所有任务完成
            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            // 后面的代码是面板业务
            if (data.Source != RequestSourceEnum.Panel) return responseBo;

            // 面板需要的数据
            List<PanelModuleEnum> panelModules = new();
            responseBo.Content = new Dictionary<string, string> { { "PanelModules", panelModules.ToSerialize() } };

            // 面板需要的提示信息
            if (data.Count == 1)
            {
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18224),
                    data.Type.GetDescription(),
                    data.Code,
                    data.ProcedureCode,
                    data.Status.GetDescription());
            }
            else if (data.Count > 1)
            {
                responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18225),
                    data.Count,
                    data.Type.GetDescription(),
                    data.ProcedureCode,
                    data.Status.GetDescription());
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
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = commonBo.ProcedureId,
                ResourceId = commonBo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.AfterStart
            });
        }

    }
}
