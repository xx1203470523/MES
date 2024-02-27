using Hymson.Authentication;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.SystemApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.SystemApi;

public class SystemApiService : ISystemApiService
{
    private readonly ICurrentUser _currentUser;

    private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
    private readonly IProcProcessRouteRepository _processRouteRepository;
    private readonly IProcMaterialRepository _materialRepository;
    private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

    private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;


    public SystemApiService(ICurrentUser currentUser,
        IPlanWorkOrderRepository planWorkOrderRepository,
        IProcProcessRouteRepository processRouteRepository,
        IProcMaterialRepository materialRepository,
        IInteWorkCenterRepository inteWorkCenterRepository,
        IManuSfcSummaryRepository manuSfcSummaryRepository
        )
    {
        _currentUser = currentUser;
        _planWorkOrderRepository = planWorkOrderRepository;
        _processRouteRepository = processRouteRepository;
        _materialRepository = materialRepository;
        _inteWorkCenterRepository = inteWorkCenterRepository;
        _manuSfcSummaryRepository = manuSfcSummaryRepository;
    }


    public async Task<IEnumerable<OEETrendChartViewDto>> GetOEETrendChartAsync(OEETrendChartQueryDto queryDto)
    {
        List<OEETrendChartViewDto> result = new();


        await Task.CompletedTask;

        return result;
    }

    public async Task<IEnumerable<PlanWorkOrderInfoViewDto>> GetPlanWorkOrderInfoAsync(PlanWorkOrderInfoQueryDto queryDto)
    {
        List<PlanWorkOrderInfoViewDto> result = new();

        ////获取工单信息
        //var planWorkOrderEntities = await _planWorkOrderRepository.GetListAsync(new()
        //{
        //    OrderCode = queryDto.OrderCode,
        //    StatusList = new() { PlanWorkOrderStatusEnum.InProduction }
        //});

        ////获取工艺路线
        //var processRouteIds = planWorkOrderEntities.Select(a => a.ProcessRouteId);
        //var procProcessRouteEntities = await _processRouteRepository.GetByIdsAsync(processRouteIds.ToArray());
        ////获取工艺路线尾工序


        ////线体
        //var inteWorkCenterIds = planWorkOrderEntities.Select(a => a.WorkCenterId.GetValueOrDefault());
        //var inteWorkCenterEntities = await _inteWorkCenterRepository.GetByIdsAsync(inteWorkCenterIds.ToArray());

        ////产品
        //var materialIds = planWorkOrderEntities.Select(a => a.ProductId);
        //var materialEntities = await _materialRepository.GetByIdsAsync(materialIds);

        ////产出数量
        //var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new() { 
            
        //});

        ////不良数

        ////计算良率

        ////计算综合良率


        await Task.CompletedTask;

        return result;
    }
}
