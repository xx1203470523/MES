using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Qkny;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.Parameter;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Services.Parameter.ProcessCollection;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Hymson.MES.EquipmentServices.Services.Qkny.PowerOnParam;
using Hymson.MES.EquipmentServices.Validators.EquVerifyHelper;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.MES.Services.Dtos.EquOpenParamRecord;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Services.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Services.EquEquipmentAlarm;
using Hymson.MES.Services.Services.EquEquipmentHeartRecord;
using Hymson.MES.Services.Services.EquEquipmentLoginRecord;
using Hymson.MES.Services.Services.EquOpenParamRecord;
using Hymson.MES.Services.Services.EquProcessParamRecord;
using Hymson.MES.Services.Services.EquProductParamRecord;
using Hymson.MES.Services.Services.EquToolLifeRecord;
using Hymson.MES.Services.Services.ManuEquipmentStatusTime;
using Hymson.MES.Services.Services.ManuEuqipmentNewestInfo;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.Qkny.Common
{
    /// <summary>
    /// 设备通用接口
    /// </summary>
    public class EquCommonService : IEquCommonService
    {
        #region 验证器

        /// <summary>
        /// 操作员登录验证器
        /// </summary>
        private readonly AbstractValidator<OperationLoginDto> _validationOperationLoginDto;

        /// <summary>
        /// 心跳验证器
        /// </summary>
        private readonly AbstractValidator<HeartbeatDto> _validationHeartbeatDto;

        /// <summary>
        /// 状态验证器
        /// </summary>
        private readonly AbstractValidator<StateDto> _validationStateDto;

        /// <summary>
        /// 状态验证器
        /// </summary>
        private readonly AbstractValidator<AlarmDto> _validationAlarmDto;

        /// <summary>
        /// CCD上传文件完成
        /// </summary>
        private readonly AbstractValidator<CCDFileUploadCompleteDto> _validationCCDFileUploadCompleteDto;

        /// <summary>
        /// 状态验证器
        /// </summary>
        private readonly AbstractValidator<ToolLifeDto> _validationToolLifeDto;

        /// <summary>
        /// 开机参数验证器
        /// </summary>
        private readonly AbstractValidator<GetRecipeListDto> _validationGetRecipeListDto;

        /// <summary>
        /// 开机参数详情
        /// </summary>
        private readonly AbstractValidator<GetRecipeDetailDto> _validationGetRecipeDetailDto;

        /// <summary>
        /// 开机参数详情
        /// </summary>
        private readonly AbstractValidator<RecipeDto> _validationRecipeDto;

        /// <summary>
        /// 设备过程参数
        /// </summary>
        private readonly AbstractValidator<EquipmentProcessParamDto> _validationEquipmentProcessParamDto;

        /// <summary>
        /// 产品过程参数
        /// </summary>
        private readonly AbstractValidator<ProductParamDto> _validationProductParamDto;

        #endregion

        /// <summary>
        /// 设备服务
        /// </summary>
        private readonly IEquEquipmentService _equEquipmentService;

        /// <summary>
        /// 设备验证
        /// </summary>
        private readonly IEquEquipmentVerifyRepository _equEquipmentVerifyRepository;

        /// <summary>
        /// 设备token
        /// </summary>
        private readonly IEquEquipmentTokenRepository _equEquipmentTokenRepository;

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
        /// 工装夹具寿命
        /// </summary>
        private readonly IEquToolLifeRecordService _equToolLifeRecordService;

        /// <summary>
        /// 开机参数
        /// </summary>
        private readonly IProcEquipmentGroupParamService _procEquipmentGroupParamService;

        /// <summary>
        /// 设备过程参数
        /// </summary>
        private readonly IEquProcessParamRecordService _equProcessParamRecordService;

        /// <summary>
        /// 产品参数
        /// </summary>
        private readonly IEquProductParamRecordService _equProductParamRecordService;

        /// <summary>
        /// 开机参数
        /// </summary>
        private readonly IEquOpenParamRecordService _equOpenParamRecordService;

        /// <summary>
        /// 参数收集
        /// </summary>
        private readonly IManuProductParameterService _manuProductParameterService;

        /// <summary>
        /// 参数采集
        /// </summary>
        private readonly IProcessCollectionService _processCollectionService;

        /// <summary>
        /// 文件上传
        /// </summary>
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquCommonService(
            IEquEquipmentService equEquipmentService,
            IEquEquipmentVerifyRepository equEquipmentVerifyRepository,
            IEquEquipmentLoginRecordService equEquipmentLoginRecordService,
            IManuEuqipmentNewestInfoService manuEuqipmentNewestInfoService,
            IEquEquipmentHeartRecordService equEquipmentHeartRecordService,
            IManuEquipmentStatusTimeService manuEquipmentStatusTimeService,
            IEquEquipmentAlarmService equEquipmentAlarmService,
            ICcdFileUploadCompleteRecordService ccdFileUploadCompleteRecordService,
            IEquToolLifeRecordService equToolLifeRecordService,
            IProcEquipmentGroupParamService procEquipmentGroupParamService,
            IEquProcessParamRecordService equProcessParamRecordService,
            IEquProductParamRecordService equProductParamRecordService,
            IEquOpenParamRecordService equOpenParamRecordService,
            AbstractValidator<OperationLoginDto> validationOperationLoginDto,
            AbstractValidator<HeartbeatDto> validationHeartbeatDto,
            AbstractValidator<StateDto> validationStateDto,
            AbstractValidator<AlarmDto> validationAlarmDto,
            AbstractValidator<CCDFileUploadCompleteDto> validationCCDFileUploadCompleteDto,
            AbstractValidator<ToolLifeDto> validationToolLifeDto,
            AbstractValidator<GetRecipeListDto> validationGetRecipeListDto,
            AbstractValidator<GetRecipeDetailDto> validationGetRecipeDetailDto,
            AbstractValidator<RecipeDto> validationRecipeDto,
            AbstractValidator<EquipmentProcessParamDto> validationEquipmentProcessParamDto,
            AbstractValidator<ProductParamDto> validationProductParamDto,
            IManuProductParameterService manuProductParameterService,
            IMinioService minioService,
            IProcessCollectionService processCollectionService,
            IEquEquipmentTokenRepository equEquipmentTokenRepository)
        {
            _equEquipmentService = equEquipmentService;
            _equEquipmentVerifyRepository = equEquipmentVerifyRepository;
            _equEquipmentLoginRecordService = equEquipmentLoginRecordService;
            _manuEuqipmentNewestInfoService = manuEuqipmentNewestInfoService;
            _equEquipmentHeartRecordService = equEquipmentHeartRecordService;
            _manuEquipmentStatusTimeService = manuEquipmentStatusTimeService;
            _equEquipmentAlarmService = equEquipmentAlarmService;
            _ccdFileUploadCompleteRecordService = ccdFileUploadCompleteRecordService;
            _equToolLifeRecordService = equToolLifeRecordService;
            _procEquipmentGroupParamService = procEquipmentGroupParamService;
            _equProcessParamRecordService = equProcessParamRecordService;
            _equProductParamRecordService = equProductParamRecordService;
            _equOpenParamRecordService = equOpenParamRecordService;
            //验证器
            _validationOperationLoginDto = validationOperationLoginDto;
            _validationHeartbeatDto = validationHeartbeatDto;
            _validationStateDto = validationStateDto;
            _validationAlarmDto = validationAlarmDto;
            _validationCCDFileUploadCompleteDto = validationCCDFileUploadCompleteDto;
            _validationToolLifeDto = validationToolLifeDto;
            _validationGetRecipeListDto = validationGetRecipeListDto;
            _validationGetRecipeDetailDto = validationGetRecipeDetailDto;
            _validationRecipeDto = validationRecipeDto;
            _validationEquipmentProcessParamDto = validationEquipmentProcessParamDto;
            _validationProductParamDto = validationProductParamDto;
            _manuProductParameterService = manuProductParameterService;
            _minioService = minioService;
            _processCollectionService = processCollectionService;
            _equEquipmentTokenRepository = equEquipmentTokenRepository;
        }

        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<OperationLoginReturnDto> OperatorLoginAsync(OperationLoginDto dto)
        {
            EquVerifyHelper.OperationLoginDto(dto);
            //await _validationOperationLoginDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            long equipmentId = equResModel.EquipmentId;
            //2. 校验用户名密码是否和设备匹配(equ_equipment_verify)
            var verifyList = await _equEquipmentVerifyRepository.GetEquipmentVerifyByEquipmentIdAsync(equipmentId);
            if (verifyList == null || !verifyList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45011)).WithData("EquipmentCode", dto.EquipmentCode);
            }
            var verifyModel = verifyList.Where(m => m.Account == dto.OperatorUserID && m.Password == dto.OperatorPassword).FirstOrDefault();
            if (verifyModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45012)).WithData("EquipmentCode", dto.EquipmentCode);
            }
            var tokenModel = await _equEquipmentTokenRepository.GetByEquipmentIdAsync(equipmentId);
            if(tokenModel == null || string.IsNullOrEmpty(tokenModel.Token) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45014)).WithData("EquipmentCode", dto.EquipmentCode);
            }
            //3.1 新增登录记录
            EquEquipmentLoginRecordSaveDto loginRecordDto = new EquEquipmentLoginRecordSaveDto();
            loginRecordDto.Id = IdGenProvider.Instance.CreateId();
            loginRecordDto.SiteId = equResModel.SiteId;
            loginRecordDto.Account = dto.OperatorUserID;
            loginRecordDto.Password = dto.OperatorPassword;
            loginRecordDto.EquipmentId = equipmentId;
            loginRecordDto.CreateOn = HymsonClock.Now();
            loginRecordDto.CreateBy = equResModel.EquipmentCode;
            loginRecordDto.UpdateOn = loginRecordDto.CreateOn;
            loginRecordDto.UpdateBy = equResModel.EquipmentCode;
            //3.2 记录最新状态
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
            newestDto.EquipmentId = equipmentId;
            //5. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _equEquipmentLoginRecordService.AddAsync(loginRecordDto);
            await _manuEuqipmentNewestInfoService.AddOrUpdateAsync(newestDto);
            trans.Complete();

            OperationLoginReturnDto result = new OperationLoginReturnDto();
            result.AccountType = ((int)verifyModel.AccountType).ToString();
            result.Authorization = $"Bearer {tokenModel.Token}";

            return result;
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task HeartbeatAsync(HeartbeatDto dto)
        {
            await _validationHeartbeatDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
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
            EquVerifyHelper.StateDto(dto);
            //await _validationStateDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
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
        /// 故障上报004
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AlarmAsync(AlarmDto dto)
        {
            await _validationAlarmDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
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
            await _validationCCDFileUploadCompleteDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //2. 添加文件信息
            List<CcdFileUploadCompleteRecordSaveDto> saveDtoList = new List<CcdFileUploadCompleteRecordSaveDto>();
            foreach (var sfcItem in dto.SfcList)
            {
                foreach (var uriItem in sfcItem.UriList)
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
        }

        /// <summary>
        /// 工装寿命上报042
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ToolLifeAsync(ToolLifeDto dto)
        {
            EquVerifyHelper.ToolLifeDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //2. 添加记录
            List<EquToolLifeRecordSaveDto> saveDtos = new();
            if (!string.IsNullOrWhiteSpace(dto.ToolCode))
            {
                EquToolLifeRecordSaveDto saveDto = new EquToolLifeRecordSaveDto();
                saveDto.EquipmentId = equResModel.EquipmentId;
                saveDto.ToolCode = dto.ToolCode;
                saveDto.ToolLife = dto.UsedLife;
                saveDto.CreatedBy = dto.EquipmentCode;
                saveDto.CreatedOn = HymsonClock.Now();
                saveDto.UpdatedBy = dto.EquipmentCode;
                saveDto.UpdatedOn = saveDto.CreatedOn;

                saveDtos.Add(saveDto);
            }
            if (dto.ToolLifes != null && dto.ToolLifes.Any())
            {
                saveDtos.AddRange(dto.ToolLifes.Select(item => new EquToolLifeRecordSaveDto
                {
                    EquipmentId = equResModel.EquipmentId,
                    ToolCode = item.ToolCode,
                    ToolLife = item.UsedLife,
                    CreatedBy = dto.EquipmentCode,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = dto.EquipmentCode,
                    UpdatedOn = HymsonClock.Now()
                }));
            }
            //3 数据库操作
            await _equToolLifeRecordService.AddRangeAsync(saveDtos);
        }

        /// <summary>
        /// 获取开机参数列表007
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<GetRecipeListReturnDto>> GetRecipeListAsync(GetRecipeListDto dto)
        {
            await _validationGetRecipeListDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //2. 获取数据
            ProcEquipmentGroupParamEquProductQuery query = new ProcEquipmentGroupParamEquProductQuery();
            query.EquipmentId = equResModel.EquipmentId;
            query.ProductCode = dto.ProductCode;
            query.EquipmentCode = dto.EquipmentCode;
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
        }

        /// <summary>
        /// 获取开机参数明细008
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<GetRecipeDetailReturnDto> GetRecipeDetailAsync(GetRecipeDetailDto dto)
        {
            EquVerifyHelper.GetRecipeDetailDto(dto);
            //await _validationGetRecipeDetailDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //2. 获取开机参数编码的对应激活的数据
            ProcEquipmentGroupParamCodeDetailQuery query = new ProcEquipmentGroupParamCodeDetailQuery();
            query.Code = dto.RecipeCode;
            query.SiteId = equResModel.SiteId;
            var list = await _procEquipmentGroupParamService.GetDetailByCode(query);
            GetRecipeDetailReturnDto result = new GetRecipeDetailReturnDto();
            result.Version = list[0].Version;
            result.LastUpdateOnTime = list[0].UpdatedOn ?? list[0].CreatedOn;
            foreach (var item in list)
            {
                if (item == null)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(item.ParameterCode) == false)
                {
                    RecipeParamDto param = new RecipeParamDto();
                    param.ParamUpper = item.MaxValue == null ? "" : item.MaxValue.ToString();
                    param.ParamLower = item.MinValue == null ? "" : item.MinValue.ToString();
                    param.ParamValue = item.CenterValue == null ? "" : item.CenterValue.ToString();
                    param.ParamCode = item.ParameterCode;
                    param.ParamName = item.ParameterName;
                    result.ParamList.Add(param);
                }
            }

            return result;
        }

        /// <summary>
        /// 开机参数校验009
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task RecipeAsync(RecipeDto dto)
        {
            await _validationRecipeDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
            //2. 参数上下限校验
            //暂不加
            //3. 版本校验
            ProcEquipmentGroupCheckQuery query = new ProcEquipmentGroupCheckQuery();
            query.SiteId = equResModel.SiteId;
            query.Code = dto.RecipeCode;
            query.Version = dto.Version;
            query.MaterialCode = dto.ProductCode;
            var entity = await _procEquipmentGroupParamService.GetEntityByCodeVersion(query);
            //4. 记录
            List<EquOpenParamRecordSaveDto> saveDtoList = new List<EquOpenParamRecordSaveDto>();
            foreach (var item in dto.ParamList)
            {
                EquOpenParamRecordSaveDto saveDto = new EquOpenParamRecordSaveDto();
                saveDto.ParamCode = item.ParamCode;
                saveDto.ParamValue = item.ParamValue;
                saveDtoList.Add(saveDto);
            }
            long bitchId = IdGenProvider.Instance.CreateId();
            saveDtoList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
                m.CollectionTime = m.CreatedOn;
                m.BatchId = bitchId;
                m.RecipeId = entity.Id;
            });

            //5. 数据库操作
            await _equOpenParamRecordService.AddMultAsync(saveDtoList);
        }

        /// <summary>
        /// 设备过程参数026
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto)
        {
            await _validationEquipmentProcessParamDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
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
                m.Location = dto.Location;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            EquipmentProcessParameterCollectDto parameterCollectDto = new()
            {
                SiteId = equResModel.SiteId,
                EquipmentId = equResModel.EquipmentId,
                Location = dto.Location,
                Parameters = dto.ParamList.Select(x => new EquipmentProcessParameterDto
                {
                    ParameterCode = x.ParamCode,
                    ParameterValue = x.ParamValue,
                    CollectionTime = x.CollectionTime
                })
            };
            //3. 数据操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _equProcessParamRecordService.AddMultAsync(saveDtoList);
            await _processCollectionService.EquipmentCollectionAsync(parameterCollectDto);
            trans.Complete();
        }

        /// <summary>
        /// 产品参数上传043
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ProductParamAsync(ProductParamDto dto)
        {
            EquVerifyHelper.ProductParamDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 产品过程参数
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

            await _manuProductParameterService.ProductProcessCollectAsync(parameterCollectBo);
        }

        /// <summary>
        /// 产品参数上传046
        /// 多个条码参数相同
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ProductParamSameMultSfcAsync(ProductParamSameMultSfcDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 产品过程参数
            var parameterBo = new ProductProcessParameterBo
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                Time = HymsonClock.Now(),
                ProcedureId = equResModel.ProcedureId,
                ResourceId = equResModel.ResId,
                SFCs = dto.SfcList,
                Parameters = dto.ParamList.Select(x => new ProductParameterBo
                {
                    ParameterCode = x.ParamCode,
                    ParameterValue = x.ParamValue,
                    CollectionTime = x.CollectionTime
                })
            };

            await _manuProductParameterService.ProductProcessCollectAsync(parameterBo);
        }

        /// <summary>
        /// 设备文件上传055
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task EquFileUploadAsync(EquFileUploadDto dto)
        {
            EquVerifyHelper.EquFileUploadDto(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResProcedureAsync(dto);

            //文件列表
            var fileList = dto.FormCollection.Files;
            if (fileList == null || !fileList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45290));
            }

            List<CcdFileUploadCompleteRecordSaveDto> saveDtoList = new();

            foreach (var file in fileList)
            {
                var fileDir = $"{equResModel.ProcedureCode}/{HymsonClock.Now():yyyyMMdd}/{equResModel.EquipmentCode}/{dto.Sfc}";
                //上传
                using var stream = file.OpenReadStream();
                var uploadResult = await _minioService.PutObjectAsync(file.FileName, stream, file.ContentType, fileDir);

                saveDtoList.Add(new CcdFileUploadCompleteRecordSaveDto
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = equResModel.SiteId,
                    EquipmentId = equResModel.EquipmentId,
                    Sfc = dto.Sfc,
                    SfcIsPassed = dto.Passed,
                    Uri = uploadResult.AbsoluteUrl,
                    UriIsPassed = dto.Passed,
                    CreatedBy = equResModel.EquipmentCode,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = equResModel.EquipmentCode,
                    UpdatedOn = HymsonClock.Now()
                });
            }

            //保存
            await _ccdFileUploadCompleteRecordService.AddMultAsync(saveDtoList);
        }

    }
}
