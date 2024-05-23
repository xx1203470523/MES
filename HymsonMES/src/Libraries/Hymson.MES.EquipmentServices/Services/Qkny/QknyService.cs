using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuBind;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.CoreServices.Services.Qkny;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ToolBindMaterial;
using Hymson.MES.EquipmentServices.Services.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Hymson.MES.EquipmentServices.Services.Qkny.Formula;
using Hymson.MES.EquipmentServices.Services.Qkny.InteVehicle;
using Hymson.MES.EquipmentServices.Services.Qkny.LoadPoint;
using Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder;
using Hymson.MES.EquipmentServices.Services.Qkny.PowerOnParam;
using Hymson.MES.EquipmentServices.Services.Qkny.ProcSortingRule;
using Hymson.MES.EquipmentServices.Services.Qkny.WhMaterialInventory;
using Hymson.MES.Services.Dtos.AgvTaskRecord;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.MES.Services.Services.AgvTaskRecord;
using Hymson.MES.Services.Services.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Services.EquEquipmentAlarm;
using Hymson.MES.Services.Services.EquEquipmentHeartRecord;
using Hymson.MES.Services.Services.EquEquipmentLoginRecord;
using Hymson.MES.Services.Services.EquProcessParamRecord;
using Hymson.MES.Services.Services.EquProductParamRecord;
using Hymson.MES.Services.Services.EquToolLifeRecord;
using Hymson.MES.Services.Services.ManuEquipmentStatusTime;
using Hymson.MES.Services.Services.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Services.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Services.Services.ManuFeedingNoProductionRecord;
using Hymson.MES.Services.Services.ManuFeedingTransferRecord;
using Hymson.MES.Services.Services.ManuFillingDataRecord;
using Hymson.MessagePush.Helper;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.Attributes;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Dapper.SqlMapper;

namespace Hymson.MES.EquipmentServices.Services.Qkny
{
    /// <summary>
    /// 顷刻设备服务
    /// </summary>
    public class QknyService : IQknyService
    {
        #region 验证器
        private readonly AbstractValidator<OperationLoginDto> _validationOperationLoginDto;
        #endregion

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderService _planWorkOrderService;

        /// <summary>
        /// 库存条码接收
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 上料
        /// </summary>
        private readonly IManuFeedingService _manuFeedingService;

        /// <summary>
        /// AGV任务记录
        /// </summary>
        private readonly IAgvTaskRecordService _agvTaskRecordService;

        /// <summary>
        /// 设备过程参数
        /// </summary>
        private readonly IEquProcessParamRecordService _equProcessParamRecordService;

        /// <summary>
        /// 业务接口（创建条码服务）
        /// </summary>
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;

        /// <summary>
        /// 在制品服务
        /// </summary>
        private readonly IManuSfcProduceService _manuSfcProduceService;

        /// <summary>
        /// 条码信息
        /// </summary>
        private readonly IManuSfcService _manuSfcServicecs;

        /// <summary>
        /// 产品参数
        /// </summary>
        private readonly IEquProductParamRecordService _equProductParamRecordService;

        /// <summary>
        /// 服务接口（过站）
        /// </summary>
        private readonly IManuPassStationService _manuPassStationService;

        /// <summary>
        /// 上料点
        /// </summary>
        private readonly IProcLoadPointService _procLoadPointService;

        /// <summary>
        /// 物料库存
        /// </summary>
        private readonly IWhMaterialInventoryService _whMaterialInventoryService;

        /// <summary>
        /// 载具
        /// </summary>
        private readonly IInteVehicleService _inteVehicleService;

        /// <summary>
        /// 条码
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码步骤
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 工地那
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 工装条码绑定仓储
        /// </summary>
        private readonly IManuToolingBindRepository _manuToolingBindRepository;

        /// <summary>
        /// 降级录入仓储
        /// </summary>
        private readonly IManuDowngradingRepository _manuDowngradingRepository;

        /// <summary>
        /// 设备服务
        /// </summary>
        private readonly IEquEquipmentService _equEquipmentService;

