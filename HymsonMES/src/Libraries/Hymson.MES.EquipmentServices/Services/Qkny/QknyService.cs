using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.AgvTaskRecord;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Qkny;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Qkny;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPoint.View;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.EquipmentServices.Dtos.Manufacture.ProductionProcess;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny.Formula;
using Hymson.MES.EquipmentServices.Services.Qkny.InteVehicle;
using Hymson.MES.EquipmentServices.Services.Qkny.LoadPoint;
using Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder;
using Hymson.MES.EquipmentServices.Services.Qkny.PowerOnParam;
using Hymson.MES.EquipmentServices.Services.Qkny.WhMaterialInventory;
using Hymson.MES.EquipmentServices.Validators.Manufacture.Qkny;
using Hymson.MES.Services.Dtos.AgvTaskRecord;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Dtos.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord;
using Hymson.MES.Services.Dtos.ManuFeedingTransferRecord;
using Hymson.MES.Services.Dtos.ManuFillingDataRecord;
using Hymson.MES.Services.Services.AgvTaskRecord;
using Hymson.MES.Services.Services.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Services.EquEquipmentAlarm;
using Hymson.MES.Services.Services.EquEquipmentHeartRecord;
using Hymson.MES.Services.Services.EquEquipmentLoginRecord;
using Hymson.MES.Services.Services.EquProcessParamRecord;
using Hymson.MES.Services.Services.EquProductParamRecord;
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
        /// 设备验证
        /// </summary>
        private readonly IEquEquipmentVerifyRepository _equEquipmentVerifyRepository;

        /// <summary>
        /// 登录记录
        /// </summary>
        private readonly IEquEquipmentLoginRecordService _equEquipmentLoginRecordService;

        /// <summary>
        /// 最新信息
        /// </summary>
        private readonly IManuEuqipmentNewestInfoService _manuEuqipmentNewestInfoService;

        /// <summary>
        /// 心跳记录
        /// </summary>
        private readonly IEquEquipmentHeartRecordService _equEquipmentHeartRecordService;

        /// <summary>
        /// 状态上报
        /// </summary>
        private readonly IManuEquipmentStatusTimeService _manuEquipmentStatusTimeService;

        /// <summary>
        /// 报警
        /// </summary>
        private readonly IEquEquipmentAlarmService _equEquipmentAlarmService;

        /// <summary>
        /// CCD文件上传
        /// </summary>
        private readonly ICcdFileUploadCompleteRecordService _ccdFileUploadCompleteRecordService;

        /// <summary>
        /// 开机参数
        /// </summary>
        private readonly IProcEquipmentGroupParamService _procEquipmentGroupParamService;

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
        /// 上料点关联资源
        /// </summary>
        private readonly IProcLoadPointLinkResourceRepository _procLoadPointLinkResourceRepository;

        /// <summary>
        /// AGV任务记录
        /// </summary>
        private readonly IAgvTaskRecordService _agvTaskRecordService;

        /// <summary>
        /// 配方
        /// </summary>
        private readonly IProcFormulaService _procFormulaService;

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
        /// 上料完成记录(制胶匀浆)
        /// </summary>
        private readonly IManuFeedingCompletedZjyjRecordService _manuFeedingCompletedZjyjRecordService;

        /// <summary>
        /// 批次转移
        /// </summary>
        private readonly IManuFeedingTransferRecordService _manuFeedingTransferRecordService;

        /// <summary>
        /// 设备投料非生产投料
        /// </summary>
        private readonly IManuFeedingNoProductionRecordService _manuFeedingNoProductionRecordService;

        /// <summary>
        /// 补液数据上报
        /// </summary>
        private readonly IManuFillingDataRecordService _manuFillingDataRecordService;

        /// <summary>
        /// 载具
        /// </summary>
        private readonly IInteVehicleService _inteVehicleService;

        /// <summary>
        /// 通用接口
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QknyService(IEquEquipmentRepository equEquipmentRepository,
            IEquEquipmentVerifyRepository equEquipmentVerifyRepository,
            IEquEquipmentLoginRecordService equEquipmentLoginRecordService,
            IManuEuqipmentNewestInfoService manuEuqipmentNewestInfoService,
            IEquEquipmentHeartRecordService equEquipmentHeartRecordService,
            IManuEquipmentStatusTimeService manuEquipmentStatusTimeService,
            IEquEquipmentAlarmService equEquipmentAlarmService,
            ICcdFileUploadCompleteRecordService ccdFileUploadCompleteRecordService,
            IProcEquipmentGroupParamService procEquipmentGroupParamService,
            IPlanWorkOrderService planWorkOrderService,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IManuFeedingService manuFeedingService,
            IProcLoadPointLinkResourceRepository procLoadPointLinkResourceRepository,
            IAgvTaskRecordService agvTaskRecordService,
            IEquProcessParamRecordService equProcessParamRecordService,
            IProcFormulaService procFormulaService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IManuSfcProduceService manuSfcProduceService,
            IManuSfcService manuSfcServicecs,
            IEquProductParamRecordService equProductParamRecordService,
            IManuPassStationService manuPassStationService,
            IProcLoadPointService procLoadPointService,
            IWhMaterialInventoryService whMaterialInventoryService,
            IManuFeedingCompletedZjyjRecordService manuFeedingCompletedZjyjRecordService,
            IManuFeedingTransferRecordService manuFeedingTransferRecordService,
            IManuFeedingNoProductionRecordService manuFeedingNoProductionRecordService,
            IManuFillingDataRecordService manuFillingDataRecordService,
            IInteVehicleService inteVehicleService,
            IManuCommonService manuCommonService,
            AbstractValidator<OperationLoginDto> validationOperationLoginDto)
        {
            _equEquipmentRepository = equEquipmentRepository;
            _equEquipmentVerifyRepository = equEquipmentVerifyRepository;
            _equEquipmentLoginRecordService = equEquipmentLoginRecordService;
            _manuEuqipmentNewestInfoService = manuEuqipmentNewestInfoService;
            _equEquipmentHeartRecordService = equEquipmentHeartRecordService;
            _manuEquipmentStatusTimeService = manuEquipmentStatusTimeService;
            _equEquipmentAlarmService = equEquipmentAlarmService;
            _ccdFileUploadCompleteRecordService = ccdFileUploadCompleteRecordService;
            _procEquipmentGroupParamService = procEquipmentGroupParamService;
            _planWorkOrderService = planWorkOrderService;
            _whMaterialInventoryRepository = whMaterialInventoryRepository; 
            _manuFeedingService = manuFeedingService;
            _procLoadPointLinkResourceRepository = procLoadPointLinkResourceRepository;
            _agvTaskRecordService = agvTaskRecordService;
            _equProcessParamRecordService = equProcessParamRecordService;
            _procFormulaService = procFormulaService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _manuSfcProduceService = manuSfcProduceService;
            _manuSfcServicecs = manuSfcServicecs;
            _equProductParamRecordService = equProductParamRecordService;
            _manuPassStationService = manuPassStationService;
            _procLoadPointService = procLoadPointService;
            _whMaterialInventoryService = whMaterialInventoryService;
            _manuFeedingCompletedZjyjRecordService = manuFeedingCompletedZjyjRecordService;
            _manuFeedingTransferRecordService = manuFeedingTransferRecordService;
            _manuFeedingNoProductionRecordService = manuFeedingNoProductionRecordService;
            _manuFillingDataRecordService = manuFillingDataRecordService;
            _inteVehicleService = inteVehicleService;
            _manuCommonService = manuCommonService;
            //校验器
            _validationOperationLoginDto = validationOperationLoginDto;
        }

        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OperatorLoginAsync(OperationLoginDto dto)
        {
            await _validationOperationLoginDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 校验用户名密码是否和设备匹配(equ_equipment_verify)
            var verifyList = await _equEquipmentVerifyRepository.GetEquipmentVerifyByEquipmentIdAsync(equResModel.EquipmentId);
            if (verifyList == null || !verifyList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45011)).WithData("EquipmentCode", dto.EquipmentCode); ;
            }
            bool verifyCheck = verifyList.Where(m => m.Account == dto.OperatorUserID && m.Password == dto.OperatorPassword).Any();
            if (verifyCheck == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45012)).WithData("EquipmentCode", dto.EquipmentCode); ;
            }
            //3. 新增登录记录
            EquEquipmentLoginRecordSaveDto loginRecordDto = new EquEquipmentLoginRecordSaveDto();
            loginRecordDto.Id = IdGenProvider.Instance.CreateId();
            loginRecordDto.SiteId = equResModel.SiteId;
            loginRecordDto.Account = dto.OperatorUserID;
            loginRecordDto.Password = dto.OperatorPassword;
            loginRecordDto.EquipmentId = equResModel.EquipmentId;
            loginRecordDto.CreateOn = HymsonClock.Now();
            loginRecordDto.CreateBy = equResModel.EquipmentCode;
            loginRecordDto.UpdateOn = loginRecordDto.CreateOn;
            loginRecordDto.UpdateBy = equResModel.EquipmentCode;
            //4. 记录最新状态
            ManuEuqipmentNewestInfoSaveDto newestDto = new ManuEuqipmentNewestInfoSaveDto();
            newestDto.Id = IdGenProvider.Instance.CreateId();
            newestDto.Type = NewestInfoEnum.Login;
            newestDto.LoginResult = "1";
            newestDto.LoginResultUpdateOn = HymsonClock.Now();
            newestDto.CreatedOn = newestDto.LoginResultUpdateOn;
            newestDto.CreatedBy = equResModel.EquipmentCode;
            newestDto.UpdatedOn = newestDto.CreatedOn;
            newestDto.UpdatedBy = equResModel.EquipmentCode;
            newestDto.SiteId = equResModel.SiteId;
            newestDto.EquipmentId = equResModel.EquipmentId;
            //5. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _equEquipmentLoginRecordService.AddAsync(loginRecordDto);
            await _manuEuqipmentNewestInfoService.AddOrUpdateAsync(newestDto);
            trans.Complete();
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task HeartbeatAsync(HeartbeatDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 添加心跳记录
            EquEquipmentHeartRecordSaveDto heartRecordDto = new EquEquipmentHeartRecordSaveDto();
            heartRecordDto.Id = IdGenProvider.Instance.CreateId();
            heartRecordDto.SiteId = equResModel.SiteId;
            heartRecordDto.IsOnline = dto.IsOnline == true ? "1" : "0";
            heartRecordDto.EquipmentId = equResModel.EquipmentId;
            heartRecordDto.CreatedOn = HymsonClock.Now();
            heartRecordDto.CreatedBy = equResModel.EquipmentCode;
            heartRecordDto.UpdatedOn = heartRecordDto.CreatedOn;
            heartRecordDto.UpdatedBy = equResModel.EquipmentCode;
            heartRecordDto.Remark = "";
            //3. 记录最新状态
            ManuEuqipmentNewestInfoSaveDto newestDto = new ManuEuqipmentNewestInfoSaveDto();
            newestDto.Id = IdGenProvider.Instance.CreateId();
            newestDto.Type = NewestInfoEnum.Heart;
            newestDto.Heart = heartRecordDto.IsOnline;
            newestDto.HeartUpdateOn = HymsonClock.Now();
            newestDto.CreatedOn = newestDto.HeartUpdateOn;
            newestDto.CreatedBy = equResModel.EquipmentCode;
            newestDto.UpdatedOn = newestDto.HeartUpdateOn;
            newestDto.UpdatedBy = equResModel.EquipmentCode;
            newestDto.SiteId = equResModel.SiteId;
            newestDto.EquipmentId = equResModel.EquipmentId;
            //4. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _equEquipmentHeartRecordService.AddAsync(heartRecordDto);
            await _manuEuqipmentNewestInfoService.AddOrUpdateAsync(newestDto);
            trans.Complete();
        }

        /// <summary>
        /// 状态上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task StateAsync(StateDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 添加状态时间
            ManuEquipmentStatusTimeSaveDto statusDto = new ManuEquipmentStatusTimeSaveDto();
            statusDto.EquipmentId = equResModel.EquipmentId;
            statusDto.NextStatus = dto.StateCode.ToUpper();
            statusDto.CreatedBy = equResModel.EquipmentCode;
            statusDto.EquipmentDownReason = dto.DownReason;
            //3. 记录最新状态
            ManuEuqipmentNewestInfoSaveDto newestDto = new ManuEuqipmentNewestInfoSaveDto();
            newestDto.Id = IdGenProvider.Instance.CreateId();
            newestDto.Type = NewestInfoEnum.Status;
            newestDto.Status = dto.StateCode.ToUpper();
            newestDto.StatusUpdateOn = HymsonClock.Now();
            newestDto.CreatedOn = newestDto.StatusUpdateOn;
            newestDto.CreatedBy = equResModel.EquipmentCode;
            newestDto.UpdatedOn = newestDto.StatusUpdateOn;
            newestDto.UpdatedBy = equResModel.EquipmentCode;
            newestDto.SiteId = equResModel.SiteId;
            newestDto.EquipmentId = equResModel.EquipmentId;
            newestDto.DownReason = dto.DownReason;
            //4. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuEquipmentStatusTimeService.AddAsync(statusDto);
            await _manuEuqipmentNewestInfoService.AddOrUpdateAsync(newestDto);
            trans.Complete();
        }

        /// <summary>
        /// 故障上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AlarmAsync(AlarmDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 添加故障记录
            EquEquipmentAlarmSaveDto saveDto = new EquEquipmentAlarmSaveDto();
            saveDto.Id = IdGenProvider.Instance.CreateId();
            saveDto.EquipmentId = equResModel.EquipmentId;
            saveDto.Status = dto.Status;
            saveDto.AlarmCode = dto.AlarmCode.ToUpper();
            saveDto.AlarmMsg = dto.AlarmMsg;
            saveDto.AlarmLevel = dto.AlarmLevel.ToUpper();
            saveDto.CreatedBy = equResModel.EquipmentCode;
            saveDto.CreatedOn = HymsonClock.Now();
            saveDto.UpdatedBy = saveDto.CreatedBy;
            saveDto.UpdatedOn = saveDto.CreatedOn;
            saveDto.SiteId = equResModel.SiteId;
            await _equEquipmentAlarmService.AddAsync(saveDto);
        }

        /// <summary>
        /// CCD文件上传完成006
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CcdFileUploadCompleteAsync(CCDFileUploadCompleteDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 添加文件信息
            List<CcdFileUploadCompleteRecordSaveDto> saveDtoList = new List<CcdFileUploadCompleteRecordSaveDto>();
            foreach(var sfcItem in dto.SfcList)
            {
                foreach(var uriItem in sfcItem.UriList)
                {
                    CcdFileUploadCompleteRecordSaveDto model = new CcdFileUploadCompleteRecordSaveDto();
                    model.Id = IdGenProvider.Instance.CreateId();
                    model.EquipmentId = equResModel.EquipmentId;
                    model.SiteId = equResModel.SiteId;
                    model.CreatedOn = HymsonClock.Now();
                    model.CreatedBy = equResModel.EquipmentCode;
                    model.UpdatedOn = model.CreatedOn;
                    model.UpdatedBy = model.CreatedBy;
                    model.Sfc = sfcItem.Sfc;
                    model.SfcIsPassed = sfcItem.Passed;
                    model.Uri = uriItem.Uri;
                    model.UriIsPassed = uriItem.Passed;
                    saveDtoList.Add(model);
                }
            }
            await _ccdFileUploadCompleteRecordService.AddMultAsync(saveDtoList);
            //1. 新增表 ccd_file_upload_complete_record，用于记录每个条码对应的CCD文件路径及是否合格
            //2. 明细和主表记录到一张表
        }

        /// <summary>
        /// 获取开机参数列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<GetRecipeListReturnDto>> GetRecipeListAsync(GetRecipeListDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 获取数据
            ProcEquipmentGroupParamEquProductQuery query = new ProcEquipmentGroupParamEquProductQuery();
            query.EquipmentId = equResModel.EquipmentId;
            query.ProductCode = dto.ProductCode;
            var paramList = await _procEquipmentGroupParamService.QueryByEquProductAsync(query);
            List<GetRecipeListReturnDto> resultList = new List<GetRecipeListReturnDto>();
            foreach (var item in paramList)
            {
                GetRecipeListReturnDto result = new GetRecipeListReturnDto();
                result.RecipeCode = item.Code;
                result.ProductCode = item.MaterialCode;
                result.Version = item.Version;
                result.LastUpdateOnTime = item.UpdatedOn ?? item.CreatedOn;
                resultList.Add(result);
            }
            return resultList;

            //TODO
            //1. 获取 proc_equipment_group_param 表中type=1的数据，并转换成相应数据格式
            //2. 对应系统 Recipe参数 功能
        }

        /// <summary>
        /// 获取开机参数明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<GetRecipeDetailReturnDto> GetRecipeDetailAsync(GetRecipeDetailDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 获取开机参数编码的对应激活的数据
            ProcEquipmentGroupParamCodeDetailQuery query = new ProcEquipmentGroupParamCodeDetailQuery();
            query.Code = dto.RecipeCode;
            query.SiteId = equResModel.SiteId;
            var list = await _procEquipmentGroupParamService.GetDetailByCode(query);
            GetRecipeDetailReturnDto result = new GetRecipeDetailReturnDto();
            result.Version = list[0].Version;
            result.LastUpdateOnTime = list[0].UpdatedOn ?? list[0].CreatedOn;
            foreach(var item in list)
            {
                if(item == null)
                {
                    continue;
                }
                if(string.IsNullOrEmpty(item.ParameterCode) == false)
                {
                    RecipeParamDto param = new RecipeParamDto();
                    param.ParamUpper = item.MaxValue == null ? "" : item.MaxValue.ToString();
                    param.ParamLower = item.MinValue == null ? "" : item.MinValue.ToString();
                    param.ParamCode = item.ParameterCode;
                    result.ParamList.Add(param);
                }
            }

            return result;
        }

        /// <summary>
        /// 开机参数校验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task RecipeAsync(RecipeDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 参数上下限校验
            //暂不加
            //3. 版本校验
            ProcEquipmentGroupCheckQuery query = new ProcEquipmentGroupCheckQuery();
            query.SiteId = equResModel.SiteId;
            query.Code = dto.RecipeCode;
            query.Version = dto.Version;
            query.MaterialCode = dto.ProductCode;
            var entity = await _procEquipmentGroupParamService.GetEntityByCodeVersion(query);
        }

        /// <summary>
        /// 原材料上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task FeedingAsync(FeedingDto dto)
        {
            ManuFeedingMaterialSaveDto saveDto = new ManuFeedingMaterialSaveDto();
            saveDto.BarCode = dto.Sfc;
            if (dto.IsFeedingPoint == false)
            {
                //1. 获取设备基础信息
                EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
                PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);

                saveDto.Source = ManuSFCFeedingSourceEnum.BOM;
                saveDto.SiteId = equResModel.SiteId;
                saveDto.ResourceId = equResModel.ResId;
            }
            else
            {
                //根据
                ProcLoadPointCodeLinkResourceQuery query = new ProcLoadPointCodeLinkResourceQuery();
                query.LoadPoint = dto.EquipmentCode;
                var res = await _procLoadPointLinkResourceRepository.GetByCodeAsync(query);
                if(res == null || res.Any() == false || res.Count() != 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45040));
                }
                saveDto.Source = ManuSFCFeedingSourceEnum.FeedingPoint;
                saveDto.FeedingPointId = res.FirstOrDefault()!.LoadPointId;
                saveDto.SiteId = res.FirstOrDefault()!.SiteId!;
                saveDto.ResourceId = res.FirstOrDefault()!.ResourceId!;
            }
            //3. 上料
            var feedResult = await _manuFeedingService.CreateAsync(saveDto);

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
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
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
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 调用AGV接口
            var result = ""; //await HttpHelper.HttpPostAsync("", dto.Content, "");
            //4. 存储数据
            AgvTaskRecordSaveDto saveDto = new AgvTaskRecordSaveDto();
            saveDto.Id = IdGenProvider.Instance.CreateId();
            saveDto.SiteId = equResModel.SiteId;
            saveDto.EquipmentId = equResModel.EquipmentId;
            saveDto.SendContent = dto.Content;
            saveDto.TaskType = dto.Type;
            saveDto.ReceiveContent = result;
            saveDto.CreatedOn = HymsonClock.Now();
            saveDto.CreatedBy = dto.EquipmentCode;
            saveDto.UpdatedOn = saveDto.CreatedOn;
            saveDto.UpdatedBy = saveDto.CreatedBy;
            await _agvTaskRecordService.AddAsync(saveDto);
        }

        /// <summary>
        /// 获取配方列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<FormulaListGetReturnDto>> FormulaListGetAsync(FormulaListGetDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(equDto);
            //2. 查询设备激活工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 查询上料点当前物料（缓存罐(上料点)一次只能有一种物料）
            //3.1 查询上料点
            ProcLoadPointQuery pointQuery = new ProcLoadPointQuery();
            pointQuery.SiteId = equResModel.SiteId;
            pointQuery.LoadPoint = dto.EquipmentCode;
            var loadPoint = await _procLoadPointService.GetProcLoadPointEntitiesAsync(pointQuery);
            //3.2 查询当前上料点最后一次的物料
            //GetFeedingPointNewQuery pointRecrod = new GetFeedingPointNewQuery() { FeedingPointId = loadPoint.Id };
            //var loadPointRecord = await _manuFeedingService.GetFeedingPointNewAsync(pointRecrod);
            WhMaterialInventoryBarCodeQuery whMaterialQuery = new WhMaterialInventoryBarCodeQuery();
            whMaterialQuery.SiteId = equResModel.SiteId;
            whMaterialQuery.BarCode = dto.SfcList.IsNullOrEmpty() == true ? "" : dto.SfcList[0];
            var sfcMaterial = await _whMaterialInventoryService.GetByBarCodeAsync(whMaterialQuery);
            //4. 校验物料是否在激活工单对应的BOM里
            var curPointMaterialId = sfcMaterial.MaterialId;
            var orderMaterialList = await _planWorkOrderService.GetWorkOrderMaterialAsync(planEntity.ProductBOMId);
            if(orderMaterialList.Contains(curPointMaterialId) == false)
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
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(equDto);
            //2. 查询设备激活工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 构造数据
            ManuFeedingMaterialSaveDto saveDto = new ManuFeedingMaterialSaveDto();
            //saveDto.BarCode = dto.Sfc;
            saveDto.Source = ManuSFCFeedingSourceEnum.BOM;
            saveDto.SiteId = equResModel.SiteId;
            saveDto.ResourceId = equResModel.ResId;
            //3. 上料（考虑改成对条码进行合批）
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
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            var inEqu = dbList.Where(m =>m.Code == dto.EquipmentCodeIn).FirstOrDefault()!;
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
            else if(outEqu.DataType == "2" && inEqu.DataType == "2")
            {
                saveDto.TransferType = ManuSFCFeedingTransferEnum.Resource;
                saveDto.SourceId = outEqu.ResId;
                saveDto.DestId = inEqu.ResId;
            }
            else if(outEqu.DataType == "1" && inEqu.DataType == "2")
            {
                saveDto.TransferType = ManuSFCFeedingTransferEnum.FeedingPointResource;
                saveDto.SourceId= outEqu.ResId;
                saveDto.DestId = inEqu.ResId;
            }
            else if(outEqu.DataType == "2" && inEqu.DataType == "1")
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
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //
            DateTime curDate = HymsonClock.Now();
            List<ManuFeedingNoProductionRecordSaveDto> saveDtoList = new List<ManuFeedingNoProductionRecordSaveDto>();
            foreach(var item in dto.ConsumeSfcList)
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
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
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
        /// 产出米数上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundMetersReportAsync(OutboundMetersReportDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //构造数据
            UpdateQtyBySfcCommand command = new UpdateQtyBySfcCommand();
            command.SFC = dto.Sfc;
            command.Qty = dto.OkQty;
            command.SiteId = equResModel.SiteId;
            command.UpdatedBy = dto.EquipmentCode;
            command.UpdatedOn = HymsonClock.Now();

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
                m.Sfc = dto.Sfc;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });

            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            OutStationRequestBo outReqBo = new OutStationRequestBo();
            outReqBo.SFC = dto.Sfc;
            outReqBo.IsQualified = true;
            outBo.OutStationRequestBos = new List<OutStationRequestBo>() { outReqBo };
            // 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuSfcProduceService.UpdateQtyBySfcAsync(command);
            await _manuSfcServicecs.UpdateQtyBySfcAsync(command);
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
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
        /// 出站027
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
            if(dto.BindFeedingCodeList.IsNullOrEmpty() == false)
            {
                List<OutStationConsumeBo> conList = new List<OutStationConsumeBo>();
                foreach(var item in  dto.BindFeedingCodeList)
                {
                    conList.Add(new OutStationConsumeBo() { BarCode = item });
                }
                outReqBo.ConsumeList = conList;
            }
            if(dto.NgList.IsNullOrEmpty() == false)
            {
                List<OutStationUnqualifiedBo> unCodeList = new List<OutStationUnqualifiedBo>();
                foreach(var item in dto.NgList)
                {
                    unCodeList.Add(new OutStationUnqualifiedBo() { UnqualifiedCode = item });
                }
                outReqBo.OutStationUnqualifiedList = unCodeList;
            }
            outBo.OutStationRequestBos = new List<OutStationRequestBo>() { outReqBo };
            //2.2 出站参数
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
                m.Sfc = dto.Sfc;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            //3. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
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
            foreach(var item in dto.SfcList)
            {
                InboundMoreReturnDto model = new InboundMoreReturnDto();
                model.Sfc = item;
                resultList.Add(model);
            }

            return resultList;
        }

        /// <summary>
        /// 出站多个
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
            //3. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            var outResult = await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
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
        /// 补液数据上报034
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task FillingDataAsync(FillingDataDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
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
            if(vehicleList.IsNullOrEmpty() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45111));
            }

            //TODO
            //2. 校验托盘是否存在系统中（待确认）
            //3. 托盘(载具)表 inte_vehicle_freight_stack 是否存在数据
        }

        /// <summary>
        /// 单电芯校验036
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ContainerSfcCheckAsync(ContainerSfcCheckDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 校验电芯是否存在
            ManuSfcProduceBySfcQuery query = new ManuSfcProduceBySfcQuery();
            query.SiteId = equResModel.SiteId;
            query.Sfc = dto.Sfc;
            await _manuSfcProduceService.GetBySFCAsync(query);
        }

        /// <summary>
        /// 托盘电芯绑定(在制品容器)037
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task BindContainerAsync(BindContainerDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 托盘电芯绑定
            InteVehicleBindDto bindDto = new InteVehicleBindDto();
            bindDto.ContainerCode = dto.ContainerCode;
            foreach(var item in dto.ContainerSfcList)
            {
                bindDto.SfcList.Add(new InteVehicleSfcDto { Sfc = item.Sfc, Location = item.Location });
            }
            bindDto.SiteId = equResModel.SiteId;
            bindDto.UserName = dto.EquipmentCode;
            await _inteVehicleService.VehicleBindOperationAsync(bindDto);
        }

        /// <summary>
        /// 托盘电芯解绑(在制品容器)038
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UnBindContainerAsync(UnBindContainerDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 托盘电芯绑定
            InteVehicleUnBindDto bindDto = new InteVehicleUnBindDto();
            bindDto.ContainerCode = dto.ContainCode;
            bindDto.SfcList = dto.SfcList;
            bindDto.SiteId = equResModel.SiteId;
            bindDto.UserName = dto.EquipmentCode;
            await _inteVehicleService.VehicleUnBindOperationAsync(bindDto);

            //TODO
            //1. 校验电芯是否在托盘中
            //2. inte_vehicle_freight_stack 删除绑定数据
            //3. 添加 inte_vehicle_freight_record 解绑记录
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
    }
}
