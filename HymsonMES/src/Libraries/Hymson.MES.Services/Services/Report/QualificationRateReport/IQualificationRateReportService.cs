using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.QualificationRateReport;

namespace Hymson.MES.Services.Services.QualificationRateReport
{
    /// <summary>
    /// 服务接口（合格率报表）
    /// </summary>
    public interface IQualificationRateReportService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualificationRateReportDto>> GetPagedListAsync(QualificationRateReportPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取所有工序
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> GetProcdureListAsync();

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<ExportResultDto> ExportExcelAsync(QualificationRateReportPagedQueryDto queryDto);
    }
}