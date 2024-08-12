﻿using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Extension;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Stator;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class MainService : IMainService
    {
        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        private readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（定子条码关系）
        /// </summary>
        private readonly IStatorBarCodeRepository _statorBarCodeRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 仓储接口（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（产品NG记录表）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        /// 仓储接口（产品参数）
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="statorBarCodeRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="manuProductNgRecordRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        public MainService(IWaterMarkService waterMarkService,
            ISysConfigRepository sysConfigRepository,
            IStatorBarCodeRepository statorBarCodeRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcParameterRepository procParameterRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuProductNgRecordRepository manuProductNgRecordRepository,
            IManuProductParameterRepository manuProductParameterRepository)
        {
            _waterMarkService = waterMarkService;
            _sysConfigRepository = sysConfigRepository;
            _statorBarCodeRepository = statorBarCodeRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _procParameterRepository = procParameterRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
        }


        /// <summary>
        /// 获取基础配置（定子）
        /// </summary>
        /// <param name="producreCode"></param>
        /// <returns></returns>
        public async Task<BaseStatorBo> GetStatorBaseConfigAsync(string producreCode = "")
        {
            // 初始化对象
            var baseDto = new BaseStatorBo
            {
                User = "StatorTask",
                Time = HymsonClock.Now()
            };

            // 读取站点配置
            var siteConfigEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (siteConfigEntities == null || !siteConfigEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139))
                    .WithData("name", SysConfigEnum.MainSite.GetDescription());
            }

            // 站点配置
            var siteConfigEntity = siteConfigEntities.FirstOrDefault(f => f.Code == "MainSite")
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12842)).WithData("Msg", "站点配置不存在！");

            // 站点赋值
            baseDto.SiteId = siteConfigEntity.Value.ParseToLong();

            // 填充工序
            if (!string.IsNullOrWhiteSpace(producreCode))
            {
                // 读取当前工序
                var procedureEntity = await _procProcedureRepository.GetByCodeAsync(new EntityByCodeQuery
                {
                    Site = baseDto.SiteId,
                    Code = $"{StatorConst.PRODUCRE_PREFIX}{producreCode}"
                });
                if (procedureEntity == null) return baseDto;
                baseDto.ProcedureId = procedureEntity.Id;
            }

            // 读取产线配置
            var lineConfigEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.NioBaseConfig });
            if (lineConfigEntities == null || !lineConfigEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139))
                    .WithData("name", SysConfigEnum.NioBaseConfig.GetDescription());
            }

            // 定子线配置
            var lineConfigEntity = lineConfigEntities.FirstOrDefault(f => f.Code == "NioStatorConfig")
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12842)).WithData("Msg", "定子线配置不存在！");

            // 定子线赋值
            var configBo = lineConfigEntity.Value.ToDeserialize<NIOConfigBaseDto>();
            if (configBo != null)
            {
                // 读取产线
                var workLineEntity = await _inteWorkCenterRepository.GetByCodeAsync(new EntityByCodeQuery
                {
                    Site = baseDto.SiteId,
                    Code = configBo.ProductionLineId
                });
                if (workLineEntity == null) return baseDto;

                baseDto.WorkLineId = workLineEntity.Id;

                // 读取产线当前激活的工单
                var workOrderEntities = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLineEntity.Id);
                if (workOrderEntities == null || !workOrderEntities.Any()) return baseDto;

                // 这里激活的工单应该只能有一个
                var workOrderEntity = workOrderEntities.FirstOrDefault();
                if (workOrderEntity == null) return baseDto;

                // 填充信息
                baseDto.WorkOrderId = workOrderEntity.Id;
                baseDto.ProductBOMId = workOrderEntity.ProductBOMId;
                baseDto.ProcessRouteId = workOrderEntity.ProcessRouteId;
                baseDto.ProductId = workOrderEntity.ProductId;
            }

            return baseDto;
        }

        /// <summary>
        /// 保存转换数据（附带参数）
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="producreCode"></param>
        /// <param name="parameterCodes"></param>
        /// <returns></returns>
        public async Task<StatorSummaryBo> ConvertDataTableAsync(DataTable dataTable, string producreCode, IEnumerable<string>? parameterCodes = null)
        {
            // 初始化对象
            var statorBo = await GetStatorBaseConfigAsync(producreCode);

            var id_key = "ID";
            var barCode_key = "Barcode";
            switch (producreCode)
            {
                case "OP120":
                    id_key = "ID_stator";
                    break;
                default:
                    id_key = "ID";
                    barCode_key = "Barcode";
                    break;
            }

            // 批量读取条码（MES）
            var barCodes = dataTable.AsEnumerable().Select(s => $"{s[barCode_key]}").Distinct();
            var manuSFCEntities = await GetSFCEntitiesAsync(statorBo.SiteId, barCodes);

            // 批量读取条码信息（MES）
            var manuSFCInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(manuSFCEntities.Select(s => s.Id));

            // 批量读取条码（定子）
            var ids = dataTable.AsEnumerable().Select(s => $"{s[id_key]}").Distinct();
            var statorSFCEntities = await GetStatorBarCodeEntitiesAsync(statorBo.SiteId, ids);

            // 遍历记录
            var summaryBo = new StatorSummaryBo { };
            foreach (DataRow dr in dataTable.Rows)
            {
                // ID是否无效数据
                var statorId = dr[id_key].ParseToLong();
                if (statorId == 0) continue;

                StatorBarCodeEntity? statorSFCEntity = statorSFCEntities.FirstOrDefault(f => f.InnerId == statorId);
                switch (producreCode)
                {
                    case "OP120":
                        if (statorSFCEntity != null)
                        {
                            statorSFCEntity.WireID_1 = dr["ID_wire1"].ParseToLong();
                            statorSFCEntity.WireID_2 = dr["ID_wire2"].ParseToLong();
                            statorSFCEntity.UpdatedOn = statorBo.Time;
                            summaryBo.UpdateStatorBarCodeEntities.Add(statorSFCEntity);
                        }
                        break;
                    case "OP190":
                    default:
                        break;
                }

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

                // 如果是不合格
                var isOk = $"{dr["Result"]}" == "OK";
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

                // 如果没有需要解析的参数
                if (parameterCodes == null || !parameterCodes.Any()) continue;

                // 读取标准参数
                var parameterEntities = await GetParameterEntitiesAsync(parameterCodes, summaryBo.StatorBo);

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

        /// <summary>
        /// 保存转换数据（附带参数）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="barCodes"></param>
        /// <param name="parameterCodes"></param>
        /// <returns></returns>
        public async Task<StatorSummaryBo> ConvertDataListAsync<T>(IEnumerable<T> entities, IEnumerable<string> barCodes, IEnumerable<string>? parameterCodes = null) where T : BaseOPEntity
        {
            var producreCode = $"{typeof(T).Name}";

            // 初始化对象
            var statorBo = await GetStatorBaseConfigAsync(producreCode);

            // 批量读取条码（MES）
            var manuSFCEntities = await GetSFCEntitiesAsync(statorBo.SiteId, barCodes);

            // 批量读取条码信息（MES）
            var manuSFCInfoEntities = await GetSFCInfoEntitiesAsync(manuSFCEntities.Select(s => s.Id));

            // 批量读取条码（定子）
            var statorSFCEntities = await GetStatorBarCodeEntitiesAsync(statorBo.SiteId, entities.Select(s => s.ID).Distinct());

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
                if (StatorConst.IgnoreString.Contains(barCode) || string.IsNullOrWhiteSpace(barCode)) continue;

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

                // 条码信息
                var manuSFCInfoEntity = manuSFCInfoEntities.FirstOrDefault(f => f.SfcId == manuSFCId);
                if (manuSFCInfoEntity == null)
                {
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
                var parameterEntities = await GetParameterEntitiesAsync(parameterCodes, summaryBo.StatorBo);

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

        /// <summary>
        /// 获取参数编码
        /// </summary>
        /// <param name="parameterCodes"></param>
        /// <param name="statorBo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterEntity>> GetParameterEntitiesAsync(IEnumerable<string> parameterCodes, BaseStatorBo statorBo)
        {
            // 插入参数
            await _procParameterRepository.InsertsAsync(parameterCodes.Select(s => new ProcParameterEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                ParameterUnit = "1",
                ParameterCode = s,
                ParameterName = s,
                Remark = StatorConst.USER,

                SiteId = statorBo.SiteId,
                CreatedBy = statorBo.User,
                CreatedOn = statorBo.Time,
                UpdatedBy = statorBo.User,
                UpdatedOn = statorBo.Time
            }));

            // 读取标准参数
            var parameterEntities = await _procParameterRepository.GetByCodesAsync(new Data.Repositories.Process.Query.ProcParametersByCodeQuery
            {
                SiteId = statorBo.SiteId,
                Codes = parameterCodes
            });

            return parameterEntities;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        public async Task<int> SaveBaseDataAsync(StatorSummaryBo summaryBo)
        {
            var rows = 0;

            List<Task<int>> saveTasks = new()
            {
                // 定子数据
                _statorBarCodeRepository.InsertRangeAsync(summaryBo.AddStatorBarCodeEntities),
                _statorBarCodeRepository.UpdateRangeAsync(summaryBo.UpdateStatorBarCodeEntities),

                // 基础信息
                _manuSfcRepository.ReplaceRangeAsync(summaryBo.ManuSFCEntities),
                _manuSfcInfoRepository.ReplaceRangeAsync(summaryBo.ManuSFCInfoEntities),
                _manuSfcStepRepository.InsertRangeAsync(summaryBo.ManuSfcStepEntities),
                _whMaterialInventoryRepository.UpdateQtyAsync(summaryBo.UpdateWhMaterialInventoryEntities),
                _manuSfcCirculationRepository.InsertRangeAsync(summaryBo.ManuSfcCirculationEntities),
                _manuProductBadRecordRepository.InsertRangeAsync(summaryBo.ManuProductBadRecordEntities),
                _manuProductNgRecordRepository.InsertRangeAsync(summaryBo.ManuProductNgRecordEntities),

                // 参数
                _procParameterRepository.InsertsAsync(summaryBo.ProcParameterEntities),
                _manuProductParameterRepository.InsertRangeMavelAsync(summaryBo.ManuProductParameterEntities)
            };

            var rowArray = await Task.WhenAll(saveTasks);
            rows += rowArray.Sum();
            return rows;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="buzKey"></param>
        /// <param name="waterLevel"></param>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        public async Task<int> SaveBaseDataWithCommitAsync(string buzKey, long waterLevel, StatorSummaryBo summaryBo)
        {
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            rows += await SaveBaseDataAsync(summaryBo);
            if (rows > 0)
            {
                rows += await _waterMarkService.RecordWaterMarkAsync(buzKey, waterLevel);
                trans.Complete();
            }

            return rows;
        }



        #region 仓储方法
        /// <summary>
        /// 批量获取（条码）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="barCodes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetSFCEntitiesAsync(long siteId, IEnumerable<string> barCodes)
        {
            // 批量读取条码（MES）
            return await _manuSfcRepository.GetEntitiesAsync(new ManuSfcQuery
            {
                SiteId = siteId,
                SFCs = barCodes
            });
        }

        /// <summary>
        /// 批量获取（条码信息）
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetSFCInfoEntitiesAsync(IEnumerable<long> sfcIds)
        {
            // 批量读取条码信息（MES）
            return await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcIds);
        }

        /// <summary>
        /// 批量获取（物料信息）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetMaterialEntitiesAsync(IEnumerable<long> ids)
        {
            return await _procMaterialRepository.GetByIdsAsync(ids);
        }

        /// <summary>
        /// 批量获取（库存条码）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="barCodes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryEntity>> GetMaterialInventoryEntitiesAsync(long siteId, IEnumerable<string> barCodes)
        {
            // 批量读取条码（MES）
            return await _whMaterialInventoryRepository.GetByBarCodesAsync(new Data.Repositories.Warehouse.WhMaterialInventory.Query.WhMaterialInventoryBarCodesQuery
            {
                SiteId = siteId,
                BarCodes = barCodes
            });
        }

        /// <summary>
        /// 批量获取（定子条码）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StatorBarCodeEntity>> GetStatorBarCodeEntitiesAsync(long siteId, IEnumerable<string> ids)
        {
            // 批量读取条码（定子）
            return await _statorBarCodeRepository.GetEntitiesAsync(new StatorBarCodeQuery
            {
                SiteId = siteId,
                InnerIds = ids
            });

        }

        #endregion

    }
}
