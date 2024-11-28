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
    /// 不良记录报表
    /// </summary>
    public interface IBadRecordReportService
    {
        /// <summary>
        /// 根据查询条件获取不良报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductBadRecordReportViewDto>> GetPageListAsync(BadRecordReportDto param);

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<NioPushCollectionExportResultDto> ExprotAsync(BadRecordReportDto param);

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<NioPushCollectionExportResultDto> LogExprotAsync(ManuProductBadRecordLogReportPagedQueryDto param);

        /// <summary>
        /// 查询前十的不良记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<ManuProductBadRecordReportViewDto>> GetTopTenBadRecordAsync(BadRecordReportDto param);

        /// <summary>
        /// 根据查询条件获取不良日志报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductBadRecordLogReportViewDto>> GetLogPageListAsync(ManuProductBadRecordLogReportPagedQueryDto param);

        /// <summary>
        /// 查询不合格代码列表（不良报告日志）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductBadRecordLogReportResponseDto>> GetLogPageDetailListAsync(ManuProductBadRecordLogReportRequestDto request);

    }
}
