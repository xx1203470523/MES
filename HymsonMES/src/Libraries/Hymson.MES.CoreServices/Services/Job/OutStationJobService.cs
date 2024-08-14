using FluentValidation;
using FluentValidation.Results;
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
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuSfcMarking;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 出站
    /// </summary>
    [Job("出站", JobTypeEnum.Standard)]
    public class OutStationJobService : IJobService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<OutStationJobService> _logger;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（降级品继承）
        /// </summary>
        private readonly IManuDegradedProductExtendService _manuDegradedProductExtendService;

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
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（产品NG记录表）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        /// 仓储接口（降级录入）
        /// </summary>
        private readonly IManuDowngradingRepository _manuDowngradingRepository;

        /// <summary>
        /// 仓储接口（降级品录入记录）
        /// </summary>
        private readonly IManuDowngradingRecordRepository _manuDowngradingRecordRepository;

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
        /// Marking信息表仓储
        /// </summary>
        private readonly IManuSfcMarkingRepository _manuSfcMarkingRepository;

        /// <summary>
        /// Marking执行表仓储
        /// </summary>
        private readonly IManuSfcMarkingExecuteRepository _manuSfcMarkingExecuteRepository;

        /// <summary>
        /// Marking继承
        /// </summary>
        private readonly IManuSfcMarkingCoreService _manuSfcMarkingCoreService;

        /// <summary>
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 统计服务
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OutStationJobService(ILogger<OutStationJobService> logger,
            IMasterDataService masterDataService,
            IManuDegradedProductExtendService manuDegradedProductExtendService,
            IManuFeedingRepository manuFeedingRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuDowngradingRepository manuDowngradingRepository,
            IManuDowngradingRecordRepository manuDowngradingRecordRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuProductNgRecordRepository manuProductNgRecordRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IManuSfcMarkingRepository manuSfcMarkingRepository,
            IManuSfcMarkingExecuteRepository manuSfcMarkingExecuteRepository,
            IManuSfcMarkingCoreService manuSfcMarkingCoreService,
            ILocalizationService localizationService,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _logger = logger;
            _masterDataService = masterDataService;
            _manuDegradedProductExtendService = manuDegradedProductExtendService;
            _manuFeedingRepository = manuFeedingRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuDowngradingRepository = manuDowngradingRepository;
            _manuDowngradingRecordRepository = manuDowngradingRecordRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _manuSfcMarkingRepository = manuSfcMarkingRepository;
            _manuSfcMarkingExecuteRepository = manuSfcMarkingExecuteRepository;
            _manuSfcMarkingCoreService = manuSfcMarkingCoreService;
            _localizationService = localizationService;
            _planWorkOrderRepository = planWorkOrderRepository;
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
            if (commonBo.OutStationRequestBos == null || !commonBo.OutStationRequestBos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16370));
            }

            // 判断条码是否为空
            if (commonBo.OutStationRequestBos.Any(a => a.SFC == null))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16382));
            }

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            // 判断条码锁状态
            await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, multiSFCBo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.Activity, _localizationService)
                              .VerifyResource(commonBo.ResourceId);

            // 条码对应工序是否和出站工序一致
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCProcedureEntities = sfcProduceEntities.Where(w => w.ProcedureId != commonBo.ProcedureId);
            if (noMatchSFCProcedureEntities.Any())
            {
                foreach (var sfcProduceEntity in noMatchSFCProcedureEntities)
                {
                    var inProcedureEntity = await _masterDataService.GetProcedureEntityByIdAsync(sfcProduceEntity.ProcedureId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES16358)).WithData("Procedure", sfcProduceEntity.ProcedureId);

                    var outProcedureEntity = await _masterDataService.GetProcedureEntityByIdAsync(commonBo.ProcedureId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES16358)).WithData("Procedure", commonBo.ProcedureId);

                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcProduceEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("SFC", sfcProduceEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("InProcedure", inProcedureEntity.Code);
                    validationFailure.FormattedMessagePlaceholderValues.Add("OutProcedure", outProcedureEntity.Code);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16359);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any())
                {
                    throw new ValidationException("", validationFailures);
                }
            }

            // 获取生产工单（附带工单状态校验）
            _ = await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceWorkOrderByIdsAsync, new WorkOrderIdsBo
            {
                WorkOrderIds = sfcProduceEntities.Select(s => s.WorkOrderId)
            });

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
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;
            if (commonBo.OutStationRequestBos == null || !commonBo.OutStationRequestBos.Any()) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
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

            // 条码信息
            //var manuSFCEntities = await _manuSfcRepository.GetByIdsAsync(sfcProduceEntities.Select(s => s.SFCId));
            var manuSFCEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_manuSfcRepository.GetListAsync,
                new ManuSfcQuery
                {
                    Ids = sfcProduceEntities.Select(s => s.SFCId)
                }
              );
            if (manuSFCEntities == null || !manuSFCEntities.Any()) return default;

            // 全部物料加载数据
            List<ManuFeedingEntity> allFeedingEntities = new();

            // 批量读取物料加载数据（需要实时数据，勿缓存）
            var resourceFeeds = await commonBo.Proxy.GetValueAsync(_manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync, new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = commonBo.ResourceId
            });
            if (resourceFeeds != null) allFeedingEntities.AddRange(resourceFeeds);

            // 通过资源 -> 上料点
            var loadPoints = await _masterDataService.GetLoadPointLinkEntitiesByResourceIdAsync(commonBo.ResourceId);
            var pointFeeds = await _manuFeedingRepository.GetByFeedingPointIdWithOutZeroAsync(new GetByFeedingPointIdsQuery
            {
                FeedingPointIds = loadPoints.Select(s => s.LoadPointId)
            });
            if (pointFeeds != null) allFeedingEntities = allFeedingEntities.UnionBy(pointFeeds, s => s.Id).ToList();

            // 查询工序信息
            var procProcedureEntity = await _masterDataService.GetProcedureEntityByIdAsync(commonBo.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17114)).WithData("Procedure", commonBo.ProcedureId);

            // 复投所需参数对象
            var procedureRejudgeBo = new ProcedureRejudgeBo
            {
                SiteId = commonBo.SiteId,
                ProcedureId = procProcedureEntity.Id,
                ProcedureCode = procProcedureEntity.Code,
                Cycle = procProcedureEntity.Cycle ?? 1,
                Type = procProcedureEntity.Type,
                IsRejudge = procProcedureEntity.IsRejudge ?? TrueOrFalseEnum.No,
                IsValidNGCode = procProcedureEntity.IsValidNGCode ?? TrueOrFalseEnum.No
            };

            // 填充其他设置
            procedureRejudgeBo = await FillingProcedureRejudgeBoAsync(procedureRejudgeBo, commonBo.OutStationRequestBos);
            //_logger.LogInformation($"工序中关于复判的相关参数 -> {procedureRejudgeBo.ToSerialize()}");

            // 遍历所有条码
            var responseBos = new List<OutStationResponseBo>();
            var responseSummaryBo = new OutStationResponseSummaryBo();
            foreach (var requestBo in commonBo.OutStationRequestBos)
            {
                var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(s => s.SFC == requestBo.SFC)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", requestBo.SFC);

                var manuSfcEntity = manuSFCEntities.FirstOrDefault(s => s.Id == sfcProduceEntity.SFCId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17102)).WithData("SFC", requestBo.SFC);

                var workOrderEntity = await _masterDataService.GetWorkOrderEntityByIdAsync(sfcProduceEntity.WorkOrderId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16373)).WithData("SFC", requestBo.SFC);

                // 单条码返回值
                var responseBo = new OutStationResponseBo();

                // 2024.03.19 克明大佬说出站时需要清除设备ID值
                sfcProduceEntity.EquipmentId = null;

                // 是否有传是否合格标识
                if (requestBo.IsQualified.HasValue && !requestBo.IsQualified.Value)
                {
                    // 是否有设置"标记代码"（顷刻是未设置缺陷代码）
                    if (procedureRejudgeBo.MarkUnqualifiedId.HasValue)
                    {
                        // 不合格出站（平台）
                        responseBo = await OutStationForUnQualifiedProcedureAsync(commonBo, requestBo, manuSfcEntity, sfcProduceEntity, procedureRejudgeBo);
                    }
                    else
                    {
                        // 不合格出站（顷刻）
                        responseBo = await OutStationForUnQualifiedProcedureWithoutMarkAsync(commonBo, requestBo, manuSfcEntity, sfcProduceEntity, procedureRejudgeBo);
                    }
                }
                else
                {
                    // 合格出站（为了逻辑清晰，跟上面的不合格出站区分开）
                    responseBo = await OutStationForQualifiedProcedureAsync(commonBo, requestBo, manuSfcEntity, sfcProduceEntity);
                }

                // 保存单个条码的出站结果
                if (responseBo == null) continue;

                #region 物料消耗明细数据
                // 判断是否半成品
                var isSmiFinished = sfcProduceEntity.ProductId != workOrderEntity.ProductId;

                // 组合物料数据（放缓存）
                var initialMaterialSummary = await commonBo.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsWithSmiFinishedAsync, new MaterialDeductRequestBo
                {
                    SiteId = commonBo.SiteId,
                    ProcedureId = commonBo.ProcedureId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProductId = isSmiFinished ? sfcProduceEntity.ProductId : 0
                });
                if (initialMaterialSummary == null) continue;

                MaterialConsumptionBo consumptionBo = new();
                // 指定消耗物料
                if (requestBo.ConsumeList != null && requestBo.ConsumeList.Any())
                {
                    consumptionBo = ExecutenMaterialConsumptionWithBarCode(ref allFeedingEntities, initialMaterialSummary, requestBo, sfcProduceEntity);
                }
                // 默认消耗逻辑
                else
                {
                    consumptionBo = ExecutenMaterialConsumptionWithBOM(ref allFeedingEntities, initialMaterialSummary, sfcProduceEntity);
                }
                responseBo.UpdateFeedingQtyByIdCommands = consumptionBo.UpdateFeedingQtyByIdCommands;
                responseBo.ManuSfcCirculationEntities = consumptionBo.ManuSfcCirculationEntities;
                #endregion

                //投入条码
                var inputSfcs = new List<string>();
                if (responseBo.ManuSfcCirculationEntities != null && responseBo.ManuSfcCirculationEntities.Any())
                {
                    inputSfcs.AddRange(responseBo.ManuSfcCirculationEntities.Select(x => x.CirculationBarCode));
                }
                if (requestBo.BindSfcs != null && requestBo.BindSfcs.Any())
                {
                    inputSfcs.AddRange(requestBo.BindSfcs);
                }

                #region 降级品继承
                if (responseBo.ProcessRouteType == ProcessRouteTypeEnum.ProductionRoute && inputSfcs.Any())
                {
                    var degradedProductExtendBo = new DegradedProductExtendBo
                    {
                        SiteId = commonBo.SiteId,
                        UserName = commonBo.UserName,
                        KeyValues = inputSfcs.Distinct().Select(item => new DegradedProductExtendKeyValueBo
                        {
                            BarCode = item,
                            SFC = sfcProduceEntity.SFC
                        }).ToList()
                    };

                    // 取得降级品记录
                    var downgradingEntities = await _manuDegradedProductExtendService.GetManuDownGradingsAsync(degradedProductExtendBo);
                    var (manuDowngradingEntities, manuDowngradingRecordEntities) = await _manuDegradedProductExtendService.GetManuDowngradingsByConsumesAsync(degradedProductExtendBo, downgradingEntities);

                    responseBo.DowngradingEntities = manuDowngradingEntities;
                    responseBo.DowngradingRecordEntities = manuDowngradingRecordEntities;
                }
                #endregion

                #region Marking继承

                if (inputSfcs.Any())
                {
                    var (markingEntities, markingExecuteEntities) = await _manuSfcMarkingCoreService.GetMarkingInheritEntityAsync(new ManuSfcMarkingBo
                    {
                        SiteId = commonBo.SiteId,
                        UserName = commonBo.UserName,
                        SFC = sfcProduceEntity.SFC,
                        ConsumeSFCs = inputSfcs.Distinct(),
                    });

                    responseBo.MarkingEntities = markingEntities;
                    responseBo.MarkingExecuteEntities = markingExecuteEntities;
                }

                #endregion

                responseSummaryBo.Code = requestBo.SFC;
                if (commonBo.Type == ManuFacePlateBarcodeTypeEnum.Vehicle) responseSummaryBo.Code = requestBo.VehicleCode ?? "";

                responseSummaryBo.IsLastProcedure = responseBo.IsLastProcedure;
                responseSummaryBo.ProcedureCode = responseBo.NextProcedureCode;
                responseSummaryBo.Status = responseBo.SFCEntity.Status;

                responseBos.Add(responseBo);
            }

            // 归集每个条码的结果
            if (!responseBos.Any()) return responseSummaryBo;
            responseSummaryBo.SFCEntities = responseBos.Select(s => s.SFCEntity);
            responseSummaryBo.SFCProduceEntities = responseBos.Select(s => s.SFCProduceEntitiy);
            responseSummaryBo.SFCStepEntities = responseBos.Select(s => s.SFCStepEntity);
            responseSummaryBo.WhMaterialInventoryEntities = responseBos.Where(w => w.MaterialInventoryEntity != null).Select(s => s.MaterialInventoryEntity);
            responseSummaryBo.WhMaterialStandingbookEntities = responseBos.Where(w => w.MaterialStandingbookEntity != null).Select(s => s.MaterialStandingbookEntity);
            responseSummaryBo.UpdateFeedingQtyByIdCommands = responseBos.SelectMany(s => s.UpdateFeedingQtyByIdCommands);
            responseSummaryBo.ManuSfcCirculationEntities = responseBos.SelectMany(s => s.ManuSfcCirculationEntities);
            responseSummaryBo.DowngradingEntities = responseBos.Where(w => w.DowngradingEntities != null).SelectMany(s => s.DowngradingEntities);
            responseSummaryBo.DowngradingRecordEntities = responseBos.Where(w => w.DowngradingRecordEntities != null).SelectMany(s => s.DowngradingRecordEntities);
            responseSummaryBo.ProductBadRecordEntities = responseBos.Where(w => w.ProductBadRecordEntities != null).SelectMany(s => s.ProductBadRecordEntities);
            responseSummaryBo.ProductNgRecordEntities = responseBos.Where(w => w.ProductNgRecordEntities != null).SelectMany(s => s.ProductNgRecordEntities);
            responseSummaryBo.SFCProduceBusinessEntities = responseBos.Where(w => w.SFCProduceBusinessEntity != null).Select(s => s.SFCProduceBusinessEntity);
            responseSummaryBo.MarkingEntities = responseBos.Where(w => w.MarkingEntities != null).SelectMany(s => s.MarkingEntities);
            responseSummaryBo.MarkingExecuteEntities = responseBos.Where(w => w.MarkingExecuteEntities != null).SelectMany(s => s.MarkingExecuteEntities);

            // 删除 manu_sfc_produce
            responseSummaryBo.DeletePhysicalByProduceIdsCommand = new PhysicalDeleteSFCProduceByIdsCommand
            {
                SiteId = commonBo.SiteId,
                Ids = responseBos.Where(w => w.IsLastProcedure).Select(s => s.SFCProduceEntitiy.Id)
            };

            // 删除 manu_sfc_produce_business
            responseSummaryBo.DeleteSfcProduceBusinesssBySfcInfoIdsCommand = new DeleteSFCProduceBusinesssByIdsCommand
            {
                SiteId = commonBo.SiteId,
                SfcInfoIds = responseBos.Where(w => w.IsLastProcedure).Select(s => s.SFCProduceEntitiy.Id)
            };

            responseSummaryBo.Source = commonBo.Source;
            responseSummaryBo.Type = commonBo.Type;
            responseSummaryBo.Count = commonBo.Type == ManuFacePlateBarcodeTypeEnum.Vehicle ? commonBo.OutStationRequestBos.Select(s => s.VehicleCode).Distinct().Count() : responseBos.Count;
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
            if (obj is not OutStationResponseSummaryBo data) return responseBo;

            // 更新物料库存
            if (data.UpdateFeedingQtyByIdCommands != null && data.UpdateFeedingQtyByIdCommands.Any())
            {
                responseBo.Rows += await _manuFeedingRepository.UpdateFeedingQtyByIdAsync(data.UpdateFeedingQtyByIdCommands);

                // 未更新到全部需更新的数据，事务回滚
                if (data.UpdateFeedingQtyByIdCommands.Count() > responseBo.Rows)
                {
                    //_logger.LogError($"MES18218 -> Rows: {responseBo.Rows}");
                    //_logger.LogError($"MES18218 -> Command: {data.UpdateFeedingQtyByIdCommands.ToSerialize()}");

                    responseBo.IsSuccess = false;
                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18218), string.Join(',', data.SFCProduceEntities!.Select(s => s.SFC)));
                    return responseBo;
                }
            }

            // 更新数据
            List<Task<int>> tasks = new()
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                _manuSfcProduceRepository.UpdateRangeWithStatusCheckAsync(data.SFCProduceEntities),

                // 删除 manu_sfc_produce
                _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(data.DeletePhysicalByProduceIdsCommand),

                // 删除 manu_sfc_produce_business
                _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdsAsync(data.DeleteSfcProduceBusinesssBySfcInfoIdsCommand),

                // 入库 / 台账
                _whMaterialInventoryRepository.InsertsAsync(data.WhMaterialInventoryEntities),
                _whMaterialStandingbookRepository.InsertsAsync(data.WhMaterialStandingbookEntities),

                // 降级品记录
                _manuDowngradingRepository.InsertsAsync(data.DowngradingEntities),
                _manuDowngradingRecordRepository.InsertsAsync(data.DowngradingRecordEntities),

                // manu_sfc 更新状态
                _manuSfcRepository.UpdateRangeWithStatusCheckAsync(data.SFCEntities),

                // 添加流转记录
                _manuSfcCirculationRepository.InsertRangeAsync(data.ManuSfcCirculationEntities),

                // 插入 manu_sfc_step
                _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities),

                // 插入不良记录
                _manuProductBadRecordRepository.InsertRangeAsync(data.ProductBadRecordEntities),

                // 插入NG记录
                _manuProductNgRecordRepository.InsertRangeAsync(data.ProductNgRecordEntities),
                
                // 添加在制维修业务
                _manuSfcProduceRepository.InsertSfcProduceBusinessRangeAsync(data.SFCProduceBusinessEntities),

                //Marking继承
                _manuSfcMarkingRepository.InsertRangeAsync(data.MarkingEntities),
                _manuSfcMarkingExecuteRepository.InsertRangeAsync(data.MarkingExecuteEntities)
            };

            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            // 后面的代码是面板业务
            if (data.Source != RequestSourceEnum.Panel) return responseBo;

            // 面板需要的数据
            List<PanelModuleEnum> panelModules = new();
            responseBo.Content = new Dictionary<string, string> {
                        { "PanelModules", panelModules.ToSerialize() },
                        { "Qty", $"{data.Count}" },
                        { "IsLastProcedure", $"{data.IsLastProcedure}" },
                        { "NextProcedureCode", $"{data.ProcedureCode}" }
                    };

            var WorkOrderId = data.SFCProduceEntities;

            // 面板需要的提示信息
            if (data.IsLastProcedure)
            {
                if (data.Count == 1)
                {
                    var SFCProduceEntity = data.SFCProduceEntities!.FirstOrDefault();
                    if (SFCProduceEntity != null)
                    {
                        responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18226),
                        data.Type.GetDescription(),
                        data.Code);
                    }
                }
                else if (data.Count > 1)
                {
                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18227),
                        data.Count,
                        data.Type.GetDescription());
                }
            }
            else
            {
                if (data.Count == 1)
                {
                    var SFCProduceEntity = data.SFCProduceEntities!.FirstOrDefault();
                    if (SFCProduceEntity != null) responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18224),
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
                LinkPoint = ResourceJobLinkPointEnum.AfterFinish
            });
        }


        #region 内部方法（仅仅是为了逻辑清晰）
        /// <summary>
        /// 合格工序出站
        /// </summary>
        /// <param name="commonBo"></param>
        /// <param name="requestBo"></param>
        /// <param name="manuSfcEntity"></param>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        private async Task<OutStationResponseBo?> OutStationForQualifiedProcedureAsync(JobRequestBo commonBo, OutStationRequestBo requestBo, ManuSfcEntity manuSfcEntity, ManuSfcProduceEntity sfcProduceEntity)
        {
            if (commonBo == null) return default;
            if (commonBo.Proxy == null) return default;

            // 待执行的命令
            OutStationResponseBo responseBo = new();

            if (sfcProduceEntity == null) return default;

            // 读取产品基础信息
            var procMaterialEntityTask = _masterDataService.GetProcMaterialEntityWithNullCheckAsync(sfcProduceEntity.ProductId);

            // 读取当前工艺路线信息
            var procProcessRouteEntityTask = _masterDataService.GetProcProcessRouteEntityWithNullCheckAsync(sfcProduceEntity.ProcessRouteId);

            var procMaterialEntity = await procMaterialEntityTask;
            var procProcessRouteEntity = await procProcessRouteEntityTask;

            // 工艺路线类型
            responseBo.ProcessRouteType = procProcessRouteEntity.Type;

            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await commonBo.Proxy.GetValueAsync(_masterDataService.GetNextProcedureAsync, new ManuRouteProcedureWithWorkOrderBo
            {
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                ProcedureId = commonBo.ProcedureId,
            });

            // 条码状态（当前状态）
            var currentStatus = sfcProduceEntity.Status;

            // 初始化步骤
            var stepEntity = new ManuSfcStepEntity
            {
                // 插入 manu_sfc_step 状态为出站（默认值）
                Operatetype = ManuSfcStepTypeEnum.OutStock,
                CurrentStatus = currentStatus,
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

            // 更新条码信息
            manuSfcEntity.UpdatedBy = commonBo.UserName;
            manuSfcEntity.UpdatedOn = commonBo.Time;

            // 更新在制条码信息
            sfcProduceEntity.UpdatedBy = commonBo.UserName;
            sfcProduceEntity.UpdatedOn = commonBo.Time;

            // 已完工（ 如果没有尾工序，就表示已完工）
            if (nextProcedure == null)
            {
                responseBo.IsLastProcedure = true;

                // 条码状态为"完成"
                manuSfcEntity.Status = SfcStatusEnum.Complete;
                sfcProduceEntity.Status = SfcStatusEnum.Complete;

                stepEntity.Operatetype = responseBo.ProcessRouteType == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？

                // 生产主工艺路线才进行入库
                if (responseBo.ProcessRouteType == ProcessRouteTypeEnum.ProductionRoute)
                {
                    // 新增 wh_material_inventory
                    responseBo.MaterialInventoryEntity = new WhMaterialInventoryEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SupplierId = 0,//自制品 没有
                        MaterialId = sfcProduceEntity.ProductId,
                        MaterialBarCode = sfcProduceEntity.SFC,
                        Batch = "",//自制品 没有
                        MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                        QuantityResidue = sfcProduceEntity.Qty,
                        ScrapQty = sfcProduceEntity.ScrapQty,
                        Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                        Source = MaterialInventorySourceEnum.ManuComplete,
                        SiteId = commonBo.SiteId,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    };

                    // 新增 wh_material_standingbook
                    responseBo.MaterialStandingbookEntity = new WhMaterialStandingbookEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialCode = procMaterialEntity.MaterialCode,
                        MaterialName = procMaterialEntity.MaterialName,
                        MaterialVersion = procMaterialEntity.Version ?? "",
                        MaterialBarCode = sfcProduceEntity.SFC,
                        Batch = "",//自制品 没有
                        Quantity = sfcProduceEntity.Qty,
                        Unit = procMaterialEntity.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.ManuComplete,
                        Source = MaterialInventorySourceEnum.ManuComplete,
                        SiteId = commonBo.SiteId,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    };
                }
            }
            // 未完工（下一工序排队）
            else
            {
                responseBo.NextProcedureCode = nextProcedure.Code;

                // 条码状态跟在制品状态一致
                manuSfcEntity.Status = SfcStatusEnum.lineUp;
                sfcProduceEntity.Status = SfcStatusEnum.lineUp;

                // 更新下一工序
                sfcProduceEntity.ProcedureId = nextProcedure.Id;

                // 一旦切换工序，复投次数重置
                sfcProduceEntity.RepeatedCount = 0;

                // 不置空的话，进站时，可能校验不通过
                sfcProduceEntity.ResourceId = null;
            }

            // 保存操作后的状态
            stepEntity.AfterOperationStatus = sfcProduceEntity.Status;

            // 更新信息
            responseBo.SFCEntity = manuSfcEntity;
            responseBo.SFCStepEntity = stepEntity;
            responseBo.SFCProduceEntitiy = sfcProduceEntity;

            return responseBo;
        }

        /// <summary>
        /// 不合格工序出站（标记复判NG版）
        /// </summary>
        /// <param name="commonBo"></param>
        /// <param name="requestBo"></param>
        /// <param name="manuSfcEntity"></param>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="procedureRejudgeBo"></param>
        /// <returns></returns>
        private async Task<OutStationResponseBo?> OutStationForUnQualifiedProcedureAsync(JobRequestBo commonBo, OutStationRequestBo requestBo, ManuSfcEntity manuSfcEntity, ManuSfcProduceEntity sfcProduceEntity, ProcedureRejudgeBo procedureRejudgeBo)
        {
            if (commonBo == null) return default;
            if (commonBo.Proxy == null) return default;

            // 只有【测试】类型工序才允许不合格出站，请检查【工序维护】！
            if (procedureRejudgeBo.Type != ProcedureTypeEnum.Test)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17118))
                    .WithData("Procedure", procedureRejudgeBo.ProcedureCode)
                    .WithData("Type", procedureRejudgeBo.Type.GetDescription());
            }

            // 检查不合格代码信息是否为空
            if (requestBo.OutStationUnqualifiedList == null || !requestBo.OutStationUnqualifiedList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17109))
                    .WithData("SFC", requestBo.SFC);
            }

            // 当前条码的不合格代码（未去重）
            var unqualifiedCodesWithoutDistinct = requestBo.OutStationUnqualifiedList!.Select(s => s.UnqualifiedCode);

            // 如果有传不合格代码，进行校验是否为空字符
            if (unqualifiedCodesWithoutDistinct.Any(a => string.IsNullOrWhiteSpace(a)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17117))
                    .WithData("SFC", requestBo.SFC);
            }

            // 校验是否有重复
            var unqualifiedCodes = unqualifiedCodesWithoutDistinct.Distinct();
            if (unqualifiedCodesWithoutDistinct.Count() != unqualifiedCodes.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17119))
                    .WithData("SFC", requestBo.SFC);
            }

            // 检查不在系统中的不合格代码（如果有设置需要校验NG信息）
            var ngCodeNotInSystem = unqualifiedCodes.Except(procedureRejudgeBo.UnqualifiedCodeEntities.Select(s => s.UnqualifiedCode));
            if (ngCodeNotInSystem.Any() && procedureRejudgeBo.IsValidNGCode == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17110))
                    .WithData("SFC", requestBo.SFC)
                    .WithData("NGCode", string.Join(',', ngCodeNotInSystem));
            }

            // 待执行的命令
            OutStationResponseBo responseBo = new();

            // 读取产品基础信息
            var procMaterialEntityTask = _masterDataService.GetProcMaterialEntityWithNullCheckAsync(sfcProduceEntity.ProductId);

            // 读取当前工艺路线信息
            var procProcessRouteEntityTask = _masterDataService.GetProcProcessRouteEntityWithNullCheckAsync(sfcProduceEntity.ProcessRouteId);

            var procMaterialEntity = await procMaterialEntityTask;
            var procProcessRouteEntity = await procProcessRouteEntityTask;

            // 工艺路线类型
            responseBo.ProcessRouteType = procProcessRouteEntity.Type;

            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await commonBo.Proxy.GetValueAsync(_masterDataService.GetNextProcedureAsync, new ManuRouteProcedureWithWorkOrderBo
            {
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                ProcedureId = commonBo.ProcedureId,
            });

            // 条码状态（当前状态）
            var currentStatus = sfcProduceEntity.Status;

            // 初始化步骤
            var stepEntity = new ManuSfcStepEntity
            {
                // 插入 manu_sfc_step 状态为出站（默认值）
                Operatetype = ManuSfcStepTypeEnum.OutStock,
                CurrentStatus = currentStatus,
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

            // 更新条码信息
            manuSfcEntity.UpdatedBy = commonBo.UserName;
            manuSfcEntity.UpdatedOn = commonBo.Time;

            // 更新在制条码信息
            sfcProduceEntity.UpdatedBy = commonBo.UserName;
            sfcProduceEntity.UpdatedOn = commonBo.Time;

            var isMoreThanCycle = sfcProduceEntity.RepeatedCount >= procedureRejudgeBo.Cycle;
            var disposalResult = ProductBadDisposalResultEnum.AutoHandle;
            var productBadRecordStatus = ProductBadRecordStatusEnum.Close;

            // 是否（置于不合格工艺路线首工序排队）
            var isLinkToUnQualifiedProcessRouteQueuing = false;

            // 是否（非末工序置于下工序排队，末工序为完成）
            var isLinkToQualifiedProcessRouteQueuing = false;

            // 是否匹配首次不良
            var isMatchBlockCode = false;
            if (procedureRejudgeBo.BlockUnqualifiedIds != null && procedureRejudgeBo.BlockUnqualifiedIds.Any())
            {
                var blockUnqualifiedEntities = await _masterDataService.GetUnqualifiedEntitiesByIdsAsync(procedureRejudgeBo.BlockUnqualifiedIds);

                // 判断NGCode中是否含有首次不良工序
                var ngCodeInBlock = unqualifiedCodes.Intersect(blockUnqualifiedEntities.Select(s => s.UnqualifiedCode));
                if (ngCodeInBlock.Any()) isMatchBlockCode = true;
            }

            // 默认为标记编码
            var unqualifiedId = procedureRejudgeBo.MarkUnqualifiedId;

            #region 无需复判
            if (procedureRejudgeBo.IsRejudge == TrueOrFalseEnum.No)
            {
                #region 如果超过复投次数
                if (isMoreThanCycle)
                {
                    if (procedureRejudgeBo.LastUnqualified != null) unqualifiedId = procedureRejudgeBo.LastUnqualified.Id;
                    isLinkToUnQualifiedProcessRouteQueuing = true;
                }
                #endregion

                #region 未超过复投次数（当前工序排队）
                else
                {
                    // 条码状态跟在制品状态一致
                    manuSfcEntity.Status = SfcStatusEnum.lineUp;
                    sfcProduceEntity.Status = SfcStatusEnum.lineUp;

                    // 更新下一工序（默认情况，如果下面的含有首次不良工序命中，会更新该值）
                    sfcProduceEntity.ProcedureId = commonBo.ProcedureId;
                    sfcProduceEntity.ResourceId = commonBo.ResourceId;
                }
                #endregion

                // 匹配首次不良代码 
                if (isMatchBlockCode) isLinkToUnQualifiedProcessRouteQueuing = true;
            }
            #endregion

            #region 需要复判
            else
            {
                #region 如果超过复投次数
                if (isMoreThanCycle)
                {
                    if (procedureRejudgeBo.LastUnqualified != null) unqualifiedId = procedureRejudgeBo.LastUnqualified.Id;
                    isLinkToQualifiedProcessRouteQueuing = true;
                }
                #endregion

                #region 未超过复投次数（当前工序排队）
                else
                {
                    // 条码状态跟在制品状态一致
                    manuSfcEntity.Status = SfcStatusEnum.lineUp;
                    sfcProduceEntity.Status = SfcStatusEnum.lineUp;

                    // 更新下一工序（默认情况，如果下面的含有首次不良工序命中，会更新该值）
                    sfcProduceEntity.ProcedureId = commonBo.ProcedureId;
                    sfcProduceEntity.ResourceId = commonBo.ResourceId;
                }
                #endregion

                // 匹配首次不良代码 
                if (isMatchBlockCode) isLinkToQualifiedProcessRouteQueuing = true;
            }
            #endregion

            #region 置于不合格工艺路线排队（首工序）
            if (isLinkToUnQualifiedProcessRouteQueuing)
            {
                productBadRecordStatus = ProductBadRecordStatusEnum.Open;
                if (procedureRejudgeBo.LastUnqualified != null) unqualifiedId = procedureRejudgeBo.LastUnqualified.Id;

                // 添加维修业务
                disposalResult = ProductBadDisposalResultEnum.Repair;
                sfcProduceEntity.IsRepair = TrueOrFalseEnum.Yes;

                // 记录当前工序信息
                responseBo.SFCProduceBusinessEntity = new ManuSfcProduceBusinessEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    SfcProduceId = sfcProduceEntity.Id,
                    BusinessType = ManuSfcProduceBusinessType.Repair,
                    BusinessContent = new SfcProduceRepairBo
                    {
                        ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                        ProcedureId = commonBo.ProcedureId
                    }.ToSerialize(),
                    CreatedBy = commonBo.UserName,
                    CreatedOn = commonBo.Time,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = commonBo.Time
                };

                // 修改下工序
                responseBo.NextProcedureCode = procedureRejudgeBo.NextProcedureCode;

                // 条码状态跟在制品状态一致
                manuSfcEntity.Status = SfcStatusEnum.lineUp;
                sfcProduceEntity.Status = SfcStatusEnum.lineUp;

                // 更新不合格工艺路线
                if (procedureRejudgeBo.LastUnqualified != null && procedureRejudgeBo.LastUnqualified.ProcessRouteId.HasValue)
                {
                    sfcProduceEntity.ProcessRouteId = procedureRejudgeBo.LastUnqualified.ProcessRouteId.Value;
                }

                // 更新下一工序
                if (!procedureRejudgeBo.IsHasUnQualifiedProcessRoute) throw new CustomerValidationException(nameof(ErrorCode.MES17115)).WithData("Procedure", procedureRejudgeBo.ProcedureCode);
                sfcProduceEntity.ProcedureId = procedureRejudgeBo.NextProcedureId;

                // 一旦切换工序，复投次数重置
                sfcProduceEntity.RepeatedCount = 0;

                // 不置空的话，进站时，可能校验不通过
                sfcProduceEntity.ResourceId = null;
            }
            #endregion

            #region 置于合格工艺路线排队（非末工序置于下工序排队，末工序为完成）
            if (isLinkToQualifiedProcessRouteQueuing)
            {
                productBadRecordStatus = ProductBadRecordStatusEnum.Open;
                if (procedureRejudgeBo.LastUnqualified != null) unqualifiedId = procedureRejudgeBo.LastUnqualified.Id;

                // 已完工（置于在制完成）
                if (nextProcedure == null)
                {
                    //responseBo.IsLastProcedure = true;

                    // 清空复投次数
                    sfcProduceEntity.RepeatedCount = 0;

                    // 标记条码为"在制-完成"
                    //responseBo.IsCompleted = true;
                    manuSfcEntity.Status = SfcStatusEnum.InProductionComplete;
                    sfcProduceEntity.Status = SfcStatusEnum.InProductionComplete;
                }
                // 未完工（置于下工序排队）
                else
                {
                    disposalResult = ProductBadDisposalResultEnum.WaitingJudge;
                    responseBo.NextProcedureCode = nextProcedure.Code;

                    // 条码状态跟在制品状态一致
                    manuSfcEntity.Status = SfcStatusEnum.lineUp;
                    sfcProduceEntity.Status = SfcStatusEnum.lineUp;

                    // 更新下一工序
                    sfcProduceEntity.ProcedureId = nextProcedure.Id;

                    // 一旦切换工序，复投次数重置
                    sfcProduceEntity.RepeatedCount = 0;

                    // 不置空的话，进站时，可能校验不通过
                    sfcProduceEntity.ResourceId = null;
                }
            }
            #endregion

            // 保存操作后的状态
            stepEntity.AfterOperationStatus = sfcProduceEntity.Status;

            // 更新信息
            responseBo.SFCEntity = manuSfcEntity;
            responseBo.SFCStepEntity = stepEntity;
            responseBo.SFCProduceEntitiy = sfcProduceEntity;

            // 如果有标记缺陷
            if (unqualifiedId.HasValue)
            {
                // 记录下复判相关参数
                var remark = new StringBuilder();
                remark.Append($"Cycle:{procedureRejudgeBo.Cycle};");
                remark.Append($"IsRejudge: {procedureRejudgeBo.IsRejudge};");
                remark.Append($"IsValidNGCode:{procedureRejudgeBo.IsValidNGCode};");
                remark.Append($"MarkUnqualifiedId:{procedureRejudgeBo.MarkUnqualifiedId};");

                // 添加不良记录
                var badRecordEntity = new ManuProductBadRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    FoundBadOperationId = commonBo.ProcedureId,
                    FoundBadResourceId = commonBo.ResourceId,
                    OutflowOperationId = commonBo.ProcedureId,
                    UnqualifiedId = unqualifiedId.Value,
                    SfcStepId = stepEntity.Id,
                    SFC = stepEntity.SFC,
                    SfcInfoId = stepEntity.SFCInfoId,
                    Qty = stepEntity.Qty,
                    Status = productBadRecordStatus,
                    Source = ProductBadRecordSourceEnum.EquipmentReBad,
                    Remark = remark.ToString(),
                    DisposalResult = disposalResult,
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                };
                responseBo.ProductBadRecordEntities = new List<ManuProductBadRecordEntity> { badRecordEntity };

                // 添加NG记录
                responseBo.ProductNgRecordEntities = unqualifiedCodes.Select(s => new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    BadRecordId = badRecordEntity.Id,
                    UnqualifiedId = badRecordEntity.UnqualifiedId,
                    NGCode = s,
                    Remark = "",
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });
            }

            return responseBo;
        }

        /// <summary>
        /// 不合格工序出站（无标记复判NG版）
        /// </summary>
        /// <param name="commonBo"></param>
        /// <param name="requestBo"></param>
        /// <param name="manuSfcEntity"></param>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="procedureRejudgeBo"></param>
        /// <returns></returns>
        private async Task<OutStationResponseBo?> OutStationForUnQualifiedProcedureWithoutMarkAsync(JobRequestBo commonBo, OutStationRequestBo requestBo, ManuSfcEntity manuSfcEntity, ManuSfcProduceEntity sfcProduceEntity, ProcedureRejudgeBo procedureRejudgeBo)
        {
            if (commonBo == null) return default;
            if (commonBo.Proxy == null) return default;

            // 只有【测试】类型工序才允许不合格出站，请检查【工序维护】！
            if (procedureRejudgeBo.Type != ProcedureTypeEnum.Test)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17118))
                    .WithData("Procedure", procedureRejudgeBo.ProcedureCode)
                    .WithData("Type", procedureRejudgeBo.Type.GetDescription());
            }

            // 检查不合格代码信息是否为空
            if (requestBo.OutStationUnqualifiedList == null || !requestBo.OutStationUnqualifiedList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17109))
                    .WithData("SFC", requestBo.SFC);
            }

            // 当前条码的不合格代码（未去重）
            var unqualifiedCodesWithoutDistinct = requestBo.OutStationUnqualifiedList!.Select(s => s.UnqualifiedCode);

            // 如果有传不合格代码，进行校验是否为空字符
            if (unqualifiedCodesWithoutDistinct.Any(a => string.IsNullOrWhiteSpace(a)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17117))
                    .WithData("SFC", requestBo.SFC);
            }

            // 校验是否有重复
            var unqualifiedCodes = unqualifiedCodesWithoutDistinct.Distinct();
            if (unqualifiedCodesWithoutDistinct.Count() != unqualifiedCodes.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17119))
                    .WithData("SFC", requestBo.SFC);
            }

            // 检查不在系统中的不合格代码（如果有设置需要校验NG信息）
            var ngCodeNotInSystem = unqualifiedCodes.Except(procedureRejudgeBo.UnqualifiedCodeEntities.Select(s => s.UnqualifiedCode));
            if (ngCodeNotInSystem.Any() && procedureRejudgeBo.IsValidNGCode == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17110))
                    .WithData("SFC", requestBo.SFC)
                    .WithData("NGCode", string.Join(',', ngCodeNotInSystem));
            }

            // 待执行的命令
            OutStationResponseBo responseBo = new();

            // 读取产品基础信息
            var procMaterialEntityTask = _masterDataService.GetProcMaterialEntityWithNullCheckAsync(sfcProduceEntity.ProductId);

            // 读取当前工艺路线信息
            var procProcessRouteEntityTask = _masterDataService.GetProcProcessRouteEntityWithNullCheckAsync(sfcProduceEntity.ProcessRouteId);

            var procMaterialEntity = await procMaterialEntityTask;
            var procProcessRouteEntity = await procProcessRouteEntityTask;

            // 工艺路线类型
            responseBo.ProcessRouteType = procProcessRouteEntity.Type;

            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await commonBo.Proxy.GetValueAsync(_masterDataService.GetNextProcedureAsync, new ManuRouteProcedureWithWorkOrderBo
            {
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                ProcedureId = commonBo.ProcedureId,
            });

            // 条码状态（当前状态）
            var currentStatus = sfcProduceEntity.Status;

            // 初始化步骤
            var stepEntity = new ManuSfcStepEntity
            {
                // 插入 manu_sfc_step 状态为出站（默认值）
                Operatetype = ManuSfcStepTypeEnum.OutStock,
                CurrentStatus = currentStatus,
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

            // 更新条码信息
            manuSfcEntity.UpdatedBy = commonBo.UserName;
            manuSfcEntity.UpdatedOn = commonBo.Time;

            // 更新在制条码信息
            sfcProduceEntity.UpdatedBy = commonBo.UserName;
            sfcProduceEntity.UpdatedOn = commonBo.Time;

            // 是否超过循环次数
            var isMoreThanCycle = sfcProduceEntity.RepeatedCount >= procedureRejudgeBo.Cycle;

            #region 未超过复投次数（当前工序排队）
            if (!isMoreThanCycle)
            {
                // 条码状态跟在制品状态一致
                manuSfcEntity.Status = SfcStatusEnum.lineUp;
                sfcProduceEntity.Status = SfcStatusEnum.lineUp;

                // 更新下一工序（默认情况，如果下面的含有首次不良工序命中，会更新该值）
                sfcProduceEntity.ProcedureId = commonBo.ProcedureId;
                sfcProduceEntity.ResourceId = commonBo.ResourceId;
            }
            #endregion

            #region 置于合格工艺路线排队（非末工序置于下工序排队，末工序为完成）
            else
            {
                // 已完工（置于在制完成）
                if (nextProcedure == null)
                {
                    //responseBo.IsLastProcedure = true;

                    // 清空复投次数
                    sfcProduceEntity.RepeatedCount = 0;

                    // 标记条码为"在制-完成"
                    //responseBo.IsCompleted = true;
                    manuSfcEntity.Status = SfcStatusEnum.InProductionComplete;
                    sfcProduceEntity.Status = SfcStatusEnum.InProductionComplete;
                }
                // 未完工（置于下工序排队）
                else
                {
                    responseBo.NextProcedureCode = nextProcedure.Code;

                    // 条码状态跟在制品状态一致
                    manuSfcEntity.Status = SfcStatusEnum.lineUp;
                    sfcProduceEntity.Status = SfcStatusEnum.lineUp;

                    // 更新下一工序
                    sfcProduceEntity.ProcedureId = nextProcedure.Id;

                    // 一旦切换工序，复投次数重置
                    sfcProduceEntity.RepeatedCount = 0;

                    // 不置空的话，进站时，可能校验不通过
                    sfcProduceEntity.ResourceId = null;
                }
            }
            #endregion

            // 保存操作后的状态
            stepEntity.AfterOperationStatus = sfcProduceEntity.Status;

            // 更新信息
            responseBo.SFCEntity = manuSfcEntity;
            responseBo.SFCStepEntity = stepEntity;
            responseBo.SFCProduceEntitiy = sfcProduceEntity;

            // 取的当前不合格代码
            unqualifiedCodes = unqualifiedCodes.Intersect(procedureRejudgeBo.UnqualifiedCodeEntities.Select(s => s.UnqualifiedCode));

            List<ManuProductBadRecordEntity> productBadRecordEntities = new();
            List<ManuProductNgRecordEntity> productNgRecordEntities = new();
            foreach (var item in unqualifiedCodes)
            {
                var unqualifiedEntity = procedureRejudgeBo.UnqualifiedCodeEntities.FirstOrDefault(f => f.UnqualifiedCode == item);
                if (unqualifiedEntity == null) continue;

                var badRecordId = IdGenProvider.Instance.CreateId();

                // 添加不良记录
                productBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = badRecordId,
                    SiteId = commonBo.SiteId,
                    FoundBadOperationId = commonBo.ProcedureId,
                    FoundBadResourceId = commonBo.ResourceId,
                    OutflowOperationId = commonBo.ProcedureId,
                    UnqualifiedId = unqualifiedEntity.Id,
                    SfcStepId = stepEntity.Id,
                    SFC = stepEntity.SFC,
                    SfcInfoId = stepEntity.SFCInfoId,
                    Qty = stepEntity.Qty,
                    Status = ProductBadRecordStatusEnum.Open,
                    Source = ProductBadRecordSourceEnum.EquipmentReBad,
                    Remark = "",
                    DisposalResult = ProductBadDisposalResultEnum.WaitingJudge,
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });

                // 添加NG记录
                productNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    BadRecordId = badRecordId,
                    UnqualifiedId = unqualifiedEntity.Id,
                    NGCode = item,
                    Remark = "",
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });
            }
            responseBo.ProductBadRecordEntities = productBadRecordEntities;
            responseBo.ProductNgRecordEntities = productNgRecordEntities;

            return responseBo;
        }

        /// <summary>
        /// 执行物料消耗（默认BOM清单）
        /// </summary>
        /// <param name="allFeedingEntities"></param>
        /// <param name="summaryBo"></param>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        private MaterialConsumptionBo ExecutenMaterialConsumptionWithBOM(ref List<ManuFeedingEntity> allFeedingEntities, MaterialDeductResponseSummaryBo summaryBo, ManuSfcProduceEntity sfcProduceEntity)
        {
            // 物料消耗对象
            MaterialConsumptionBo responseBo = new();

            if (summaryBo == null) return responseBo;
            if (summaryBo.InitialMaterials == null) return responseBo;

            // 物料ID集合
            var materialIds = summaryBo.InitialMaterials.Select(s => s.MaterialId);

            // 过滤扣料集合
            var feedings = allFeedingEntities.Where(w => w.Qty > 0 && materialIds.Contains(w.MaterialId));

            // 通过物料分组
            var manuFeedingsDictionary = feedings?.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 取得半成品的总数量
            var smiFinishedUsages = 1m;
            if (summaryBo.SmiFinisheds != null && summaryBo.SmiFinisheds.Any()) smiFinishedUsages = summaryBo.SmiFinisheds.Sum(s => s.Usages);

            // 过滤扣料集合
            List<UpdateFeedingQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            foreach (var materialBo in summaryBo.InitialMaterials)
            {
                if (manuFeedingsDictionary == null) continue;

                // 半成品时扣减数量 = 产出数量 * (1 / 半成品用量总和 * 物料用料 * (1 + 物料损耗%) * 物料消耗系数 ÷ 100)
                // 需扣减数量 = 产出数量 * 物料用量 * (1 + 物料损耗%) * 物料消耗系数 ÷ 100（因为每次不一定是只产出一个，所以也要*数量）
                decimal residue = sfcProduceEntity.Qty * materialBo.Usages / smiFinishedUsages;

                if (materialBo.Loss.HasValue && materialBo.Loss > 0) residue *= (1 + materialBo.Loss.Value / 100);
                if (materialBo.ConsumeRatio > 0) residue *= (materialBo.ConsumeRatio / 100);

                /*
                // 收集方式是批次
                if (materialBo.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, sfcProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialBo.SerialNumber != MaterialSerialNumberEnum.Batch) continue;
                */

                // 进行扣料
                _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, sfcProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
            }

            responseBo.UpdateFeedingQtyByIdCommands = updates;
            responseBo.ManuSfcCirculationEntities = adds;

            // 回改外层的上料数据集合数据
            foreach (var item in allFeedingEntities)
            {
                var feed = updates.FirstOrDefault(f => f.Id == item.Id);
                if (feed == null) continue;

                item.Qty = feed.Qty;
            }

            return responseBo;
        }

        /// <summary>
        /// 执行物料消耗（指定物料条码）
        /// </summary>
        /// <param name="allFeedingEntities"></param>
        /// <param name="summaryBo"></param>
        /// <param name="requestBo"></param>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        private MaterialConsumptionBo ExecutenMaterialConsumptionWithBarCode(ref List<ManuFeedingEntity> allFeedingEntities, MaterialDeductResponseSummaryBo summaryBo, OutStationRequestBo requestBo, ManuSfcProduceEntity sfcProduceEntity)
        {
            // 物料消耗对象
            MaterialConsumptionBo responseBo = new();

            if (requestBo.ConsumeList == null) return responseBo;
            if (summaryBo == null) return responseBo;
            if (summaryBo.InitialMaterials == null) return responseBo;

            List<ManuFeedingEntity> feedings = new();
            var consumelist = requestBo.ConsumeList.ToList();
            foreach (var item in consumelist)
            {
                var feedingEntity = allFeedingEntities.FirstOrDefault(f => f.BarCode == item.BarCode);
                if (feedingEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17113))
                        .WithData("BarCodes", item.BarCode);
                }

                var bomMaterial = summaryBo.InitialMaterials.FirstOrDefault(f => f.MaterialId == feedingEntity.MaterialId);
                if (bomMaterial == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17107))
                        .WithData("BarCodes", item.BarCode);
                }

                item.MaterialId = bomMaterial.MaterialId;
                feedings.Add(feedingEntity);
            }

            // 只保留传过来的消耗条码
            List<MaterialDeductResponseBo> filterMaterials = new();
            foreach (var item in summaryBo.InitialMaterials)
            {
                var consume = consumelist.FirstOrDefault(f => f.MaterialId == item.MaterialId);
                if (consume == null) continue;

                if (consume.ConsumeQty.HasValue)
                {
                    // 因为每次不一定是只产出一个，所以也要*数量
                    item.Usages = consume.ConsumeQty.Value * sfcProduceEntity.Qty;
                }

                // 如果不保留替代品（如果保留，就删除这句）
                item.ReplaceMaterials = Enumerable.Empty<MaterialDeductItemBo>();

                filterMaterials.Add(item);
            }

            // 通过物料分组
            var manuFeedingsDictionary = feedings?.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 取得半成品的总数量
            var smiFinishedUsages = 1m;
            if (summaryBo.SmiFinisheds != null && summaryBo.SmiFinisheds.Any()) smiFinishedUsages = summaryBo.SmiFinisheds.Sum(s => s.Usages);

            // 过滤扣料集合
            List<UpdateFeedingQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            foreach (var materialBo in filterMaterials)
            {
                if (manuFeedingsDictionary == null) continue;

                // 半成品时扣减数量 = 产出数量 * (1 / 半成品用量总和 * 物料用料 * (1 + 物料损耗) * 物料消耗系数 ÷ 100)
                // 需扣减数量 = 物料用量 * (1 + 物料损耗) * 物料消耗系数 ÷ 100
                decimal residue = sfcProduceEntity.Qty * materialBo.Usages / smiFinishedUsages;// materialBo.Usages / smiFinishedUsages;

                if (materialBo.Loss.HasValue && materialBo.Loss > 0) residue *= (1 + materialBo.Loss.Value / 100);
                if (materialBo.ConsumeRatio > 0) residue *= (materialBo.ConsumeRatio / 100);

                /*
                // 收集方式是批次
                if (materialBo.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, sfcProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialBo.SerialNumber != MaterialSerialNumberEnum.Batch) continue;
                */

                // 进行扣料
                _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, sfcProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
            }

            responseBo.UpdateFeedingQtyByIdCommands = updates;
            responseBo.ManuSfcCirculationEntities = adds;

            // 回改外层的上料数据集合数据
            foreach (var item in allFeedingEntities)
            {
                var feed = updates.FirstOrDefault(f => f.Id == item.Id);
                if (feed == null) continue;

                item.Qty = feed.Qty;
            }

            return responseBo;
        }

        /// <summary>
        /// 填充工序复投设置对象
        /// </summary>
        /// <param name="procedureRejudgeBo"></param>
        /// <param name="outStationRequestBos"></param>
        /// <returns></returns>
        private async Task<ProcedureRejudgeBo> FillingProcedureRejudgeBoAsync(ProcedureRejudgeBo procedureRejudgeBo, IEnumerable<OutStationRequestBo> outStationRequestBos)
        {
            if (outStationRequestBos == null) return procedureRejudgeBo;

            // 归集错误码
            List<string> unqualifiedCodes = new();
            foreach (var item in outStationRequestBos)
            {
                if (item.OutStationUnqualifiedList == null || !item.OutStationUnqualifiedList.Any()) continue;
                unqualifiedCodes.AddRange(item.OutStationUnqualifiedList.Select(s => s.UnqualifiedCode));
            }

            // 如果有传不合格代码，读取能跟数据库对应上的不合格代码
            if (unqualifiedCodes.Any())
            {
                procedureRejudgeBo.UnqualifiedCodeEntities = await _masterDataService.GetUnqualifiedEntitiesByCodesAsync(new QualUnqualifiedCodeByCodesQuery
                {
                    SiteId = procedureRejudgeBo.SiteId,
                    Codes = unqualifiedCodes
                });
            }

            // 查询工序关联的复投不合格代码
            var procedureRejudgeEntities = await _masterDataService.GetProcedureRejudgeEntitiesAsync(new EntityByParentIdQuery
            {
                SiteId = procedureRejudgeBo.SiteId,
                ParentId = procedureRejudgeBo.ProcedureId
            });

            // 如果有复投相关设置
            if (!procedureRejudgeEntities.Any()) return procedureRejudgeBo;

            // 不合格代码
            var unqualifiedCodeEntities = await _masterDataService.GetUnqualifiedEntitiesByIdsAsync(procedureRejudgeEntities.Select(s => s.UnqualifiedCodeId));

            // 复投相关设置分组
            var procedureRejudgeEntitiesDic = procedureRejudgeEntities.ToLookup(e => e.DefectType).ToDictionary(d => d.Key, d => d);
            foreach (var item in procedureRejudgeEntitiesDic)
            {
                switch (item.Key)
                {
                    case RejudgeUnqualifiedCodeEnum.Mark:
                        procedureRejudgeBo.MarkUnqualifiedId = item.Value.FirstOrDefault()?.UnqualifiedCodeId;
                        break;
                    case RejudgeUnqualifiedCodeEnum.Last:
                        var lastProcProcedureRejudgeEntity = item.Value.FirstOrDefault();
                        if (lastProcProcedureRejudgeEntity == null) continue;

                        procedureRejudgeBo.LastUnqualified = unqualifiedCodeEntities.FirstOrDefault(f => f.Id == lastProcProcedureRejudgeEntity?.UnqualifiedCodeId);
                        break;
                    case RejudgeUnqualifiedCodeEnum.Block:
                        procedureRejudgeBo.BlockUnqualifiedIds = item.Value.Select(s => s.UnqualifiedCodeId);
                        break;
                    default:
                        break;
                }
            }

            // 检查工序是否有设置"标记编码"和"缺陷编码"
            if (!procedureRejudgeBo.MarkUnqualifiedId.HasValue || procedureRejudgeBo.LastUnqualified == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES17115))
                    .WithData("Procedure", procedureRejudgeBo.ProcedureCode);

            // 检查不合格代码是否有设置"不合格工艺路线"
            if (!procedureRejudgeBo.LastUnqualified.ProcessRouteId.HasValue || procedureRejudgeBo.LastUnqualified.ProcessRouteId.Value == 0)
                throw new CustomerValidationException(nameof(ErrorCode.MES17116))
                    .WithData("Code", procedureRejudgeBo.LastUnqualified.UnqualifiedCode);

            // 填充工艺路线值
            procedureRejudgeBo.IsHasUnQualifiedProcessRoute = true;

            // 取得不合格工艺路线首工序
            var processRouteProcedureDto = await _masterDataService.GetFirstProcedureAsync(procedureRejudgeBo.LastUnqualified.ProcessRouteId.Value)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17111))
                .WithData("Ids", procedureRejudgeBo.LastUnqualified.ProcessRouteId);

            // 首工序信息
            var nextProcedureEntity = await _masterDataService.GetProcedureEntityByIdAsync(processRouteProcedureDto.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17114))
                .WithData("Procedure", processRouteProcedureDto.ProcedureId);

            // 置于不合格工艺路线首工序排队
            procedureRejudgeBo.NextProcedureId = nextProcedureEntity.Id;
            procedureRejudgeBo.NextProcedureCode = nextProcedureEntity.Code;

            return procedureRejudgeBo;
        }
        #endregion

    }
}
