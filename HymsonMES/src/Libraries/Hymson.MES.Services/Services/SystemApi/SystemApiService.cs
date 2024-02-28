using Hymson.Authentication;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.SystemApi;
using Hymson.MES.Services.Services.Report.EquAlarmReport;
using Hymson.Utils;
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

    private readonly IProcProcedureRepository _procProcedureRepository;
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


    public SystemApiService(ICurrentUser currentUser,
        IProcProcedureRepository procProcedureRepository,
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
        IEquAlarmReportService equAlarmReportService
        )
    {
        _currentUser = currentUser;
        _procProcedureRepository = procProcedureRepository;
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
    }


    /// <summary>
    /// 首页——OEE趋势图
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<OEETrendChartViewDto>> GetOEETrendChartAsync(OEETrendChartQueryDto queryDto)
    {
        List<OEETrendChartViewDto> result = new();

        //获取每个段对应的设备 OP20，OP220，OP170，分别对应电芯，模组，Pack
        //计算每个段的产出只统计尾工序
        var procedureCodes = new string[] { "OP20", "OP220", "OP170" };
        var ProcedureEntities = await _procProcedureRepository.GetByCodesAsync(procedureCodes, 123456);

        //根据工序获取资源
        var resourceTypeIds = ProcedureEntities.Select(a => a.ResourceTypeId.GetValueOrDefault());
        var procResourceEntities = await _procResourceRepository.GetByResTypeIdsAsync(new() { IdsArr = resourceTypeIds.ToArray() });

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
        //获取当天生产数据
        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new()
        {
            WorkOrderId = queryDto.OrderId,
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
                    OEE = (equipmenttheory?.TheoryOnTime - (equAlarm?.DurationTime / 3600)) / equipmenttheory?.TheoryOnTime
                    * quanlifiedQty / outputQty
                    * outputQty / equipmenttheory?.TheoryOnTime,
                    Type = item switch
                    {
                        "OP20" => "电芯段",
                        "OP220" => "模组段",
                        "OP170" => "Pack段",
                        _ => ""
                    }
                };
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
    public async Task<PlanWorkOrderInfoViewDto> GetPlanWorkOrderInfoAsync(PlanWorkOrderInfoQueryDto queryDto)
    {
        PlanWorkOrderInfoViewDto result = new();

        //获取工单信息
        var planWorkOrderEntity = await _planWorkOrderRepository.GetOneAsync(new()
        {
            OrderCode = queryDto.OrderCode,
            StatusList = new() { PlanWorkOrderStatusEnum.InProduction }
        });
        if (planWorkOrderEntity == null) return result;

        //获取工艺路线
        var procProcessRouteEntity = await _processRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId);
        //获取工艺路线尾工序
        var procProcessRouteDetialEntities = await _processRouteDetailLinkRepository.GetListAsync(new()
        {
            ProcessRouteId = procProcessRouteEntity.Id
        });
        var lastProcedureId = procProcessRouteDetialEntities.OrderBy(a => a.SerialNo).FirstOrDefault()?.ProcessRouteId;

        //获取电芯段工序Id
        var cellProcedureId = await _procProcedureRepository.GetByCodeAsync("OP020", 123456);

        //线体
        var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByIdAsync(planWorkOrderEntity.WorkCenterId.GetValueOrDefault());

        //产品
        var materialEntity = await _materialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);

        var manuSfcSummaryView = await _manuSfcSummaryRepository.GetManuSfcSummaryViewAsync(new()
        {
            WorkOrderId = inteWorkCenterEntity.Id,
            ProcedureId = lastProcedureId
        });

        var cellManuSfcSummaryView = await _manuSfcSummaryRepository.GetManuSfcSummaryViewAsync(new()
        {
            WorkOrderId = inteWorkCenterEntity.Id,
            ProcedureId = cellProcedureId.Id
        });

        //产出数量
        result.Qty = planWorkOrderEntity.Qty;
        //不良数
        result.UnqualifiedQty = manuSfcSummaryView.UnqualifiedQty;
        //完成率
        result.CompletionRate = Math.Round(result.Qty.GetValueOrDefault() / planWorkOrderEntity.Qty, 0);
        //完工数量
        result.Completionty = manuSfcSummaryView.OutputQty;
        result.ClassType = HymsonClock.Now().Hour >= 8 && HymsonClock.Now().Hour <= 20 ? DetailClassTypeEnum.Morning : DetailClassTypeEnum.Night;
        result.OrderCode = planWorkOrderEntity.OrderCode;
        result.ProcessRouteId = procProcessRouteEntity.Id;
        result.ProcessRouteName = procProcessRouteEntity.Name;
        result.ProductId = materialEntity.Id;
        result.ProductName = materialEntity.MaterialName;
        result.WorkCenterId = inteWorkCenterEntity.Id;
        result.WorkCenterName = inteWorkCenterEntity.Name;
        result.QualifiedRate = Math.Round(manuSfcSummaryView.QualifiedQty.GetValueOrDefault() / manuSfcSummaryView.OutputQty.GetValueOrDefault(), 2);
        result.PlanAchievementRate = Math.Round(result.Qty.GetValueOrDefault() / planWorkOrderEntity.Qty, 0);
        result.CellQty = cellManuSfcSummaryView.OutputQty.GetValueOrDefault();

        return result;
    }
}