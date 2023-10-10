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
using Hymson.MES.CoreServices.Bos.Common;
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
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

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
        /// <param name="manuDowngradingRepository"></param>
        /// <param name="manuDowngradingRecordRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
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
            IManuDowngradingRepository manuDowngradingRepository,
            IManuDowngradingRecordRepository manuDowngradingRecordRepository,
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
            _manuDowngradingRepository = manuDowngradingRepository;
            _manuDowngradingRecordRepository = manuDowngradingRecordRepository;
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
            if (param is not JobRequestBo commonBo) return;
            if (commonBo == null) return;
            if (commonBo.OutStationRequestBos == null || commonBo.OutStationRequestBos.Any() == false) return;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return;

            // 判断条码锁状态
            await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, multiSFCBo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.Activity, _localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.Activity)}"))
                              .VerifyProcedure(commonBo.ProcedureId)
                              .VerifyResource(commonBo.ResourceId);

            // 获取生产工单（附带工单状态校验）

            _ = await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceWorkOrderByIdsAsync, new WorkOrderIdsBo { WorkOrderIds = sfcProduceEntities.Select(s => s.WorkOrderId) });

            /*
            // 验证BOM主物料数量
            await _manuCommonService.VerifyBomQtyAsync(new ManuProcedureBomBo
            {
                SiteId = commonBo.SiteId,
                SFCs = commonBo.SFCs,
                ProcedureId = commonBo.ProcedureId,
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
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
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
            if (commonBo.OutStationRequestBos == null || commonBo.OutStationRequestBos.Any() == false) return default;

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, commonBo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return default;

            // 条码信息
            var manuSFCEntities = await _manuSfcRepository.GetByIdsAsync(sfcProduceEntities.Select(s => s.SFCId));
            if (manuSFCEntities == null || manuSFCEntities.Any() == false) return default;

            // 批量读取物料加载数据（需要实时数据，勿缓存）
            var allFeedingEntitiesByResourceId = await commonBo.Proxy.GetValueAsync(_manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync, new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = commonBo.ResourceId
            });
            var allFeedingEntities = allFeedingEntitiesByResourceId.AsList();

            // 遍历所有条码
            var singleSFCResponseBos = new List<OutStationResponseBo>();
            var responseBo = new OutStationResponseSummaryBo();
            foreach (var requestBo in commonBo.OutStationRequestBos)
            {
                var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(s => s.SFC == requestBo.SFC)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17102)).WithData("SFC", requestBo.SFC);

                var manuSfcEntity = manuSFCEntities.FirstOrDefault(s => s.Id == sfcProduceEntity.SFCId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17102)).WithData("SFC", requestBo.SFC);

                var singleSFCResponseBo = new OutStationResponseBo();
                if (requestBo.IsQualified.HasValue && requestBo.IsQualified.Value == false)
                {
                    // 不合格出站
                    singleSFCResponseBo = await OutStationForUnQualifiedProcedureAsync(commonBo, requestBo, sfcProduceEntity, manuSfcEntity);
                }
                else
                {
                    // 合格出站（为了逻辑清晰，跟上面的不合格出站区分开）
                    singleSFCResponseBo = await OutStationForQualifiedProcedureAsync(commonBo, requestBo, sfcProduceEntity, manuSfcEntity);
                }

                // 保存单个条码的出站结果
                if (singleSFCResponseBo == null) continue;

                #region 物料消耗明细数据
                // 组合物料数据（放缓存）
                var initialMaterials = await commonBo.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsAsync, new MaterialDeductRequestBo
                {
                    SiteId = commonBo.SiteId,
                    ProcedureId = commonBo.ProcedureId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId
                });

                MaterialConsumptionBo consumptionBo = new();
                // 指定消耗物料
                if (requestBo.ConsumeList != null && requestBo.ConsumeList.Any())
                {
                    consumptionBo = ExecutenMaterialConsumptionWithBarCode(ref allFeedingEntities, initialMaterials, commonBo, requestBo, sfcProduceEntity);
                }
                // 默认消耗逻辑
                else
                {
                    consumptionBo = ExecutenMaterialConsumptionWithBOM(ref allFeedingEntities, initialMaterials, commonBo, sfcProduceEntity);
                }
                singleSFCResponseBo.UpdateQtyByIdCommands = consumptionBo.UpdateQtyByIdCommands;
                singleSFCResponseBo.ManuSfcCirculationEntities = consumptionBo.ManuSfcCirculationEntities;
                #endregion

                #region 降级品信息
                if (singleSFCResponseBo.IsCompleted
                    && singleSFCResponseBo.ProcessRouteType == ProcessRouteTypeEnum.ProductionRoute
                    && responseBo.ManuSfcCirculationEntities != null
                    && responseBo.ManuSfcCirculationEntities.Any())
                {
                    var degradedProductExtendBo = new DegradedProductExtendBo
                    {
                        SiteId = commonBo.SiteId,
                        UserName = commonBo.UserName
                    };

                    // 添加降级品记录
                    degradedProductExtendBo.KeyValues.AddRange(responseBo.ManuSfcCirculationEntities.Select(s => new DegradedProductExtendKeyValueBo
                    {
                        BarCode = s.CirculationBarCode,
                        SFC = sfcProduceEntity.SFC
                    }));

                    // 取得降级品记录
                    var downgradingEntities = await _manuDegradedProductExtendService.GetManuDownGradingsAsync(degradedProductExtendBo);
                    var (manuDowngradingEntities, manuDowngradingRecordEntities) = await _manuDegradedProductExtendService.GetManuDowngradingsByConsumesAsync(degradedProductExtendBo, downgradingEntities);

                    singleSFCResponseBo.DowngradingEntities = manuDowngradingEntities;
                    singleSFCResponseBo.DowngradingRecordEntities = manuDowngradingRecordEntities;
                }
                #endregion

                singleSFCResponseBos.Add(singleSFCResponseBo);
            }

            // 归集每个条码的出站结果
            if (singleSFCResponseBos.Any() == false) return responseBo;
            responseBo.SFCEntities = singleSFCResponseBos.Select(s => s.SFCEntity);
            responseBo.SFCProduceEntities = singleSFCResponseBos.Select(s => s.SFCProduceEntitiy);
            responseBo.SFCStepEntities = singleSFCResponseBos.Select(s => s.SFCStepEntity);
            responseBo.WhMaterialInventoryEntities = singleSFCResponseBos.Select(s => s.MaterialInventoryEntity);
            responseBo.WhMaterialStandingbookEntities = singleSFCResponseBos.Select(s => s.MaterialStandingbookEntity);
            responseBo.MultiUpdateSummaryOutStationCommands = singleSFCResponseBos.Select(s => s.UpdateSummaryOutStationCommand);
            responseBo.UpdateQtyByIdCommands = singleSFCResponseBos.SelectMany(s => s.UpdateQtyByIdCommands);
            responseBo.ManuSfcCirculationEntities = singleSFCResponseBos.SelectMany(s => s.ManuSfcCirculationEntities);
            responseBo.DowngradingEntities = singleSFCResponseBos.SelectMany(s => s.DowngradingEntities);
            responseBo.DowngradingRecordEntities = singleSFCResponseBos.SelectMany(s => s.DowngradingRecordEntities);
            responseBo.ProductBadRecordEntities = singleSFCResponseBos.SelectMany(s => s.ProductBadRecordEntities);

            // 删除 manu_sfc_produce
            responseBo.DeletePhysicalByProduceIdsCommand = new DeletePhysicalByProduceIdsCommand
            {
                SiteId = commonBo.SiteId,
                Ids = singleSFCResponseBos.Where(w => w.IsCompleted).Select(s => s.SFCProduceEntitiy.Id)
            };

            // 删除 manu_sfc_produce_business
            responseBo.DeleteSfcProduceBusinesssBySfcInfoIdsCommand = new DeleteSfcProduceBusinesssBySfcInfoIdsCommand
            {
                SiteId = commonBo.SiteId,
                SfcInfoIds = singleSFCResponseBos.Where(w => w.IsCompleted).Select(s => s.SFCProduceEntitiy.Id)
            };

            var singleSFCResponseBosByWorkOrderId = singleSFCResponseBos
                .Where(w => w.IsCompleted)
                .Select(s => s.SFCProduceEntitiy)
                .ToLookup(w => w.WorkOrderId).ToDictionary(d => d.Key, d => d);
            foreach (var item in singleSFCResponseBosByWorkOrderId)
            {
                // 更新完工数量
                responseBo.UpdateQtyCommands.Add(new UpdateQtyCommand
                {
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    WorkOrderId = item.Key,
                    Qty = item.Value.Count()
                });
            }

            // 额外给面板用来显示的参数
            if (singleSFCResponseBos.Count() == 1)
            {
                var singleSFCResponse = singleSFCResponseBos.FirstOrDefault();
                if (singleSFCResponse != null)
                {
                    responseBo.IsCompleted = singleSFCResponse.IsCompleted;
                    responseBo.NextProcedureCode = singleSFCResponse.NextProcedureCode;
                }
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
            if (obj is not OutStationResponseSummaryBo data) return responseBo;

            // 更新物料库存
            if (data.UpdateQtyByIdCommands != null && data.UpdateQtyByIdCommands.Any())
            {
                responseBo.Rows += await _manuFeedingRepository.UpdateQtyByIdAsync(data.UpdateQtyByIdCommands);

                // 未更新到全部需更新的数据，事务回滚
                if (data.UpdateQtyByIdCommands.Count() > responseBo.Rows)
                {
                    responseBo.Rows = -1;
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
                _manuSfcProduceRepository.DeletePhysicalRangeByIdsSqlAsync(data.DeletePhysicalByProduceIdsCommand),

                // 删除 manu_sfc_produce_business
                _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdsAsync(data.DeleteSfcProduceBusinesssBySfcInfoIdsCommand),

                // 更新完工数量
                _planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderIdsAsync(data.UpdateQtyCommands),

                // 入库 / 台账
                _whMaterialInventoryRepository.InsertsAsync(data.WhMaterialInventoryEntities),
                _whMaterialStandingbookRepository.InsertsAsync(data.WhMaterialStandingbookEntities),

                // 降级品记录
                _manuDowngradingRepository.InsertsAsync(data.DowngradingEntities),
                _manuDowngradingRecordRepository.InsertsAsync(data.DowngradingRecordEntities),

                // manu_sfc 更新状态
                _manuSfcRepository.UpdateRangeWithStatusCheckAsync(data.SFCEntities),

                // 汇总表
                _manuSfcSummaryRepository.UpdateSummaryOutStationRangeAsync(data.MultiUpdateSummaryOutStationCommands),

                // 添加流转记录
                _manuSfcCirculationRepository.InsertRangeAsync(data.ManuSfcCirculationEntities),

                // 插入 manu_sfc_step
                _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities),

                // 插入不良记录
                _manuProductBadRecordRepository.InsertRangeAsync(data.ProductBadRecordEntities)
            };

            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            // 单条码过站时（面板过站）
            if (data.SFCEntities!.Count() == 1)
            {
                var SFCProduceEntity = data.SFCProduceEntities!.FirstOrDefault();
                if (SFCProduceEntity != null)
                {
                    // 面板需要的参数
                    responseBo.Content = new Dictionary<string, string> {
                        { "PackageCom", "False" },
                        { "BadEntryCom", "False" },
                        { "Qty", "1" },
                        { "IsLastProcedure", $"{data.IsCompleted}" },
                        { "NextProcedureCode", $"{data.NextProcedureCode}" }
                    };

                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES16349), SFCProduceEntity.SFC);
                    //responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES16351), data.FirstSFC, data.NextProcedureCode);
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

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
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
        /// <param name="sfcProduceEntity"></param>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        private async Task<OutStationResponseBo?> OutStationForQualifiedProcedureAsync(JobRequestBo commonBo, OutStationRequestBo requestBo, ManuSfcProduceEntity sfcProduceEntity, ManuSfcEntity manuSfcEntity)
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

            // 查询工序信息
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(commonBo.ProcedureId);
            var cycle = procProcedureEntity.Cycle ?? 1;

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
                VehicleCode = requestBo.VehicleCode,
                EquipmentId = commonBo.EquipmentId,
                SiteId = commonBo.SiteId,
                CreatedBy = commonBo.UserName,
                CreatedOn = commonBo.Time,
                UpdatedBy = commonBo.UserName,
                UpdatedOn = commonBo.Time
            };

            // 合格产出更新
            responseBo.UpdateSummaryOutStationCommand = new MultiUpdateSummaryOutStationCommand
            {
                Id = sfcProduceEntity.SfcSummaryId ?? 0,
                OutputQty = sfcProduceEntity.Qty,
                EndOn = commonBo.Time,
                UpdatedBy = commonBo.UserName,
                UpdatedOn = commonBo.Time
            };

            // 更新条码信息
            manuSfcEntity.UpdatedBy = commonBo.UserName;
            manuSfcEntity.UpdatedOn = commonBo.Time;

            // 已完工（ 如果没有尾工序，就表示已完工）
            if (nextProcedure == null)
            {
                // 条码状态为"完成"
                manuSfcEntity.Status = SfcStatusEnum.Complete;

                stepEntity.Operatetype = responseBo.ProcessRouteType == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                stepEntity.CurrentStatus = SfcStatusEnum.Complete;  // TODO 这里的状态？？

                // 生产主工艺路线
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
                        QuantityResidue = procMaterialEntity.Batch,
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
                        Quantity = procMaterialEntity.Batch,
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

                // TODO 非生产主工艺路线呢？？
            }
            // 未完工
            else
            {
                responseBo.IsCompleted = false;
                responseBo.NextProcedureCode = nextProcedure.Code;

                sfcProduceEntity.Status = SfcStatusEnum.lineUp;
                sfcProduceEntity.UpdatedBy = commonBo.UserName;
                sfcProduceEntity.UpdatedOn = commonBo.Time;

                // 条码状态跟在制品状态一致
                manuSfcEntity.Status = sfcProduceEntity.Status;

                // 更新下一工序
                sfcProduceEntity.ProcedureId = nextProcedure.Id;

                // 一旦切换工序，复投次数重置
                sfcProduceEntity.RepeatedCount = 0;

                // 不置空的话，进站时，可能校验不通过
                sfcProduceEntity.ResourceId = null;
            }

            /*
            // 如果超过复投次数
            if (sfcProduceEntity.RepeatedCount > cycle)
            {
                stepEntity.CurrentStatus = SfcStatusEnum.InProductionComplete;
            }
            */

            responseBo.SFCEntity = manuSfcEntity;
            responseBo.SFCStepEntity = stepEntity;
            responseBo.SFCProduceEntitiy = sfcProduceEntity;

            return responseBo;
        }

        /// <summary>
        /// 不合格工序出站
        /// </summary>
        /// <param name="commonBo"></param>
        /// <param name="requestBo"></param>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        private async Task<OutStationResponseBo?> OutStationForUnQualifiedProcedureAsync(JobRequestBo commonBo, OutStationRequestBo requestBo, ManuSfcProduceEntity sfcProduceEntity, ManuSfcEntity manuSfcEntity)
        {
            if (commonBo == null) return default;
            if (commonBo.Proxy == null) return default;

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

            // 查询工序信息
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(commonBo.ProcedureId);
            var cycle = procProcedureEntity.Cycle ?? 1;

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
                EquipmentId = commonBo.EquipmentId,
                VehicleCode = requestBo.VehicleCode,
                SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                SiteId = commonBo.SiteId,
                CreatedBy = commonBo.UserName,
                CreatedOn = commonBo.Time,
                UpdatedBy = commonBo.UserName,
                UpdatedOn = commonBo.Time
            };

            // 更新条码信息
            manuSfcEntity.UpdatedBy = commonBo.UserName;
            manuSfcEntity.UpdatedOn = commonBo.Time;

            // 已完工（ 如果没有尾工序，就表示已完工）
            if (nextProcedure == null)
            {
                // 条码状态为"完成-在制"
                manuSfcEntity.Status = SfcStatusEnum.InProductionComplete;

                stepEntity.Operatetype = responseBo.ProcessRouteType == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                stepEntity.CurrentStatus = SfcStatusEnum.InProductionComplete;
            }
            // 未完工
            else
            {
                sfcProduceEntity.Status = SfcStatusEnum.lineUp;
                sfcProduceEntity.UpdatedBy = commonBo.UserName;
                sfcProduceEntity.UpdatedOn = commonBo.Time;

                // 条码状态跟在制品状态一致
                manuSfcEntity.Status = sfcProduceEntity.Status;

                // 不合格复投的话，默认当前工序，当前资源继续进站
                sfcProduceEntity.ProcedureId = commonBo.ProcedureId;
                sfcProduceEntity.ResourceId = commonBo.ResourceId;

                // 如果超过复投次数
                if (sfcProduceEntity.RepeatedCount > cycle)
                {
                    // 清空复投次数
                    sfcProduceEntity.RepeatedCount = 0;

                    // 标记条码为"在制-完成"
                    //responseBo.IsCompleted = true;
                    manuSfcEntity.Status = SfcStatusEnum.InProductionComplete;
                    sfcProduceEntity.Status = SfcStatusEnum.InProductionComplete;
                    stepEntity.CurrentStatus = SfcStatusEnum.InProductionComplete;
                }
            }

            responseBo.SFCEntity = manuSfcEntity;
            responseBo.SFCStepEntity = stepEntity;
            responseBo.SFCProduceEntitiy = sfcProduceEntity;

            // 记录出站不良记录（如果有传不合格代码）
            if (requestBo.OutStationUnqualifiedList != null && requestBo.OutStationUnqualifiedList.Any())
            {
                var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery
                {
                    SiteId = commonBo.SiteId,
                    Codes = requestBo.OutStationUnqualifiedList.Select(s => s.UnqualifiedCode)
                });

                // 添加不良记录
                responseBo.ProductBadRecordEntities = qualUnqualifiedCodeEntities.Select(s => new ManuProductBadRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    FoundBadOperationId = commonBo.ProcedureId,
                    FoundBadResourceId = commonBo.ResourceId,
                    OutflowOperationId = commonBo.ProcedureId,
                    UnqualifiedId = s.Id,
                    SfcStepId = stepEntity.Id,
                    SFC = stepEntity.SFC,
                    SfcInfoId = stepEntity.SFCInfoId,
                    Qty = stepEntity.Qty,
                    Status = stepEntity.CurrentStatus == SfcStatusEnum.InProductionComplete ? ProductBadRecordStatusEnum.Close : ProductBadRecordStatusEnum.Open,
                    Source = ProductBadRecordSourceEnum.EquipmentReBad,
                    Remark = stepEntity.Remark,
                    //DisposalResult = ProductBadDisposalResultEnum.AutoHandle,
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });
            }

            return responseBo;
        }

        /// <summary>
        /// 执行物料消耗（默认BOM清单）
        /// </summary>
        /// <param name="allFeedingEntities"></param>
        /// <param name="initialMaterials"></param>
        /// <param name="commonBo"></param>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        private MaterialConsumptionBo ExecutenMaterialConsumptionWithBOM(ref List<ManuFeedingEntity> allFeedingEntities, IEnumerable<MaterialDeductResponseBo>? initialMaterials, JobRequestBo commonBo, ManuSfcProduceEntity sfcProduceEntity)
        {
            // 物料消耗对象
            MaterialConsumptionBo responseBo = new();

            if (commonBo == null) return responseBo;
            if (commonBo.Proxy == null) return responseBo;
            if (initialMaterials == null) return responseBo;

            // 物料ID集合
            var materialIds = initialMaterials.Select(s => s.MaterialId);

            // 过滤扣料集合
            var feedings = allFeedingEntities.Where(w => w.Qty > 0 && materialIds.Contains(w.MaterialId));

            // 通过物料分组
            var manuFeedingsDictionary = feedings?.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 过滤扣料集合
            List<UpdateQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            List<MultiUpdateSummaryOutStationCommand> updateSummaryOutStationCommands = new();
            foreach (var materialBo in initialMaterials)
            {
                if (manuFeedingsDictionary == null) continue;

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

            responseBo.UpdateQtyByIdCommands = updates;
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
        /// 执行物料消耗（指定物料）
        /// </summary>
        /// <param name="allFeedingEntities"></param>
        /// <param name="initialMaterials"></param>
        /// <param name="commonBo"></param>
        /// <param name="requestBo"></param>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private MaterialConsumptionBo ExecutenMaterialConsumptionWithBarCode(ref List<ManuFeedingEntity> allFeedingEntities, IEnumerable<MaterialDeductResponseBo>? initialMaterials, JobRequestBo commonBo, OutStationRequestBo requestBo, ManuSfcProduceEntity sfcProduceEntity)
        {
            // 物料消耗对象
            MaterialConsumptionBo responseBo = new();

            if (commonBo == null) return responseBo;
            if (commonBo.Proxy == null) return responseBo;
            if (requestBo.ConsumeList == null) return responseBo;
            if (initialMaterials == null) return responseBo;

            // 如果存在传过来的消耗编码不在BOM清单里面的物料，直接返回异常
            var hasBarCodeNotInBOM = requestBo.ConsumeList.Select(s => s.BarCode).Except(initialMaterials.Select(s => s.MaterialCode)).Any();
            if (hasBarCodeNotInBOM)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17105));
            }

            // 只保留传过来的消耗编码
            List<MaterialDeductResponseBo> filterMaterials = new();
            foreach (var item in initialMaterials)
            {
                var consume = requestBo.ConsumeList.FirstOrDefault(f => f.BarCode == item.MaterialCode);
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

            // 过滤扣料集合
            var feedings = allFeedingEntities.Where(w => w.Qty > 0 && materialIds.Contains(w.MaterialId));

            // 通过物料分组
            var manuFeedingsDictionary = feedings?.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 过滤扣料集合
            List<UpdateQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            List<MultiUpdateSummaryOutStationCommand> updateSummaryOutStationCommands = new();
            foreach (var materialBo in initialMaterials)
            {
                if (manuFeedingsDictionary == null) continue;

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

            responseBo.UpdateQtyByIdCommands = updates;
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
        #endregion

    }
}
