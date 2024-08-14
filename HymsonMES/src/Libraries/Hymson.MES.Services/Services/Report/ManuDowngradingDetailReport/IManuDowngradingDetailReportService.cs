using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 服务接口（降级品明细报表）
    /// </summary>
    public interface IManuDowngradingDetailReportService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuDowngradingDetailReportDto>> GetPagedListAsync(ManuDowngradingDetailReportPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 导出查询列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ManuDowngradingDetailExportResultDto> ExprotListAsync(ManuDowngradingDetailReportPagedQueryDto param);
    }
}