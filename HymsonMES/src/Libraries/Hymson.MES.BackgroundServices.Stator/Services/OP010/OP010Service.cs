using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Extension;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP010Service : IOP010Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP010Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP010> _opRepository;

        /// <summary>
        /// 服务接口（基础）
        /// </summary>
        public readonly IMainService _mainService;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opRepository"></param>
        /// <param name="mainService"></param>
        /// <param name="waterMarkService"></param>
        /// <param name="procParameterRepository"></param>
        public OP010Service(ILogger<OP010Service> logger,
            IOPRepository<OP010> opRepository,
            IMainService mainService,
            IWaterMarkService waterMarkService,
            IProcParameterRepository procParameterRepository)
        {
            _logger = logger;
            _opRepository = opRepository;
            _mainService = mainService;
            _waterMarkService = waterMarkService;
            _procParameterRepository = procParameterRepository;
        }


        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(int limitCount)
        {
            var producreCode = $"{typeof(OP010).Name}";
            var buzKey = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(buzKey);

            // 根据水位读取数据
            var entities = await _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (entities == null || !entities.Any())
            {
                _logger.LogDebug($"【{producreCode}】没有要拉取的数据！");
                return 0;
            }

            // 先定位条码位置
            var barCodes = entities.Select(s => s.wire1_barcode);

            // 获取转换数据（基础数据）
            var summaryBo = await ConvertDataListAsync(entities, barCodes, _parameterCodes);

            // 保存数据
            return await _mainService.SaveBaseDataWithCommitAsync(buzKey, entities.Max(m => m.index), summaryBo);
        }

        /// <summary>
        /// 保存转换数据（附带参数）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="barCodes"></param>
        /// <param name="parameterCodes"></param>
        /// <returns></returns>
        private async Task<StatorSummaryBo> ConvertDataListAsync<T>(IEnumerable<T> entities, IEnumerable<string> barCodes, IEnumerable<string>? parameterCodes = null) where T : BaseOPEntity
        {
            var producreCode = $"{typeof(T).Name}";

            // 初始化对象
            var statorBo = await _mainService.GetStatorBaseConfigAsync(producreCode);

            // 批量读取条码（MES）
            var manuSFCEntities = await _mainService.GetSFCEntitiesAsync(statorBo.SiteId, barCodes);

            // 批量读取条码（定子）
            var statorSFCEntities = await _mainService.GetStatorBarCodeEntitiesAsync(statorBo.SiteId, entities.Select(s => s.ID).Distinct());

            // 遍历记录
            var summaryBo = new StatorSummaryBo { };
            foreach (var opEntity in entities)
            {
                var barCode = "";

                // ID是否无效数据
                var id = opEntity.ID.ParseToLong();
                if (id == 0) continue;

                StatorBarCodeEntity? statorSFCEntity = statorSFCEntities.FirstOrDefault(f => f.InnerId == id);
                switch (producreCode)
                {
                    case "OP010":
                        barCode = $"{opEntity.GetType().GetProperty("wire1_barcode")?.GetValue(opEntity)}";
                        break;
                    case "OP190":
                    case "OP210":
                        barCode = opEntity.Barcode;
                        if (statorSFCEntity != null)
                        {
                            statorSFCEntity.OuterBarCode = barCode;
                            statorSFCEntity.UpdatedOn = statorBo.Time;
                            summaryBo.UpdateStatorBarCodeEntities.Add(statorSFCEntity);
                        }
                        break;
                    case "OP340":
                        barCode = $"{opEntity.GetType().GetProperty("busbar_barcode")?.GetValue(opEntity)}";
                        if (statorSFCEntity != null)
                        {
                            statorSFCEntity.BusBarCode = barCode;
                            statorSFCEntity.UpdatedOn = statorBo.Time;
                            summaryBo.UpdateStatorBarCodeEntities.Add(statorSFCEntity);
                        }
                        break;
                    case "OP490":
                        barCode = $"{opEntity.GetType().GetProperty("LaserBarcode")?.GetValue(opEntity)}";
                        if (statorSFCEntity != null)
                        {
                            statorSFCEntity.ProductionCode = barCode;
                            statorSFCEntity.UpdatedOn = statorBo.Time;
                            summaryBo.UpdateStatorBarCodeEntities.Add(statorSFCEntity);
                        }
                        break;
                    case "OP070":
                        barCode = opEntity.Barcode;
                        break;
                    default:
                        break;
                }

                // 条码是否无效数据
                if (barCode == "-" || barCode == "_" || string.IsNullOrWhiteSpace(barCode)) continue;

                // OP070特殊处理
                if (producreCode == "OP070")
                {
                    var uniqueId = $"{id}{barCode}".ToLongID();

                    // 不存在就插入
                    if (statorSFCEntity != null)
                    {
                        //if (barCode == "-" || string.IsNullOrWhiteSpace(barCode)) break;
                        if (statorSFCEntity.InnerBarCode != "-" && !string.IsNullOrWhiteSpace(statorSFCEntity.InnerBarCode)) break;

                        statorSFCEntity.InnerBarCode = barCode;
                        statorSFCEntity.UpdatedOn = statorBo.Time;
                        summaryBo.UpdateStatorBarCodeEntities.Add(statorSFCEntity);
                    }
                    else if (!summaryBo.AddStatorBarCodeEntities.Any(a => a.Id == uniqueId))
                    {
                        summaryBo.AddStatorBarCodeEntities.Add(new StatorBarCodeEntity
                        {
                            Id = uniqueId,
                            InnerId = id,
                            InnerBarCode = barCode,
                            SiteId = statorBo.SiteId,
                            CreatedOn = statorBo.Time
                        });
                    }
                }

                // 条码ID
                var manuSFCId = IdGenProvider.Instance.CreateId();
                var manuSFCInfoId = IdGenProvider.Instance.CreateId();
                var manuSFCStepId = IdGenProvider.Instance.CreateId();
                var manuBadRecordId = IdGenProvider.Instance.CreateId();

                var manuSFCEntity = manuSFCEntities.FirstOrDefault(f => f.SFC == barCode);
                if (manuSFCEntity == null)
                {
                    // 插入条码
                    summaryBo.ManuSFCEntities.Add(new ManuSfcEntity
                    {
                        Id = manuSFCId,
                        Qty = StatorConst.QTY,
                        SFC = barCode,
                        IsUsed = YesOrNoEnum.No,
                        Type = SfcTypeEnum.NoProduce,
                        Status = SfcStatusEnum.Complete,

                        SiteId = statorBo.SiteId,
                        CreatedBy = statorBo.User,
                        CreatedOn = statorBo.Time,
                        UpdatedBy = StatorConst.USER,
                        UpdatedOn = opEntity.RDate
                    });
                }
                else
                {
                    // 已存在条码
                    manuSFCId = manuSFCEntity.Id;
                }

                // 插入条码信息
                summaryBo.ManuSFCInfoEntities.Add(new ManuSfcInfoEntity
                {
                    Id = manuSFCInfoId,
                    SfcId = manuSFCId,
                    WorkOrderId = statorBo.WorkOrderId,
                    ProductId = statorBo.ProductId,
                    ProductBOMId = statorBo.ProductBOMId,
                    ProcessRouteId = statorBo.ProcessRouteId,
                    IsUsed = false,

                    SiteId = statorBo.SiteId,
                    CreatedBy = statorBo.User,
                    CreatedOn = statorBo.Time,
                    UpdatedBy = StatorConst.USER,
                    UpdatedOn = opEntity.RDate
                });

                // 插入步骤表
                var stepEntity = new ManuSfcStepEntity
                {
                    Id = manuSFCStepId,
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = SfcStatusEnum.Activity,
                    SFC = barCode,
                    ProductId = statorBo.ProductId,
                    WorkOrderId = statorBo.WorkOrderId,
                    WorkCenterId = statorBo.WorkLineId,
                    ProductBOMId = statorBo.ProductBOMId,
                    ProcessRouteId = statorBo.ProcessRouteId,
                    SFCInfoId = manuSFCInfoId,
                    Qty = StatorConst.QTY,
                    VehicleCode = "",
                    ProcedureId = statorBo.ProcedureId,
                    ResourceId = null,
                    EquipmentId = null,
                    OperationProcedureId = statorBo.ProcedureId,
                    OperationResourceId = null,
                    OperationEquipmentId = null,

                    Remark = $"{opEntity.index}",   // 这个ID是为了外层找到对应记录

                    SiteId = statorBo.SiteId,
                    CreatedBy = statorBo.User,
                    CreatedOn = statorBo.Time,
                    UpdatedBy = StatorConst.USER,
                    UpdatedOn = opEntity.RDate
                };
                summaryBo.ManuSfcStepEntities.Add(stepEntity);

                // 如果是不合格
                var isOk = opEntity.Result == "OK";
                if (isOk) continue;

                // 插入不良记录
                summaryBo.ManuProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = manuBadRecordId,
                    FoundBadOperationId = statorBo.ProcedureId,
                    OutflowOperationId = statorBo.ProcedureId,
                    UnqualifiedId = 0,
                    SFC = barCode,
                    SfcInfoId = 0,
                    SfcStepId = manuSFCStepId,
                    Qty = 1,
                    Status = ProductBadRecordStatusEnum.Open,
                    Source = ProductBadRecordSourceEnum.EquipmentReBad,
                    Remark = "",

                    SiteId = statorBo.SiteId,
                    CreatedBy = statorBo.User,
                    CreatedOn = statorBo.Time,
                    UpdatedBy = StatorConst.USER,
                    UpdatedOn = opEntity.RDate
                });

                // 插入NG记录
                summaryBo.ManuProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    BadRecordId = manuBadRecordId,
                    UnqualifiedId = 0,
                    NGCode = "未知",

                    SiteId = statorBo.SiteId,
                    CreatedBy = statorBo.User,
                    CreatedOn = statorBo.Time,
                    UpdatedBy = StatorConst.USER,
                    UpdatedOn = opEntity.RDate
                });

                // 如果没有需要解析的参数
                if (parameterCodes == null || !parameterCodes.Any()) continue;

                // 读取标准参数
                var parameterEntities = await _mainService.GetParameterEntitiesAsync(parameterCodes, summaryBo.StatorBo);

                // 遍历参数
                foreach (var param in parameterEntities)
                {
                    // 指定对象获取值
                    var paramValue = opEntity.GetType().GetProperty(param.ParameterCode)?.GetValue(opEntity);

                    summaryBo.ManuProductParameterEntities.Add(new Core.Domain.Parameter.ManuProductParameterEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ProcedureId = stepEntity.ProcedureId ?? 0,
                        SfcstepId = stepEntity.Id,
                        SFC = stepEntity.SFC,

                        ParameterId = param.Id,
                        ParameterValue = $"{paramValue}",
                        ParameterGroupId = 0,
                        CollectionTime = opEntity.RDate ?? stepEntity.CreatedOn,

                        SiteId = stepEntity.SiteId,
                        CreatedBy = stepEntity.CreatedBy,
                        CreatedOn = stepEntity.CreatedOn,
                        UpdatedBy = stepEntity.UpdatedBy,
                        UpdatedOn = stepEntity.UpdatedOn
                    });
                }
            }

            summaryBo.StatorBo = statorBo;
            return summaryBo;
        }

    }

    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP010Service
    {
        /// <summary>
        /// 参数编码集合
        /// </summary>
        private static readonly List<string> _parameterCodes = new()
        {
            "FormingLHZDistance",
            "FormingRHZDistance",
            "FormingUpperLHXDistance",
            "FormingUpperRHXDistance",
            "FormingLowerLHXDistance",
            "FormingLowerRHXDistance",
            "FormingLHZSpeed",
            "FormingRHZSpeed",
            "FormingUpperLHXSpeed",
            "FormingUpperRHXSpeed",
            "FormingLowerLHXSpeed",
            "FormingLowerRHXSpeed"
        };
    }
}
