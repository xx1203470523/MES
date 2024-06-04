
using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums.Equipment.EquMaintenance;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem;
using Hymson.MES.Data.Repositories.EquMaintenancePlan;
using Hymson.MES.Data.Repositories.EquMaintenancePlanEquipmentRelation;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplate;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplateItemRelation;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Linq;

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
        /// <summary>
        /// 仓储接口（工作日历） 
        /// </summary>  
        private readonly IPlanCalendarRepository _planCalendarRepository;

        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        public EquMaintenancePlanCoreService(IEquMaintenancePlanRepository EquMaintenancePlanRepository, IEquMaintenancePlanEquipmentRelationRepository EquMaintenancePlanEquipmentRelationRepository, IEquMaintenanceTemplateRepository EquMaintenanceTemplateRepository, IEquEquipmentRepository equEquipmentRepository, IEquMaintenanceTemplateItemRelationRepository EquMaintenanceTemplateItemRelationRepository, IEquMaintenanceItemRepository EquMaintenanceItemRepository, IEquMaintenanceTaskSnapshotItemRepository EquMaintenanceTaskSnapshotItemRepository, IEquMaintenanceTaskSnapshotPlanRepository EquMaintenanceTaskSnapshotPlanRepository, IEquMaintenanceTaskRepository EquMaintenanceTaskRepository, IEquMaintenanceTaskItemRepository EquMaintenanceTaskItemRepository, IPlanCalendarRepository planCalendarRepository, IInteCodeRulesRepository inteCodeRulesRepository, IManuGenerateBarcodeService manuGenerateBarcodeService)
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
            _planCalendarRepository = planCalendarRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
        }
        /// <summary>
        /// 生成保养任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task GenerateEquMaintenanceTaskAsync(GenerateEquMaintenanceTaskDto param)
        {
            //计划
            var equMaintenancePlanEntity = await _EquMaintenancePlanRepository.GetByIdAsync(param.MaintenancePlanId);
            if (equMaintenancePlanEntity.Status == DisableOrEnableEnum.Disable)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12314)).WithData("Code", equMaintenancePlanEntity.Code);
            }
            if (param.ExecType == 0)
            {
                if (equMaintenancePlanEntity.FirstExecuteTime > HymsonClock.Now())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12303));
                }

                if (!equMaintenancePlanEntity.FirstExecuteTime.HasValue || !equMaintenancePlanEntity.Type.HasValue || !equMaintenancePlanEntity.Cycle.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12304));
                }

                if (equMaintenancePlanEntity.IsSkipHoliday == TrueOrFalseEnum.Yes)
                {
                    var thisDate = HymsonClock.Now();
                    var planCalendar = await _planCalendarRepository.GetOneAsync(new PlanCalendarQuery { SiteId = param.SiteId, Year = thisDate.Year, Month = thisDate.Month });
                    if (planCalendar != null && !string.IsNullOrWhiteSpace(planCalendar.Workday))
                    {
                        var workdayLike = thisDate.DayOfWeek.ToString();
                        var week = GetWeekToInt(workdayLike);
                        if (!planCalendar.Workday.Contains(week))
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES12307));
                        }
                    }
                    else
                    {
                        //暂不验证
                        //throw new CustomerValidationException(nameof(ErrorCode.MES12308));
                    }
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

                var equMaintenanceTaskId = IdGenProvider.Instance.CreateId();
                EquMaintenanceTaskEntity EquMaintenanceTask = new()
                {
                    Code = await GenerateMaintenanceOrderCodeAsync(param.SiteId, param.UserName),
                    Name = equMaintenancePlanEntity.Name,
                    //BeginTime = HymsonClock.Now(),
                    //EndTime = HymsonClock.Now(),
                    Status = EquMaintenanceTaskStautusEnum.WaitInspect,
                    IsQualified = null,
                    Remark = equMaintenancePlanEntity.Remark,

                    Id = equMaintenanceTaskId,
                    CreatedBy = param.UserName,
                    UpdatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = param.SiteId
                };
                EquMaintenanceTaskList.Add(EquMaintenanceTask);

                var endTime = HymsonClock.Now();
                if (equMaintenancePlanEntity.CompletionHour.HasValue)
                {
                    endTime.AddHours(equMaintenancePlanEntity.CompletionHour.ParseToDouble());
                }
                if (equMaintenancePlanEntity.CompletionMinute.HasValue)
                {
                    endTime.AddMinutes(equMaintenancePlanEntity.CompletionMinute.ParseToDouble());
                }
                EquMaintenanceTaskSnapshotPlanEntity EquMaintenanceTaskSnapshotPlan = new()
                {
                    MaintenanceTaskId = EquMaintenanceTask.Id,
                    MaintenancePlanId = item.MaintenancePlanId,
                    Code = equMaintenancePlanEntity.Code,
                    Name = equMaintenancePlanEntity.Name,
                    Version = equMaintenancePlanEntity.Version,
                    EquipmentId = item.EquipmentId,
                    MaintenanceTemplateId = item.MaintenanceTemplateId,
                    ExecutorIds = item.ExecutorIds ?? "",
                    LeaderIds = item.LeaderIds ?? "",
                    Type = equMaintenancePlanEntity.Type ?? 0,
                    Status = equMaintenancePlanEntity.Status,
                    BeginTime = HymsonClock.Now(), //EquMaintenancePlanEntity.BeginTime,
                    EndTime = endTime,//EquMaintenancePlanEntity.EndTime,
                    IsSkipHoliday = equMaintenancePlanEntity.IsSkipHoliday,
                    FirstExecuteTime = equMaintenancePlanEntity.FirstExecuteTime,
                    Cycle = equMaintenancePlanEntity.Cycle ?? 1,
                    CompletionHour = equMaintenancePlanEntity.CompletionHour,
                    CompletionMinute = equMaintenancePlanEntity.CompletionMinute,
                    PreGeneratedMinute = equMaintenancePlanEntity.PreGeneratedMinute,
                    Remark = equMaintenancePlanEntity.Remark,

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


        /// <summary>
        /// 生成保养任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task GenerateEquMaintenanceTaskAsync(EquMaintenanceAutoCreateIntegrationEvent param)
        {
            await GenerateEquMaintenanceTaskAsync(new GenerateEquMaintenanceTaskDto
            {
                SiteId = param.SiteId,
                UserName = param.UserName,
                MaintenancePlanId = param.MaintenancePlanId,
                ExecType = param.ExecType
            });
        }

        /// <summary>
        /// 停止保养任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>  
        public async Task StopEquMaintenanceTaskAsync(EquMaintenanceAutoStopIntegrationEvent param)
        {

        }

        #region 帮助
        private static string GetWeekToInt(string weekName)
        {
            int week = -1;
            switch (weekName)
            {
                case "Sunday":
                    week = 0;
                    break;
                case "Monday":
                    week = 1;
                    break;
                case "Tuesday":
                    week = 2;
                    break;
                case "Wednesday":
                    week = 3;
                    break;
                case "Thursday":
                    week = 4;
                    break;
                case "Friday":
                    week = 5;
                    break;
                case "Saturday":
                    week = 6;
                    break;
            }
            return week.ToString();
        }



        /// <summary>
        /// 点检单号生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<string> GenerateMaintenanceOrderCodeAsync(long siteId, string userName)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = siteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.Maintain
            });
            if (codeRules == null || !codeRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12311));
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12312));
            }

            var orderCodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeBo
            {
                SiteId = siteId,
                UserName = userName,
                CodeRuleId = codeRules.First().Id,
                Count = 1
            });

            return orderCodes.First();
        }
        #endregion
    }
}
