using Hymson.Authentication;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
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
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.SystemAp;
using Hymson.MES.Services.Dtos.SystemApi;
using Hymson.MES.Services.Services.Report.EquAlarmReport;
using Hymson.Utils;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.SystemApi;

public class SystemApiService : ISystemApiService
{
    private readonly ICurrentUser _currentUser;

    private readonly IEquStatusRepository _equStatusRepository;
    private readonly IProcProcedureRepository _procProcedureRepository;
    private readonly IProcProcedurePlanRepository _procProcedurePlanRepository;
    private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
    private readonly IProcProcessRouteRepository _processRouteRepository;
    private readonly IProcProcessRouteDetailLinkRepository _processRouteDetailLinkRepository;
    private readonly IProcMaterialRepository _materialRepository;
    private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

    private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
    private readonly IProcResourceRepository _procResourceRepository;
    private readonly IEquEquipmentRepository _equEquipmentRepository;
    private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
    private readonly IEquEquipmentTheoryRepository _equEquipmentTheoryRepository;
    private readonly IEquAlarmReportService _equAlarmReportService;

    //不良相关
    private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;
    private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;
    private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;


    public SystemApiService(ICurrentUser currentUser,
        IEquStatusRepository equipmentStatusRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcProcedurePlanRepository procProcedurePlanRepository,
        IPlanWorkOrderRepository planWorkOrderRepository,
        IProcProcessRouteRepository processRouteRepository,
        IProcProcessRouteDetailLinkRepository processRouteDetailLinkRepository,
        IProcMaterialRepository materialRepository,
        IInteWorkCenterRepository inteWorkCenterRepository,
        IManuSfcSummaryRepository manuSfcSummaryRepository,
        IProcResourceRepository procResourceRepository,
        IEquEquipmentRepository equEquipmentRepository,
        IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
        IEquEquipmentTheoryRepository equEquipmentTheoryRepository,
        IEquAlarmReportService equAlarmReportService,
        IManuSfcStepNgRepository manuSfcStepNgRepository,
        IManuProductBadRecordRepository manuProductBadRecordRepository,
        IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository
        )
    {
        _currentUser = currentUser;
        _equStatusRepository = equipmentStatusRepository;
        _procProcedureRepository = procProcedureRepository;
        _procProcedurePlanRepository = procProcedurePlanRepository;
        _planWorkOrderRepository = planWorkOrderRepository;
        _processRouteRepository = processRouteRepository;
        _processRouteDetailLinkRepository = processRouteDetailLinkRepository;
        _materialRepository = materialRepository;
        _inteWorkCenterRepository = inteWorkCenterRepository;
        _manuSfcSummaryRepository = manuSfcSummaryRepository;
        _procResourceRepository = procResourceRepository;
        _equEquipmentRepository = equEquipmentRepository;
        _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
        _equEquipmentTheoryRepository = equEquipmentTheoryRepository;
        _equAlarmReportService = equAlarmReportService;
        _manuSfcStepNgRepository = manuSfcStepNgRepository;
        _manuProductBadRecordRepository = manuProductBadRecordRepository;
        _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
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
            endDate = DateTime.Parse(endDate.AddDays(-1).ToString("yyyy-MM-dd 08:30:00"));
        }
        else if (HymsonClock.Now() > endDate)
        {
            startDate = DateTime.Parse(startDate.AddDays(1).ToString("yyyy-MM-dd 18:30:00"));
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
                var equipmenttheory = equipmentTheoryEntities.FirstOrDefault(a => a.EquipmentId == equipment?.Id);

                //停机时间
                var equAlarm = equAlarmDurationTimeEntities.FirstOrDefault(a => a.EquipmentId == equipment?.Id);

                var outputQty = summary.Sum(a => a.Qty);
                var quanlifiedQty = summary.Sum(a => a.QualityStatus);

                OEETrendChartViewDto newitem = new()
                {
                    EndTime = startDate.ToString("HH:30") + "-" + startDate.AddHours(2).ToString("HH:30"),
                    OEE = ((equipmenttheory?.TheoryOnTime - (equAlarm?.DurationTime / 3600)) / equipmenttheory?.TheoryOnTime
                    * quanlifiedQty / outputQty
                    * outputQty / equipmenttheory?.TheoryOnTime) ?? 0,
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

            startDate = startDate.AddHours(2);
            if (startDate.AddHours(2) == endDate) break;
        }


        return result;
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

        var procedureIds = ProcedureEntities.Select(a => a.Id).ToArray();
        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            StartTime = startDate,
            EndTime = endDate,
            ProcedureIds = procedureIds,
            ProductIds = productIds
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
        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            StartTime = startDate,
            EndTime = endDate,
            ProcedureIds = procedureIds,
            ProductIds = productIds
        });


        while (true)
        {
            foreach (var item in procedureCodes)
            {
                var procedure = ProcedureEntities.Where(a => a.Code == item).FirstOrDefault();
                var summary = manuSfcSummaryEntities.Where(a => a.ProcedureId == procedure?.Id
                && endDate >= startDate && endDate <= startDate.AddDays(1));

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
                    OneQualifiedRate = oneQuanlifiedQty / outputQty
                };
            }


            startDate = startDate.AddDays(1);
            if (startDate.AddDays(1) == endDate) break;
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
            CreatedOnEnd = endDate
        });

        List<QualUnqualifiedCodeEntity> unQualifiedList = new();
        var unQualifiedCodes = manuSfcStepNgEntities.Select(a => a.UnqualifiedCode);
        var unQualifiedIds = manuProductBadRecordEntities.Select(a => a.UnqualifiedId);
        var unQualifiedEntities = await _qualUnqualifiedCodeRepository.GetListAsync(new() { Ids = unQualifiedIds });
        unQualifiedList.AddRange(unQualifiedEntities);
        unQualifiedEntities = await _qualUnqualifiedCodeRepository.GetListAsync(new() { UnqualifiedCodes = unQualifiedCodes });
        unQualifiedList.AddRange(unQualifiedEntities);

        foreach (var item in manuSfcStepNgEntities)
        {
            var unQualifiedEntity = unQualifiedEntities.FirstOrDefault(a => a.UnqualifiedCode == item.UnqualifiedCode);

            var unQualifiedItem = result.FirstOrDefault(a => a.UnQualifiedName == unQualifiedEntity?.UnqualifiedCodeName);

            if (unQualifiedItem != null) unQualifiedItem.UnQualifiedQty += 1;
            else result.Add(new() { UnQualifiedName = unQualifiedEntity?.UnqualifiedCodeName ?? "", UnQualifiedQty = 1 });
        }

        foreach (var item in manuProductBadRecordEntities)
        {
            var unQualifiedEntity = unQualifiedEntities.FirstOrDefault(a => a.Id == item.UnqualifiedId);

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

        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            WorkOrderIds = planWorkOrderIds,
            ProcedureIds = procedureIds.ToArray()
        });

        if (queryDto.Type != null)
        {
            procedureCodes = procedureCodes.Where(a=>a == procedureCodes[(int)queryDto.Type]).ToArray();
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

            startDate = startDate.AddDays(1);
            if (startDate.AddDays(1) == endDate) break;
        }

        return result;
    }

    #endregion

    #region 设备实时状态

    /// <summary>
    /// 查询设备运行状态
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<EquipmentStatusDto>> GetEquipmentStatusAsync()
    {
        List<EquipmentStatusDto> result = new();




        return result;
    }

    #endregion

    #endregion

}