        /// <summary>
        /// 参数收集
        /// </summary>
        private readonly IManuProductParameterService _manuProductParameterService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QknyService(IEquEquipmentRepository equEquipmentRepository,
            IPlanWorkOrderService planWorkOrderService,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IManuFeedingService manuFeedingService,
            IAgvTaskRecordService agvTaskRecordService,
            IEquProcessParamRecordService equProcessParamRecordService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IManuSfcProduceService manuSfcProduceService,
            IManuSfcService manuSfcServicecs,
            IEquProductParamRecordService equProductParamRecordService,
            IManuPassStationService manuPassStationService,
            IProcLoadPointService procLoadPointService,
            IWhMaterialInventoryService whMaterialInventoryService,
            IInteVehicleService inteVehicleService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuToolingBindRepository manuToolingBindRepository,
            IManuDowngradingRepository manuDowngradingRepository,
            IEquEquipmentService equEquipmentService,
            AbstractValidator<OperationLoginDto> validationOperationLoginDto,
            IManuProductParameterService manuProductParameterService)
        {
            _equEquipmentRepository = equEquipmentRepository;
            _planWorkOrderService = planWorkOrderService;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _manuFeedingService = manuFeedingService;
            _agvTaskRecordService = agvTaskRecordService;
            _equProcessParamRecordService = equProcessParamRecordService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _manuSfcProduceService = manuSfcProduceService;
            _manuSfcServicecs = manuSfcServicecs;
            _equProductParamRecordService = equProductParamRecordService;
            _manuPassStationService = manuPassStationService;
            _procLoadPointService = procLoadPointService;
            _whMaterialInventoryService = whMaterialInventoryService;
            _inteVehicleService = inteVehicleService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuToolingBindRepository = manuToolingBindRepository;
            _equEquipmentService = equEquipmentService;
            _manuDowngradingRepository = manuDowngradingRepository;
            //校验器
            _validationOperationLoginDto = validationOperationLoginDto;
            _manuProductParameterService = manuProductParameterService;
        }


        /// <summary>
        /// 原材料上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task FeedingAsync(FeedingDto dto)
        {
            long siteId = 0;
            var feedingMaterialSaveDto = new ManuFeedingMaterialSaveDto()
            {
                UserName = dto.EquipmentCode
            };
            //组装参数
            var feedingMaterialSaveDtos = new List<ManuFeedingMaterialSaveDto>();
            if (dto.IsFeedingPoint == true)
            {
                ProcLoadPointQuery query = new ProcLoadPointQuery();
                query.LoadPoint = dto.EquipmentCode;
                var loadPoint = await _procLoadPointService.GetProcLoadPointAsync(query);
                siteId = loadPoint.SiteId;
                //saveDto.ResourceId = res.FirstOrDefault()!.ResourceId!;
                feedingMaterialSaveDto.Source = ManuSFCFeedingSourceEnum.FeedingPoint;
                feedingMaterialSaveDto.FeedingPointId = loadPoint.Id;
                feedingMaterialSaveDto.BarCode = dto.Sfc;
                feedingMaterialSaveDto.SiteId = siteId;
                feedingMaterialSaveDtos.Add(feedingMaterialSaveDto);
            }
            else
            {
                //1. 获取设备基础信息
                EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResLineAsync(dto);
                siteId = equResModel.SiteId;
                var sfcs = new List<string>() { dto.Sfc };
                if (dto.IsTooling)
                {
                    //查询工装绑定的条码
                    var toolingBindEntities = await _manuToolingBindRepository.GetEntitiesAsync(new Data.Repositories.Manufacture.Query.ManuToolingBindQuery
                    {
                        SiteId = equResModel.SiteId,
                        ToolingCode = dto.Sfc,
                        Status = Core.Enums.Common.BindStatusEnum.Bind
                    });
                    if (toolingBindEntities == null || !toolingBindEntities.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES45041));
                    }
                    sfcs = toolingBindEntities.Select(x => x.Barcode).ToList();
                }

