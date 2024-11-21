using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.NioPushCollection;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（领料记录详情）
    /// </summary>
    public interface IManuRequistionOrderReportService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ReportRequistionOrderResultDto>> GetPagedListAsync(ReportRequistionOrderQueryDto pagedQueryDto);

        /// <summary>
        /// 查询仓库地址分组
        /// </summary>
        /// <returns></returns>
        Task<List<ManuRequistionOrderGroupDto>> GetWarehouseListAsync();

        /// <summary>
        /// 导出信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<NioPushCollectionExportResultDto> ExprotAsync(ReportRequistionOrderQueryDto param);

    }
}