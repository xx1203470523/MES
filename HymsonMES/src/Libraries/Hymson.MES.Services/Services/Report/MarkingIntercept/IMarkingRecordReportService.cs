using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Marking;

namespace Hymson.MES.Services.Services.Marking
{
    /// <summary>
    /// 服务接口（Marking拦截汇总表）
    /// </summary>
    public interface IMarkingRecordReportService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<MarkingRecordReportDto>> GetPagedListAsync(MarkingInterceptReportPagedQueryDto pagedQueryDto);

    }
}