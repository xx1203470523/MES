
using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums.Equipment.EquMaintenance;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem;
using Hymson.MES.Data.Repositories.EquMaintenancePlan;
using Hymson.MES.Data.Repositories.EquMaintenancePlanEquipmentRelation;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplate;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplateItemRelation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.CoreServices.Services.EquMaintenancePlan
{
    /// <summary>
    /// 设备保养计划 服务
    /// </summary>
    public class EquMaintenancePlanCoreService : IEquMaintenancePlanCoreService 
    {
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



        public EquMaintenancePlanCoreService(IEquMaintenancePlanRepository EquMaintenancePlanRepository, IEquMaintenancePlanEquipmentRelationRepository EquMaintenancePlanEquipmentRelationRepository, IEquMaintenanceTemplateRepository EquMaintenanceTemplateRepository, IEquEquipmentRepository equEquipmentRepository, IEquMaintenanceTemplateItemRelationRepository EquMaintenanceTemplateItemRelationRepository, IEquMaintenanceItemRepository EquMaintenanceItemRepository, IEquMaintenanceTaskSnapshotItemRepository EquMaintenanceTaskSnapshotItemRepository, IEquMaintenanceTaskSnapshotPlanRepository EquMaintenanceTaskSnapshotPlanRepository, IEquMaintenanceTaskRepository EquMaintenanceTaskRepository, IEquMaintenanceTaskItemRepository EquMaintenanceTaskItemRepository)
        {
             
            _EquMaintenancePlanRepository = EquMaintenancePlanRepository;
            _EquMaintenancePlanEquipmentRelationRepository = EquMaintenancePlanEquipmentRelationRepository;
            _EquMaintenanceTemplateRepository = EquMaintenanceTemplateRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _EquMaintenanceTemplateItemRelationRepository = EquMaintenanceTemplateItemRelationRepository;
            _EquMaintenanceItemRepository = EquMaintenanceItemRepository;
            _EquMaintenanceTaskSnapshotItemRepository = EquMaintenanceTaskSnapshotItemRepository;
            _EquMaintenanceTaskSnapshotPlanRepository = EquMaintenanceTaskSnapshotPlanRepository;
            _EquMaintenanceTaskRepository = EquMaintenanceTaskRepository;
            _EquMaintenanceTaskItemRepository = EquMaintenanceTaskItemRepository;
        }
        /// <summary>
        /// 生成保养任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task GenerateEquMaintenanceTaskAsync(GenerateEquMaintenanceTaskDto param)
        {
            //计划
            var EquMaintenancePlanEntity = await _EquMaintenancePlanRepository.GetByIdAsync(param.MaintenancePlanId);
            if (param.ExecType == 0)
            {
                if (EquMaintenancePlanEntity.FirstExecuteTime < HymsonClock.Now())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12303));
                }

                if (!EquMaintenancePlanEntity.FirstExecuteTime.HasValue || !EquMaintenancePlanEntity.Type.HasValue || EquMaintenancePlanEntity.Cycle.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12304));
                }
            }
            var EquMaintenancePlanEquipmentRelations = await _EquMaintenancePlanEquipmentRelationRepository.GetByMaintenancePlanIdsAsync(param.MaintenancePlanId);
            if (EquMaintenancePlanEquipmentRelations == null || !EquMaintenancePlanEquipmentRelations.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12302));
            }
            var equipmentIds = EquMaintenancePlanEquipmentRelations.Select(it => it.EquipmentId).ToArray();
            var equEquipments = await _equEquipmentRepository.GetByIdAsync(equipmentIds);

            //模板与项目
            var MaintenanceTemplateIds = EquMaintenancePlanEquipmentRelations.Select(it => it.MaintenanceTemplateId).ToArray();
            var EquMaintenanceTemplateItemRelations = await _EquMaintenanceTemplateItemRelationRepository.GetEquMaintenanceTemplateItemRelationEntitiesAsync(
                  new EquMaintenanceTemplateItemRelationQuery { SiteId = param.SiteId, MaintenanceTemplateIds = MaintenanceTemplateIds });

            //项目  
            var MaintenanceItemIds = EquMaintenanceTemplateItemRelations.Select(it => it.MaintenanceItemId).ToArray();
            var EquMaintenanceItems = await _EquMaintenanceItemRepository.GetByIdsAsync(MaintenanceItemIds);
            if (EquMaintenanceItems == null || !EquMaintenanceItems.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12301));
            }
            //项目快照  
            List<EquMaintenanceTaskSnapshotItemEntity> EquMaintenanceTaskSnapshotItemList = new();
            List<EquMaintenanceTaskSnapshotPlanEntity> EquMaintenanceTaskSnapshotPlanList = new();
            List<EquMaintenanceTaskEntity> EquMaintenanceTaskList = new();
            List<EquMaintenanceTaskItemEntity> EquMaintenanceTaskItemList = new();

            #region 组装
            foreach (var item in EquMaintenancePlanEquipmentRelations)
            {
                var equEquipment = equEquipments.Where(it => it.Id == item.EquipmentId).FirstOrDefault();

                EquMaintenanceTaskEntity EquMaintenanceTask = new()
                {
                    Code = EquMaintenancePlanEntity.Code + equEquipment?.EquipmentCode,
                    Name = EquMaintenancePlanEntity.Name,
                    BeginTime = HymsonClock.Now(),
                    EndTime = HymsonClock.Now(),
                    Status = EquMaintenanceTaskStautusEnum.WaitInspect,
                    IsQualified = null,
                    Remark = EquMaintenancePlanEntity.Remark,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = param.UserName,
                    UpdatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = param.SiteId
                };
                EquMaintenanceTaskList.Add(EquMaintenanceTask);

                EquMaintenanceTaskSnapshotPlanEntity EquMaintenanceTaskSnapshotPlan = new()
                {
                    MaintenanceTaskId = EquMaintenanceTask.Id,
                    MaintenancePlanId = item.MaintenancePlanId,
                    Code = EquMaintenancePlanEntity.Code,
                    Name = EquMaintenancePlanEntity.Name,
                    Version = EquMaintenancePlanEntity.Version,
                    EquipmentId = item.EquipmentId,
                    MaintenanceTemplateId = item.MaintenanceTemplateId,
                    ExecutorIds = item.ExecutorIds ?? "",
                    LeaderIds = item.LeaderIds ?? "",
                    Type = EquMaintenancePlanEntity.Type ?? 0,
                    Status = EquMaintenancePlanEntity.Status,
                    BeginTime = EquMaintenancePlanEntity.BeginTime,
                    EndTime = EquMaintenancePlanEntity.EndTime,
                    IsSkipHoliday = EquMaintenancePlanEntity.IsSkipHoliday,
                    FirstExecuteTime = EquMaintenancePlanEntity.FirstExecuteTime,
                    Cycle = EquMaintenancePlanEntity.Cycle ?? 1,
                    CompletionHour = EquMaintenancePlanEntity.CompletionHour,
                    CompletionMinute = EquMaintenancePlanEntity.CompletionMinute,
                    PreGeneratedMinute = EquMaintenancePlanEntity.PreGeneratedMinute,
                    Remark = EquMaintenancePlanEntity.Remark,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = param.UserName,
                    UpdatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = param.SiteId
                };
                EquMaintenanceTaskSnapshotPlanList.Add(EquMaintenanceTaskSnapshotPlan);

                var thisEquMaintenanceTemplateItemRelations = EquMaintenanceTemplateItemRelations.Where(it => it.MaintenanceTemplateId == item.MaintenanceTemplateId);
                var thisMaintenanceItemIds = thisEquMaintenanceTemplateItemRelations.Select(it => it.MaintenanceItemId).ToArray();
                var thisEquMaintenanceItems = EquMaintenanceItems.Where(it => thisMaintenanceItemIds.Contains(it.Id));

                foreach (var thisEquMaintenanceItem in thisEquMaintenanceItems)
                {

                    var thisEquMaintenanceTemplateItemRelation = thisEquMaintenanceTemplateItemRelations.Where(it => it.MaintenanceItemId == thisEquMaintenanceItem.Id).FirstOrDefault();
                    EquMaintenanceTaskSnapshotItemEntity EquMaintenanceTaskSnapshotItem = new()
                    {
                        MaintenanceTaskId = EquMaintenanceTask.Id,
                        MaintenanceItemId = thisEquMaintenanceItem.Id,
                        Code = thisEquMaintenanceItem.Code ?? "",
                        Name = thisEquMaintenanceItem.Name ?? "",
                        Status = thisEquMaintenanceItem.Status,
                        DataType = thisEquMaintenanceItem.DataType,
                        CheckType = thisEquMaintenanceItem.CheckType,
                        CheckMethod = thisEquMaintenanceItem.CheckMethod ?? "",
                        UnitId = thisEquMaintenanceItem.UnitId,
                        OperationContent = thisEquMaintenanceItem.OperationContent ?? "",
                        Components = thisEquMaintenanceItem.Components ?? "",
                        Remark = thisEquMaintenanceItem.Remark ?? "",
                        ReferenceValue = thisEquMaintenanceTemplateItemRelation?.Center,
                        UpperLimit = thisEquMaintenanceTemplateItemRelation?.UpperLimit,
                        LowerLimit = thisEquMaintenanceTemplateItemRelation?.LowerLimit,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = param.SiteId
                    };
                    EquMaintenanceTaskSnapshotItemList.Add(EquMaintenanceTaskSnapshotItem);

                    EquMaintenanceTaskItemEntity EquMaintenanceTaskItem = new()
                    {
                        MaintenanceTaskId = EquMaintenanceTask.Id,
                        MaintenanceItemSnapshotId = EquMaintenanceTaskSnapshotItem.Id,
                        InspectionValue = "",
                        IsQualified = TrueOrFalseEnum.No,
                        Remark = EquMaintenanceTaskSnapshotItem.Remark,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = param.SiteId
                    };
                    EquMaintenanceTaskItemList.Add(EquMaintenanceTaskItem);

                }
            }
            #endregion

            using var trans = TransactionHelper.GetTransactionScope();

            await _EquMaintenanceTaskRepository.InsertRangeAsync(EquMaintenanceTaskList);

            await _EquMaintenanceTaskSnapshotPlanRepository.InsertRangeAsync(EquMaintenanceTaskSnapshotPlanList);

            await _EquMaintenanceTaskSnapshotItemRepository.InsertRangeAsync(EquMaintenanceTaskSnapshotItemList);

            await _EquMaintenanceTaskItemRepository.InsertRangeAsync(EquMaintenanceTaskItemList);

            trans.Complete();

        }

    }
}
