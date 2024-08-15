﻿using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Extension;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP050Service : IOP050Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP050Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP050> _opRepository;

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
        public OP050Service(ILogger<OP050Service> logger,
            IOPRepository<OP050> opRepository,
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
            var producreCode = $"{typeof(OP050).Name}";
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

            var id_key = "ID";
            var ids = dataTable.AsEnumerable().Select(s => $"{s[id_key]}").Distinct();

            // 批量读取条码（铜线）
            var wireSFCEntities = await _mainService.GetWireBarCodeEntitiesAsync(statorBo.SiteId, ids);

            // 批量读取条码（MES）
            var barCodes = wireSFCEntities.Select(s => s.WireBarCode).Distinct();
            var manuSFCEntities = await _mainService.GetSFCEntitiesAsync(statorBo.SiteId, barCodes);

            // 批量读取条码信息（MES）
            var manuSFCInfoEntities = await _mainService.GetSFCInfoEntitiesAsync(manuSFCEntities.Select(s => s.Id));

            // 遍历记录
            var summaryBo = new StatorSummaryBo { };
            foreach (DataRow dr in dataTable.Rows)
            {
                // ID是否无效数据
                var wireId = dr[id_key].ParseToLong();
                if (wireId == 0) continue;

                WireBarCodeEntity? wireEntity = wireSFCEntities.FirstOrDefault(f => f.WireId == wireId);
                if (wireEntity == null) continue;

                var time = dr["RDate"].ToTime();

                // 条码ID
                var manuSFCStepId = IdGenProvider.Instance.CreateId();
                var manuBadRecordId = IdGenProvider.Instance.CreateId();

                var barCode = wireEntity.WireBarCode;
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
    public partial class OP050Service
    {
        /// <summary>
        /// 参数编码集合
        /// </summary>
        private static readonly List<string> _parameterCodes = new()
        {
            "pressCount",
            "CrimpingPosition01",
            "CrimpingPressDistance01",
            "CrimpingPressLoad01",
            "CrimpingPosition02",
            "CrimpingPressDistance02",
            "CrimpingPressLoad02",
            "CrimpingPosition03",
            "CrimpingPressDistance03",
            "CrimpingPressLoad03",
            "CrimpingPosition04",
            "CrimpingPressDistance04",
            "CrimpingPressLoad04",
            "CrimpingPosition05",
            "CrimpingPressDistance05",
            "CrimpingPressLoad05",
            "CrimpingPosition06",
            "CrimpingPressDistance06",
            "CrimpingPressLoad06",
            "CrimpingPosition07",
            "CrimpingPressDistance07",
            "CrimpingPressLoad07",
            "CrimpingPosition08",
            "CrimpingPressDistance08",
            "CrimpingPressLoad08",
            "CrimpingPosition09",
            "CrimpingPressDistance09",
            "CrimpingPressLoad09",
            "CrimpingPosition10",
            "CrimpingPressDistance10",
            "CrimpingPressLoad10",
            /*
            "CrimpingPosition11",
            "CrimpingPressDistance11",
            "CrimpingPressLoad11",
            "CrimpingPosition12",
            "CrimpingPressDistance12",
            "CrimpingPressLoad12",
            "CrimpingPosition13",
            "CrimpingPressDistance13",
            "CrimpingPressLoad13",
            "CrimpingPosition14",
            "CrimpingPressDistance14",
            "CrimpingPressLoad14",
            "CrimpingPosition15",
            "CrimpingPressDistance15",
            "CrimpingPressLoad15"
            */
        };

    }
}
