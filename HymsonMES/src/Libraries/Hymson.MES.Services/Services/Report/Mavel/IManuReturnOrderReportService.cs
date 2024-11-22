using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.NioPushCollection;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（工单退料记录详情）
    /// </summary>
    public interface IManuReturnOrderReportService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ReportReturnOrderResultDto>> GetPagedListAsync(ReportReturnOrderQueryDto pagedQueryDto);

        /// <summary>
        /// 导出信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<NioPushCollectionExportResultDto> ExprotAsync(ReportReturnOrderQueryDto param);


    }
}