                foreach (var item in sfcs)
                {
                    feedingMaterialSaveDto = new ManuFeedingMaterialSaveDto()
                    {
                        SiteId = equResModel.SiteId,
                        UserName = dto.EquipmentCode,
                        BarCode = item
                    };

                    feedingMaterialSaveDto.Source = ManuSFCFeedingSourceEnum.BOM;
                    feedingMaterialSaveDto.ResourceId = equResModel.ResId;
                    feedingMaterialSaveDtos.Add(feedingMaterialSaveDto);
                }

            }

            //3. 上料
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            foreach (var item in feedingMaterialSaveDtos)
            {
                var feedResult = await _manuFeedingService.CreateAsync(item);
            }
            //工装解绑
            if (dto.IsTooling)
            {
                await _manuToolingBindRepository.UnBindToolingAsync(new Data.Repositories.Manufacture.ManuToolingBind.Command.UnBindToolingCommand
                {
                    SiteId = siteId,
                    ToolingCode = dto.Sfc,
                    UpdatedBy = dto.EquipmentCode,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            trans.Complete();

            //TODO
            //1. 校验物料是否在lims系统发过来的条码表lims_material(wh_material_inventory)，验证是否存在及合格，以及生成日期
            //2. 添加上料表信息 manu_feeding
            //3. 添加上料记录表信息 manu_feeding_record
            //4. 参考物料加载逻辑 ManuFeedingService.CreateAsync
        }

        /// <summary>
        /// 半成品上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task HalfFeedingAsync(HalfFeedingDto dto)
        {
            //TODO 和上料保持一致，使用BOM上料

            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResLineAsync(dto);
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId, equResModel.ResId);
            //2. 构造数据
            ManuFeedingMaterialSaveDto saveDto = new ManuFeedingMaterialSaveDto();
            saveDto.BarCode = dto.Sfc;
            saveDto.Source = ManuSFCFeedingSourceEnum.BOM;
            saveDto.SiteId = equResModel.SiteId;
            saveDto.ResourceId = equResModel.ResId;
            //3. 上料
            var feedResult = await _manuFeedingService.CreateAsync(saveDto);
        }

        /// <summary>
        /// AGV叫料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AgvMaterialAsync(AgvMaterialDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 校验设备是否激活工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId, equResModel.ResId);
            //3. 调用AGV接口
            var result = ""; //await HttpHelper.HttpPostAsync("", dto.Content, "");
            //4. 存储数据
            AgvTaskRecordSaveDto saveDto = new AgvTaskRecordSaveDto();
            saveDto.Id = IdGenProvider.Instance.CreateId();
            saveDto.SiteId = equResModel.SiteId;
            saveDto.EquipmentId = equResModel.EquipmentId;
            saveDto.SendContent = dto.Content;
            saveDto.TaskType = dto.TaskType;
            saveDto.ReceiveContent = result;
            saveDto.CreatedOn = HymsonClock.Now();
            saveDto.CreatedBy = dto.EquipmentCode;
            saveDto.UpdatedOn = saveDto.CreatedOn;
            saveDto.UpdatedBy = saveDto.CreatedBy;
            await _agvTaskRecordService.AddAsync(saveDto);
        }

        /// <summary>
        /// 请求产出极卷码023
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<string>> GenerateSfcAsync(GenerateSfcDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            //2.1 条码数据
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId, equResModel.ResId);
            CreateBarcodeByWorkOrderBo query = new CreateBarcodeByWorkOrderBo();
            query.WorkOrderId = planEntity.Id;
            query.ResourceId = equResModel.ResId;
            query.SiteId = equResModel.SiteId;
            query.Qty = dto.Qty;
            query.ProcedureId = equResModel.ProcedureId;
            query.UserName = equResModel.EquipmentCode;
            //2.2 进站数据
            SFCInStationBo inBo = new SFCInStationBo();
            inBo.SiteId = equResModel.SiteId;
            inBo.UserName = equResModel.EquipmentCode;
            inBo.ProcedureId = equResModel.ProcedureId;
            inBo.ResourceId = equResModel.ResId;
            inBo.EquipmentId = equResModel.EquipmentId;
            //3. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            var sfcObjList = await _manuCreateBarcodeService.CreateBarcodeByWorkOrderIdAsync(query, null);
            List<string> sfcList = sfcObjList.Select(m => m.SFC).ToList();
            inBo.SFCs = sfcList.ToArray();
            var inResult = await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
            trans.Complete();

