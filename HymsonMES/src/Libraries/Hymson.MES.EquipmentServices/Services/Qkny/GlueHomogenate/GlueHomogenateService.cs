using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Qkny;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Hymson.MES.EquipmentServices.Services.Qkny.Formula;
using Hymson.MES.EquipmentServices.Services.Qkny.LoadPoint;
using Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder;
using Hymson.MES.EquipmentServices.Services.Qkny.WhMaterialInventory;
using Hymson.MES.Services.Dtos.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord;
using Hymson.MES.Services.Dtos.ManuFeedingTransferRecord;
using Hymson.MES.Services.Services.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Services.Services.ManuFeedingNoProductionRecord;
using Hymson.MES.Services.Services.ManuFeedingTransferRecord;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.Qkny.GlueHomogenate
{
    /// <summary>
    /// 制胶匀浆工序服务
    /// </summary>
    public class GlueHomogenateService : IGlueHomogenateService
    {
        #region 验证器

        /// <summary>
        /// 配方列表获取
        /// </summary>
        private readonly AbstractValidator<FormulaListGetDto> _validationFormulaListGetDto;

        /// <summary>
        /// 配方详情获取
        /// </summary>
        private readonly AbstractValidator<FormulaDetailGetDto> _validationFormulaDetailGetDto;

        /// <summary>
        /// 配方版本校验
        /// </summary>
        private readonly AbstractValidator<FormulaVersionExamineDto> _validationFormulaVersionExamine;

        #endregion

        /// <summary>
        /// 设备服务
        /// </summary>
        private readonly IEquEquipmentService _equEquipmentService;

        /// <summary>
        /// 配方
        /// </summary>
        private readonly IProcFormulaService _procFormulaService;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderService _planWorkOrderService;

        /// <summary>
        /// 上料点
        /// </summary>
        private readonly IProcLoadPointService _procLoadPointService;

        /// <summary>
        /// 物料库存
        /// </summary>
        private readonly IWhMaterialInventoryService _whMaterialInventoryService;

        /// <summary>
        /// 上料
        /// </summary>
        private readonly IManuFeedingService _manuFeedingService;

        /// <summary>
        /// 上料完成记录(制胶匀浆)
        /// </summary>
        private readonly IManuFeedingCompletedZjyjRecordService _manuFeedingCompletedZjyjRecordService;

        /// <summary>
        /// 业务接口（创建条码服务）
        /// </summary>
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;

        /// <summary>
        /// 服务接口（过站）
        /// </summary>
        private readonly IManuPassStationService _manuPassStationService;

        /// <summary>
        /// 批次转移
        /// </summary>
        private readonly IManuFeedingTransferRecordService _manuFeedingTransferRecordService;

        /// <summary>
        /// 设备投料非生产投料
        /// </summary>
        private readonly IManuFeedingNoProductionRecordService _manuFeedingNoProductionRecordService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GlueHomogenateService(IEquEquipmentService equEquipmentService,
            IProcFormulaService procFormulaService,
            IPlanWorkOrderService planWorkOrderService,
            IProcLoadPointService procLoadPointService,
            IWhMaterialInventoryService whMaterialInventoryService,
            IManuFeedingService manuFeedingService,
            IManuFeedingCompletedZjyjRecordService manuFeedingCompletedZjyjRecordService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IManuPassStationService manuPassStationService,
            IManuFeedingTransferRecordService manuFeedingTransferRecordService,
            IManuFeedingNoProductionRecordService manuFeedingNoProductionRecordService,
            AbstractValidator<FormulaListGetDto> validationFormulaListGetDto,
            AbstractValidator<FormulaDetailGetDto> validationFormulaDetailGetDto,
            AbstractValidator<FormulaVersionExamineDto> validationFormulaVersionExamine
            )
        {
            _equEquipmentService = equEquipmentService;
            _procFormulaService = procFormulaService;
            _planWorkOrderService = planWorkOrderService;
            _procLoadPointService = procLoadPointService;
            _whMaterialInventoryService = whMaterialInventoryService;
            _manuFeedingService = manuFeedingService;
            _manuFeedingCompletedZjyjRecordService = manuFeedingCompletedZjyjRecordService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _manuPassStationService = manuPassStationService;
            _manuFeedingTransferRecordService = manuFeedingTransferRecordService;
            _manuFeedingNoProductionRecordService = manuFeedingNoProductionRecordService;
            _validationFormulaListGetDto = validationFormulaListGetDto;
            _validationFormulaDetailGetDto = validationFormulaDetailGetDto;
            _validationFormulaVersionExamine = validationFormulaVersionExamine;
        }

        /// <summary>
        /// 获取配方列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<FormulaListGetReturnDto>> FormulaListGetAsync(FormulaListGetDto dto)
        {
            await _validationFormulaListGetDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 获取数据
            ProcFormulaListQueryDto query = new ProcFormulaListQueryDto();
            query.EquipmentId = equResModel.EquipmentId;
            query.ProductCode = dto.ProductCode;
            var list = await _procFormulaService.GetFormulaListAsync(query);
            List<FormulaListGetReturnDto> resultList = new List<FormulaListGetReturnDto>();
            foreach (var item in list)
            {
                FormulaListGetReturnDto result = new FormulaListGetReturnDto();
                result.FormulaCode = item.Code;
                result.ProductCode = item.MaterialCode;
                result.Version = item.Version;
                result.LastUpdateOnTime = item.UpdatedOn ?? item.CreatedOn;
                resultList.Add(result);
            }
            return resultList;
        }

        /// <summary>
        /// 获取配方明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<FormulaDetailGetReturnDto> FormulaDetailGetAsync(FormulaDetailGetDto dto)
        {
            await _validationFormulaDetailGetDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 获取数据
            ProcFormulaDetailQueryDto query = new ProcFormulaDetailQueryDto();
            query.FormulaCode = dto.FormulaCode;
            query.SiteId = equResModel.SiteId;
            var list = await _procFormulaService.GetFormulaDetailAsync(query);
            FormulaDetailGetReturnDto result = new FormulaDetailGetReturnDto();
            result.Version = list[0].Version;
            foreach (var item in list)
            {
                FormulaParamList paramModel = new FormulaParamList();
                paramModel.SepOrder = item.Serial;
                paramModel.Category = item.OperationCode;
                paramModel.MarterialCode = item.MaterialCode;
                paramModel.MarerialGroupCode = item.MaterialGroupCode;
                paramModel.ParameCode = item.ParameterCode;
                paramModel.ParamValue = item.Setvalue;
                paramModel.FunctionCode = item.FunctionName;
                paramModel.Unit = item.Unit;

                result.ParamList.Add(paramModel);
            }

            return result;
        }

        /// <summary>
        /// 配方校验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task FormulaVersionExamineAsync(FormulaVersionExamineDto dto)
        {
            await _validationFormulaVersionExamine.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 版本校验
            ProcFormlaGetByCodeVersionDto query = new ProcFormlaGetByCodeVersionDto();
            query.FormulaCode = dto.FormulaCode;
            query.Version = dto.Version;
            query.SiteId = equResModel.SiteId;
            var entity = await _procFormulaService.GetEntityByCodeVersion(query);
        }

        /// <summary>
        /// 设备投料前校验(制胶匀浆)017
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ConsumeEquBeforeCheckAsync(ConsumeEquBeforeCheckDto dto)
        {
            //TODO 业务说明
            //1. 缓存罐(上料点)一次只能有一种物料
            //2. 从缓存罐投料到搅拌机（中间的计量罐不管）
            //3. 设备上传的具体条码不管，即使有多个条码，型号也是一致，直接取缓存罐最后一次上料的

            //1. 获取设备基础信息
            QknyBaseDto equDto = new QknyBaseDto();
            equDto.EquipmentCode = dto.ConsumeEquipmentCode;
            equDto.ResourceCode = dto.ConsumeResourceCodeCode;
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 查询设备激活工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 查询上料点当前物料（缓存罐(上料点)一次只能有一种物料）
            //3.1 查询上料点
            //ProcLoadPointQuery pointQuery = new ProcLoadPointQuery();
            //pointQuery.SiteId = equResModel.SiteId;
            //pointQuery.LoadPoint = dto.EquipmentCode;
            //var loadPoint = await _procLoadPointService.GetProcLoadPointEntitiesAsync(pointQuery);
            WhMaterialInventoryBarCodeQuery whMaterialQuery = new WhMaterialInventoryBarCodeQuery();
            whMaterialQuery.SiteId = equResModel.SiteId;
            whMaterialQuery.BarCode = dto.SfcList.IsNullOrEmpty() == true ? "" : dto.SfcList[0];
            var sfcMaterial = await _whMaterialInventoryService.GetByBarCodeAsync(whMaterialQuery);
            //4. 校验物料是否在激活工单对应的BOM里
            var curPointMaterialId = sfcMaterial.MaterialId;
            var orderMaterialList = await _planWorkOrderService.GetWorkOrderMaterialAsync(planEntity.ProductBOMId);
            if (orderMaterialList.Contains(curPointMaterialId) == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45080));
            }
        }

        /// <summary>
        /// 设备投料(制胶匀浆)018
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ConsumeEquAsync(ConsumeEquDto dto)
        {
            //1. 获取设备基础信息
            QknyBaseDto equDto = new QknyBaseDto();
            equDto.EquipmentCode = dto.ConsumeEquipmentCode;
            equDto.ResourceCode = dto.ConsumeResourceCodeCode;
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 查询设备激活工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 构造数据
            ManuFeedingMaterialSaveDto saveDto = new ManuFeedingMaterialSaveDto();
            //saveDto.BarCode = dto.Sfc;
            saveDto.Source = ManuSFCFeedingSourceEnum.BOM;
            saveDto.SiteId = equResModel.SiteId;
            saveDto.ResourceId = equResModel.ResId;
            //4. 上料（考虑改成对条码进行合批）
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            foreach (var item in dto.ConsumeSfcList)
            {
                saveDto.BarCode = item.Sfc;
                var feedResult = await _manuFeedingService.CreateAsync(saveDto);
            }
            trans.Complete();

            //TODO
            //1. 类似上料，上到搅拌机或者制胶机
            //2. 进行上料点
        }

        /// <summary>
        /// 上料完成(制胶匀浆)019
        /// </summary>
        /// <param name="dto"></param>
        public async Task FeedingCompletedAsync(FeedingCompletedDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 组装数据
            ManuFeedingCompletedZjyjRecordSaveDto saveDto = new ManuFeedingCompletedZjyjRecordSaveDto();
            saveDto.SiteId = equResModel.SiteId;
            saveDto.EquipmentId = equResModel.EquipmentId;
            saveDto.BeforeFeedingQty = dto.BeforeFeedingQty;
            saveDto.AfterFeedingQty = dto.AfterFeedingQty;
            saveDto.FeedingQty = dto.AfterFeedingQty - dto.BeforeFeedingQty;
            saveDto.CreatedBy = dto.EquipmentCode;
            saveDto.CreatedOn = HymsonClock.Now();
            saveDto.UpdatedBy = saveDto.CreatedBy;
            saveDto.UpdatedOn = saveDto.CreatedOn;
            //3. 数据库操作
            await _manuFeedingCompletedZjyjRecordService.AddAsync(saveDto);

            return;
        }

        /// <summary>
        /// 设备产出
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> OutputEquAsync(QknyBaseDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 查询设备激活工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 构造数据
            //3.1 产出条码
            CreateBarcodeByWorkOrderBo query = new CreateBarcodeByWorkOrderBo();
            query.WorkOrderId = planEntity.Id;
            query.ResourceId = equResModel.ResId;
            query.SiteId = equResModel.SiteId;
            query.Qty = 1;
            query.ProcedureId = equResModel.ProcedureId;
            query.UserName = equResModel.EquipmentCode;
            //3.2 进站数据
            SFCInStationBo inStationBo = new SFCInStationBo();
            inStationBo.UserName = equResModel.EquipmentCode;
            inStationBo.EquipmentId = equResModel.EquipmentId;
            inStationBo.ResourceId = equResModel.ResId;
            inStationBo.ProcedureId = equResModel.ProcedureId;
            inStationBo.SiteId = equResModel.SiteId;
            inStationBo.SFCs = new List<string>() { };
            //3.3 出站数据
            SFCOutStationBo sfcOutBo = new SFCOutStationBo();
            OutStationRequestBo outBo = new OutStationRequestBo();
            outBo.IsQualified = true;
            sfcOutBo.OutStationRequestBos = new List<OutStationRequestBo>() { outBo };
            sfcOutBo.EquipmentId = equResModel.EquipmentId;
            sfcOutBo.ResourceId = equResModel.ResId;
            sfcOutBo.ProcedureId = equResModel.ProcedureId;
            sfcOutBo.SiteId = equResModel.SiteId;
            sfcOutBo.UserName = equResModel.EquipmentName;
            //4. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            var sfcList = await _manuCreateBarcodeService.CreateBarcodeByWorkOrderIdAsync(query, null);
            inStationBo.SFCs = sfcList.Select(m => m.SFC).ToList();
            var inResult = await _manuPassStationService.InStationRangeBySFCAsync(inStationBo, RequestSourceEnum.EquipmentApi);
            outBo.SFC = sfcList.Select(m => m.SFC).FirstOrDefault();
            var outResult = await _manuPassStationService.OutStationRangeBySFCAsync(sfcOutBo, RequestSourceEnum.EquipmentApi);
            trans.Complete();

            return sfcList.Select(m => m.SFC).First();
        }

        /// <summary>
        /// 批次转移021
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task BatchMoveAsync(BatchMoveDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 查询设备激活工单
            //PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 查询转移的类型及数据
            List<string> codeList = new List<string>() { dto.EquipmentCodeIn, dto.EquipmentCodeOut };
            ProcLoadPointEquipmentQuery query = new ProcLoadPointEquipmentQuery();
            query.CodeList = codeList;
            var dbList = await _procLoadPointService.GetPointOrEquipmmentAsync(query);
            #region 构造数据
            //4. 构造数据
            //4.1 转移数据
            var outEqu = dbList.Where(m => m.Code == dto.EquipmentCodeOut).FirstOrDefault()!;
            var inEqu = dbList.Where(m => m.Code == dto.EquipmentCodeIn).FirstOrDefault()!;
            ManuFeedingTransferSaveDto saveDto = new ManuFeedingTransferSaveDto();
            saveDto.Sfc = dto.Sfc;
            saveDto.Qty = dto.Qty > 0 ? dto.Qty : null;
            saveDto.OpeationBy = equResModel.EquipmentCode;
            saveDto.LoadPointResoucesId = inEqu.ResId;
            saveDto.SourceId = outEqu.Id;
            saveDto.DestId = inEqu.Id;
            if (outEqu.DataType == "1" && inEqu.DataType == "1")
            {
                saveDto.TransferType = ManuSFCFeedingTransferEnum.FeedingPoint;
                saveDto.SourceId = outEqu.Id;
                saveDto.DestId = inEqu.Id;
                saveDto.LoadPointResoucesId = inEqu.ResId;
            }
            else if (outEqu.DataType == "2" && inEqu.DataType == "2")
            {
                saveDto.TransferType = ManuSFCFeedingTransferEnum.Resource;
                saveDto.SourceId = outEqu.ResId;
                saveDto.DestId = inEqu.ResId;
            }
            else if (outEqu.DataType == "1" && inEqu.DataType == "2")
            {
                saveDto.TransferType = ManuSFCFeedingTransferEnum.FeedingPointResource;
                saveDto.SourceId = outEqu.ResId;
                saveDto.DestId = inEqu.ResId;
            }
            else if (outEqu.DataType == "2" && inEqu.DataType == "1")
            {
                saveDto.TransferType = ManuSFCFeedingTransferEnum.ResourceFeedingPoint;
                saveDto.SourceId = outEqu.ResId; //转出设备的资源id
                saveDto.DestId = inEqu.Id; //转入设备的上料点id
                saveDto.LoadPointResoucesId = inEqu.ResId; //转入设备的资源id
            }
            //4.2 记录数据
            ManuFeedingTransferRecordSaveDto recordDto = new ManuFeedingTransferRecordSaveDto();
            recordDto.EquipmentId = equResModel.EquipmentId;
            recordDto.Sfc = dto.Sfc;
            recordDto.EquipmentCodeOut = dto.EquipmentCodeOut;
            recordDto.EquipmentCodeIn = dto.EquipmentCodeIn;
            recordDto.Qty = dto.Qty;
            recordDto.CreatedBy = dto.EquipmentCode;
            recordDto.CreatedOn = HymsonClock.Now();
            recordDto.UpdatedBy = recordDto.CreatedBy;
            recordDto.UpdatedOn = recordDto.CreatedOn;
            #endregion
            //5. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuFeedingService.ManuFeedingTransfer(saveDto);
            await _manuFeedingTransferRecordService.AddAsync(recordDto);
            trans.Complete();
        }

        /// <summary>
        /// 设备投料非生产投料(制胶匀浆)022
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ConsumeInNonProductionEquAsync(ConsumeInNonProductionEquDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //
            DateTime curDate = HymsonClock.Now();
            List<ManuFeedingNoProductionRecordSaveDto> saveDtoList = new List<ManuFeedingNoProductionRecordSaveDto>();
            foreach (var item in dto.ConsumeSfcList)
            {
                ManuFeedingNoProductionRecordSaveDto model = new ManuFeedingNoProductionRecordSaveDto();
                model.EquipmentId = equResModel.EquipmentId;
                model.ConsumeEquipmentCode = dto.ConsumeEquipmentCode;
                model.ConsumeResourceCodeCode = dto.ConsumeResourceCodeCode;
                model.Sfc = item.Sfc;
                model.Qty = item.Qty;
                model.Category = item.Category;
                model.CreatedBy = dto.EquipmentCode;
                model.CreatedOn = curDate;
                model.UpdatedBy = dto.EquipmentCode;
                model.UpdatedOn = curDate;
                saveDtoList.Add(model);
            }
            await _manuFeedingNoProductionRecordService.AddMultAsync(saveDtoList);
            //TODO
            //1. 使用NMP和DIW洗罐子用到
        }

    }
}
