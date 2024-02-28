using Hymson.MES.Services.Dtos.SystemApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.SystemApi;

public interface ISystemApiService
{
    /// <summary>
    /// 首页-工单基本信息
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<PlanWorkOrderInfoViewDto> GetPlanWorkOrderInfoAsync(PlanWorkOrderInfoQueryDto queryDto);

    /// <summary>
    /// 首页-OEE趋势图
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<OEETrendChartViewDto>> GetOEETrendChartAsync(OEETrendChartQueryDto queryDto);


}
