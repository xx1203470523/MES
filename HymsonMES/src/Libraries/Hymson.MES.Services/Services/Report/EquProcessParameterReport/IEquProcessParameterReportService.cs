using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 服务接口（设备过程参数报表）
    /// </summary>
    public interface IEquProcessParameterReportService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquProcessParameterReportDto>> GetPagedListAsync(EquProcessParameterReportPagedQueryDto pagedQueryDto);

    }
}