using Hymson.Authentication;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Report;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Proc;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.SystemApi;
using Hymson.MES.Services.Dtos.SystemApi.Kanban;
using Hymson.MES.Services.Services.Report.EquAlarmReport;
using Hymson.Utils;
using MySqlX.XDevAPI.Common;

namespace Hymson.MES.Services.Services.SystemApi;

public class SystemApiService : ISystemApiService
{
    private readonly ICurrentUser _currentUser;

    private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
    private readonly IManuSfcStepRepository _manuSfcStepRepository;

    private readonly IProcProcedureRepository _procProcedureRepository;
    private readonly IProcProcedurePlanRepository _procProcedurePlanRepository;
    private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
    private readonly IProcProcessRouteRepository _processRouteRepository;
    private readonly IProcProcessRouteDetailLinkRepository _processRouteDetailLinkRepository;
    private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
    private readonly IProcMaterialRepository _materialRepository;
    private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

    private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
    private readonly IProcResourceRepository _procResourceRepository;
    private readonly IEquEquipmentRepository _equEquipmentRepository;
    private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
    private readonly IEquEquipmentTheoryRepository _equEquipmentTheoryRepository;
    private readonly IEquAlarmReportService _equAlarmReportService;
    private readonly IEquAlarmRepository _equAlarmRepository;
    private readonly IEquStatusRepository _equStatusRepository;

    //不良相关
    private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;
    private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;
    private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
    private readonly IQualUnqualifiedGroupRepository _qualUnqualifiedGroupRepository;
    private readonly IQualUnqualifiedGroupProcedureRelationRepository _qualUnqualifiedGroupProcedureRelationRepository;
    private readonly IQualUnqualifiedCodeGroupRelationRepository _qualUnqualifiedCodeGroupRelationRepository;



    public SystemApiService(ICurrentUser currentUser,
        IManuSfcInfoRepository manuSfcInfoRepository,
        IManuSfcStepRepository manuSfcStepRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcProcedurePlanRepository procProcedurePlanRepository,
        IPlanWorkOrderRepository planWorkOrderRepository,
        IProcProcessRouteRepository processRouteRepository,
        IProcProcessRouteDetailLinkRepository processRouteDetailLinkRepository,
        IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
        IProcMaterialRepository materialRepository,
        IInteWorkCenterRepository inteWorkCenterRepository,
        IManuSfcSummaryRepository manuSfcSummaryRepository,
        IProcResourceRepository procResourceRepository,
        IEquEquipmentRepository equEquipmentRepository,
        IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
        IEquEquipmentTheoryRepository equEquipmentTheoryRepository,
        IEquAlarmReportService equAlarmReportService,
        IEquAlarmRepository equAlarmRepository,
        IEquStatusRepository equipmentStatusRepository,
        IManuSfcStepNgRepository manuSfcStepNgRepository,
        IManuProductBadRecordRepository manuProductBadRecordRepository,
        IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
        IQualUnqualifiedGroupRepository qualUnqualifiedGroupRepository,
        IQualUnqualifiedGroupProcedureRelationRepository qualUnqualifiedGroupProcedureRelationRepository,
        IQualUnqualifiedCodeGroupRelationRepository qualUnqualifiedCodeGroupRelationRepository
        )
    {
        _currentUser = currentUser;
        _manuSfcInfoRepository = manuSfcInfoRepository;
        _manuSfcStepRepository = manuSfcStepRepository;
        _equStatusRepository = equipmentStatusRepository;
        _procProcedureRepository = procProcedureRepository;
        _procProcedurePlanRepository = procProcedurePlanRepository;
        _planWorkOrderRepository = planWorkOrderRepository;
        _processRouteRepository = processRouteRepository;
        _processRouteDetailLinkRepository = processRouteDetailLinkRepository;
        _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
        _materialRepository = materialRepository;
        _inteWorkCenterRepository = inteWorkCenterRepository;
        _manuSfcSummaryRepository = manuSfcSummaryRepository;
        _procResourceRepository = procResourceRepository;
        _equEquipmentRepository = equEquipmentRepository;
        _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
        _equEquipmentTheoryRepository = equEquipmentTheoryRepository;
        _equAlarmReportService = equAlarmReportService;
        _equAlarmRepository = equAlarmRepository;
        _manuSfcStepNgRepository = manuSfcStepNgRepository;
        _manuProductBadRecordRepository = manuProductBadRecordRepository;
        _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
        _qualUnqualifiedGroupRepository = qualUnqualifiedGroupRepository;
        _qualUnqualifiedGroupProcedureRelationRepository = qualUnqualifiedGroupProcedureRelationRepository;
        _qualUnqualifiedCodeGroupRelationRepository = qualUnqualifiedCodeGroupRelationRepository;
    }

    #region 看板


    #region 首页