            return sfcList;
        }

        /// <summary>
        /// 产出数量上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundMetersReportAsync(OutboundMetersReportDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 工单信息
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId, equResModel.ResId);
            //3. 查询条码信息
            var manuSfcInfo = await _manuSfcRepository.GetManSfcAboutInfoBySfcAsync(new ManuSfcAboutInfoBySfcQuery()
            {
                SiteId = equResModel.SiteId,
                Sfc = dto.Sfc,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", dto.Sfc);
            //4. 校验工单数量
            //暂不校验

            //构造数据
            UpdateQtyBySfcCommand command = new UpdateQtyBySfcCommand();
            command.SFC = dto.Sfc;
            command.Qty = dto.OkQty;
            command.SiteId = equResModel.SiteId;
            command.UpdatedBy = dto.EquipmentCode;
            command.UpdatedOn = HymsonClock.Now();
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
            //上料条码
            List<OutStationConsumeBo> consumeSfcList = new List<OutStationConsumeBo>();
            if (dto.OutputType == "1" || dto.OutputType == "2")
            {
                //查询当前设备的上料
                EntityByResourceIdQuery loadQuery = new EntityByResourceIdQuery();
                loadQuery.SiteId = equResModel.SiteId;
                loadQuery.Resourceid = equResModel.ResId;
                var loadList = await _manuFeedingService.GetAllByResourceIdAsync(loadQuery);
                foreach (var item in loadList)
                {
                    OutStationConsumeBo consumeSfc = new OutStationConsumeBo();
                    consumeSfc.BarCode = item.BarCode;
                    consumeSfc.MaterialId = item.MaterialId;
                    consumeSfc.ConsumeQty = item.Qty;
                    consumeSfcList.Add(consumeSfc);
                }
            }
            //NG代码
            List<OutStationUnqualifiedBo> ngList = new List<OutStationUnqualifiedBo>();
            if (dto.NgList != null && dto.NgList.Count > 0)
            {
                foreach (var item in dto.NgList)
                {
                    ngList.Add(new OutStationUnqualifiedBo() { UnqualifiedCode = item.NgCode, UnqualifiedQty = item.Qty });
                }
            }

            //进出站数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            OutStationRequestBo outReqBo = new OutStationRequestBo();
            outReqBo.SFC = dto.Sfc;
            outReqBo.IsQualified = true;
            outReqBo.QualifiedQty = dto.OkQty;
            outReqBo.UnQualifiedQty = dto.NgQty;
            outReqBo.ConsumeList = consumeSfcList;
            outReqBo.OutStationUnqualifiedList = ngList;
            outBo.OutStationRequestBos = new List<OutStationRequestBo>() { outReqBo };
            //步骤记录
            ManuSfcStepEntity sfcStep = new ManuSfcStepEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = equResModel.SiteId,
                SFC = dto.Sfc,
                ProductId = manuSfcInfo.ProductId,
                WorkOrderId = manuSfcInfo.WorkOrderId,
                ProductBOMId = planEntity.ProductBOMId,
                WorkCenterId = equResModel.LineId,
                Qty = dto.OkQty,
                ResourceId = equResModel.ResId,
                ProcedureId = equResModel.ProcedureId,
                Operatetype = ManuSfcStepTypeEnum.SfcQtyAdjust,
                CurrentStatus = manuSfcInfo.Status,
                CreatedBy = dto.EquipmentCode,
                UpdatedBy = dto.EquipmentCode,
                Remark = "设备上报产出更改"
            };
            //扣减/增可下达数量
            UpdatePassDownQuantityCommand updateQtyCommand = new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planEntity.Id,
                PlanQuantity = planEntity.Qty * (1 + planEntity.OverScale / 100),
                PassDownQuantity = dto.TotalQty - manuSfcInfo.Qty,//再次下达的数量
                UserName = equResModel.EquipmentCode,
                UpdateDate = HymsonClock.Now()
            };

            // 数据库操作
            // 参考条码数量更改
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuSfcProduceService.UpdateQtyBySfcAsync(command);
            await _manuSfcServicecs.UpdateQtyBySfcAsync(command);
            int updateNum = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(updateQtyCommand);
            if (updateNum == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", planEntity.OrderCode);
            }
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _manuSfcStepRepository.InsertAsync(sfcStep);
            await _manuProductParameterService.ProductProcessCollectAsync(parameterBo);
            trans.Complete();
        }

        /// <summary>
        /// 获取下发条码(用于CCD面密度)025
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<CcdGetBarcodeReturnDto> CcdGetBarcodeAsync(CCDFileUploadCompleteDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 查询数据
            ManuSfcEquipmentNewestQuery query = new ManuSfcEquipmentNewestQuery();
            query.EquipmentId = equResModel.EquipmentId;
            var dbModel = await _manuSfcProduceService.GetEquipmentNewestSfc(query);
            //3. 构造数据
            CcdGetBarcodeReturnDto result = new CcdGetBarcodeReturnDto();
            result.Id = dbModel.Id.ToString();
            result.Sfc = dbModel.SFC;
            result.Qty = dbModel.Qty;
            result.ProductCode = dbModel.MaterialCode;
            return result;
        }

        /// <summary>
        /// 设备过程参数026
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto)
        {
            if (dto.ParamList == null || dto.ParamList.Count == 0)
            {
                return;
            }
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 添加数据
            List<EquProcessParamRecordSaveDto> saveDtoList = new List<EquProcessParamRecordSaveDto>();
            foreach (var item in dto.ParamList)
            {
                EquProcessParamRecordSaveDto saveDto = new EquProcessParamRecordSaveDto();
                saveDto.ParamCode = item.ParamCode;
                saveDto.ParamValue = item.ParamValue;
                saveDto.CollectionTime = item.CollectionTime;
                saveDtoList.Add(saveDto);
            }
            saveDtoList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            //3. 数据操作
            await _equProcessParamRecordService.AddMultAsync(saveDtoList);
        }

        /// <summary>
        /// 进站027
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task InboundAsync(InboundDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            SFCInStationBo inBo = new SFCInStationBo();
            inBo.SiteId = equResModel.SiteId;
            inBo.UserName = equResModel.EquipmentCode;
            inBo.ProcedureId = equResModel.ProcedureId;
            inBo.ResourceId = equResModel.ResId;
            inBo.EquipmentId = equResModel.EquipmentId;
            List<string> sfcList = new List<string>() { dto.Sfc };
            inBo.SFCs = sfcList.ToArray();
            //3. 进站
            var inResult = await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 出站028
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundAsync(OutboundDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            //2.1 出站数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            OutStationRequestBo outReqBo = new OutStationRequestBo();
            outReqBo.SFC = dto.Sfc;
            outReqBo.IsQualified = dto.Passed == 1;
            if (dto.BindFeedingCodeList.IsNullOrEmpty() == false)
            {
                List<OutStationConsumeBo> conList = new List<OutStationConsumeBo>();
                foreach (var item in dto.BindFeedingCodeList)
                {
                    conList.Add(new OutStationConsumeBo() { BarCode = item });
                }
                outReqBo.ConsumeList = conList;
            }
            if (dto.NgList.IsNullOrEmpty() == false)
            {
                List<OutStationUnqualifiedBo> unCodeList = new List<OutStationUnqualifiedBo>();
                foreach (var item in dto.NgList)
                {
                    unCodeList.Add(new OutStationUnqualifiedBo() { UnqualifiedCode = item });
                }
                outReqBo.OutStationUnqualifiedList = unCodeList;
            }
            outBo.OutStationRequestBos = new List<OutStationRequestBo>() { outReqBo };
            //2.2 产品过程参数
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
            //3. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _manuProductParameterService.ProductProcessCollectAsync(parameterBo);
            trans.Complete();
        }

        /// <summary>
        /// 进站多个029
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<InboundMoreReturnDto>> InboundMoreAsync(InboundMoreDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            SFCInStationBo inBo = new SFCInStationBo();
            inBo.SiteId = equResModel.SiteId;
            inBo.UserName = equResModel.EquipmentCode;
            inBo.ProcedureId = equResModel.ProcedureId;
            inBo.ResourceId = equResModel.ResId;
            inBo.EquipmentId = equResModel.EquipmentId;
            inBo.SFCs = dto.SfcList.ToArray();
            //3. 进站
            var inResult = await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
            //4. 返回
            List<InboundMoreReturnDto> resultList = new List<InboundMoreReturnDto>();
            foreach (var item in dto.SfcList)
            {
                InboundMoreReturnDto model = new InboundMoreReturnDto();
                model.Sfc = item;
                resultList.Add(model);
            }

            return resultList;
        }

        /// <summary>
        /// 出站多个030
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<OutboundMoreReturnDto>> OutboundMoreAsync(OutboundMoreDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            //3. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            var outResult = await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
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
        /// 产品参数上传043
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ProductParamAsync(ProductParamDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 出站参数
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
            foreach (var item in dto.SfcList)
            {
                List<EquProductParamRecordSaveDto> sfcParamList = new List<EquProductParamRecordSaveDto>();
                foreach (var sfcItem in item.ParamList)
                {
                    EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                    saveDto.ParamCode = sfcItem.ParamCode;
                    saveDto.ParamValue = sfcItem.ParamValue;
                    saveDto.CollectionTime = sfcItem.CollectionTime;
                    sfcParamList.Add(saveDto);
                }
                sfcParamList.ForEach(m =>
                {
                    m.SiteId = equResModel.SiteId;
                    m.Sfc = item.Sfc;
                    m.EquipmentId = equResModel.EquipmentId;
                    m.CreatedOn = HymsonClock.Now();
                    m.CreatedBy = dto.EquipmentCode;
                    m.UpdatedOn = m.CreatedOn;
                    m.UpdatedBy = m.CreatedBy;
                });
                saveDtoList.AddRange(sfcParamList);
            }
            //3. 出站
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
        }

        /// <summary>
        /// 库存接收047
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task MaterialInventoryAsync(MaterialInventoryDto dto)
        {
            if (dto == null || dto.BarCodeList == null || !dto.BarCodeList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            await _whMaterialInventoryService.MaterialInventoryAsync(dto);
        }

        /// <summary>
        /// 工装条码绑定048
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ToolBindMaterialAsync(ToolBindMaterialDto dto)
        {
            //获取设备基础信息
            var equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //校验条码是否已绑定工装
            var bindSfcs = await _manuToolingBindRepository.GetEntitiesAsync(new Data.Repositories.Manufacture.Query.ManuToolingBindQuery
            {
                SiteId = equResModel.SiteId,
                Barcodes = dto.ContainerSfcList,
                Status = Core.Enums.Common.BindStatusEnum.Bind
            });
            if (bindSfcs != null && bindSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45280)).WithData("Barcode", string.Join('|', bindSfcs.Select(x => x.Barcode)));
            }
            //校验条码是否在库存中
            var existBarcodeEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                SiteId = equResModel.SiteId,
                BarCodes = dto.ContainerSfcList
            });
            if (existBarcodeEntities == null || !existBarcodeEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45281)).WithData("Barcode", string.Join('|', dto.ContainerSfcList));
            }
            var exceptBarcodes = dto.ContainerSfcList.Except(existBarcodeEntities.Select(x => x.MaterialBarCode));
            if (exceptBarcodes != null && exceptBarcodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45281)).WithData("Barcode", string.Join('|', exceptBarcodes));
            }
            //入库
            var entities = dto.ContainerSfcList.Select(item => new ManuToolingBindEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = equResModel.SiteId,
                ToolingCode = dto.ContainerCode,
                Barcode = item,
                Status = Core.Enums.Common.BindStatusEnum.Bind,
                CreatedBy = dto.EquipmentCode,
                UpdatedBy = dto.EquipmentCode
            });

            await _manuToolingBindRepository.InsertRangeAsync(entities);
        }

        /// <summary>
        /// 获取电芯降级信息051
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<SortingSfcInfo>> GetSfcInfoAsync(GetSfcInfoDto dto)
        {
            //获取设备基础信息
            var equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //查询降级信息
            var downgradingSfcs = await _manuDowngradingRepository.GetBySFCsAsync(new ManuDowngradingBySFCsQuery
            {
                SiteId = equResModel.SiteId,
                SFCs = dto.SfcList
            });

            return downgradingSfcs.Select(x => new SortingSfcInfo
            {
                SFC = x.SFC,
                Grade = x.Grade
            }).ToList();
        }

        /// <summary>
        /// 分选拆盘052
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task SortingUnBindAsync(SortingUnBindDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);
            //2. 托盘电芯解绑
            InteVehicleUnBindDto bindDto = new InteVehicleUnBindDto();
            bindDto.ContainerCode = dto.ContainCode;
            bindDto.SfcList = dto.SfcList;
            bindDto.SiteId = equResModel.SiteId;
            bindDto.UserName = dto.EquipmentCode;

            //3. 数据操作
            await _inteVehicleService.VehicleUnBindOperationAsync(bindDto);
        }

        /// <summary>
        /// 分选出站053
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task SortingOutboundAsync(SortingOutboundDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);
            //2. 参数组装
            //2.1 托盘电芯绑定
            var vehicleBindDto = new InteVehicleBindDto
            {
                SiteId = equResModel.SiteId,
                UserName = dto.EquipmentCode,
                ContainerCode = dto.ContainerCode,
                SfcList = dto.ContainerSfcList.Select(x => new InteVehicleSfcDto
                {
                    Sfc = x.Sfc,
                    Location = x.Location,
                }).ToList()
            };
            //2.2 托盘出站
            VehicleOutStationBo outStationBo = new VehicleOutStationBo
            {
                SiteId = equResModel.SiteId,
                UserName = dto.EquipmentCode,
                EquipmentId = equResModel.EquipmentId,
                ResourceId = equResModel.ResId,
                ProcedureId = equResModel.ProcedureId,
                OutStationRequestBos = new OutStationRequestBo[] { new() { VehicleCode = dto.ContainerCode, IsQualified = dto.Passed == 1 } }
            };
            //2.3 托盘参数
            var paramRecordSaveDtos = dto.ParamList.Select(x => new EquProductParamRecordSaveDto
            {
                ParamCode = x.ParamCode,
                ParamValue = x.ParamValue,
                CollectionTime = x.CollectionTime,
                SiteId = equResModel.SiteId,
                Sfc = dto.ContainerCode,
                EquipmentId = equResModel.EquipmentId,
                CreatedBy = dto.EquipmentCode,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = dto.EquipmentCode,
                UpdatedOn = HymsonClock.Now()
            }).ToList();

            //3. 数据操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _inteVehicleService.VehicleBindOperationAsync(vehicleBindDto);
            await _manuPassStationService.OutStationRangeByVehicleAsync(outStationBo, RequestSourceEnum.EquipmentApi);
            await _equProductParamRecordService.AddMultAsync(paramRecordSaveDtos);
            trans.Complete();
        }

        /// <summary>
        /// 获取设备token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> GetEquTokenAsync(QknyBaseDto dto)
        {
            return await _equEquipmentService.GetEquTokenAsync(dto);
        }

        /// <summary>
        /// 获取设备资源对应的基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<EquEquipmentResAllView> GetEquResAllAsync(QknyBaseDto param)
        {
            EquResAllQuery query = new EquResAllQuery();
            query.EquipmentCode = param.EquipmentCode;
            query.ResCode = param.ResourceCode;
            EquEquipmentResAllView equResAllModel = await _equEquipmentRepository.GetEquResAllAsync(query);
            if (equResAllModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return equResAllModel;
        }

        /// <summary>
        /// 获取设备资源对应的基础信息
        /// 用于化成NG电芯上报，此时实际发生不良的工序是可能是上面的多个工序出现的
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private async Task<List<EquEquipmentResAllView>> GetMultEquResAllAsync(MultEquResAllQuery query)
        {
            var list = await _equEquipmentRepository.GetMultEquResAllAsync(query);
            if (list == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return list.ToList(); ;
        }
    }
}
