using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Enums.Qkny;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FluentValidation;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Services.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Services.EquEquipmentLoginRecord;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Hymson.MES.Services.Services.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.MES.Services.Services.ManuEquipmentStatusTime;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;
using Hymson.MES.Services.Services.EquEquipmentAlarm;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Services.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;
using Hymson.MES.Services.Services.EquToolLifeRecord;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Services.Qkny.PowerOnParam;

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
            AbstractValidator<OperationLoginDto> validationOperationLoginDto,
            AbstractValidator<HeartbeatDto> validationHeartbeatDto,
            AbstractValidator<StateDto> validationStateDto,
            AbstractValidator<AlarmDto> validationAlarmDto,
            AbstractValidator<CCDFileUploadCompleteDto> validationCCDFileUploadCompleteDto,
            AbstractValidator<ToolLifeDto> validationToolLifeDto,
            AbstractValidator<GetRecipeListDto> validationGetRecipeListDto,
            AbstractValidator<GetRecipeDetailDto> validationGetRecipeDetailDto,
            AbstractValidator<RecipeDto> validationRecipeDto)
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
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
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
            //3.1 新增登录记录
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
            await _validationHeartbeatDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
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
            await _validationStateDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
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
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
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
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
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
            await _validationToolLifeDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
            //2. 添加记录
            EquToolLifeRecordSaveDto saveDto = new EquToolLifeRecordSaveDto();
            saveDto.EquipmentId = equResModel.EquipmentId;
            saveDto.ToolCode = dto.ToolCode;
            saveDto.ToolLife = dto.UsedLife;
            saveDto.CreatedBy = dto.EquipmentCode;
            saveDto.CreatedOn = HymsonClock.Now();
            saveDto.UpdatedBy = dto.EquipmentCode;
            saveDto.UpdatedOn = saveDto.CreatedOn;
            //3 数据库操作
            await _equToolLifeRecordService.AddAsync(saveDto);
        }

        /// <summary>
        /// 获取开机参数列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<GetRecipeListReturnDto>> GetRecipeListAsync(GetRecipeListDto dto)
        {
            await _validationGetRecipeListDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
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
            await _validationGetRecipeDetailDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
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
            await _validationRecipeDto.ValidateAndThrowAsync(dto);
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAllAsync(dto);
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



    }
}
