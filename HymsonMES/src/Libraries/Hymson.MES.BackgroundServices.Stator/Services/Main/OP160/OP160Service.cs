using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP160Service : IOP160Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP160Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP160> _opRepository;

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
        public OP160Service(ILogger<OP160Service> logger,
            IOPRepository<OP160> opRepository,
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
            var producreCode = $"{typeof(OP160).Name}";
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
                _logger.LogDebug($"【 {producreCode} 】没有要拉取的数据！");
                return 0;
            }

            // 先定位条码位置
            var barCodes = entities.Select(s => s.PaperLotBarcode);

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
        private async Task<StatorSummaryBo> ConvertDataListAsync(IEnumerable<OP160> entities, IEnumerable<string> barCodes, IEnumerable<string>? parameterCodes = null)
        {
            var producreCode = $"{typeof(OP160).Name}";

            // 初始化对象
            var statorBo = await _mainService.GetStatorBaseConfigAsync(producreCode);

            // 批量读取条码（MES）
            var manuSFCEntities = await _mainService.GetSFCEntitiesAsync(statorBo.SiteId, barCodes);

            // 批量读取条码信息（MES）
            var manuSFCInfoEntities = await _mainService.GetSFCInfoEntitiesAsync(manuSFCEntities.Select(s => s.Id));

            // 批量读取物料条码（MES）
            var inventoryEntities = await _mainService.GetMaterialInventoryEntitiesAsync(statorBo.SiteId, barCodes);

            /*
            // 批量读取物料信息（MES）
            var materialEntities = await _mainService.GetMaterialEntitiesAsync(inventoryEntities.Select(s => s.MaterialId));
            */

            // 批量读取条码（定子）
            var statorSFCEntities = await _mainService.GetStatorBarCodeEntitiesAsync(new StatorBarCodeQuery
            {
                SiteId = statorBo.SiteId,
                InnerIds = entities.Select(s => s.ID).Distinct()
            });

            // 物料信息（绝缘纸）
            var materialEntity = await _mainService.GetMaterialEntityAsync(new EntityByCodeQuery
            {
                Site = statorBo.SiteId,
                Code = _materialCode
            });
            var materialId = materialEntity?.Id ?? 0;

            // 物料信息（内定子）
            var innerMaterialEntity = await _mainService.GetMaterialEntityAsync(new EntityByCodeQuery
            {
                Site = statorBo.SiteId,
                Code = _innerStatorCode
            });
            var innerStatorId = innerMaterialEntity?.Id ?? 0;

            // 遍历记录
            var summaryBo = new StatorSummaryBo { };
            foreach (var opEntity in entities)
            {
                var barCode = opEntity.PaperLotBarcode;
                var time = opEntity.RDate;

                // ID是否无效数据
                var id = opEntity.ID.ParseToLong();
                if (id == 0) continue;

                StatorBarCodeEntity? statorSFCEntity = statorSFCEntities.FirstOrDefault(f => f.InnerId == id);
                if (statorSFCEntity == null) continue;

                statorSFCEntity.PaperTopLotBarcode = barCode;
                statorSFCEntity.UpdatedOn = statorBo.Time;
                summaryBo.UpdateStatorBarCodeEntities.Add(statorSFCEntity);

                // 条码是否无效数据
                if (StatorConst.IgnoreString.Contains(barCode) || string.IsNullOrWhiteSpace(barCode)) continue;

                // 条码ID
                var manuSFCId = IdGenProvider.Instance.CreateId();
                var manuSFCInfoId = IdGenProvider.Instance.CreateId();
                var manuSFCStepId = IdGenProvider.Instance.CreateId();
                var manuBadRecordId = IdGenProvider.Instance.CreateId();

                // 条码
                var manuSFCEntity = manuSFCEntities.FirstOrDefault(f => f.SFC == barCode);
                if (manuSFCEntity == null)
                {
                    // 插入条码
                    summaryBo.ManuSFCEntities.Add(new ManuSfcEntity
                    {
                        Id = manuSFCId,
                        Qty = StatorConst.QTY,
                        SFC = barCode,
                        IsUsed = YesOrNoEnum.Yes,
                        Type = SfcTypeEnum.NoProduce,
                        Status = SfcStatusEnum.Complete,

                        SiteId = statorBo.SiteId,
                        CreatedBy = statorBo.User,
                        CreatedOn = statorBo.Time,
                        UpdatedBy = StatorConst.USER,
                        UpdatedOn = time
                    });
                }
                else
                {
                    // 已存在条码
                    manuSFCId = manuSFCEntity.Id;
                }

                // 条码信息
                var manuSFCInfoEntity = manuSFCInfoEntities.FirstOrDefault(f => f.SfcId == manuSFCId);
                if (manuSFCInfoEntity == null)
                {
                    // 插入条码信息
                    summaryBo.ManuSFCInfoEntities.Add(new ManuSfcInfoEntity
                    {
                        Id = manuSFCInfoId,
                        SfcId = manuSFCId,
                        WorkOrderId = null,
                        ProductId = materialId,
                        ProductBOMId = null,
                        ProcessRouteId = null,
                        IsUsed = true,

                        SiteId = statorBo.SiteId,
                        CreatedBy = statorBo.User,
                        CreatedOn = statorBo.Time,
                        UpdatedBy = StatorConst.USER,
                        UpdatedOn = time
                    });
                }
                else
                {
                    // 已存在条码
                    manuSFCInfoId = manuSFCInfoEntity.Id;
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
                    UpdatedOn = time
                };
                summaryBo.ManuSfcStepEntities.Add(stepEntity);

                var inventoryEntity = inventoryEntities.FirstOrDefault(f => f.MaterialBarCode == barCode);
                if (inventoryEntity != null)
                {
                    // 扣减物料库存
                    inventoryEntity.QuantityResidue -= StatorConst.QTY;
                    inventoryEntity.Status = WhMaterialInventoryStatusEnum.ToBeUsed;
                    inventoryEntity.UpdatedOn = statorBo.Time;
                    inventoryEntity.UpdatedBy = StatorConst.USER;
                    summaryBo.UpdateWhMaterialInventoryEntities.Add(inventoryEntity);
                }

                /*
                var materialId = inventoryEntity?.MaterialId ?? 0;
                var materialEntity = materialEntities.FirstOrDefault(f => f.Id == materialId);
                */

                if (materialEntity != null)
                {
                    // 添加台账
                    summaryBo.WhMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                    {
                        MaterialCode = materialEntity.MaterialCode,
                        MaterialName = materialEntity.MaterialName,
                        MaterialVersion = materialEntity.Version ?? "",
                        MaterialBarCode = barCode,
                        //Batch = "",//自制品 没有
                        Quantity = StatorConst.QTY,
                        Unit = materialEntity.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.ManuComplete,
                        Source = MaterialInventorySourceEnum.ManuComplete,

                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = statorBo.SiteId,
                        CreatedBy = statorBo.User,
                        CreatedOn = statorBo.Time,
                        UpdatedBy = StatorConst.USER,
                        UpdatedOn = time
                    });
                }

                // 插入流转记录
                summaryBo.ManuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                {
                    WorkOrderId = statorBo.WorkOrderId,
                    ProductId = materialId,// 51558094604398592
                    ProcedureId = statorBo.ProcedureId,
                    ResourceId = null,
                    SFC = barCode,

                    CirculationBarCode = statorSFCEntity.InnerBarCode,
                    CirculationWorkOrderId = statorBo.WorkOrderId,
                    CirculationProductId = innerStatorId,// 51558094067523584
                    CirculationMainProductId = innerStatorId,// 51558094067523584
                    CirculationQty = StatorConst.QTY,
                    CirculationType = SfcCirculationTypeEnum.Consume,

                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = statorBo.SiteId,
                    CreatedBy = statorBo.User,
                    CreatedOn = statorBo.Time,
                    UpdatedBy = "OP160",//StatorConst.USER,
                    UpdatedOn = time
                });

                // 如果是不合格
                var isOk = opEntity.Result == "OK";
                if (!isOk)
                {
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

                // 如果没有需要解析的参数
                if (parameterCodes == null || !parameterCodes.Any()) continue;

            }

            summaryBo.StatorBo = statorBo;
            return summaryBo;
        }

    }

    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP160Service
    {
        /// <summary>
        /// 编码（NHN绝缘纸-0.25mm x 130mm）
        /// </summary>
        private const string _materialCode = "030107000002";

        /// <summary>
        /// 编码（內定子铁芯-NIO4.8量产φ154x121.5）
        /// </summary>
        private const string _innerStatorCode = "030101000002";

    }

}
