using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.AgvTaskRecord;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Qkny;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.CoreServices.Services.Qkny;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder;
using Hymson.MES.EquipmentServices.Services.Qkny.PowerOnParam;
using Hymson.MES.EquipmentServices.Validators.Manufacture.Qkny;
using Hymson.MES.Services.Dtos.AgvTaskRecord;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Services.AgvTaskRecord;
using Hymson.MES.Services.Services.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Services.EquEquipmentAlarm;
using Hymson.MES.Services.Services.EquEquipmentHeartRecord;
using Hymson.MES.Services.Services.EquEquipmentLoginRecord;
using Hymson.MES.Services.Services.EquProcessParamRecord;
using Hymson.MES.Services.Services.ManuEquipmentStatusTime;
using Hymson.MES.Services.Services.ManuEuqipmentNewestInfo;
using Hymson.MessagePush.Helper;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Org.BouncyCastle.Asn1.Ocsp;
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
        /// 设备过程参数
        /// </summary>
        private readonly IEquProcessParamRecordService _equProcessParamRecordService;

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
        /// 获取配方列表
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
            //2. 获取配方编码的对应激活的数据
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
                saveDto.FeedingPointId = res.FirstOrDefault().LoadPointId;
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
        /// 设备过程参数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto)
        {
            if(dto.ParamList == null || dto.ParamList.Count == 0)
            {
                return;
            }
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 添加数据
            List<EquProcessParamRecordSaveDto> saveDtoList = new List<EquProcessParamRecordSaveDto>();
            foreach(var item in dto.ParamList)
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
