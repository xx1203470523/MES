using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.EquHeartbeatReport
{
    public interface IEquHeartbeatReportService
    {
        /// <summary>
        /// 根据查询条件获取设备心跳状态报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquHeartbeatReportViewDto>> GetEquHeartbeatReportPageListAsync(EquHeartbeatReportPagedQueryDto pageQuery);
        /// <summary>
        /// 导出设备心跳记录
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<ExportResultDto> EquHeartbeatReportExportAsync(EquHeartbeatReportPagedQueryDto pageQuery);
    }
}
