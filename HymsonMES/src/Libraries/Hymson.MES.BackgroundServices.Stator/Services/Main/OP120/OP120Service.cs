using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Extension;
using Hymson.Snowflake;
using Hymson.Utils;
using IdGen;
using System.Data;
using static Dapper.SqlMapper;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP120Service : IOP120Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP120Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP120> _opRepository;

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
        public OP120Service(ILogger<OP120Service> logger,
            IOPRepository<OP120> opRepository,
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
            var producreCode = $"{typeof(OP120).Name}";
            var buzKey = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(buzKey);

            // 根据水位读取数据
            var dataTable = await _opRepository.GetDataTableByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                _logger.LogDebug($"【{producreCode}】没有要拉取的数据！");
                return 0;
            }

            // 获取转换数据（基础数据）
            var summaryBo = await ConvertDataTableAsync(dataTable, producreCode, _parameterCodes);

            // 保存数据
            var waterLevel = dataTable.AsEnumerable().Select(s => s["index"].ParseToLong());
            return await _mainService.SaveBaseDataWithCommitAsync(buzKey, waterLevel.Max(m => m), summaryBo);

        }

        /// <summary>
        /// 保存转换数据（附带参数）
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="producreCode"></param>
        /// <param name="parameterCodes"></param>
        /// <returns></returns>
        private async Task<StatorSummaryBo> ConvertDataTableAsync(DataTable dataTable, string producreCode, IEnumerable<string>? parameterCodes = null)
        {
            // 初始化对象
            var statorBo = await _mainService.GetStatorBaseConfigAsync(producreCode);

            var id_key = "ID_stator";
            var barCode_key = "Barcode";

            // 批量读取条码（MES）
            var barCodes = dataTable.AsEnumerable().Select(s => $"{s[barCode_key]}").Distinct();
            var manuSFCEntities = await _mainService.GetSFCEntitiesAsync(statorBo.SiteId, barCodes);

            // 批量读取条码信息（MES）
            var manuSFCInfoEntities = await _mainService.GetSFCInfoEntitiesAsync(manuSFCEntities.Select(s => s.Id));

            // 批量读取物料条码（MES）
            var inventoryEntities = await _mainService.GetMaterialInventoryEntitiesAsync(statorBo.SiteId, barCodes);

            // 批量读取物料信息（MES）
            var materialEntities = await _mainService.GetMaterialEntitiesAsync(inventoryEntities.Select(s => s.MaterialId));

            // 批量读取条码（定子）
            var ids = dataTable.AsEnumerable().Select(s => $"{s[id_key]}").Distinct();
            var statorSFCEntities = await _mainService.GetStatorBarCodeEntitiesAsync(new StatorBarCodeQuery
            {
                SiteId = statorBo.SiteId,
                InnerIds = ids
            });

            // 批量读取条码（定子铜线关系）
            var statorWireRelationEntities = await _mainService.GetStatorWireRelationEntitiesAsync(statorBo.SiteId, ids);

            // 遍历记录
            var summaryBo = new StatorSummaryBo { };
            foreach (DataRow dr in dataTable.Rows)
            {
                // ID是否无效数据
                var statorId = dr[id_key].ParseToLong();
                if (statorId == 0) continue;

                StatorBarCodeEntity? statorSFCEntity = statorSFCEntities.FirstOrDefault(f => f.InnerId == statorId);
                if (statorSFCEntity == null) continue;

                // 添加定子条码关系
                var id_wire1 = dr["ID_wire1"].ParseToLong();
                if (id_wire1 > 0)
                {
                    summaryBo.AddStatorWireRelationEntities.Add(new StatorWireRelationEntity
                    {
                        Id = $"{statorId}{id_wire1}".ToLongID(),
                        InnerId = statorId,
                        WireId = id_wire1,
                        CreatedOn = statorBo.Time,
                        Remark = $"{dr["index"]}",   // 这个ID是为了外层找到对应记录
                        SiteId = statorBo.SiteId
                    });
                }
                var id_wire2 = dr["ID_wire2"].ParseToLong();
                if (id_wire2 > 0)
                {
                    summaryBo.AddStatorWireRelationEntities.Add(new StatorWireRelationEntity
                    {
                        Id = $"{statorId}{id_wire2}".ToLongID(),
                        InnerId = statorId,
                        WireId = id_wire2,
                        CreatedOn = statorBo.Time,
                        Remark = $"{dr["index"]}",   // 这个ID是为了外层找到对应记录
                        SiteId = statorBo.SiteId
                    });
                }

                statorSFCEntity.UpdatedOn = statorBo.Time;
                summaryBo.UpdateStatorBarCodeEntities.Add(statorSFCEntity);

                var time = dr["RDate"].ToTime();
                var barCode = $"{dr[barCode_key]}";

                // 条码是否无效数据
                if (StatorConst.IgnoreString.Contains(barCode) || string.IsNullOrWhiteSpace(barCode)) continue;

                // 条码ID
                var manuSFCStepId = IdGenProvider.Instance.CreateId();
                var manuBadRecordId = IdGenProvider.Instance.CreateId();

                var manuSFCEntity = manuSFCEntities.FirstOrDefault(f => f.SFC == barCode);
                if (manuSFCEntity == null) continue;

                var manuSFCInfoEntity = manuSFCInfoEntities.FirstOrDefault(f => f.SfcId == manuSFCEntity.Id);
                if (manuSFCInfoEntity == null) continue;

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
                    VehicleCode = "",
                    ProcedureId = statorBo.ProcedureId,
                    ResourceId = null,
                    EquipmentId = null,
                    OperationProcedureId = statorBo.ProcedureId,
                    OperationResourceId = null,
                    OperationEquipmentId = null,

                    Remark = $"{dr["index"]}",   // 这个ID是为了外层找到对应记录

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

                var materialId = inventoryEntity?.MaterialId ?? 0;
                var materialEntity = materialEntities.FirstOrDefault(f => f.Id == materialId);
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

                // 铜线这里有点问题
                /*
                // 插入流转记录
                summaryBo.ManuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                {
                    WorkOrderId = statorBo.WorkOrderId,
                    ProductId = 51558094476468224,//TODO materialId,
                    ProcedureId = statorBo.ProcedureId,
                    ResourceId = null,
                    SFC = barCode,

                    CirculationBarCode = statorSFCEntity.InnerBarCode,
                    CirculationWorkOrderId = statorBo.WorkOrderId,
                    CirculationProductId = 51558094067523584,//TODO statorBo.ProductId,
                    CirculationMainProductId = 51558094067523584,//TODOstatorBo.ProductId,
                    CirculationQty = StatorConst.QTY,
                    CirculationType = SfcCirculationTypeEnum.Consume,

                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = statorBo.SiteId,
                    CreatedBy = statorBo.User,
                    CreatedOn = statorBo.Time,
                    UpdatedBy = "OP120",//StatorConst.USER,
                    UpdatedOn = time
                });
                */

                // 如果是不合格
                var isOk = $"{dr["Result"]}" == "OK";
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

                // 读取标准参数
                var parameterEntities = await _mainService.GetParameterEntitiesAsync(parameterCodes, statorBo);
                summaryBo.ProcParameterEntities.AddRange(parameterEntities);

                // 遍历参数
                foreach (var param in parameterEntities)
                {
                    summaryBo.ManuProductParameterEntities.Add(new Core.Domain.Parameter.ManuProductParameterEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ProcedureId = stepEntity.ProcedureId ?? 0,
                        SfcstepId = stepEntity.Id,
                        SFC = stepEntity.SFC,

                        ParameterId = param.Id,
                        ParameterValue = $"{dr[param.ParameterCode]}",
                        ParameterGroupId = 0,
                        CollectionTime = time,

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
    public partial class OP120Service
    {
        /// <summary>
        /// 参数编码集合
        /// </summary>
        private static readonly List<string> _parameterCodes = new()
        {
            "SpindleWorkPositionSpeed",
            "SpindleWork1AxisSpeed",
            "SpindleWorkSingleSpeed",
            "SpindleWorkPosition",
            "SpindleWork1stPosition",
            "SpindleWorkEndPosition",
            "90Degree1stCalibPositionStart",
            "90Degree1stCalibPositionWork",
            "90Degree2ndCalibPositionStart",
            "90Degree2ndCalibPositionWork",
            "90Degree3rdCalibPositionStart",
            "90Degree3rdCalibPositionWork",
            "90Degree4thCalibPositionStart",
            "90Degree4thCalibPositionWork",
            "90Degree5thCalibPositionStart",
            "90Degree5thCalibPositionWork",
            "60Degree1stCalibPositionStart",
            "60Degree1stCalibPositionWork",
            "60Degree2ndCalibPositionStart",
            "60Degree2ndCalibPositionWork",
            "60Degree3rdCalibPositionStart",
            "60Degree3rdCalibPositionWork",
            "60Degree4thCalibPositionStart",
            "60Degree4thCalibPositionWork",
            "60Degree5thCalibPositionStart",
            "60Degree5thCalibPositionWork"
        };
    }
}