    /// <summary>
    /// 首页——OEE趋势图
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<OEETrendChartViewDto>> GetOEETrendChartAsync(OEETrendChartQueryDto queryDto)
    {
        List<OEETrendChartViewDto> result = new();

        //获取每个段对应的设备 OP020，OP220，OP170，分别对应电芯，模组，Pack
        //计算每个段的产出只统计尾工序
        var procedureCodes = new string[] { "OP020", "OP220", "OP170" };
        var ProcedureEntities = await _procProcedureRepository.GetByCodesAsync(procedureCodes, 123456);

        //根据工序获取资源
        var resourceTypeIds = ProcedureEntities.Select(a => a.ResourceTypeId.GetValueOrDefault());
        var procResourceEntities = await _procResourceRepository.GetByResTypeIdsAsync(new() { IdsArr = resourceTypeIds.ToArray(), SiteId = 123456 });

        //根据资源关联设备表查找绑定关系
        var resourceIds = procResourceEntities.Select(a => a.Id);
        var resourceBindEquipmentEntities = await _procResourceEquipmentBindRepository.GetByResourceIdsAsync(resourceIds.ToArray());

        //获取设备信息
        var equipmentIds = resourceBindEquipmentEntities.Select(a => a.EquipmentId);
        var equipmentEntities = await _equEquipmentRepository.GetByIdsAsync(equipmentIds.ToArray());

        //获取设备理论开机时长
        var equipmentCodes = equipmentEntities.Select(a => a.EquipmentCode).ToArray();
        var equipmentTheoryEntities = await _equEquipmentTheoryRepository.GetListAsync(new() { EquipmentCodes = equipmentCodes });

        //看板展示日期范围
        var startDate = DateTime.Parse(HymsonClock.Now().ToString("yyyy-MM-dd 08:30:00"));
        var endDate = DateTime.Parse(HymsonClock.Now().ToString("yyyy-MM-dd 18:30:00"));
        if (HymsonClock.Now() < startDate)
        {
            startDate = DateTime.Parse(startDate.AddDays(-1).ToString("yyyy-MM-dd 18:30:00"));
            endDate = DateTime.Parse(endDate.ToString("yyyy-MM-dd 08:30:00"));
        }
        else if (HymsonClock.Now() > endDate)
        {
            startDate = DateTime.Parse(startDate.ToString("yyyy-MM-dd 18:30:00"));
            endDate = DateTime.Parse(endDate.AddDays(1).ToString("yyyy-MM-dd 08:30:00"));
        }

        //获取停机时长
        var equAlarmDurationTimeEntities = await _equAlarmReportService.GetEquAlarmDurationTimeAsync(new()
        {
            EquipmentIds = equipmentIds,
            BeginTime = startDate,
            EndTime = endDate
        });

        var procedureIds = ProcedureEntities.Select(a => a.Id).ToArray();

        var workOrderEntity = await _planWorkOrderRepository.GetOneAsync(new() { OrderCode = queryDto.OrderCode });
        //获取当天生产数据
        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            WorkOrderId = workOrderEntity.Id,
            ProcedureIds = procedureIds,
            StartTime = startDate,
            EndTime = endDate
        });

        //间隔两个小时，计算每台设备的时间稼动率，
        //计算公式为  （每俩小时产量/俩小时预计产能）* 良率 * （（计划开机时长-故障时间）/计划开机时长）
        while (true)
        {
            foreach (var item in procedureCodes)
            {
                var procedure = ProcedureEntities.Where(a => a.Code == item).FirstOrDefault();
                var resource = procResourceEntities.FirstOrDefault(a => a.ResTypeId == procedure?.ResourceTypeId);
                var resourceBindEquipment = resourceBindEquipmentEntities.FirstOrDefault(a => a.ResourceId == resource?.Id);
                var equipment = equipmentEntities.FirstOrDefault(a => a.Id == resourceBindEquipment?.Id);
                var summary = manuSfcSummaryEntities.Where(a => a.ProcedureId == procedure?.Id
                && endDate >= startDate && endDate <= startDate.AddHours(2));
                var equipmenttheory = equipmentTheoryEntities.FirstOrDefault(a => a.EquipmentCode == equipment?.EquipmentCode)?.TheoryOutputQty ?? 0;

                //停机时间
                var equAlarm = equAlarmDurationTimeEntities.FirstOrDefault(a => a.EquipmentId == equipment?.Id)?.DurationTime ?? 0;

                var outputQty = summary.Sum(a => a.Qty) ?? 0;
                var quanlifiedQty = summary.Sum(a => a.QualityStatus) ?? 0;

                OEETrendChartViewDto newitem = new()
                {
                    EndTime = startDate.ToString("HH:30") + "-" + startDate.AddHours(2).ToString("HH:30"),
                    OEE = ((equipmenttheory - (equAlarm / 3600)) / (equipmenttheory == 0 ? 1 : equipmenttheory)
                    * quanlifiedQty / (outputQty == 0 ? 1 : outputQty)
                    * outputQty / (equipmenttheory == 0 ? 1 : equipmenttheory)),
                    Type = item switch
                    {
                        "OP020" => "电芯段",
                        "OP220" => "模组段",
                        "OP170" => "Pack段",
                        _ => ""
                    }
                };

                result.Add(newitem);
            }

            if (startDate.AddHours(2) == endDate) break;
            startDate = startDate.AddHours(2);
        }


        return result;
    }

    /// <summary>
    /// 获取Pack生产数据
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<PackProductionViewDto>> GetPackProductionAsync(PackProductionQueryDto queryDto)
    {
        List<PackProductionViewDto> list = new();

        var planWorkOrderEntity = await _planWorkOrderRepository.GetOneAsync(new() { OrderCode = queryDto.OrderCode });

        var proceRouteEntity = await _processRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId);
        if (proceRouteEntity == null) return list;

        var proceRouteDetailEntities = await _procProcessRouteDetailNodeRepository.GetListAsync(new() { ProcessRouteId = proceRouteEntity.Id });

        var lastProcedureId = proceRouteDetailEntities.OrderByDescending(a => a.SerialNo).FirstOrDefault()?.ProcedureId;

        var procedureEntity = await _procProcedureRepository.GetByIdAsync(lastProcedureId.GetValueOrDefault());
        var procedureTheoryEntity = await _procProcedurePlanRepository.GetOneAsync(new() { ProcedureId = procedureEntity.Id });
        var planOutputQty = (procedureTheoryEntity.PlanOutputQty * 2) ?? 0;

        var startDate = new DateTime();
        var endDate = new DateTime();
        var nowDate = HymsonClock.Now();
        if (nowDate >= DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00")) && nowDate <= DateTime.Parse(nowDate.ToString("yyyy-MM-dd 20:30:00")))
        {
            startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
            endDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 20:30:00"));
        }
        else if (nowDate <= DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00")))
        {
            startDate = DateTime.Parse(nowDate.AddDays(-1).ToString("yyyy-MM-dd 20:30:00"));
            endDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
        }
        else
        {
            startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 20:30:00"));
            endDate = DateTime.Parse(nowDate.AddDays(1).ToString("yyyy-MM-dd 08:30:00"));
        }

        var manuSFCSummary = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            StartTime = startDate,
            EndTime = endDate,
            WorkOrderId = planWorkOrderEntity.Id,
            ProcedureId = lastProcedureId
        });


        while (true)
        {
            var newItem = new PackProductionViewDto();
            var outputQty = manuSFCSummary.Where(a => a.EndTime >= startDate && a.EndTime < startDate.AddHours(2)).Sum(a => a.Qty);

            list.Add(new PackProductionViewDto()
            {
                EndTime = startDate.ToString("HH:mm") + "-" + startDate.AddHours(2).ToString("HH:mm"),
                OutputQty = outputQty,
                PlanOutputQty = planOutputQty * 2,
                PlanCompletionRate = outputQty / ((planOutputQty * 2) == 0 ? 1 : (planOutputQty * 2))
            });


            if (startDate.AddHours(2) == endDate) break;
            startDate = startDate.AddHours(2);
        }

        return list;
    }

    /// <summary>
    /// 首页-工单基本信息
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<PlanWorkOrderInfoViewDto>> GetPlanWorkOrderInfoAsync(PlanWorkOrderInfoQueryDto queryDto)
    {
        List<PlanWorkOrderInfoViewDto> result = new();

        var productEntity = await _materialRepository.GetListAsync(new()
        {
            MaterialNameLike = queryDto.ProductName
        });

        //根据选择产品过滤工单
        var productIds = productEntity.Select(a => a.Id).ToList();

        //获取工单信息
        var planWorkOrderEntities = await _planWorkOrderRepository.GetListAsync(new()
        {
            OrderCode = queryDto.OrderCode,
            StatusList = new() { PlanWorkOrderStatusEnum.InProduction },
            ProductIds = productIds
        });
        if (planWorkOrderEntities == null) return result;


        //获取工艺路线
        var procProcessRouteIds = planWorkOrderEntities.Select(a => a.ProcessRouteId).ToArray();
        var procProcessRouteEntities = await _processRouteRepository.GetByIdsAsync(procProcessRouteIds);
        //获取工艺路线尾工序
        var procProcessRouteDetialEntities = await _processRouteDetailLinkRepository.GetListAsync(new()
        {
            ProcessRouteIds = procProcessRouteIds
        });

        //获取每个工单的尾工序
        var lastProcedureIds = new Dictionary<long, long>();
        foreach (var item in planWorkOrderEntities)
        {
            var id = procProcessRouteDetialEntities.Where(a => a.ProcessRouteId == item.ProcessRouteId).OrderBy(a => a.SerialNo).FirstOrDefault()?.ProcessRouteId;

            lastProcedureIds.Add(item.Id, id.GetValueOrDefault());
        }

        //获取电芯段工序Id
        var cellProcedureId = await _procProcedureRepository.GetByCodeAsync("OP020", 123456);

        //线体
        var workCenterds = planWorkOrderEntities.Select(a => a.WorkCenterId.GetValueOrDefault()).ToArray();
        var inteWorkCenterEntities = await _inteWorkCenterRepository.GetByIdsAsync(workCenterds);

        //产品
        var materialIds = planWorkOrderEntities.Select(a => a.ProductId).ToArray();
        var materialEntities = await _materialRepository.GetByIdsAsync(materialIds);

        var workOrderIds = planWorkOrderEntities.Select(a => a.Id).ToArray();
        var manuSfcSummaryViews = await _manuSfcSummaryRepository.GetManuSfcSummaryViewAsync(new()
        {
            WorkOrderIds = workOrderIds,
            ProcedureIds = lastProcedureIds.Select(a => a.Value).ToArray()
        });

        var cellManuSfcSummaryViews = await _manuSfcSummaryRepository.GetManuSfcSummaryViewAsync(new()
        {
            WorkOrderIds = workOrderIds,
            ProcedureId = cellProcedureId.Id
        });

        foreach (var planWorkOrderEntity in planWorkOrderEntities)
        {
            PlanWorkOrderInfoViewDto newItem = new();

            var procProcessRouteEntity = procProcessRouteEntities.FirstOrDefault(a => a.Id == planWorkOrderEntity.ProcessRouteId);
            var workCenterEntity = inteWorkCenterEntities.FirstOrDefault(a => a.Id == planWorkOrderEntity.WorkCenterId);
            var materialEntity = materialEntities.FirstOrDefault(a => a.Id == planWorkOrderEntity.ProductId);

            var manuSfcSummaryView = manuSfcSummaryViews.Where(a => a.WorkOrderId == planWorkOrderEntity.Id).FirstOrDefault();
            var cellManuSfcSummaryView = cellManuSfcSummaryViews.Where(a => a.WorkOrderId == planWorkOrderEntity.Id).FirstOrDefault();

            newItem.StartTime = planWorkOrderEntity.PlanStartTime;
            //产出数量
            newItem.Qty = planWorkOrderEntity.Qty;
            //不良数
            newItem.UnqualifiedQty = (manuSfcSummaryView?.OutputQty - manuSfcSummaryView?.QualifiedQty) ?? 0;
            newItem.UnqualifiedRate = Math.Round(((manuSfcSummaryView?.OutputQty - manuSfcSummaryView?.QualifiedQty) / (manuSfcSummaryView?.OutputQty ?? 1)) ?? 0, 2);
            //完成率
            newItem.CompletionRate = Math.Round(newItem.Qty.GetValueOrDefault() / planWorkOrderEntity.Qty, 0);
            //完工数量
            newItem.Completionty = manuSfcSummaryView?.OutputQty ?? 0;
            newItem.ClassType = HymsonClock.Now().Hour >= 8 && HymsonClock.Now().Hour <= 20 ? DetailClassTypeEnum.Morning : DetailClassTypeEnum.Night;
            newItem.OrderCode = planWorkOrderEntity.OrderCode;
            newItem.ProcessRouteId = procProcessRouteEntity?.Id;
            newItem.ProcessRouteName = procProcessRouteEntity?.Name;
            newItem.ProductId = materialEntity?.Id;
            newItem.ProductName = materialEntity?.MaterialName;
            newItem.WorkCenterId = workCenterEntity?.Id;
            newItem.WorkCenterName = workCenterEntity?.Name;
            newItem.QualifiedRate = Math.Round((manuSfcSummaryView?.QualifiedQty ?? 0) / (manuSfcSummaryView?.OutputQty ?? 1), 2);
            newItem.PlanAchievementRate = Math.Round(newItem.Qty.GetValueOrDefault() / planWorkOrderEntity.Qty, 0);
            newItem.CellQty = cellManuSfcSummaryView?.OutputQty ?? 0;
            result.Add(newItem);
        }
        return result;
    }

    #endregion

    #region 质量模块

    /// <summary>
    /// 今日一次合格率（风冷/液冷）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<OneQualifiedViewDto> GetOneQualifiedRateAsync(OneQualifiedQueryDto queryDto)
    {
        OneQualifiedViewDto result = new();
        var productEntity = await _materialRepository.GetListAsync(new()
        {
            MaterialNameLike = queryDto.ProductName
        });

        //根据选择产品过滤工单
        var productIds = productEntity.Select(a => a.Id).ToList();

        var planWorkOrderEntities = await _planWorkOrderRepository.GetListAsync(new()
        {
            ProductIds = productIds
        });
        var planWorkOrderIds = planWorkOrderEntities.Select(a => a.Id);

        //获取每个段对应的设备 OP020，OP220，OP170，分别对应电芯，模组，Pack
        //计算每个段的产出只统计尾工序
        var procedureCodes = new string[] { "OP020", "OP220", "OP170" };
        var ProcedureEntities = await _procProcedureRepository.GetByCodesAsync(procedureCodes, 123456);

        var startDate = new DateTime();
        var endDate = new DateTime();
        var nowDate = HymsonClock.Now();
        if (nowDate >= DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00")))
        {
            startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
            endDate = nowDate;
        }
        else
        {
            startDate = DateTime.Parse(nowDate.AddDays(-1).ToString("yyyy-MM-dd 08:30:00"));
            endDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
        }

        if (planWorkOrderIds?.Any() == false) planWorkOrderIds = new long[] { -1 };

        var procedureIds = ProcedureEntities.Select(a => a.Id).ToArray();
        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            StartTime = startDate,
            EndTime = endDate,
            ProcedureIds = procedureIds,
            ProductIds = productIds,
            WorkOrderIds = planWorkOrderIds
        });

        //电芯段一次合格率
        var cellProcedure = ProcedureEntities.FirstOrDefault(a => a.Code == "OP020");
        var cellSummary = manuSfcSummaryEntities.Where(a => a.ProcedureId == cellProcedure?.Id);
        var cellOneQualifiedQty = cellSummary.Sum(a => a.FirstQualityStatus) ?? 0;
        var cellOutputQty = cellSummary.Sum(a => a?.Qty.GetValueOrDefault());

        result.CellOneQualifiedRate = cellOneQualifiedQty / (cellOutputQty == 0 ? 1 : cellOutputQty);

        //电芯段一次合格率
        var moduleProcedure = ProcedureEntities.FirstOrDefault(a => a.Code == "OP220");
        var moduleSummary = manuSfcSummaryEntities.Where(a => a.ProcedureId == cellProcedure?.Id);
        var moduleOneQualifiedQty = moduleSummary.Sum(a => a.FirstQualityStatus);
        var moduleOutputQty = moduleSummary.Sum(a => a.Qty);
        result.ModuleOneQualifiedRate = moduleOneQualifiedQty / (moduleOutputQty == 0 ? 1 : moduleOutputQty);

        //电芯段一次合格率
        var packProcedure = ProcedureEntities.FirstOrDefault(a => a.Code == "OP170");
        var packSummary = manuSfcSummaryEntities.Where(a => a.ProcedureId == cellProcedure?.Id);
        var packOneQualifiedQty = packSummary.Sum(a => a.FirstQualityStatus);
        var packOutputQty = packSummary.Sum(a => a.Qty);
        result.PackOneQualifiedRate = packOneQualifiedQty / (packOutputQty == 0 ? 1 : packOutputQty);

        return result;
    }

    /// <summary>
    /// 按月获取一次合格率（风冷/液冷）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<OneQualifiedMonthViewDto>> GetMonthOneQualifiedRateAsync(OneQualifiedMonthQueryDto queryDto)
    {
        List<OneQualifiedMonthViewDto> result = new();

        var productEntities = await _materialRepository.GetListAsync(new()
        {
            MaterialNameLike = queryDto.ProductName
        });

        //根据选择产品过滤工单
        var productIds = productEntities.Select(a => a.Id).ToList();
        var planWorkOrderEntities = await _planWorkOrderRepository.GetListAsync(new()
        {
            ProductIds = productIds
        });
        var planWorkOrderIds = planWorkOrderEntities.Select(a => a.Id);

        //获取每个段对应的设备 OP020，OP220，OP170，分别对应电芯，模组，Pack
        //计算每个段的产出只统计尾工序
        var procedureCodes = new string[] { "OP020", "OP220", "OP170" };
        var ProcedureEntities = await _procProcedureRepository.GetByCodesAsync(procedureCodes, 123456);

        var startDate = new DateTime();
        var endDate = new DateTime();
        var nowDate = HymsonClock.Now();

        startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-01 08:30:00"));
        endDate = DateTime.Parse(nowDate.AddMonths(1).ToString("yyyy-MM-01 08:30:00"));

        var procedureIds = ProcedureEntities.Select(a => a.Id).ToArray();

        if (planWorkOrderIds?.Any() == false) planWorkOrderIds = new long[] { -1 };

        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            StartTime = startDate,
            EndTime = endDate,
            ProcedureIds = procedureIds,
            ProductIds = productIds,
            WorkOrderIds = planWorkOrderIds
        });


        while (true)
        {
            foreach (var item in procedureCodes)
            {
                var procedure = ProcedureEntities.Where(a => a.Code == item).FirstOrDefault();
                var summary = manuSfcSummaryEntities.Where(a => a.ProcedureId == procedure?.Id
                && a.EndTime >= startDate && a.EndTime <= startDate.AddDays(1));

                var outputQty = summary.Sum(a => a.Qty);
                var oneQuanlifiedQty = summary.Sum(a => a.FirstQualityStatus);

                OneQualifiedMonthViewDto newitem = new()
                {
                    DayTime = startDate.ToString("dd"),
                    Type = item switch
                    {
                        "OP020" => "电芯段",
                        "OP220" => "模组段",
                        "OP170" => "Pack段",
                        _ => ""
                    },
                    OneQualifiedRate = oneQuanlifiedQty / (outputQty == 0 ? 1 : outputQty)
                };

                result.Add(newitem);
            }


            if (startDate.AddDays(1) == endDate) break;
            startDate = startDate.AddDays(1);
        }

        return result;
    }

    /// <summary>
    /// 电芯，模组，Pack获取不良分布（日/月，风冷/液冷）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<DefectDistributionViewDto>> GetDefectDistributionAsync(DefectDistributionQueryDto queryDto)
    {
        List<DefectDistributionViewDto> result = new();

        var productEntities = await _materialRepository.GetListAsync(new()
        {
            MaterialNameLike = queryDto.ProductName
        });

        //根据选择产品过滤工单
        var productIds = productEntities.Select(a => a.Id).Distinct().ToList();

        var planWorkOrderEntities = await _planWorkOrderRepository.GetListAsync(new()
        {
            ProductIds = productIds
        });
        var planWorkOrderIds = planWorkOrderEntities.Select(a => a.Id);

        //获取每个段对应的设备 OP020，OP220，OP170，分别对应电芯，模组，Pack
        //计算每个段的产出只统计尾工序
        var procedureCodes = new string[] { "OP020", "OP220", "OP170" };
        var ProcedureEntities = await _procProcedureRepository.GetByCodesAsync(procedureCodes, 123456);
        var procedureIds = ProcedureEntities.Select(a => a.Id);

        var procedureEntity = ProcedureEntities.FirstOrDefault(a =>
                a.Code == (queryDto.Type switch
                {
                    SFCTypeEnum.Cell => "OP020",
                    SFCTypeEnum.Module => "OP220",
                    SFCTypeEnum.Pack => "OP170",
                    _ => ""
                })
            );

        var startDate = new DateTime();
        var endDate = new DateTime();
        var nowDate = HymsonClock.Now();

        if (queryDto.DateType == DateTypeEnum.Month)
        {
            startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-01 08:30:00"));
            endDate = DateTime.Parse(nowDate.AddMonths(1).ToString("yyyy-MM-01 08:30:00"));
        }
        else if (queryDto.DateType == DateTypeEnum.Day)
        {
            if (nowDate >= DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00")))
            {
                startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
                endDate = nowDate;
            }
            else
            {
                startDate = DateTime.Parse(nowDate.AddDays(-1).ToString("yyyy-MM-dd 08:30:00"));
                endDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
            }
        }
        else
            return result;

        //设备上报不良记录
        var manuSfcStepNgEntities = await _manuSfcStepNgRepository.GetListAsync(new()
        {
            CreatedOnStart = startDate,
            CreatedOnEnd = endDate
        });

        var manuProductBadRecordEntities = await _manuProductBadRecordRepository.GetListAsync(new()
        {
            CreatedOnStart = startDate,
            CreatedOnEnd = endDate,
            FoundBadOperationId = procedureEntity?.Id
        });

        //获取SfcInfo，再根据workOrderId筛选
        var manuSfcInfoIds = manuProductBadRecordEntities.Select(a => a.SfcInfoId.GetValueOrDefault());
        var sfcinfoEntities = await _manuSfcInfoRepository.GetByIdsAsync(manuSfcInfoIds.ToArray());
        sfcinfoEntities = sfcinfoEntities.Where(a => planWorkOrderEntities.Any(b => b.Id == a.WorkOrderId));

        //获取manuStep，根据workorderId筛选
        var manuSfcStepIds = manuSfcStepNgEntities.Select(a => a.BarCodeStepId);
        var manuSfcStepEntities = await _manuSfcStepRepository.GetByIdsAsync(manuSfcStepIds.ToArray());
        manuSfcStepEntities = manuSfcStepEntities.Where(a => planWorkOrderEntities.Any(b => b.Id == a.WorkOrderId) && a.ProcedureId == procedureEntity?.Id);

        //获取需要用到不合格代码
        List<QualUnqualifiedCodeEntity> unQualifiedList = new();

        var unQualifiedIds = manuProductBadRecordEntities.Select(a => a.UnqualifiedId);
        var unQualifiedEntities = await _qualUnqualifiedCodeRepository.GetListAsync(new() { Ids = unQualifiedIds });
        unQualifiedList.AddRange(unQualifiedEntities);

        var unQualifiedCodes = manuSfcStepNgEntities.Select(a => a.UnqualifiedCode);
        unQualifiedEntities = await _qualUnqualifiedCodeRepository.GetListAsync(new() { UnqualifiedCodes = unQualifiedCodes });
        unQualifiedList.AddRange(unQualifiedEntities);

        //查询工序绑定的不良集合
        var qualUnqualifiedGroupPrcocedureEntitiees = await _qualUnqualifiedGroupProcedureRelationRepository.GetListAsync(new() {
            ProcedureIds = procedureIds
        });

        var groupIds = qualUnqualifiedGroupPrcocedureEntitiees.Select(a => a.UnqualifiedGroupId);
        //查询不良集合
        var qualUnqualifiedGroupEntities = await _qualUnqualifiedCodeGroupRelationRepository.GetListAsync(new() { 
            UnqualifiedGroupIds = groupIds
        });
        var unqualifiedIds = qualUnqualifiedGroupEntities.Select(a => a.UnqualifiedCodeId);

        unQualifiedEntities = await _qualUnqualifiedCodeRepository.GetListAsync(new() { Ids = unqualifiedIds });
        unQualifiedList.AddRange(unQualifiedEntities);

        foreach (var item in unQualifiedEntities)
        {
            result.Add(new()
            {
                UnQualifiedName = item.UnqualifiedCodeName,
                UnQualifiedQty = 0
            });
        }


        foreach (var item in manuSfcStepNgEntities)
        {
            var manuSfcStepEntity = manuSfcStepEntities.FirstOrDefault(a => a.Id == item.BarCodeStepId);
            if (manuSfcStepEntity == null) continue;

            var unQualifiedEntity = unQualifiedList.FirstOrDefault(a => a.UnqualifiedCode == item.UnqualifiedCode);

            var unQualifiedItem = result.FirstOrDefault(a => a.UnQualifiedName == unQualifiedEntity?.UnqualifiedCodeName);

            if (unQualifiedItem != null) unQualifiedItem.UnQualifiedQty += 1;
            else result.Add(new() { UnQualifiedName = unQualifiedEntity?.UnqualifiedCodeName ?? "", UnQualifiedQty = 1 });
        }

        foreach (var item in manuProductBadRecordEntities)
        {
            var manuSfcInfo = sfcinfoEntities.FirstOrDefault(a => a.Id == item.SfcInfoId);
            if (manuSfcInfo == null) continue;

            var unQualifiedEntity = unQualifiedList.FirstOrDefault(a => a.Id == item.UnqualifiedId);

            var unQualifiedItem = result.FirstOrDefault(a => a.UnQualifiedName == unQualifiedEntity?.UnqualifiedCodeName);

            if (unQualifiedItem != null) unQualifiedItem.UnQualifiedQty += 1;
            else result.Add(new() { UnQualifiedName = unQualifiedEntity?.UnqualifiedCodeName ?? "", UnQualifiedQty = 1 });
        }

        return result;
    }

    #endregion

    #region 生产板块

    /// <summary>
    /// 工序日产出滚动图
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcedureDayOutputViewDto>> GetProcedureDayOutputAsync(ProcedureDayOutputQueryDto queryDto)
    {
        List<ProcedureDayOutputViewDto> result = new();

        var productEntities = await _materialRepository.GetListAsync(new()
        {
            MaterialNameLike = queryDto.ProductName
        });

        //根据选择产品过滤工单
        var productIds = productEntities.Select(a => a.Id).Distinct().ToList();
        var planWorkOrderEntities = await _planWorkOrderRepository.GetListAsync(new()
        {
            ProductIds = productIds
        });
        var planWorkOrderIds = planWorkOrderEntities.Select(a => a.Id);

        var startDate = new DateTime();
        var endDate = new DateTime();
        var nowDate = HymsonClock.Now();
        if (nowDate >= DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00")))
        {
            startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
            endDate = nowDate;
        }
        else
        {
            startDate = DateTime.Parse(nowDate.AddDays(-1).ToString("yyyy-MM-dd 08:30:00"));
            endDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
        }

        if (planWorkOrderIds?.Any() == false) planWorkOrderIds = new long[] { -1 };

        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            StartTime = startDate,
            EndTime = endDate,
            WorkOrderIds = planWorkOrderIds
        });

        var procedureIds = manuSfcSummaryEntities.Select(a => a.ProcedureId).Distinct();
        var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds.ToArray());
        var procedurePlanEntities = await _procProcedurePlanRepository.GetListAsync(new()
        {
            ProcedureIds = procedureIds
        });

        foreach (var item in procedureIds)
        {
            ProcedureDayOutputViewDto newItem = new();

            var procedureEntity = procedureEntities.FirstOrDefault(a => a.Id == item);
            var procedurePlanEntity = procedurePlanEntities.FirstOrDefault(b => b.ProcedureId == item);
            var OutputQty = manuSfcSummaryEntities.Where(a => a.ProcedureId == item).Sum(a => a.Qty);
            var planQty = procedurePlanEntity?.PlanOutputQty ?? 0;

            newItem.ProcedureId = procedureEntity?.Id;
            newItem.ProcedureName = procedureEntity?.Name;
            newItem.OutputQty = OutputQty;
            newItem.PlanQty = planQty;
            newItem.PlanCompleteRate = planQty / newItem.OutputQty;

            result.Add(newItem);
        }

        return result;
    }

    /// <summary>
    /// 工序生产产能
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProductionCapacityViewDto>> GetProductionCapacityAsync(ProductionCapacityQueryDto queryDto)
    {
        List<ProductionCapacityViewDto> result = new();

        var productEntities = await _materialRepository.GetListAsync(new()
        {
            MaterialNameLike = queryDto.ProductName
        });

        //根据选择产品过滤工单
        var productIds = productEntities.Select(a => a.Id).ToList();
        var planWorkOrderEntities = await _planWorkOrderRepository.GetListAsync(new()
        {
            ProductIds = productIds
        });
        var planWorkOrderIds = planWorkOrderEntities.Select(a => a.Id);


        //获取每个段对应的设备 OP020，OP220，OP170，分别对应电芯，模组，Pack
        //计算每个段的产出只统计尾工序
        var procedureCodes = new string[] { "OP020", "OP220", "OP170" };
        var ProcedureEntities = await _procProcedureRepository.GetByCodesAsync(procedureCodes, 123456);
        var procedureIds = ProcedureEntities.Select(a => a.Id);
        var procedurePlanEntities = await _procProcedurePlanRepository.GetListAsync(new() { ProcedureIds = procedureIds });

        var startDate = new DateTime();
        var endDate = new DateTime();
        var nowDate = HymsonClock.Now();

        startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-01 08:30:00"));
        endDate = DateTime.Parse(nowDate.AddMonths(1).ToString("yyyy-MM-01 08:30:00"));

        if (planWorkOrderIds?.Any() == false) planWorkOrderIds = new long[] { -1 };

        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            WorkOrderIds = planWorkOrderIds,
            ProcedureIds = procedureIds.ToArray()
        });

        if (queryDto.Type != null)
        {
            procedureCodes = procedureCodes.Where(a => a == procedureCodes[(int)queryDto.Type]).ToArray();
        }

        while (true)
        {
            foreach (var item in procedureCodes)
            {
                var procedure = ProcedureEntities.Where(a => a.Code == item).FirstOrDefault();
                var procedurePlan = procedurePlanEntities.FirstOrDefault(a => a.ProcedureId == procedure?.Id);

                var summary = manuSfcSummaryEntities.Where(a => a.ProcedureId == procedure?.Id
                && a.EndTime >= startDate && a.EndTime <= startDate.AddDays(1));

                var planQty = procedurePlan?.PlanOutputQty ?? 0;
                var outputQty = summary.Sum(a => a.Qty);

                ProductionCapacityViewDto newitem = new()
                {
                    EndTime = startDate.ToString("dd"),
                    OutputQty = outputQty ?? 0,
                    PlanQty = planQty,
                    CompletionRate = outputQty / (planQty == 0 ? 1 : planQty)
                };

                result.Add(newitem);
            }

            if (startDate.AddDays(1) == endDate) break;
            startDate = startDate.AddDays(1);
        }

        return result;
    }

    #endregion

    #region 设备板块

    /// <summary>
    /// 查询设备运行状态
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<EquipmentStatusViewDto>> GetEquipmentStatusAsync()
    {
        List<EquipmentStatusViewDto> result = new();

        var equStatusEntities = await _equStatusRepository.GetListAsync(new() { });

        var equipmentIds = equStatusEntities.Select(a => a.EquipmentId).ToArray();
        var equipmentEntities = await _equEquipmentRepository.GetByIdsAsync(equipmentIds);

        foreach (var item in equStatusEntities)
        {
            var equipmentEntity = equipmentEntities.FirstOrDefault(a => a.Id == item.EquipmentId);
            if (equipmentEntity == null) continue;

            result.Add(new()
            {
                EquipmentCode = equipmentEntity?.EquipmentCode,
                EquipmentName = equipmentEntity?.EquipmentName,
                LocalTime = item?.LocalTime,
                EquipmentStatus = item?.EquipmentStatus,
                EquipmentStatusName = item?.EquipmentStatus.GetDescription()
            });
        }

        return result;
    }

    /// <summary>
    /// 设备运行状态分布
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<EquStatusDistributionViewDto>> GetEquipmentStatusDistributionAsync()
    {
        var equStatusEntities = await _equStatusRepository.GetListAsync(new() { });

        var equipmentIds = equStatusEntities.Select(a => a.EquipmentId).ToArray();
        var equipmentEntities = await _equEquipmentRepository.GetByIdsAsync(equipmentIds);

        var result = equStatusEntities.GroupBy(a => a.EquipmentStatus)
            .Select(a => new EquStatusDistributionViewDto
            {
                EquipmentStatus = a.Key,
                Qty = a.Count(),
                EquipmentStatusName = a.Key.GetDescription()
            });

        return result;
    }

    /// <summary>
    /// 设备故障率（日/月）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<EquFaultRateViewDto>> GetEquFaultRateAsync(EquFaultRateQueryDto queryDto)
    {
        List<EquFaultRateViewDto> result = new();

        var equipmentEntities = await _equEquipmentRepository.GetBaseListAsync();
        var equipmentIds = equipmentEntities.Select(a => a.Id);

        //获取计划开机时长
        var equipmentTheoryEntities = await _equEquipmentTheoryRepository.GetListAsync(new() { });

        var startDate = new DateTime();
        var endDate = new DateTime();
        var nowDate = HymsonClock.Now();

        if (queryDto.DateType == DateTypeEnum.Month)
        {
            startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-01 08:30:00"));
            endDate = DateTime.Parse(nowDate.AddMonths(1).ToString("yyyy-MM-01 08:30:00"));
        }
        else if (queryDto.DateType == DateTypeEnum.Day)
        {
            if (nowDate >= DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00")))
            {
                startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
                endDate = nowDate;
            }
            else
            {
                startDate = DateTime.Parse(nowDate.AddDays(-1).ToString("yyyy-MM-dd 08:30:00"));
                endDate = DateTime.Parse(nowDate.ToString("yyyy-MM-dd 08:30:00"));
            }
        }
        else
            return result;

        //获取停机时长
        var equAlarmDurationTimeEntities = await _equAlarmReportService.GetEquAlarmDurationTimeAsync(new()
        {
            EquipmentIds = equipmentIds,
            BeginTime = startDate,
            EndTime = endDate
        });

        foreach (var item in equipmentEntities)
        {
            //计算停机时长
            //单位转换为小时
            var DurationTime = equAlarmDurationTimeEntities.FirstOrDefault(a => a.EquipmentId == item.Id)?.DurationTime / 3600 ?? 0;
            var equPlanWorkTime = equipmentTheoryEntities.Where(a => a.EquipmentCode == item.EquipmentCode).FirstOrDefault()?.TheoryOnTime ?? 0;

            result.Add(new()
            {
                EquipmentCode = item.EquipmentCode,
                EquipmentName = item.EquipmentName,
                FaultRate = Math.Round(equPlanWorkTime == 0 ? 0 : (equPlanWorkTime - DurationTime) / (equPlanWorkTime == 0 ? 1 : equPlanWorkTime), 4)
            });

        }

        return result;
    }

    /// <summary>
    /// 设备OEE趋势图（日/月）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<EquOEETrendChartViewDto>> GetEquOEETrendChartAsync(EquOEETrendChartQueryDto queryDto)
    {
        List<EquOEETrendChartViewDto> result = new();

        var startDate = new DateTime();
        var endDate = new DateTime();
        var searchEndDate = new DateTime();
        var nowDate = HymsonClock.Now();

        if (queryDto.DateType == DateTypeEnum.Month)
        {
            startDate = DateTime.Parse(nowDate.ToString("yyyy-01-01 08:30:00")).AddYears(-1);
            endDate = DateTime.Parse(nowDate.AddMonths(1).ToString("yyyy-01-01 08:30:00"));
            searchEndDate = startDate.AddMonths(1);
        }
        else if (queryDto.DateType == DateTypeEnum.Day)
        {
            startDate = DateTime.Parse(nowDate.ToString("yyyy-MM-01 08:30:00"));
            endDate = DateTime.Parse(nowDate.AddMonths(1).ToString("yyyy-MM-01 08:30:00"));
            searchEndDate = startDate.AddDays(1);
        }
        else
            return result;

        //获取生产数据
        var manuSfcSummary = await _manuSfcSummaryRepository.GetManuSfcSummaryDateAsync(new()
        {
            StartTime = startDate,
            EndTime = endDate,
            DateType = queryDto.DateType
        });

        //获取计划开机时长
        var equipmentTheoryEntities = await _equEquipmentTheoryRepository.GetListAsync(new() { });
        //获取停机时长
        var equAlarmEntities = await _equAlarmRepository.GetListAsync(new()
        {
            CreatedOnEnd = startDate,
            CreatedOnStart = endDate
        });

        while (true)
        {
            var leftCount = queryDto.DateType switch
            {
                DateTypeEnum.Day => 10,
                DateTypeEnum.Month => 7,
                _ => 0
            };

            //计算停机时长
            var equAlarmEntitys = equAlarmEntities.Where(a => a.CreatedOn >= startDate && a.CreatedOn < searchEndDate);
            var DurationTime = GetEquAlarmDurationTimeSum(equAlarmEntitys);

            var equPlanWorkTime = equipmentTheoryEntities.Sum(a => a.TheoryOnTime) ?? 0;

            var outputQty = manuSfcSummary.Where(a => a.EndTime == startDate.ToString("yyyy-MM-dd").Substring(leftCount))
                .FirstOrDefault()?.OutputQty ?? 0;
            var qualifiedQty = manuSfcSummary.Where(a => a.EndTime == startDate.ToString("yyyy-MM-dd").Substring(leftCount))
                .FirstOrDefault()?.QualifiedQty ?? 0;

            result.Add(new()
            {
                EndTime = queryDto.DateType switch { DateTypeEnum.Day => startDate.Day.ToString(), DateTypeEnum.Month => startDate.Month.ToString() + "月", _ => "" },
                OEE = ((equPlanWorkTime - DurationTime) / (equPlanWorkTime == 0 ? 1 : equPlanWorkTime)) * qualifiedQty / (outputQty == 0 ? 1 : outputQty)
            });

            if (queryDto.DateType == DateTypeEnum.Day)
            {
                if (startDate.AddDays(1) == endDate) break;
                else
                {
                    startDate = startDate.AddDays(1);
                    searchEndDate = startDate.AddDays(1);
                }
            }
            if (queryDto.DateType == DateTypeEnum.Month)
            {
                if (startDate.AddMonths(1) == endDate) break;
                else
                {
                    startDate = startDate.AddMonths(1);
                    searchEndDate = startDate.AddMonths(1);
                };
            }
        }


        return result;
    }

    #endregion


    #endregion


    #region Common

    /// <summary>
    /// 计算停机时长
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static decimal GetEquAlarmDurationTimeSum(IEnumerable<EquAlarmEntity> data)
    {
        EquAlarmDurationTimeDto result = new();

        List<EquAlarmComputedDto> list = new List<EquAlarmComputedDto>();
        foreach (var item in data)
        {
            EquAlarmComputedDto newitem = new EquAlarmComputedDto();
            newitem.Status = item.Status;
            newitem.EquipmentId = item.EquipmentId;

            var exitsItem = list.FirstOrDefault(a => a.EquipmentId == item.EquipmentId);

            if (exitsItem != null)
            {
                //触发
                if (item.Status == Core.Enums.EquipmentAlarmStatusEnum.Trigger)
                {
                    if (item.Status == exitsItem.Status) continue;
                    else
                    {
                        exitsItem.Status = item.Status;
                        exitsItem.BeginTime = item.LocalTime;
                        exitsItem.EndTime = null;
                    }
                }
                else
                {
                    if (item.Status == exitsItem.Status) continue;
                    else
                    {
                        if (exitsItem.BeginTime != null && exitsItem.EndTime != null)
                        {
                            exitsItem.DurationTime += exitsItem.EndTime.GetValueOrDefault().Subtract(exitsItem.BeginTime.GetValueOrDefault()).Milliseconds;
                            exitsItem.BeginTime = null;
                            exitsItem.EndTime = null;
                            exitsItem.Status = null;
                        }
                    }
                }
            }
            else
            {

                if (newitem.Status == Core.Enums.EquipmentAlarmStatusEnum.Trigger)
                    newitem.BeginTime = item.LocalTime;
                else
                    newitem.EndTime = item.LocalTime;

                if (newitem.BeginTime != null && newitem.EndTime != null)
                    newitem.DurationTime = newitem.EndTime.GetValueOrDefault().Subtract(newitem.BeginTime.GetValueOrDefault()).Milliseconds;
            }

            list.Add(newitem);

        }

        var durationTime = list.Sum(a => a.DurationTime) ?? 0;

        return durationTime;
    }

    #endregion
}