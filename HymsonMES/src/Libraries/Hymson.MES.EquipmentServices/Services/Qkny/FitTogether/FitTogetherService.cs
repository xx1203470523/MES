using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.Utils.Tools;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.Services.Services.EquProductParamRecord;
using Hymson.MES.Services.Services.ManuJzBind;
using Hymson.MES.Services.Dtos.ManuJzBind;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using Hymson.MES.Data.Repositories.ManuJzBind.Query;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder;
using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuJzBind.Command;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Services.ManuJzBindRecord;
using Hymson.MES.Services.Dtos.ManuJzBindRecord;
using Hymson.MES.CoreServices.Services.Qkny;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.EquipmentServices.Services.Qkny.WhMaterialInventory;

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
            IWhMaterialInventoryService whMaterialInventoryService)
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
        }

        /// <summary>
        /// 多极组产品出站031
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<OutboundMoreReturnDto>> OutboundMultPolarAsync(OutboundMultPolarDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 构造数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            List<OutStationRequestBo> outStationRequestBos = new();
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
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

                //出站参数
                List<EquProductParamRecordSaveDto> curSfcParamList = new List<EquProductParamRecordSaveDto>();
                foreach (var paramItem in item.ParamList)
                {
                    EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                    saveDto.ParamCode = paramItem.ParamCode;
                    saveDto.ParamValue = paramItem.ParamValue;
                    saveDto.CollectionTime = paramItem.CollectionTime;
                    curSfcParamList.Add(saveDto);
                }
                curSfcParamList.ForEach(m =>
                {
                    m.SiteId = equResModel.SiteId;
                    m.Sfc = item.Sfc;
                    m.EquipmentId = equResModel.EquipmentId;
                    m.CreatedOn = HymsonClock.Now();
                    m.CreatedBy = dto.EquipmentCode;
                    m.UpdatedOn = m.CreatedOn;
                    m.UpdatedBy = m.CreatedBy;
                });
                saveDtoList.AddRange(curSfcParamList);
            }
            outBo.OutStationRequestBos = outStationRequestBos;
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
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
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
            else if(dto.OperationType == "3")
            {
                if(string.IsNullOrEmpty(jzModel.Sfc) == true)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45271));
                }
                //以电芯做进出站
                inBo.SFCs = new List<string>() { jzModel.Sfc };
                await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
            }

            ////4. 返回
            //List<InboundMoreReturnDto> resultList = new List<InboundMoreReturnDto>();
            //foreach (var item in sfcList)
            //{
            //    InboundMoreReturnDto model = new InboundMoreReturnDto();
            //    model.Sfc = item;
            //    resultList.Add(model);
            //}
        }

        /// <summary>
        /// 电芯码下发033
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<string>> GenerateCellSfcAsync(GenerateCellSfcDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //1.1 获取工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
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
        /// 电芯极组绑定产品出站032
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundSfcPolarAsync(OutboundSfcPolarDto dto)
        {
            //TODO
            //添加清除极组在制品信息

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
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
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

            //出站参数
            List<EquProductParamRecordSaveDto> curSfcParamList = new List<EquProductParamRecordSaveDto>();
            foreach (var paramItem in dto.ParamList)
            {
                EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                saveDto.ParamCode = paramItem.ParamCode;
                saveDto.ParamValue = paramItem.ParamValue;
                saveDto.CollectionTime = paramItem.CollectionTime;
                curSfcParamList.Add(saveDto);
            }
            curSfcParamList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.Sfc = dto.Sfc;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            saveDtoList.AddRange(curSfcParamList);
            #endregion

            #region 极组出站数据
            foreach(var item in jzSfcList)
            {
                var jzOutStationRequestBo = new OutStationRequestBo
                {
                    SFC = item,
                    IsQualified = true,
                };
                outStationRequestBos.Add(outStationRequestBo);
            }
            #endregion
            outBo.OutStationRequestBos = outStationRequestBos;

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
            bindDto.Sfc = jzModel.Sfc;
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
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
            await _manuSfcProduceService.DeletePhysicalRangeAsync(jzCommand);
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
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
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

            //出站参数
            List<EquProductParamRecordSaveDto> curSfcParamList = new List<EquProductParamRecordSaveDto>();
            foreach (var paramItem in dto.ParamList)
            {
                EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                saveDto.ParamCode = paramItem.ParamCode;
                saveDto.ParamValue = paramItem.ParamValue;
                saveDto.CollectionTime = paramItem.CollectionTime;
                curSfcParamList.Add(saveDto);
            }
            curSfcParamList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.Sfc = dto.Sfc;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            saveDtoList.AddRange(curSfcParamList);

            //4. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            if(dto.OperationType == "3")
            {
                await _manuJzBindService.DeletePhysicsAsync(jzModel.Id);
            }
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
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
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResLineAsync(dto);
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //2. 查询极组条码是否已经存在
            WhMaterialInventoryBarCodeQuery whQuery = new WhMaterialInventoryBarCodeQuery();
            whQuery.SiteId = equResModel.SiteId;
            whQuery.BarCode = dto.Sfc;
            var whInfo = await _whMaterialInventoryService.GetByBarCodeAsync(whQuery);
            if (whInfo != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45230));
            }

            //条码接收的动作
            long materialId = planEntity.ProductId;
            string sfc = dto.Sfc;
            int qty = 1;

            //车间库存接收
            //走库存接收逻辑
        }

    }
}
