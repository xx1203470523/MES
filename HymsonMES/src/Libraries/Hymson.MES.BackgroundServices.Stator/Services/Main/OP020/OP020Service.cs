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
    public partial class OP020Service : IOP020Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP020Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP020> _opRepository;

        /// <summary>
        /// 服务接口（基础）
        /// </summary>
        public readonly IMainService _mainService;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opRepository"></param>
        /// <param name="mainService"></param>
        /// <param name="waterMarkService"></param>
        public OP020Service(ILogger<OP020Service> logger,
            IOPRepository<OP020> opRepository,
            IMainService mainService,
            IWaterMarkService waterMarkService)
        {
            _logger = logger;
            _opRepository = opRepository;
            _mainService = mainService;
            _waterMarkService = waterMarkService;
        }


        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(int limitCount)
        {
            var producreCode = $"{typeof(OP020).Name}";
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
            var summaryBo = await ConvertDataListAsync(entities, barCodes);

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
        private async Task<StatorSummaryBo> ConvertDataListAsync(IEnumerable<OP020> entities, IEnumerable<string> barCodes, IEnumerable<string>? parameterCodes = null)
        {
            var producreCode = $"{typeof(OP020).Name}";

            // 初始化对象
            var statorBo = await _mainService.GetStatorBaseConfigAsync(producreCode);

            // 批量读取条码（MES）
            var manuSFCEntities = await _mainService.GetSFCEntitiesAsync(statorBo.SiteId, barCodes);

            // 批量读取条码信息（MES）
            var manuSFCInfoEntities = await _mainService.GetSFCInfoEntitiesAsync(manuSFCEntities.Select(s => s.Id));

            // 批量读取条码（铜线）
            var wireSFCEntities = await _mainService.GetWireBarCodeEntitiesAsync(statorBo.SiteId, entities.Select(s => s.ID).Distinct());

            // 遍历记录
            var summaryBo = new StatorSummaryBo { };
            foreach (var opEntity in entities)
            {
                var barCode1 = opEntity.wire1_barcode;
                var barCode2 = opEntity.wire2_barcode;
                var time = opEntity.RDate;

                // ID是否无效数据
                var id = opEntity.ID.ParseToLong();
                if (id == 0) continue;

                IEnumerable<WireBarCodeEntity>? wireEntities = wireSFCEntities.Where(f => f.WireId == id);

                // 如果条码1有效
                List<WireBarCodeEntity> wireBarCodeEntities = new();
                if (!StatorConst.IgnoreString.Contains(barCode1) && !string.IsNullOrWhiteSpace(barCode1) && !wireEntities.Any(a => a.WireBarCode == barCode1))
                {
                    wireBarCodeEntities.Add(new WireBarCodeEntity
                    {
                        Id = $"{id}{barCode1}".ToLongID(),
                        WireId = id,
                        WireBarCode = barCode1,
                        SiteId = statorBo.SiteId,
                        CreatedOn = statorBo.Time,
                        UpdatedOn = time
                    });
                }

                // 如果条码2有效
                if (!StatorConst.IgnoreString.Contains(barCode2) && !string.IsNullOrWhiteSpace(barCode2) && !wireEntities.Any(a => a.WireBarCode == barCode2))
                {
                    wireBarCodeEntities.Add(new WireBarCodeEntity
                    {
                        Id = $"{id}{barCode2}".ToLongID(),
                        WireId = id,
                        WireBarCode = barCode2,
                        SiteId = statorBo.SiteId,
                        CreatedOn = statorBo.Time,
                        UpdatedOn = time
                    });
                }
                summaryBo.AddWireBarCodeEntities.AddRange(wireBarCodeEntities);

                // 遍历条码
                foreach (var wireBarCodeEntity in wireBarCodeEntities)
                {
                    // 条码ID
                    var manuSFCStepId = IdGenProvider.Instance.CreateId();
                    var manuBadRecordId = IdGenProvider.Instance.CreateId();

                    // 条码
                    var barCode = wireBarCodeEntity.WireBarCode;
                    var manuSFCEntity = manuSFCEntities.FirstOrDefault(f => f.SFC == barCode);
                    if (manuSFCEntity == null) continue;

                    // 条码信息
                    var manuSFCInfoEntity = manuSFCInfoEntities.FirstOrDefault(f => f.SfcId == manuSFCEntity.Id);
                    if (manuSFCInfoEntity == null) continue;

                    // 如果是不合格
                    var scrapQty = 0m;
                    var isOk = opEntity.Result == "OK";
                    if (!isOk)
                    {
                        scrapQty = -StatorConst.QTY;

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
                            UpdatedOn = time
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
                            UpdatedOn = time
                        });
                    }

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
                        SFCInfoId = manuSFCInfoEntity.Id,
                        Qty = StatorConst.QTY,
                        ScrapQty = scrapQty,
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

                }

                // 如果没有需要解析的参数
                if (parameterCodes == null || !parameterCodes.Any()) continue;

            }

            summaryBo.StatorBo = statorBo;
            return summaryBo;
        }

    }
}
