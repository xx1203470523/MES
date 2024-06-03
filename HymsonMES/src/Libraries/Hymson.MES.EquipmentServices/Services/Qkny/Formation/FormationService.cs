using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Qkny;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Hymson.MES.EquipmentServices.Services.Qkny.InteVehicle;
using Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder;
using Hymson.MES.EquipmentServices.Services.Qkny.ProcSortingRule;
using Hymson.MES.EquipmentServices.Validators.EquVerifyHelper;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.MES.Services.Dtos.ManuFillingDataRecord;
using Hymson.MES.Services.Services.EquProductParamRecord;
using Hymson.MES.Services.Services.ManuFillingDataRecord;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.Qkny.Formation
{
    /// <summary>
    /// 化成
    /// </summary>
    public class FormationService : IFormationService
    {
        /// <summary>
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 本地化
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 设备服务
        /// </summary>
        private readonly IEquEquipmentService _equEquipmentService;

        /// <summary>
        /// 补液数据上报
        /// </summary>
        private readonly IManuFillingDataRecordService _manuFillingDataRecordService;

        /// <summary>
        /// 通用接口
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 在制品服务
        /// </summary>
        private readonly IManuSfcProduceService _manuSfcProduceService;

        /// <summary>
        /// 载具
        /// </summary>
        private readonly IInteVehicleService _inteVehicleService;

        /// <summary>
        /// 服务接口（过站）
        /// </summary>
        private readonly IManuPassStationService _manuPassStationService;

        /// <summary>
        /// 产品参数
        /// </summary>
        private readonly IEquProductParamRecordService _equProductParamRecordService;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderService _planWorkOrderService;

        /// <summary>
        /// 分选规则
        /// </summary>
        private readonly IProcSortingRuleService _procSortingRuleService;

        /// <summary>
        /// 物料
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormationService(
            IServiceProvider serviceProvider,
            ILocalizationService localizationService,
            IEquEquipmentService equEquipmentService,
            IManuFillingDataRecordService manuFillingDataRecordService,
            IManuCommonService manuCommonService,
            IManuSfcProduceService manuSfcProduceService,
            IInteVehicleService inteVehicleService,
            IManuPassStationService manuPassStationService,
            IEquProductParamRecordService equProductParamRecordService,
            IPlanWorkOrderService planWorkOrderService,
            IProcSortingRuleService procSortingRuleService,
            IProcMaterialRepository procMaterialRepository)
        {
            _serviceProvider = serviceProvider;
            _localizationService = localizationService;
            _equEquipmentService = equEquipmentService;
            _manuFillingDataRecordService = manuFillingDataRecordService;
            _manuCommonService = manuCommonService;
            _manuSfcProduceService = manuSfcProduceService;
            _inteVehicleService = inteVehicleService;
            _manuPassStationService = manuPassStationService;
            _equProductParamRecordService = equProductParamRecordService;
            _planWorkOrderService = planWorkOrderService;
            _procSortingRuleService = procSortingRuleService;
            _procMaterialRepository = procMaterialRepository;
        }

        /// <summary>
        /// 补液数据上报034
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task FillingDataAsync(FillingDataDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 构造数据
            ManuFillingDataRecordSaveDto saveDto = new ManuFillingDataRecordSaveDto();
            saveDto.Sfc = dto.Sfc;
            saveDto.EquipmentId = equResModel.EquipmentId;
            saveDto.InTime = dto.InTime;
            saveDto.OutTime = dto.OutTime;
            saveDto.BeforeWeight = dto.BeforeWeight;
            saveDto.AfterWeight = dto.AfterWeight;
            saveDto.ElWeight = dto.ElWeight;
            saveDto.AddEl = dto.AddEl;
            saveDto.TotalEl = dto.TotalEl;
            saveDto.ManualEl = dto.ManualEl;
            saveDto.FinalEl = dto.FinalEl;
            saveDto.IsOk = dto.IsOk;
            saveDto.CreatedBy = equResModel.EquipmentCode;
            saveDto.CreatedOn = HymsonClock.Now();
            saveDto.UpdatedBy = saveDto.CreatedBy;
            saveDto.UpdatedOn = saveDto.CreatedOn;
            await _manuFillingDataRecordService.AddAsync(saveDto);

            //TODO
            //1. 新增表进行记录
        }

        /// <summary>
        /// 空托盘校验035
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task EmptyContainerCheckAsync(EmptyContainerCheckDto dto)
        {
            EquVerifyHelper.EmptyContainerCheckDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //2. 托盘校验
            //InteVehicleCodeQuery query = new InteVehicleCodeQuery();
            //query.Code = dto.ContainerCode;
            //query.SiteId = equResModel.SiteId;
            //await _inteVehicleService.GetByCodeAsync(query);
            //3. 校验托盘是否为空
            VehicleSFCRequestBo vehicleQuery = new VehicleSFCRequestBo();
            vehicleQuery.SiteId = equResModel.SiteId;
            vehicleQuery.VehicleCodes = new List<string>() { dto.ContainerCode };
            var vehicleList = await _manuCommonService.GetSfcListByVehicleCodesAsync(vehicleQuery);
            if (vehicleList.IsNullOrEmpty() == false)
            {
                string sfcListStr = string.Join(",", vehicleList);
                throw new CustomerValidationException(nameof(ErrorCode.MES45111))
                    .WithData("ContainerCode", dto.ContainerCode)
                    .WithData("sfcListStr", sfcListStr);
            }
        }

        /// <summary>
        /// 单电芯校验036
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ContainerSfcCheckAsync(ContainerSfcCheckDto dto)
        {
            EquVerifyHelper.ContainerSfcCheckDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);

            using var scope = _serviceProvider.CreateScope();
            //param.LocalizationService = _localizationService;
            //param.Proxy = scope.ServiceProvider.GetRequiredService<IJobContextProxy>();

            //JobRequestBo verifyParam = new JobRequestBo();
            //verifyParam.LocalizationService = _localizationService;
            //verifyParam.Proxy = scope.ServiceProvider.GetRequiredService<IJobContextProxy>();
            //verifyParam.UserName = dto.EquipmentCode;
            //verifyParam.SiteId = equResModel.SiteId;
            //verifyParam.EquipmentId = equResModel.EquipmentId;
            //verifyParam.ResourceId = equResModel.ResId;
            //verifyParam.ProcedureId = equResModel.ProcedureId;
            //List<InStationRequestBo> inList = new List<InStationRequestBo>();
            //InStationRequestBo inModel = new InStationRequestBo() { SFC = dto.Sfc };
            //inList.Add(inModel);
            //verifyParam.InStationRequestBos = inList;
            //await _manuCommonService.VerifyProcedureAsync(verifyParam);

            //2. 校验电芯是否存在
            ManuSfcProduceBySfcQuery query = new ManuSfcProduceBySfcQuery();
            query.SiteId = equResModel.SiteId;
            query.Sfc = dto.Sfc;
            var sfcEntity = await _manuSfcProduceService.GetBySFCAsync(query);
            //校验电芯是否合格
            if (sfcEntity.Status == SfcStatusEnum.Scrapping)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45091))
                    .WithData("Sfc", dto.Sfc);
            }
        }

        /// <summary>
        /// 托盘电芯绑定(在制品容器)037
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task BindContainerAsync(BindContainerDto dto)
        {
            EquVerifyHelper.BindContainerDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = new EquEquipmentResAllView();
            if(dto.OperationType == 0)
            {
                equResModel = await _equEquipmentService.GetEquResAsync(dto);
            }
            else
            {
                equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            }
            //2. 托盘电芯绑定
            InteVehicleBindDto bindDto = new InteVehicleBindDto();
            bindDto.ContainerCode = dto.ContainerCode;
            foreach (var item in dto.ContainerSfcList)
            {
                bindDto.SfcList.Add(new InteVehicleSfcDto { Sfc = item.Sfc, Location = item.Location });
            }
            bindDto.SiteId = equResModel.SiteId;
            bindDto.UserName = dto.EquipmentCode;
            //多条码进站
            SFCInStationBo inStationBo = new SFCInStationBo
            {
                SiteId = equResModel.SiteId,
                UserName = dto.EquipmentCode,
                EquipmentId = equResModel.EquipmentId,
                ResourceId = equResModel.ResId,
                ProcedureId = equResModel.ProcedureId,
                SFCs = dto.ContainerSfcList.Select(x => x.Sfc)
            };
            //托盘出站
            VehicleOutStationBo outStationBo = new VehicleOutStationBo
            {
                SiteId = equResModel.SiteId,
                UserName = dto.EquipmentCode,
                EquipmentId = equResModel.EquipmentId,
                ResourceId = equResModel.ResId,
                ProcedureId = equResModel.ProcedureId,
                OutStationRequestBos = new OutStationRequestBo[] { new() { VehicleCode = dto.ContainerCode, IsQualified = true } }
            };

            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            if((dto.OperationType & 1) == 1)
            {
                await _manuPassStationService.InStationRangeBySFCAsync(inStationBo);
            }
            await _inteVehicleService.VehicleBindOperationAsync(bindDto);
            if((dto.OperationType & 2) == 2)
            {
                await _manuPassStationService.OutStationRangeByVehicleAsync(outStationBo);
            }
            trans.Complete();

            //TODO 添加进出站逻辑
            //拿完整的工艺路线进行测试
        }

        /// <summary>
        /// 托盘电芯解绑(在制品容器)038
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UnBindContainerAsync(UnBindContainerDto dto)
        {
            EquVerifyHelper.UnBindContainerDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //2. 托盘电芯绑定
            InteVehicleUnBindDto bindDto = new InteVehicleUnBindDto();
            bindDto.ContainerCode = dto.ContainCode;
            bindDto.SfcList = dto.SfcList;
            bindDto.SiteId = equResModel.SiteId;
            bindDto.UserName = dto.EquipmentCode;

            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            //await _manuPassStationService.InStationRangeByVehicleAsync(inStationBo);
            await _inteVehicleService.VehicleUnBindOperationAsync(bindDto);
            //await _manuPassStationService.OutStationRangeBySFCAsync(outStationBo);
            trans.Complete();
        }

        /// <summary>
        /// 托盘NG电芯上报039
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ContainerNgReportAsync(ContainerNgReportDto dto)
        {
            EquVerifyHelper.ContainerNgReportDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);
            //2. 获取实际NG设备信息
            MultEquResAllQuery factQuery = new MultEquResAllQuery();
            factQuery.EquipmentCodeList = dto.NgSfcList.Select(m => m.NgEquipmentCode).ToList();
            factQuery.ResCodeList = dto.NgSfcList.Select(m => m.NgResourceCode).ToList();
            var factList = await _equEquipmentService.GetMultEquResAllAsync(factQuery);
            if (factList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001))
                    .WithData("EquipmentCode", equResModel.EquipmentCode).WithData("ResourceCode", equResModel.ResCode);
            }
            //3. 组装数据
            InteVehicleNgSfcDto ngDto = new InteVehicleNgSfcDto();
            ngDto.ContainerCode = dto.ContainerCode;
            foreach (var item in dto.NgSfcList)
            {
                var sfcEquResModel = factList.Where(m => m.EquipmentCode == item.NgEquipmentCode && m.ResCode == item.NgResourceCode).FirstOrDefault();
                if (sfcEquResModel == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45001));
                }

                InteVehicleSfcDetailDto ngModel = new InteVehicleSfcDetailDto();
                ngModel.NgCode = item.NgCode;
                ngModel.Sfc = item.Sfc;
                ngModel.OperationId = sfcEquResModel.ProcedureId;
                ngModel.ResourceId = sfcEquResModel.ResId;
                ngDto.NgSfcList.Add(ngModel);
            }
            ngDto.SiteId = equResModel.SiteId;
            ngDto.UserName = equResModel.EquipmentName;
            ngDto.OperationId = equResModel.ProcedureId;
            //数据库
            await _inteVehicleService.ContainerNgReportAsync(ngDto);
        }

        /// <summary>
        /// 托盘进站(容器进站)040
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task InboundInContainerAsync(InboundInContainerDto dto)
        {
            EquVerifyHelper.InboundInContainerDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);
            //2. 托盘进站
            VehicleInStationBo inDto = new VehicleInStationBo();
            inDto.EquipmentId = equResModel.EquipmentId;
            inDto.ResourceId = equResModel.ResId;
            inDto.SiteId = equResModel.SiteId;
            inDto.ProcedureId = equResModel.ProcedureId;
            inDto.UserName = dto.EquipmentCode;
            inDto.VehicleCodes = new List<string> { dto.ContainerCode };
            await _manuPassStationService.InStationRangeByVehicleAsync(inDto, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 托盘出站(容器出站)041
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundInContainerAsync(OutboundInContainerDto dto)
        {
            EquVerifyHelper.OutboundInContainerDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);
            //2. 构造数据
            //2.1 托盘出站时护具
            VehicleOutStationBo outDto = new VehicleOutStationBo();
            outDto.EquipmentId = equResModel.EquipmentId;
            outDto.ResourceId = equResModel.ResId;
            outDto.SiteId = equResModel.SiteId;
            outDto.ProcedureId = equResModel.ProcedureId;
            outDto.UserName = dto.EquipmentCode;
            OutStationRequestBo outBo = new OutStationRequestBo();
            outBo.VehicleCode = dto.ContainerCode;
            outBo.IsQualified = true;
            outDto.OutStationRequestBos = new List<OutStationRequestBo>() { outBo };
            //2.2 托盘参数
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
            foreach (var item in dto.ParamList)
            {
                EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                saveDto.ParamCode = item.ParamCode;
                saveDto.ParamValue = item.ParamValue;
                saveDto.CollectionTime = item.CollectionTime;
                saveDtoList.Add(saveDto);
            }
            saveDtoList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.Sfc = dto.ContainerCode;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            //3. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuPassStationService.OutStationRangeByVehicleAsync(outDto, RequestSourceEnum.EquipmentApi);
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
            trans.Complete();
        }

        /// <summary>
        /// 分选规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<ProcSortRuleDto>> SortingRuleAsync(SortingRuleDto dto)
        {
            EquVerifyHelper.SortingRuleDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //2. 查询激活的工单
            //PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId, equResModel.ResId);

            long productId = 0;
            if (string.IsNullOrEmpty(dto.ProductCode) == true)
            {
                ManuSfcProduceBySfcQuery sfcQuery = new ManuSfcProduceBySfcQuery();
                sfcQuery.Sfc = dto.Sfc;
                sfcQuery.SiteId = equResModel.SiteId;
                var sfcInfo = await _manuSfcProduceService.GetBySFCAsync(sfcQuery);
                productId = sfcInfo.ProductId;
            }
            else
            {
                EntityByCodeQuery matQuery = new EntityByCodeQuery();
                matQuery.Site = equResModel.SiteId;
                matQuery.Code = dto.ProductCode;
                var dbMaterial = await _procMaterialRepository.GetByCodeAsync(matQuery);
                if(dbMaterial == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45161));
                }
                productId = dbMaterial!.Id;
            }

            //3. 查询分选规则
            ProcSortRuleDetailEquQuery query = new ProcSortRuleDetailEquQuery();
            query.MaterialId = productId;
            var resultList = (await _procSortingRuleService.GetSortRuleDetailAsync(query)).ToList();
            return resultList!;
        }

    }
}
