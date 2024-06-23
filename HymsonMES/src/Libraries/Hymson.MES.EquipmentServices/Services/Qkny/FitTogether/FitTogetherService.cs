using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuBind;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.CoreServices.Services.Qkny;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuJzBind.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.ManuJzBind.Query;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder;
using Hymson.MES.EquipmentServices.Services.Qkny.WhMaterialInventory;
using Hymson.MES.EquipmentServices.Validators.EquVerifyHelper;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.MES.Services.Dtos.ManuJzBind;
using Hymson.MES.Services.Dtos.ManuJzBindRecord;
using Hymson.MES.Services.Services.EquProductParamRecord;
using Hymson.MES.Services.Services.ManuJzBind;
using Hymson.MES.Services.Services.ManuJzBindRecord;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.Qkny.FitTogether
{
    /// <summary>
    /// 装配
    /// </summary>
    public class FitTogetherService : IFitTogetherService
    {
        /// <summary>
        /// 设备服务
        /// </summary>
        private readonly IEquEquipmentService _equEquipmentService;

        /// <summary>
        /// 服务接口（过站）
        /// </summary>
        private readonly IManuPassStationService _manuPassStationService;

        /// <summary>
        /// 产品参数
        /// </summary>
        private readonly IEquProductParamRecordService _equProductParamRecordService;

        /// <summary>
        /// 极组绑定
        /// </summary>
        private readonly IManuJzBindService _manuJzBindService;

        /// <summary>
        /// 极组绑定记录
        /// </summary>
        private readonly IManuJzBindRecordService _manuJzBindRecordService;

        /// <summary>
        /// 业务接口（创建条码服务）
        /// </summary>
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderService _planWorkOrderService;

        /// <summary>
        /// 在制品
        /// </summary>
        private readonly IManuSfcProduceService _manuSfcProduceService;

        /// <summary>
        /// 物料库存
        /// </summary>
        private readonly IWhMaterialInventoryService _whMaterialInventoryService;

        /// <summary>
        /// 多语言
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 条码合并
        /// </summary>
        private readonly IManuMergeService _manuMergeService;

        /// <summary>
        /// 参数收集
        /// </summary>
        private readonly IManuProductParameterService _manuProductParameterService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FitTogetherService(IEquEquipmentService equEquipmentService,
            IManuPassStationService manuPassStationService,
            IEquProductParamRecordService equProductParamRecordService,
            IManuJzBindService manuJzBindService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IPlanWorkOrderService planWorkOrderService,
            IManuJzBindRecordService manuJzBindRecordService,
            IManuSfcProduceService manuSfcProduceService,
            IWhMaterialInventoryService whMaterialInventoryService,
            ILocalizationService localizationService,
            IManuMergeService manuMergeService,
            IManuProductParameterService manuProductParameterService)
        {
            _equEquipmentService = equEquipmentService;
            _manuPassStationService = manuPassStationService;
            _equProductParamRecordService = equProductParamRecordService;
            _manuJzBindService = manuJzBindService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _planWorkOrderService = planWorkOrderService;
            _manuJzBindRecordService = manuJzBindRecordService;
            _manuSfcProduceService = manuSfcProduceService;
            _whMaterialInventoryService = whMaterialInventoryService;
            _localizationService = localizationService;
            _manuMergeService = manuMergeService;
            _manuProductParameterService = manuProductParameterService;
        }

        /// <summary>
        /// 多极组产品出站031
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<OutboundMoreReturnDto>> OutboundMultPolarAsync(OutboundMultPolarDto dto)
        {
            EquVerifyHelper.OutboundMultPolarDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);
            //2. 构造数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            List<OutStationRequestBo> outStationRequestBos = new();
            foreach (var item in dto.SfcList)
            {
                //出站数据
                var outStationRequestBo = new OutStationRequestBo
                {
                    SFC = item.Sfc,
                    IsQualified = item.Passed == 1
                };
                // 消耗条码
                if (item.BindFeedingCodeList != null && item.BindFeedingCodeList.Any())
                {
                    outStationRequestBo.ConsumeList = item.BindFeedingCodeList.Select(s => new OutStationConsumeBo { BarCode = s });
                }
                // 不合格代码
                if (item.NgList != null && item.NgList.Any())
                {
                    outStationRequestBo.OutStationUnqualifiedList = item.NgList.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s });
                }
                outStationRequestBos.Add(outStationRequestBo);
            }
            outBo.OutStationRequestBos = outStationRequestBos;

            //产品过程参数
            ProductParameterCollectBo parameterCollectBo = new()
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                Time = HymsonClock.Now(),
                ProcedureId = equResModel.ProcedureId,
                ResourceId = equResModel.ResId,
                SFCList = dto.SfcList.Select(x => new ProductParameterCollectInfo
                {
                    SFC = x.Sfc,
                    Parameters = x.ParamList.Select(z => new ProductParameterBo
                    {
                        ParameterCode = z.ParamCode,
                        ParameterValue = z.ParamValue,
                        CollectionTime = z.CollectionTime
                    })
                })
            };

            //2.1 绑定记录
            ManuJzBindSaveDto bindDto = new ManuJzBindSaveDto();
            bindDto.EquipmentId = equResModel.EquipmentId;
            bindDto.JzSfc1 = dto.SfcList[0].Sfc;
            bindDto.JzSfc2 = dto.SfcList[1].Sfc;
            bindDto.Sfc = "";
            bindDto.BindType = "1";
            bindDto.CreatedOn = HymsonClock.Now();
            bindDto.CreatedBy = dto.EquipmentCode;
            bindDto.UpdatedBy = dto.EquipmentCode;
            bindDto.UpdatedOn = bindDto.CreatedOn;
            bindDto.SiteId = equResModel.SiteId;
            //3. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuJzBindService.AddAsync(bindDto);
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _manuProductParameterService.ProductProcessCollectAsync(parameterCollectBo);
            trans.Complete();
            //4. 返回
            List<OutboundMoreReturnDto> resultList = new List<OutboundMoreReturnDto>();
            foreach (var item in dto.SfcList)
            {
                OutboundMoreReturnDto model = new OutboundMoreReturnDto();
                model.Sfc = item.Sfc;
                resultList.Add(model);
            }

            return resultList;
        }

        /// <summary>
        /// 绑定后极组单个条码进站049
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task InboundBindJzSingleAsync(InboundBindJzSingleDto dto)
        {
            EquVerifyHelper.InboundBindJzSingleDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);

            //1.1 获取当前极组绑定的两个极组
            ManuJzBindQuery query = new ManuJzBindQuery();
            query.JzSfc = dto.Sfc;
            query.SiteId = equResModel.SiteId;
            var jzModel = await _manuJzBindService.GetByJzSfcAsync(query);

            SFCInStationBo inBo = new SFCInStationBo();
            inBo.SiteId = equResModel.SiteId;
            inBo.UserName = equResModel.EquipmentCode;
            inBo.ProcedureId = equResModel.ProcedureId;
            inBo.ResourceId = equResModel.ResId;
            inBo.EquipmentId = equResModel.EquipmentId;

            //首次发生以极组做进出站
            if (dto.OperationType == "1")
            {
                //2. 构造数据
                List<string> sfcList = new List<string>() { jzModel.JzSfc1, jzModel.JzSfc2 };
                inBo.SFCs = sfcList;
                //3. 进站
                await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
            }
            else if (dto.OperationType == "3")
            {
                if (string.IsNullOrEmpty(jzModel.Sfc) == true)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45271))
                        .WithData("Sfc", dto.Sfc);
                }
                //以电芯做进出站
                inBo.SFCs = new List<string>() { jzModel.Sfc };
                await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
            }
        }

        /// <summary>
        /// 生成电芯码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<string>> GenerateCellSfcAsync(GenerateCellSfcDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //1.1 获取工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel);
            //2. 构造数据
            CreateBarcodeByWorkOrderBo query = new CreateBarcodeByWorkOrderBo();
            query.WorkOrderId = planEntity.Id;
            query.ResourceId = equResModel.ResId;
            query.SiteId = equResModel.SiteId;
            query.Qty = dto.Qty;
            query.ProcedureId = equResModel.ProcedureId;
            query.UserName = equResModel.EquipmentCode;
            //3. 数据库操作
            var sfcObjList = await _manuCreateBarcodeService.CreateBarcodeByWorkOrderIdAsync(query, null);

            List<string> sfcList = sfcObjList.Select(m => m.SFC).ToList();
            return sfcList;
        }

        /// <summary>
        /// 生成24位国标电芯码
        /// 电芯码下发033
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> Create24GbCodeAsync(GenerateDxSfcDto dto)
        {
            EquVerifyHelper.GenerateDxSfcDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);
            //2. 查询极组信息
            ManuJzBindQuery query = new ManuJzBindQuery();
            query.JzSfc = dto.Sfc;
            query.SiteId = equResModel.SiteId;
            var jzModel = await _manuJzBindService.GetByJzSfcAsync(query);
            //生成电芯码参数
            CreateCellBarcodeBo param = new CreateCellBarcodeBo();
            param.Barcodes = new[] { jzModel.JzSfc1, jzModel.JzSfc2 };
            param.SiteId = equResModel.SiteId;
            param.UserName = dto.EquipmentCode;
            param.ProcedureId = equResModel.ProcedureId;
            param.EquipmentId = equResModel.EquipmentId;
            param.ResourceId = equResModel.ResId;

            return await _manuCreateBarcodeService.CreateCellBarCodeBySfcAsync(param);
        }

        /// <summary>
        /// 接收24位电芯码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task Receive24GbCodeAsync(RecviceDxSfcDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //1.1 获取工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel);
            //接收电芯码参数
            ReceiveCellBarcodeBo param = new ReceiveCellBarcodeBo();
            param.EquipmentId = equResModel.EquipmentId;
            param.ResourceId = equResModel.ResId;
            param.ProcedureId = equResModel.ProcedureId;
            param.LineId = equResModel.LineId;
            param.WorkOrderId = planEntity.Id;
            param.ProductId = planEntity.ProductId;
            param.BomId = planEntity.ProductBOMId;
            param.ProcessRouteId = planEntity.ProcessRouteId;
            param.SfcList = dto.SfcList;
            param.SiteId = equResModel.SiteId;
            param.UserName = dto.EquipmentCode;

            await _manuCreateBarcodeService.ReceiveCellBarCodeBySfcAsync(param);
        }

        /// <summary>
        /// 电芯极组绑定产品出站032
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundSfcPolarAsync(OutboundSfcPolarDto dto)
        {
            EquVerifyHelper.OutboundSfcPolarDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);
            //2. 查询极组信息
            ManuJzBindQuery query = new ManuJzBindQuery();
            query.JzSfc = dto.JzSfc;
            query.SiteId = equResModel.SiteId;
            var jzModel = await _manuJzBindService.GetByJzSfcAsync(query);
            List<string> jzSfcList = new List<string>() { jzModel.JzSfc1, jzModel.JzSfc2 };

            //3. 构造数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            List<OutStationRequestBo> outStationRequestBos = new();
            #region 电芯出站数据
            var outStationRequestBo = new OutStationRequestBo
            {
                SFC = dto.Sfc,
                IsQualified = dto.Passed == 1
            };
            // 消耗条码
            if (dto.BindFeedingCodeList != null && dto.BindFeedingCodeList.Any())
            {
                outStationRequestBo.ConsumeList = dto.BindFeedingCodeList.Select(s => new OutStationConsumeBo { BarCode = s });
            }
            // 不合格代码
            if (dto.NgList != null && dto.NgList.Any())
            {
                outStationRequestBo.OutStationUnqualifiedList = dto.NgList.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s });
            }
            outStationRequestBos.Add(outStationRequestBo);
            //极组出站数据
            foreach(var item in jzSfcList)
            {
                if(dto.JzIsOut == false) //决定极组是否出站
                {
                    continue;
                }
                var jzOutStationRequestBo = new OutStationRequestBo
                {
                    SFC = item,
                    //电芯极组认为一样，即都合格或者都NG
                    IsQualified = dto.Passed == 1
                };
                // 消耗条码
                if (dto.BindFeedingCodeList != null && dto.BindFeedingCodeList.Any())
                {
                    outStationRequestBo.ConsumeList = dto.BindFeedingCodeList.Select(s => new OutStationConsumeBo { BarCode = s });
                }
                // 不合格代码，电芯极组认为一样
                if (dto.NgList != null && dto.NgList.Any())
                {
                    outStationRequestBo.OutStationUnqualifiedList = dto.NgList.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s });
                }
                outStationRequestBos.Add(outStationRequestBo);
            }

            outBo.OutStationRequestBos = outStationRequestBos;

            //产品过程参数
            var parameterBo = new ProductProcessParameterBo
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                Time = HymsonClock.Now(),
                ProcedureId = equResModel.ProcedureId,
                ResourceId = equResModel.ResId,
                SFCs = new[] { dto.Sfc },
                Parameters = dto.ParamList.Select(x => new ProductParameterBo
                {
                    ParameterCode = x.ParamCode,
                    ParameterValue = x.ParamValue,
                    CollectionTime = x.CollectionTime
                })
            };
            #endregion

            //极组绑定数据更新
            UpdateSfcByIdCommand command = new UpdateSfcByIdCommand();
            command.Sfc = dto.Sfc;
            command.Id = jzModel.Id;
            command.UpdatedOn = HymsonClock.Now();
            command.UpdatedBy = dto.EquipmentCode;

            //记录绑定记录添加
            ManuJzBindRecordSaveDto bindDto = new ManuJzBindRecordSaveDto();
            bindDto.EquipmentId = equResModel.EquipmentId;
            bindDto.JzSfc1 = jzModel.JzSfc1;
            bindDto.JzSfc2 = jzModel.JzSfc2;
            bindDto.Sfc = dto.Sfc;
            bindDto.BindType = "1";
            bindDto.CreatedOn = HymsonClock.Now();
            bindDto.CreatedBy = dto.EquipmentCode;
            bindDto.UpdatedBy = dto.EquipmentCode;
            bindDto.UpdatedOn = bindDto.CreatedOn;
            bindDto.SiteId = equResModel.SiteId;

            //极组在制品信息删除
            DeletePhysicalBySfcsCommand jzCommand = new DeletePhysicalBySfcsCommand();
            jzCommand.SiteId = equResModel.SiteId;
            jzCommand.Sfcs = new List<string>() { jzModel.JzSfc1, jzModel.JzSfc2 };

            //3. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuJzBindService.UpdateSfcById(command);
            await _manuJzBindRecordService.AddAsync(bindDto);
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _manuSfcProduceService.DeletePhysicalRangeAsync(jzCommand);
            await _manuProductParameterService.ProductProcessCollectAsync(parameterBo);
            trans.Complete();
        }

        /// <summary>
        /// 绑定后极组单个条码出站050
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundBindJzSingle")]
        [LogDescription("绑定后极组单个条码出站050", BusinessType.OTHER, "OutboundBindJzSingle050", ReceiverTypeEnum.MES)]
        public async Task OutboundBindJzSingleAsync(OutboundBindJzSingleDto dto)
        {
            EquVerifyHelper.OutboundBindJzSingleDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);

            //2. 获取当前极组绑定的两个极组
            ManuJzBindQuery query = new ManuJzBindQuery();
            query.JzSfc = dto.Sfc;
            query.SiteId = equResModel.SiteId;
            var jzModel = await _manuJzBindService.GetByJzSfcAsync(query);

            //3. 构造数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            List<OutStationRequestBo> outStationRequestBos = new();
            var outStationRequestBo = new OutStationRequestBo
            {
                SFC = jzModel.Sfc,
                IsQualified = dto.Passed == 1
            };
            // 消耗条码
            if (dto.BindFeedingCodeList != null && dto.BindFeedingCodeList.Any())
            {
                outStationRequestBo.ConsumeList = dto.BindFeedingCodeList.Select(s => new OutStationConsumeBo { BarCode = s });
            }
            // 不合格代码
            if (dto.NgList != null && dto.NgList.Any())
            {
                outStationRequestBo.OutStationUnqualifiedList = dto.NgList.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s });
            }
            outStationRequestBos.Add(outStationRequestBo);
            outBo.OutStationRequestBos = outStationRequestBos;

            //产品过程参数
            var parameterBo = new ProductProcessParameterBo
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                Time = HymsonClock.Now(),
                ProcedureId = equResModel.ProcedureId,
                ResourceId = equResModel.ResId,
                SFCs = new[] { jzModel.Sfc },
                Parameters = dto.ParamList.Select(x => new ProductParameterBo
                {
                    ParameterCode = x.ParamCode,
                    ParameterValue = x.ParamValue,
                    CollectionTime = x.CollectionTime
                })
            };

            //4. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            if (dto.OperationType == "3")
            {
                await _manuJzBindService.DeletePhysicsAsync(jzModel.Id);
            }
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _manuProductParameterService.ProductProcessCollectAsync(parameterBo);
            //清除极组在制
            trans.Complete();
        }

        /// <summary>
        /// 卷绕极组产出上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CollingPolarAsync(CollingPolarDto dto)
        {
            EquVerifyHelper.CollingPolarDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel);
            //2. 查询极组条码是否已经存在
            WhMaterialInventoryBarCodeQuery whQuery = new WhMaterialInventoryBarCodeQuery();
            whQuery.SiteId = equResModel.SiteId;
            whQuery.BarCode = dto.Sfc;
            var whInfo = await _whMaterialInventoryService.GetByBarCodeAsync(whQuery);
            if (whInfo != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45230))
                    .WithData("Sfc", dto.Sfc);
            }
            //3. 构造数据
            //3.1 外部条码下达
            var barcodeCreateBo = new CreateBarcodeByExternalSFCBo
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                EquipmentId = equResModel.EquipmentId,
                ResourceId = equResModel.ResId,
                ProcedureId = equResModel.ProcedureId,
                WorkOrderId = planEntity.Id,
                ExternalSFCs = new BarcodeDto[] { new() { SFC = dto.Sfc, Qty = 1 } }
            };
            //3.2 进站
            var inStationBo = new SFCInStationBo
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                EquipmentId = equResModel.EquipmentId,
                ResourceId = equResModel.ResId,
                ProcedureId = equResModel.ProcedureId,
                SFCs = new[] { dto.Sfc }
            };
            //3.1 出站
            var outStationRequestBo = new OutStationRequestBo()
            {
                SFC = dto.Sfc,
                IsQualified = dto.Passed == 1,
                OutStationUnqualifiedList = dto.NgList?.Select(x => new OutStationUnqualifiedBo { UnqualifiedCode = x })
            };
            var outStationBo = new SFCOutStationBo
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                EquipmentId = equResModel.EquipmentId,
                ResourceId = equResModel.ResId,
                ProcedureId = equResModel.ProcedureId,
                OutStationRequestBos = new OutStationRequestBo[] { outStationRequestBo }
            };

            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);

            //外部条码下达
            await _manuCreateBarcodeService.CreateBarcodeByExternalSFCAsync(barcodeCreateBo, _localizationService);
            //进站
            await _manuPassStationService.InStationRangeBySFCAsync(inStationBo);
            //出站
            await _manuPassStationService.OutStationRangeBySFCAsync(outStationBo);

            trans.Complete();
        }

    }
}
