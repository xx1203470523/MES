using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（领料记录详情）
    /// </summary>
    public interface IManuBoxReportService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ReportBoxResultDto>> GetPagedListAsync(ReportBoxQueryDto pagedQueryDto);

    }
}