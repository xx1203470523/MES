/*
 *creator: Karl
 *
 *describe: 设备点检计划    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.EquSpotcheckPlan;
using Hymson.MES.Data.Repositories.EquSpotcheckPlanEquipmentRelation;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplate;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplateItemRelation;
using Hymson.MES.Services.Dtos.EquMaintenancePlan;
using Hymson.MES.Services.Dtos.EquSpotcheckPlan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;

namespace Hymson.MES.Services.Services.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划 服务
    /// </summary>
    public class EquSpotcheckPlanService : IEquSpotcheckPlanService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备点检计划 仓储
        /// </summary>
        private readonly IEquSpotcheckPlanRepository _equSpotcheckPlanRepository;
        /// <summary>
        /// 设备点检计划与设备关系仓储接口
        /// </summary>
        private readonly IEquSpotcheckPlanEquipmentRelationRepository _equSpotcheckPlanEquipmentRelationRepository;
        /// <summary>
        /// 设备点检模板仓储接口
        /// </summary>
        private readonly IEquSpotcheckTemplateRepository _equSpotcheckTemplateRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        /// <summary>
        /// 设备点检模板与项目关系仓储接口
        /// </summary>
        private readonly IEquSpotcheckTemplateItemRelationRepository _equSpotcheckTemplateItemRelationRepository;
        /// <summary>
        /// 仓储接口（设备点检项目）
        /// </summary>
        private readonly IEquSpotcheckItemRepository _equSpotcheckItemRepository;

        /// <summary>
        /// 仓储接口（项目快照）
        /// </summary> 
        private readonly IEquSpotcheckTaskSnapshotItemRepository _equSpotcheckTaskSnapshotItemRepository;

        /// <summary>
        /// 仓储接口（计划快照）
        /// </summary>  
        private readonly IEquSpotcheckTaskSnapshotPlanRepository _equSpotcheckTaskSnapshotPlanRepository;

        /// <summary>
        /// 仓储接口（任务） 
        /// </summary>  
        private readonly IEquSpotcheckTaskRepository _equSpotcheckTaskRepository;
        /// <summary>
        /// 仓储接口（任务-项目） 
        /// </summary>  
        private readonly IEquSpotcheckTaskItemRepository _equSpotcheckTaskItemRepository;
        /// <summary>
        ///  IEquSpotcheckPlanCoreService
        /// </summary>  
        private readonly IEquSpotcheckPlanCoreService _equSpotcheckPlanCoreService;


        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance2> _eventBus;

        private readonly AbstractValidator<EquSpotcheckPlanCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquSpotcheckPlanModifyDto> _validationModifyRules;

        public EquSpotcheckPlanService(ICurrentUser currentUser, ICurrentSite currentSite, IEquSpotcheckPlanRepository equSpotcheckPlanRepository, AbstractValidator<EquSpotcheckPlanCreateDto> validationCreateRules, AbstractValidator<EquSpotcheckPlanModifyDto> validationModifyRules, IEquSpotcheckPlanEquipmentRelationRepository equSpotcheckPlanEquipmentRelationRepository, IEquSpotcheckTemplateRepository equSpotcheckTemplateRepository, IEquEquipmentRepository equEquipmentRepository, IEquSpotcheckTemplateItemRelationRepository equSpotcheckTemplateItemRelationRepository, IEquSpotcheckItemRepository equSpotcheckItemRepository, IEquSpotcheckTaskSnapshotItemRepository equSpotcheckTaskSnapshotItemRepository, IEquSpotcheckTaskSnapshotPlanRepository equSpotcheckTaskSnapshotPlanRepository, IEquSpotcheckTaskRepository equSpotcheckTaskRepository, IEquSpotcheckTaskItemRepository equSpotcheckTaskItemRepository, IEquSpotcheckPlanCoreService equSpotcheckPlanCoreService, IEventBus<EventBusInstance2> eventBus)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSpotcheckPlanRepository = equSpotcheckPlanRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _equSpotcheckPlanEquipmentRelationRepository = equSpotcheckPlanEquipmentRelationRepository;
            _equSpotcheckTemplateRepository = equSpotcheckTemplateRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _equSpotcheckTemplateItemRelationRepository = equSpotcheckTemplateItemRelationRepository;
            _equSpotcheckItemRepository = equSpotcheckItemRepository;
            _equSpotcheckTaskSnapshotItemRepository = equSpotcheckTaskSnapshotItemRepository;
            _equSpotcheckTaskSnapshotPlanRepository = equSpotcheckTaskSnapshotPlanRepository;
            _equSpotcheckTaskRepository = equSpotcheckTaskRepository;
            _equSpotcheckTaskItemRepository = equSpotcheckTaskItemRepository;
            _equSpotcheckPlanCoreService = equSpotcheckPlanCoreService;
            _eventBus = eventBus;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="equSpotcheckPlanCreateDto"></param>
        /// <returns></returns>
        public async Task CreateEquSpotcheckPlanAsync(EquSpotcheckPlanCreateDto equSpotcheckPlanCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(equSpotcheckPlanCreateDto);
            if (equSpotcheckPlanCreateDto.CompletionMinute > 60 || equSpotcheckPlanCreateDto.CompletionMinute < 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12315));
            }
            if (equSpotcheckPlanCreateDto.PreGeneratedMinute > 60 || equSpotcheckPlanCreateDto.PreGeneratedMinute < 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12316));
            }
            if (equSpotcheckPlanCreateDto.BeginTime > equSpotcheckPlanCreateDto.EndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12321));
            }
            if (equSpotcheckPlanCreateDto.FirstExecuteTime < equSpotcheckPlanCreateDto.BeginTime || equSpotcheckPlanCreateDto.FirstExecuteTime > equSpotcheckPlanCreateDto.EndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12320));
            }
            if (!equSpotcheckPlanCreateDto.CompletionHour.HasValue && !equSpotcheckPlanCreateDto.CompletionHour.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12323));
            }

            var equSpotcheckPlan = await _equSpotcheckPlanRepository.GetByCodeAsync(new EquSpotcheckPlanQuery { SiteId = _currentSite.SiteId ?? 0, Code = equSpotcheckPlanCreateDto.Code, Version = equSpotcheckPlanCreateDto.Version });
            if (equSpotcheckPlan != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12305)).WithData("Code", equSpotcheckPlanCreateDto.Code).WithData("Version", equSpotcheckPlanCreateDto.Version);
            }

            //DTO转换实体
            var equSpotcheckPlanEntity = equSpotcheckPlanCreateDto.ToEntity<EquSpotcheckPlanEntity>();
            equSpotcheckPlanEntity.Id = IdGenProvider.Instance.CreateId();
            equSpotcheckPlanEntity.CreatedBy = _currentUser.UserName;
            equSpotcheckPlanEntity.UpdatedBy = _currentUser.UserName;
            equSpotcheckPlanEntity.CreatedOn = HymsonClock.Now();
            equSpotcheckPlanEntity.UpdatedOn = HymsonClock.Now();
            equSpotcheckPlanEntity.SiteId = _currentSite.SiteId ?? 0;
            equSpotcheckPlanEntity.CornExpression = GetExecuteCycle(equSpotcheckPlanEntity);

            List<EquSpotcheckPlanEquipmentRelationEntity> equSpotcheckPlanEquipmentRelationList = new();


            var equMaintenancePlanUser = equSpotcheckPlanCreateDto.RelationDto.Where(it => string.IsNullOrWhiteSpace(it.ExecutorIds) || string.IsNullOrWhiteSpace(it.LeaderIds));
            if (equMaintenancePlanUser != null && equMaintenancePlanUser.Any())
            {
                var equEquipments = await _equEquipmentRepository.GetByIdAsync(equMaintenancePlanUser.Select(item => item.Id == 0 ? item.EquipmentId : item.Id));
                throw new CustomerValidationException(nameof(ErrorCode.MES12322)).WithData("Code", string.Join(",", equEquipments.Select(it => it.EquipmentCode).ToArray()));
            }
            foreach (var item in equSpotcheckPlanCreateDto.RelationDto)
            {
                if (item.TemplateId == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12306));
                }
                EquSpotcheckPlanEquipmentRelationEntity equSpotcheckPlanEquipmentRelation = new()
                {
                    EquipmentId = item.Id == 0 ? item.EquipmentId : item.Id,
                    SpotCheckPlanId = equSpotcheckPlanEntity.Id,
                    SpotCheckTemplateId = item.TemplateId,
                    ExecutorIds = item.ExecutorIds,
                    LeaderIds = item.LeaderIds,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };
                equSpotcheckPlanEquipmentRelationList.Add(equSpotcheckPlanEquipmentRelation);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            await _equSpotcheckPlanRepository.InsertAsync(equSpotcheckPlanEntity);
            await _equSpotcheckPlanEquipmentRelationRepository.InsertsAsync(equSpotcheckPlanEquipmentRelationList);

            trans.Complete();

            //TODO 这里要另外加入口 先这样用着
            ExecPublish(equSpotcheckPlanEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquSpotcheckPlanAsync(long id)
        {
            await _equSpotcheckPlanRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSpotcheckPlanAsync(SpotcheckDeletesDto param)
        {
            var equSpotcheckPlan = await _equSpotcheckPlanRepository.GetByIdsAsync(param.Ids.ToArray());
            var equSpotcheckPlanEnable = equSpotcheckPlan.Where(it => it.Status == DisableOrEnableEnum.Enable);
            if (equSpotcheckPlanEnable.Any())
            {
                var codes = string.Join(",", equSpotcheckPlanEnable.Select(it => it.Code));
                throw new CustomerValidationException(nameof(ErrorCode.MES12313)).WithData("Code", codes);
            }

            int row = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            row += await _equSpotcheckPlanRepository.DeletesAsync(new DeleteCommand { Ids = param.Ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });

            await _equSpotcheckPlanEquipmentRelationRepository.PhysicalDeletesAsync(param.Ids);

            trans.Complete();

            return row;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equSpotcheckPlanPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckPlanDto>> GetPagedListAsync(EquSpotcheckPlanPagedQueryDto equSpotcheckPlanPagedQueryDto)
        {
            var equSpotcheckPlanPagedQuery = equSpotcheckPlanPagedQueryDto.ToQuery<EquSpotcheckPlanPagedQuery>();
            equSpotcheckPlanPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSpotcheckPlanRepository.GetPagedInfoAsync(equSpotcheckPlanPagedQuery);

            //实体到DTO转换 装载数据
            List<EquSpotcheckPlanDto> equSpotcheckPlanDtos = PrepareEquSpotcheckPlanDtos(pagedInfo);
            return new PagedInfo<EquSpotcheckPlanDto>(equSpotcheckPlanDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquSpotcheckPlanDto> PrepareEquSpotcheckPlanDtos(PagedInfo<EquSpotcheckPlanEntity> pagedInfo)
        {
            var equSpotcheckPlanDtos = new List<EquSpotcheckPlanDto>();
            foreach (var equSpotcheckPlanEntity in pagedInfo.Data)
            {
                var equSpotcheckPlanDto = equSpotcheckPlanEntity.ToModel<EquSpotcheckPlanDto>();
                equSpotcheckPlanDtos.Add(equSpotcheckPlanDto);
            }

            return equSpotcheckPlanDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equSpotcheckPlanModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquSpotcheckPlanAsync(EquSpotcheckPlanModifyDto equSpotcheckPlanModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(equSpotcheckPlanModifyDto);
            if (equSpotcheckPlanModifyDto.CompletionMinute > 60 || equSpotcheckPlanModifyDto.CompletionMinute < 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12315));
            }
            if (equSpotcheckPlanModifyDto.PreGeneratedMinute > 60 || equSpotcheckPlanModifyDto.PreGeneratedMinute < 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12316));
            }
            if (equSpotcheckPlanModifyDto.BeginTime > equSpotcheckPlanModifyDto.EndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12321));
            }
            if (equSpotcheckPlanModifyDto.FirstExecuteTime < equSpotcheckPlanModifyDto.BeginTime || equSpotcheckPlanModifyDto.FirstExecuteTime > equSpotcheckPlanModifyDto.EndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12320));
            }
            if (!equSpotcheckPlanModifyDto.CompletionHour.HasValue && !equSpotcheckPlanModifyDto.CompletionHour.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12323));
            }
            var equSpotcheckPlan = await _equSpotcheckPlanRepository.GetByCodeAsync(new EquSpotcheckPlanQuery { SiteId = _currentSite.SiteId ?? 0, Code = equSpotcheckPlanModifyDto.Code, Version = equSpotcheckPlanModifyDto.Version });
            if (equSpotcheckPlan != null && equSpotcheckPlan.Id != equSpotcheckPlanModifyDto.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12305)).WithData("Code", equSpotcheckPlanModifyDto.Code).WithData("Version", equSpotcheckPlanModifyDto.Version);
            }

            //DTO转换实体
            var equSpotcheckPlanEntity = equSpotcheckPlanModifyDto.ToEntity<EquSpotcheckPlanEntity>();
            equSpotcheckPlanEntity.UpdatedBy = _currentUser.UserName;
            equSpotcheckPlanEntity.UpdatedOn = HymsonClock.Now();
            equSpotcheckPlanEntity.CornExpression = GetExecuteCycle(equSpotcheckPlanEntity);

            List<EquSpotcheckPlanEquipmentRelationEntity> equSpotcheckPlanEquipmentRelationList = new();
            List<long> spotCheckPlanIds = new() { equSpotcheckPlanModifyDto.Id };


            var equMaintenancePlanUser = equSpotcheckPlanModifyDto.RelationDto.Where(it => string.IsNullOrWhiteSpace(it.ExecutorIds) || string.IsNullOrWhiteSpace(it.LeaderIds));
            if (equMaintenancePlanUser != null && equMaintenancePlanUser.Any())
            {
                var equEquipments = await _equEquipmentRepository.GetByIdAsync(equMaintenancePlanUser.Select(item => item.Id == 0 ? item.EquipmentId : item.Id));
                throw new CustomerValidationException(nameof(ErrorCode.MES12322)).WithData("Code", string.Join(",", equEquipments.Select(it => it.EquipmentCode).ToArray()));
            }
            foreach (var item in equSpotcheckPlanModifyDto.RelationDto)
            {
                if (item.TemplateId == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12306));
                }
                EquSpotcheckPlanEquipmentRelationEntity equSpotcheckPlanEquipmentRelation = new()
                {
                    EquipmentId = item.Id == 0 ? item.EquipmentId : item.Id,
                    SpotCheckPlanId = equSpotcheckPlanModifyDto.Id,
                    SpotCheckTemplateId = item.TemplateId,
                    ExecutorIds = item.ExecutorIds,
                    LeaderIds = item.LeaderIds,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };
                equSpotcheckPlanEquipmentRelationList.Add(equSpotcheckPlanEquipmentRelation);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            await _equSpotcheckPlanRepository.UpdateAsync(equSpotcheckPlanEntity);

            await _equSpotcheckPlanEquipmentRelationRepository.PhysicalDeletesAsync(spotCheckPlanIds);
            await _equSpotcheckPlanEquipmentRelationRepository.InsertsAsync(equSpotcheckPlanEquipmentRelationList);

            trans.Complete();

            //TODO 这里要另外加入口 先这样用着
            ExecPublish(equSpotcheckPlanEntity, true);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckPlanDto> QueryEquSpotcheckPlanByIdAsync(long id)
        {
            var equSpotcheckPlanEntity = await _equSpotcheckPlanRepository.GetByIdAsync(id);
            if (equSpotcheckPlanEntity != null)
            {
                return equSpotcheckPlanEntity.ToModel<EquSpotcheckPlanDto>();
            }
            return null;
        }

        /// <summary>
        /// 生成(Core)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task GenerateEquSpotcheckTaskCoreAsync(SpotcheckGenerateDto param)
        {
            await _equSpotcheckPlanCoreService.GenerateEquSpotcheckTaskAsync(new GenerateEquSpotcheckTaskDto { SiteId = _currentSite.SiteId ?? 0, UserName = _currentUser.UserName, ExecType = param.ExecType, SpotCheckPlanId = param.SpotCheckPlanId, });
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task GenerateEquSpotcheckTaskAsync(SpotcheckGenerateDto param)
        {
            //计划
            var equSpotcheckPlanEntity = await _equSpotcheckPlanRepository.GetByIdAsync(param.SpotCheckPlanId);
            if (param.ExecType == 0)
            {
                if (equSpotcheckPlanEntity.FirstExecuteTime < HymsonClock.Now())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12303));
                }

                if (!equSpotcheckPlanEntity.FirstExecuteTime.HasValue || !equSpotcheckPlanEntity.Type.HasValue || equSpotcheckPlanEntity.Cycle.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12304));
                }
            }
            var equSpotcheckPlanEquipmentRelations = await _equSpotcheckPlanEquipmentRelationRepository.GetBySpotCheckPlanIdsAsync(param.SpotCheckPlanId);
            if (equSpotcheckPlanEquipmentRelations == null || !equSpotcheckPlanEquipmentRelations.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12302));
            }
            var equipmentIds = equSpotcheckPlanEquipmentRelations.Select(it => it.EquipmentId).ToArray();
            var equEquipments = await _equEquipmentRepository.GetByIdAsync(equipmentIds);

            //模板与项目
            var spotCheckTemplateIds = equSpotcheckPlanEquipmentRelations.Select(it => it.SpotCheckTemplateId).ToArray();
            var equSpotcheckTemplateItemRelations = await _equSpotcheckTemplateItemRelationRepository.GetEquSpotcheckTemplateItemRelationEntitiesAsync(
                  new EquSpotcheckTemplateItemRelationQuery { SiteId = _currentSite.SiteId, SpotCheckTemplateIds = spotCheckTemplateIds });

            //项目  
            var spotCheckItemIds = equSpotcheckTemplateItemRelations.Select(it => it.SpotCheckItemId).ToArray();
            var equSpotcheckItems = await _equSpotcheckItemRepository.GetByIdsAsync(spotCheckItemIds);
            if (equSpotcheckItems == null || !equSpotcheckItems.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12301));
            }
            //项目快照  
            List<EquSpotcheckTaskSnapshotItemEntity> equSpotcheckTaskSnapshotItemList = new();
            List<EquSpotcheckTaskSnapshotPlanEntity> equSpotcheckTaskSnapshotPlanList = new();
            List<EquSpotcheckTaskEntity> equSpotcheckTaskList = new();
            List<EquSpotcheckTaskItemEntity> equSpotcheckTaskItemList = new();

            #region 组装
            foreach (var item in equSpotcheckPlanEquipmentRelations)
            {
                var equEquipment = equEquipments.Where(it => it.Id == item.EquipmentId).FirstOrDefault();

                EquSpotcheckTaskEntity equSpotcheckTask = new()
                {
                    Code = equSpotcheckPlanEntity.Code + equEquipment?.EquipmentCode,
                    Name = equSpotcheckPlanEntity.Name,
                    BeginTime = HymsonClock.Now(),
                    EndTime = HymsonClock.Now(),
                    Status = EquSpotcheckTaskStautusEnum.WaitInspect,
                    IsQualified = null,
                    Remark = equSpotcheckPlanEntity.Remark,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 0
                };
                equSpotcheckTaskList.Add(equSpotcheckTask);

                EquSpotcheckTaskSnapshotPlanEntity equSpotcheckTaskSnapshotPlan = new()
                {
                    SpotCheckTaskId = equSpotcheckTask.Id,
                    SpotCheckPlanId = item.SpotCheckPlanId,
                    Code = equSpotcheckPlanEntity.Code,
                    Name = equSpotcheckPlanEntity.Name,
                    Version = equSpotcheckPlanEntity.Version,
                    EquipmentId = item.EquipmentId,
                    SpotCheckTemplateId = item.SpotCheckTemplateId,
                    ExecutorIds = item.ExecutorIds ?? "",
                    LeaderIds = item.LeaderIds ?? "",
                    Type = equSpotcheckPlanEntity.Type ?? 0,
                    Status = equSpotcheckPlanEntity.Status,
                    BeginTime = equSpotcheckPlanEntity.BeginTime,
                    EndTime = equSpotcheckPlanEntity.EndTime,
                    IsSkipHoliday = equSpotcheckPlanEntity.IsSkipHoliday,
                    FirstExecuteTime = equSpotcheckPlanEntity.FirstExecuteTime,
                    Cycle = equSpotcheckPlanEntity.Cycle ?? 1,
                    CompletionHour = equSpotcheckPlanEntity.CompletionHour,
                    CompletionMinute = equSpotcheckPlanEntity.CompletionMinute,
                    PreGeneratedMinute = equSpotcheckPlanEntity.PreGeneratedMinute,
                    Remark = equSpotcheckPlanEntity.Remark,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 0
                };
                equSpotcheckTaskSnapshotPlanList.Add(equSpotcheckTaskSnapshotPlan);

                var thisEquSpotcheckTemplateItemRelations = equSpotcheckTemplateItemRelations.Where(it => it.SpotCheckTemplateId == item.SpotCheckTemplateId);
                var thisSpotCheckItemIds = thisEquSpotcheckTemplateItemRelations.Select(it => it.SpotCheckItemId).ToArray();
                var thisEquSpotcheckItems = equSpotcheckItems.Where(it => thisSpotCheckItemIds.Contains(it.Id));

                foreach (var thisEquSpotcheckItem in thisEquSpotcheckItems)
                {

                    var thisEquSpotcheckTemplateItemRelation = thisEquSpotcheckTemplateItemRelations.Where(it => it.SpotCheckItemId == thisEquSpotcheckItem.Id).FirstOrDefault();
                    EquSpotcheckTaskSnapshotItemEntity equSpotcheckTaskSnapshotItem = new()
                    {
                        SpotCheckTaskId = equSpotcheckTask.Id,
                        SpotCheckItemId = thisEquSpotcheckItem.Id,
                        Code = thisEquSpotcheckItem.Code ?? "",
                        Name = thisEquSpotcheckItem.Name ?? "",
                        Status = thisEquSpotcheckItem.Status ?? DisableOrEnableEnum.Enable,
                        DataType = thisEquSpotcheckItem.DataType ?? EquSpotcheckDataTypeEnum.Text,
                        CheckType = thisEquSpotcheckItem.CheckType,
                        CheckMethod = thisEquSpotcheckItem.CheckMethod ?? "",
                        UnitId = thisEquSpotcheckItem.UnitId,
                        OperationContent = thisEquSpotcheckItem.OperationContent ?? "",
                        Components = thisEquSpotcheckItem.Components ?? "",
                        Remark = thisEquSpotcheckItem.Remark ?? "",
                        ReferenceValue = thisEquSpotcheckTemplateItemRelation?.Center,
                        UpperLimit = thisEquSpotcheckTemplateItemRelation?.UpperLimit,
                        LowerLimit = thisEquSpotcheckTemplateItemRelation?.LowerLimit,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    equSpotcheckTaskSnapshotItemList.Add(equSpotcheckTaskSnapshotItem);

                    EquSpotcheckTaskItemEntity equSpotcheckTaskItem = new()
                    {
                        SpotCheckTaskId = equSpotcheckTask.Id,
                        SpotCheckItemSnapshotId = equSpotcheckTask.Id,
                        InspectionValue = "",
                        IsQualified = TrueOrFalseEnum.No,
                        Remark = equSpotcheckTaskSnapshotItem.Remark,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    equSpotcheckTaskItemList.Add(equSpotcheckTaskItem);

                }
            }
            #endregion

            using var trans = TransactionHelper.GetTransactionScope();

            await _equSpotcheckTaskRepository.InsertRangeAsync(equSpotcheckTaskList);
            await _equSpotcheckTaskSnapshotPlanRepository.InsertRangeAsync(equSpotcheckTaskSnapshotPlanList);
            await _equSpotcheckTaskSnapshotItemRepository.InsertRangeAsync(equSpotcheckTaskSnapshotItemList);
            await _equSpotcheckTaskItemRepository.InsertRangeAsync(equSpotcheckTaskItemList);

            trans.Complete();

        }

        #region 关联信息


        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="spotCheckPlanId"></param>
        /// <returns></returns>
        public async Task<List<QuerySpotcheckEquRelationListDto>> QueryEquRelationListAsync(long spotCheckPlanId)
        {
            var equSpotcheckPlanEquipmentRelations = await _equSpotcheckPlanEquipmentRelationRepository.GetBySpotCheckPlanIdsAsync(spotCheckPlanId);

            List<QuerySpotcheckEquRelationListDto> list = new();
            if (equSpotcheckPlanEquipmentRelations != null && equSpotcheckPlanEquipmentRelations.Any())
            {
                var spotCheckItemIds = equSpotcheckPlanEquipmentRelations.Select(it => it.SpotCheckTemplateId).ToArray();
                var equSpotcheckTemplates = await _equSpotcheckTemplateRepository.GetByIdsAsync(spotCheckItemIds);
                var equipmentIds = equSpotcheckPlanEquipmentRelations.Select(it => it.EquipmentId).ToArray();
                var equipments = await _equEquipmentRepository.GetByIdAsync(equipmentIds);

                foreach (var item in equSpotcheckPlanEquipmentRelations)
                {
                    var equSpotcheckTemplate = equSpotcheckTemplates.FirstOrDefault(it => it.Id == item.SpotCheckTemplateId);
                    var equipment = equipments.FirstOrDefault(it => it.Id == item.EquipmentId);
                    QuerySpotcheckEquRelationListDto itemRelation = new()
                    {
                        SpotCheckPlanId = spotCheckPlanId,
                        TemplateId = item.SpotCheckTemplateId,
                        TemplateCode = equSpotcheckTemplate?.Code ?? "",
                        TemplateVersion = equSpotcheckTemplate?.Version ?? "",
                        EquipmentId = item.EquipmentId,
                        EquipmentCode = equipment?.EquipmentCode ?? "",
                        EquipmentName = equipment?.EquipmentName ?? "",
                        ExecutorIds = item?.ExecutorIds,
                        LeaderIds = item?.LeaderIds,
                    };

                    list.Add(itemRelation);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取执行周期
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private static string? GetExecuteCycle(EquSpotcheckPlanEntity plan)
        {
            if (!plan.FirstExecuteTime.HasValue || !plan.Type.HasValue || !plan.Cycle.HasValue)
            {
                return null;
            }
            var second = plan.FirstExecuteTime.GetValueOrDefault().Second;
            var minute = plan.FirstExecuteTime.GetValueOrDefault().Minute;
            var hour = plan.FirstExecuteTime.GetValueOrDefault().Hour.ToString();
            var day = "*";
            var tail = "* ?";
            if (plan.Type == EquipmentSpotcheckTypeEnum.Hour)
            {
                hour = $"0/{plan.Cycle}";
            }
            else
            {
                day = $"*/{day}";
            }
            var expression = $"{second} {minute} {hour} {day} {tail}";
            return expression;
        }


        /// <summary>
        /// 执行发送任务
        /// </summary>
        /// <param name="equSpotcheckPlanEntity"></param>
        /// <param name="isEdit"></param>
        private void ExecPublish(EquSpotcheckPlanEntity equSpotcheckPlanEntity, bool isEdit = false)
        {
            if (!string.IsNullOrWhiteSpace(equSpotcheckPlanEntity.CornExpression) && equSpotcheckPlanEntity.FirstExecuteTime.HasValue && equSpotcheckPlanEntity.EndTime.HasValue)
            {
                if (equSpotcheckPlanEntity.Status == DisableOrEnableEnum.Enable)
                {
                    EquSpotcheckAutoCreateIntegrationEvent equSpotcheckAutoCreateIntegrationEvent = new()
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        CornExpression = equSpotcheckPlanEntity.CornExpression,
                        FirstExecuteTime = (DateTime)equSpotcheckPlanEntity.FirstExecuteTime,
                        SpotCheckPlanId = equSpotcheckPlanEntity.Id,
                        UserName = _currentUser.UserName,
                        EndTime = (DateTime)equSpotcheckPlanEntity.EndTime
                    };
                    _eventBus.Publish(equSpotcheckAutoCreateIntegrationEvent);
                }
                else if (isEdit)
                {
                    EquSpotcheckAutoStopIntegrationEvent equSpotcheckAutoCreateIntegrationEvent = new()
                    {
                        SpotCheckPlanId = equSpotcheckPlanEntity.Id,
                    };
                    _eventBus.Publish(equSpotcheckAutoCreateIntegrationEvent);
                }
            }
        }
        #endregion
    }
}
