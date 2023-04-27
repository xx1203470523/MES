/*
 *creator: Karl
 *
 *describe: 工单信息表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using Hymson.Infrastructure;
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
    /// 不良记录报表
    /// </summary>
    public interface IComUsageReportService
    {
        /// <summary>
        /// 根据查询条件获取车间作业控制报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<ComUsageReportViewDto>> GetComUsagePageListAsync(ComUsageReportPagedQueryDto param);

        /// <summary>
        /// 根据查询条件导出车间作业控制报表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ComUsageExportResultDto> ExprotComUsagePageListAsync(ComUsageReportPagedQueryDto param);
    }
}
