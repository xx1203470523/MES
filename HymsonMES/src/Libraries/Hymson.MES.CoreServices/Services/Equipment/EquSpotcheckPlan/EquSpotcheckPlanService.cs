
using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.EquSpotcheckPlan;
using Hymson.MES.Data.Repositories.EquSpotcheckPlanEquipmentRelation;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplate;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplateItemRelation;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.CoreServices.Services.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划 服务
    /// </summary>
    public class EquSpotcheckPlanCoreService : IEquSpotcheckPlanCoreService
    {
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
        /// 仓储接口（工作日历） 
        /// </summary>  
        private readonly IPlanCalendarRepository _planCalendarRepository;


        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        public EquSpotcheckPlanCoreService(IEquSpotcheckPlanRepository equSpotcheckPlanRepository, IEquSpotcheckPlanEquipmentRelationRepository equSpotcheckPlanEquipmentRelationRepository, IEquSpotcheckTemplateRepository equSpotcheckTemplateRepository, IEquEquipmentRepository equEquipmentRepository, IEquSpotcheckTemplateItemRelationRepository equSpotcheckTemplateItemRelationRepository, IEquSpotcheckItemRepository equSpotcheckItemRepository, IEquSpotcheckTaskSnapshotItemRepository equSpotcheckTaskSnapshotItemRepository, IEquSpotcheckTaskSnapshotPlanRepository equSpotcheckTaskSnapshotPlanRepository, IEquSpotcheckTaskRepository equSpotcheckTaskRepository, IEquSpotcheckTaskItemRepository equSpotcheckTaskItemRepository, IPlanCalendarRepository planCalendarRepository, IInteCodeRulesRepository inteCodeRulesRepository, IManuGenerateBarcodeService manuGenerateBarcodeService)
        {

            _equSpotcheckPlanRepository = equSpotcheckPlanRepository;
            _equSpotcheckPlanEquipmentRelationRepository = equSpotcheckPlanEquipmentRelationRepository;
            _equSpotcheckTemplateRepository = equSpotcheckTemplateRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _equSpotcheckTemplateItemRelationRepository = equSpotcheckTemplateItemRelationRepository;
            _equSpotcheckItemRepository = equSpotcheckItemRepository;
            _equSpotcheckTaskSnapshotItemRepository = equSpotcheckTaskSnapshotItemRepository;
            _equSpotcheckTaskSnapshotPlanRepository = equSpotcheckTaskSnapshotPlanRepository;
            _equSpotcheckTaskRepository = equSpotcheckTaskRepository;
            _equSpotcheckTaskItemRepository = equSpotcheckTaskItemRepository;
            _planCalendarRepository = planCalendarRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
        }
        /// <summary>
        /// 生成点检任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task GenerateEquSpotcheckTaskAsync(GenerateEquSpotcheckTaskDto param)
        {
            //计划
            var equSpotcheckPlanEntity = await _equSpotcheckPlanRepository.GetByIdAsync(param.SpotCheckPlanId);
            if (equSpotcheckPlanEntity.Status == DisableOrEnableEnum.Disable)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12314)).WithData("Code", equSpotcheckPlanEntity.Code);
            }
            if (param.ExecType == 0)
            {
                if (equSpotcheckPlanEntity.FirstExecuteTime < HymsonClock.Now())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12303));
                }

                if (!equSpotcheckPlanEntity.FirstExecuteTime.HasValue || !equSpotcheckPlanEntity.Type.HasValue || !equSpotcheckPlanEntity.Cycle.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12304));
                }

                if (equSpotcheckPlanEntity.IsSkipHoliday == TrueOrFalseEnum.Yes)
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
                  new EquSpotcheckTemplateItemRelationQuery { SiteId = param.SiteId, SpotCheckTemplateIds = spotCheckTemplateIds });

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

                var equMaintenanceTaskId = IdGenProvider.Instance.CreateId();

                EquSpotcheckTaskEntity equSpotcheckTask = new()
                {
                    Code = await GenerateSpotcheckOrderCodeAsync(param.SiteId, param.UserName),
                    Name = equSpotcheckPlanEntity.Name,
                    BeginTime = HymsonClock.Now(),
                    EndTime = HymsonClock.Now(),
                    Status = EquSpotcheckTaskStautusEnum.WaitInspect,
                    IsQualified = null,
                    Remark = equSpotcheckPlanEntity.Remark,

                    Id = equMaintenanceTaskId,
                    CreatedBy = param.UserName,
                    UpdatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = param.SiteId
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
                    CreatedBy = param.UserName,
                    UpdatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = param.SiteId
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
                        DataType = thisEquSpotcheckItem.DataType ?? DataTypeEnum.Text,
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
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = param.SiteId
                    };
                    equSpotcheckTaskSnapshotItemList.Add(equSpotcheckTaskSnapshotItem);

                    EquSpotcheckTaskItemEntity equSpotcheckTaskItem = new()
                    {
                        SpotCheckTaskId = equSpotcheckTask.Id,
                        SpotCheckItemSnapshotId = equSpotcheckTaskSnapshotItem.Id,
                        InspectionValue = "",
                        IsQualified = TrueOrFalseEnum.No,
                        Remark = equSpotcheckTaskSnapshotItem.Remark,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = param.SiteId
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
        private async Task<string> GenerateSpotcheckOrderCodeAsync(long siteId, string userName)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = siteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.Spotcheck
            });
            if (codeRules == null || !codeRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12309));
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12310));
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
