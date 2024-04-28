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
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
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
        /// 多语言服务
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
        /// <param name="localizationService"></param>
        public InStationJobService(ILogger<InStationJobService> logger,
            IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            ILocalizationService localizationService)
        {
            _logger = logger;
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
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
            await _manuCommonService.VerifyProcedureAsync(commonBo);
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

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new JobRelationBo
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
            var currentProcedureEntity = await _masterDataService.GetProcedureEntityByIdAsync(commonBo.ProcedureId)
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
                sfcProduceEntity.EquipmentId = commonBo.EquipmentId;
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
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    OperationProcedureId = commonBo.ProcedureId,
                    OperationResourceId = commonBo.ResourceId,
                    OperationEquipmentId = commonBo.EquipmentId,
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
                    EquipmentId = sfcProduceEntity.EquipmentId,
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
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
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

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new JobRelationBo
            {
                ProcedureId = commonBo.ProcedureId,
                ResourceId = commonBo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.AfterStart
            });
        }

    }
}
