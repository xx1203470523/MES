/*
 *creator: Karl
 *
 *describe: 工单信息表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 工单报告
    /// </summary>
    public interface IWorkOrderControlReportService
    {

        /// <summary>
        /// 根据查询条件获取工单报告分页数据
        /// 优化: 不模糊查询，且通过关联ID查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<WorkOrderControlReportViewDto>> GetWorkOrderControlPageListAsync(WorkOrderControlReportOptimizePagedQueryDto param);

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<NioPushCollectionExportResultDto> ExprotAsync(WorkOrderControlReportOptimizePagedQueryDto pagedQueryDto);

    }
}
