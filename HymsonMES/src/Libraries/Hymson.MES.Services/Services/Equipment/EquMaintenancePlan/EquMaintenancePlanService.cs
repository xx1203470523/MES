/*
 *creator: Karl
 *
 *describe: 设备保养计划    服务 | 代码由框架生成
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
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment.EquMaintenance;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Services.EquMaintenancePlan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem;
using Hymson.MES.Data.Repositories.EquMaintenancePlan;
using Hymson.MES.Data.Repositories.EquMaintenancePlanEquipmentRelation;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplate;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplateItemRelation;
using Hymson.MES.Services.Dtos.EquMaintenancePlan;
using Hymson.MES.Services.Dtos.EquSpotcheckPlan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
using System.Linq;

namespace Hymson.MES.Services.Services.EquMaintenancePlan
{
    /// <summary>
    /// 设备保养计划 服务
    /// </summary>
    public class EquMaintenancePlanService : IEquMaintenancePlanService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备保养计划 仓储
        /// </summary>
        private readonly IEquMaintenancePlanRepository _EquMaintenancePlanRepository;
        /// <summary>
        /// 设备保养计划与设备关系仓储接口
        /// </summary>
        private readonly IEquMaintenancePlanEquipmentRelationRepository _EquMaintenancePlanEquipmentRelationRepository;
        /// <summary>
        /// 设备保养模板仓储接口
        /// </summary>
        private readonly IEquMaintenanceTemplateRepository _EquMaintenanceTemplateRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        /// <summary>
        /// 设备保养模板与项目关系仓储接口
        /// </summary>
        private readonly IEquMaintenanceTemplateItemRelationRepository _EquMaintenanceTemplateItemRelationRepository;
        /// <summary>
        /// 仓储接口（设备保养项目）
        /// </summary>
        private readonly IEquMaintenanceItemRepository _EquMaintenanceItemRepository;

        /// <summary>
        /// 仓储接口（项目快照）
        /// </summary> 
        private readonly IEquMaintenanceTaskSnapshotItemRepository _EquMaintenanceTaskSnapshotItemRepository;

        /// <summary>
        /// 仓储接口（计划快照）
        /// </summary>  
        private readonly IEquMaintenanceTaskSnapshotPlanRepository _EquMaintenanceTaskSnapshotPlanRepository;

        /// <summary>
        /// 仓储接口（任务） 
        /// </summary>  
        private readonly IEquMaintenanceTaskRepository _EquMaintenanceTaskRepository;
        /// <summary>
        /// 仓储接口（任务-项目） 
        /// </summary>  
        private readonly IEquMaintenanceTaskItemRepository _EquMaintenanceTaskItemRepository;
        /// <summary>
        ///  IEquMaintenancePlanCoreService
        /// </summary>  
        private readonly IEquMaintenancePlanCoreService _EquMaintenancePlanCoreService;


        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance2> _eventBus;


        private readonly AbstractValidator<EquMaintenancePlanCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquMaintenancePlanModifyDto> _validationModifyRules;

        public EquMaintenancePlanService(ICurrentUser currentUser, ICurrentSite currentSite, IEquMaintenancePlanRepository EquMaintenancePlanRepository, AbstractValidator<EquMaintenancePlanCreateDto> validationCreateRules, AbstractValidator<EquMaintenancePlanModifyDto> validationModifyRules, IEquMaintenancePlanEquipmentRelationRepository EquMaintenancePlanEquipmentRelationRepository, IEquMaintenanceTemplateRepository EquMaintenanceTemplateRepository, IEquEquipmentRepository equEquipmentRepository, IEquMaintenanceTemplateItemRelationRepository EquMaintenanceTemplateItemRelationRepository, IEquMaintenanceItemRepository equMaintenanceItemRepository, IEquMaintenanceTaskSnapshotItemRepository equMaintenanceTaskSnapshotItemRepository, IEquMaintenanceTaskSnapshotPlanRepository equMaintenanceTaskSnapshotPlanRepository, IEquMaintenanceTaskRepository equMaintenanceTaskRepository, IEquMaintenanceTaskItemRepository equMaintenanceTaskItemRepository, IEquMaintenancePlanCoreService equMaintenancePlanCoreService, IEventBus<EventBusInstance2> eventBus)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _EquMaintenancePlanRepository = EquMaintenancePlanRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _EquMaintenancePlanEquipmentRelationRepository = EquMaintenancePlanEquipmentRelationRepository;
            _EquMaintenanceTemplateRepository = EquMaintenanceTemplateRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _EquMaintenanceTemplateItemRelationRepository = EquMaintenanceTemplateItemRelationRepository;
            _EquMaintenanceItemRepository = equMaintenanceItemRepository;
            _EquMaintenanceTaskSnapshotItemRepository = equMaintenanceTaskSnapshotItemRepository;
            _EquMaintenanceTaskSnapshotPlanRepository = equMaintenanceTaskSnapshotPlanRepository;
            _EquMaintenanceTaskRepository = equMaintenanceTaskRepository;
            _EquMaintenanceTaskItemRepository = equMaintenanceTaskItemRepository;
            _EquMaintenancePlanCoreService = equMaintenancePlanCoreService;
            _eventBus = eventBus;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="EquMaintenancePlanCreateDto"></param>
        /// <returns></returns>
        public async Task CreateEquMaintenancePlanAsync(EquMaintenancePlanCreateDto EquMaintenancePlanCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(EquMaintenancePlanCreateDto);
            if (EquMaintenancePlanCreateDto.CompletionMinute > 60 || EquMaintenancePlanCreateDto.CompletionMinute < 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12315));
            }
            if (EquMaintenancePlanCreateDto.PreGeneratedMinute > 60 || EquMaintenancePlanCreateDto.PreGeneratedMinute < 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12316));
            }
            if (EquMaintenancePlanCreateDto.BeginTime > EquMaintenancePlanCreateDto.EndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12321));
            }
            if (EquMaintenancePlanCreateDto.FirstExecuteTime < EquMaintenancePlanCreateDto.BeginTime || EquMaintenancePlanCreateDto.FirstExecuteTime > EquMaintenancePlanCreateDto.EndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12320));
            }
            if (!EquMaintenancePlanCreateDto.CompletionHour.HasValue && !EquMaintenancePlanCreateDto.CompletionHour.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12323));
            }
            var EquMaintenancePlan = await _EquMaintenancePlanRepository.GetByCodeAsync(new EquMaintenancePlanQuery { SiteId = _currentSite.SiteId ?? 0, Code = EquMaintenancePlanCreateDto.Code, Version = EquMaintenancePlanCreateDto.Version });
            if (EquMaintenancePlan != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12305)).WithData("Code", EquMaintenancePlanCreateDto.Code).WithData("Version", EquMaintenancePlanCreateDto.Version);
            }

            //DTO转换实体
            var equMaintenancePlanEntity = EquMaintenancePlanCreateDto.ToEntity<EquMaintenancePlanEntity>();
            equMaintenancePlanEntity.Id = IdGenProvider.Instance.CreateId();
            equMaintenancePlanEntity.CreatedBy = _currentUser.UserName;
            equMaintenancePlanEntity.UpdatedBy = _currentUser.UserName;
            equMaintenancePlanEntity.CreatedOn = HymsonClock.Now();
            equMaintenancePlanEntity.UpdatedOn = HymsonClock.Now();
            equMaintenancePlanEntity.SiteId = _currentSite.SiteId ?? 0;
            equMaintenancePlanEntity.CornExpression = GetExecuteCycle(equMaintenancePlanEntity);

            List<EquMaintenancePlanEquipmentRelationEntity> EquMaintenancePlanEquipmentRelationList = new();


            var equMaintenancePlanUser = EquMaintenancePlanCreateDto.RelationDto.Where(it => string.IsNullOrWhiteSpace(it.ExecutorIds) || string.IsNullOrWhiteSpace(it.LeaderIds));
            if (equMaintenancePlanUser != null && equMaintenancePlanUser.Any())
            {
                var equEquipments = await _equEquipmentRepository.GetByIdAsync(equMaintenancePlanUser.Select(item => item.Id == 0 ? item.EquipmentId : item.Id));
                throw new CustomerValidationException(nameof(ErrorCode.MES12322)).WithData("Code", string.Join(",", equEquipments.Select(it => it.EquipmentCode).ToArray()));
            }
            foreach (var item in EquMaintenancePlanCreateDto.RelationDto)
            {
                if (item.TemplateId == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12306));
                }
                EquMaintenancePlanEquipmentRelationEntity EquMaintenancePlanEquipmentRelation = new()
                {
                    EquipmentId = item.Id == 0 ? item.EquipmentId : item.Id,
                    MaintenancePlanId = equMaintenancePlanEntity.Id,
                    MaintenanceTemplateId = item.TemplateId,
                    ExecutorIds = item.ExecutorIds,
                    LeaderIds = item.LeaderIds,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };
                EquMaintenancePlanEquipmentRelationList.Add(EquMaintenancePlanEquipmentRelation);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            await _EquMaintenancePlanRepository.InsertAsync(equMaintenancePlanEntity);
            await _EquMaintenancePlanEquipmentRelationRepository.InsertsAsync(EquMaintenancePlanEquipmentRelationList);

            trans.Complete();

            //TODO 这里要另外加入口 先这样用着
            ExecPublish(equMaintenancePlanEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquMaintenancePlanAsync(long id)
        {
            await _EquMaintenancePlanRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquMaintenancePlanAsync(DeletesDto param)
        {

            var equMaintenancePlan = await _EquMaintenancePlanRepository.GetByIdsAsync(param.Ids.ToArray());
            var equMaintenancePlanEnable = equMaintenancePlan.Where(it => it.Status == DisableOrEnableEnum.Enable);
            if (equMaintenancePlanEnable.Any())
            {
                var codes = string.Join(",", equMaintenancePlanEnable.Select(it => it.Code));
                throw new CustomerValidationException(nameof(ErrorCode.MES12313)).WithData("Code", codes);
            }


            int row = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            row += await _EquMaintenancePlanRepository.DeletesAsync(new DeleteCommand { Ids = param.Ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });

            await _EquMaintenancePlanEquipmentRelationRepository.PhysicalDeletesAsync(param.Ids);

            trans.Complete();

            return row;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="EquMaintenancePlanPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenancePlanDto>> GetPagedListAsync(EquMaintenancePlanPagedQueryDto EquMaintenancePlanPagedQueryDto)
        {
            var EquMaintenancePlanPagedQuery = EquMaintenancePlanPagedQueryDto.ToQuery<EquMaintenancePlanPagedQuery>();
            EquMaintenancePlanPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _EquMaintenancePlanRepository.GetPagedInfoAsync(EquMaintenancePlanPagedQuery);

            //实体到DTO转换 装载数据
            List<EquMaintenancePlanDto> EquMaintenancePlanDtos = PrepareEquMaintenancePlanDtos(pagedInfo);
            return new PagedInfo<EquMaintenancePlanDto>(EquMaintenancePlanDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquMaintenancePlanDto> PrepareEquMaintenancePlanDtos(PagedInfo<EquMaintenancePlanEntity> pagedInfo)
        {
            var EquMaintenancePlanDtos = new List<EquMaintenancePlanDto>();
            foreach (var EquMaintenancePlanEntity in pagedInfo.Data)
            {
                var EquMaintenancePlanDto = EquMaintenancePlanEntity.ToModel<EquMaintenancePlanDto>();
                EquMaintenancePlanDtos.Add(EquMaintenancePlanDto);
            }

            return EquMaintenancePlanDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="EquMaintenancePlanModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquMaintenancePlanAsync(EquMaintenancePlanModifyDto EquMaintenancePlanModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(EquMaintenancePlanModifyDto);
            if (EquMaintenancePlanModifyDto.CompletionMinute > 60)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12315));
            }
            if (EquMaintenancePlanModifyDto.PreGeneratedMinute > 60)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12316));
            }
            if (EquMaintenancePlanModifyDto.BeginTime > EquMaintenancePlanModifyDto.EndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12321));
            }
            if (EquMaintenancePlanModifyDto.FirstExecuteTime < EquMaintenancePlanModifyDto.BeginTime || EquMaintenancePlanModifyDto.FirstExecuteTime > EquMaintenancePlanModifyDto.EndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12320));
            }
            if (!EquMaintenancePlanModifyDto.CompletionHour.HasValue && !EquMaintenancePlanModifyDto.CompletionHour.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12323));
            }
            var EquMaintenancePlan = await _EquMaintenancePlanRepository.GetByCodeAsync(new EquMaintenancePlanQuery { SiteId = _currentSite.SiteId ?? 0, Code = EquMaintenancePlanModifyDto.Code, Version = EquMaintenancePlanModifyDto.Version });
            if (EquMaintenancePlan != null && EquMaintenancePlan.Id != EquMaintenancePlanModifyDto.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12305)).WithData("Code", EquMaintenancePlanModifyDto.Code).WithData("Version", EquMaintenancePlanModifyDto.Version);
            }

            //DTO转换实体
            var equMaintenancePlanEntity = EquMaintenancePlanModifyDto.ToEntity<EquMaintenancePlanEntity>();
            equMaintenancePlanEntity.UpdatedBy = _currentUser.UserName;
            equMaintenancePlanEntity.UpdatedOn = HymsonClock.Now();
            equMaintenancePlanEntity.CornExpression = GetExecuteCycle(equMaintenancePlanEntity);

            List<EquMaintenancePlanEquipmentRelationEntity> EquMaintenancePlanEquipmentRelationList = new();
            List<long> MaintenancePlanIds = new() { EquMaintenancePlanModifyDto.Id };
            var equMaintenancePlanUser = EquMaintenancePlanModifyDto.RelationDto.Where(it => string.IsNullOrWhiteSpace(it.ExecutorIds) || string.IsNullOrWhiteSpace(it.LeaderIds));
            if (equMaintenancePlanUser != null && equMaintenancePlanUser.Any())
            {
                var equEquipments = await _equEquipmentRepository.GetByIdAsync(equMaintenancePlanUser.Select(item => item.Id == 0 ? item.EquipmentId : item.Id));
                throw new CustomerValidationException(nameof(ErrorCode.MES12322)).WithData("Code", string.Join(",", equEquipments.Select(it => it.EquipmentCode).ToArray()));
            }
            foreach (var item in EquMaintenancePlanModifyDto.RelationDto)
            {

                EquMaintenancePlanEquipmentRelationEntity EquMaintenancePlanEquipmentRelation = new()
                {
                    EquipmentId = item.Id == 0 ? item.EquipmentId : item.Id,
                    MaintenancePlanId = EquMaintenancePlanModifyDto.Id,
                    MaintenanceTemplateId = item.TemplateId,
                    ExecutorIds = item.ExecutorIds,
                    LeaderIds = item.LeaderIds,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };
                EquMaintenancePlanEquipmentRelationList.Add(EquMaintenancePlanEquipmentRelation);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            await _EquMaintenancePlanRepository.UpdateAsync(equMaintenancePlanEntity);

            await _EquMaintenancePlanEquipmentRelationRepository.PhysicalDeletesAsync(MaintenancePlanIds);
            await _EquMaintenancePlanEquipmentRelationRepository.InsertsAsync(EquMaintenancePlanEquipmentRelationList);

            trans.Complete();

            //TODO 这里要另外加入口 先这样用着
            ExecPublish(equMaintenancePlanEntity, true);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquMaintenancePlanDto> QueryEquMaintenancePlanByIdAsync(long id)
        {
            var EquMaintenancePlanEntity = await _EquMaintenancePlanRepository.GetByIdAsync(id);
            if (EquMaintenancePlanEntity != null)
            {
                return EquMaintenancePlanEntity.ToModel<EquMaintenancePlanDto>();
            }
            return null;
        }

        /// <summary>
        /// 生成(Core)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task GenerateEquMaintenanceTaskCoreAsync(GenerateDto param)
        {
            await _EquMaintenancePlanCoreService.GenerateEquMaintenanceTaskAsync(new GenerateEquMaintenanceTaskDto { SiteId = _currentSite.SiteId ?? 0, UserName = _currentUser.UserName, ExecType = param.ExecType, MaintenancePlanId = param.MaintenancePlanId, });
        }

        #region 关联信息
        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="MaintenancePlanId"></param>
        /// <returns></returns>
        public async Task<List<QueryEquRelationListDto>> QueryEquRelationListAsync(long MaintenancePlanId)
        {
            var EquMaintenancePlanEquipmentRelations = await _EquMaintenancePlanEquipmentRelationRepository.GetByMaintenancePlanIdsAsync(MaintenancePlanId);

            List<QueryEquRelationListDto> list = new();
            if (EquMaintenancePlanEquipmentRelations != null && EquMaintenancePlanEquipmentRelations.Any())
            {
                var MaintenanceItemIds = EquMaintenancePlanEquipmentRelations.Select(it => it.MaintenanceTemplateId).ToArray();
                var EquMaintenanceTemplates = await _EquMaintenanceTemplateRepository.GetByIdsAsync(MaintenanceItemIds);
                var equipmentIds = EquMaintenancePlanEquipmentRelations.Select(it => it.EquipmentId).ToArray();
                var equipments = await _equEquipmentRepository.GetByIdAsync(equipmentIds);

                foreach (var item in EquMaintenancePlanEquipmentRelations)
                {
                    var EquMaintenanceTemplate = EquMaintenanceTemplates.FirstOrDefault(it => it.Id == item.MaintenanceTemplateId);
                    var equipment = equipments.FirstOrDefault(it => it.Id == item.EquipmentId);
                    QueryEquRelationListDto itemRelation = new()
                    {
                        MaintenancePlanId = MaintenancePlanId,
                        TemplateId = item.MaintenanceTemplateId,
                        TemplateCode = EquMaintenanceTemplate?.Code ?? "",
                        TemplateVersion = EquMaintenanceTemplate?.Version ?? "",
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
        private static string? GetExecuteCycle(EquMaintenancePlanEntity plan)
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
            if (plan.Type == EquipmentMaintenanceTypeEnum.Hour)
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
        /// <param name="equMaintenancePlanEntity"></param>
        /// <param name="isEdit"></param>
        private void ExecPublish(EquMaintenancePlanEntity equMaintenancePlanEntity, bool isEdit = false)
        {
            if (!string.IsNullOrWhiteSpace(equMaintenancePlanEntity.CornExpression) && equMaintenancePlanEntity.FirstExecuteTime.HasValue)
            {
                if (equMaintenancePlanEntity.Status == DisableOrEnableEnum.Enable)
                {
                    EquMaintenanceAutoCreateIntegrationEvent equSpotcheckAutoCreateIntegrationEvent = new()
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        CornExpression = equMaintenancePlanEntity.CornExpression,
                        FirstExecuteTime = (DateTime)equMaintenancePlanEntity.FirstExecuteTime,
                        MaintenancePlanId = equMaintenancePlanEntity.Id,
                        UserName = _currentUser.UserName,
                    };
                    _eventBus.Publish(equSpotcheckAutoCreateIntegrationEvent);
                }
                else if (isEdit)
                {
                    EquMaintenanceAutoStopIntegrationEvent equSpotcheckAutoCreateIntegrationEvent = new()
                    {
                        MaintenancePlanId = equMaintenancePlanEntity.Id,
                    };
                    _eventBus.Publish(equSpotcheckAutoCreateIntegrationEvent);
                }
            }
        }
        #endregion
    }
}
