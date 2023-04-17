using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码打印 service接口
    /// </summary>
    public interface IPlanSfcPrintService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task CreateAsync(PlanSfcPrintCreateDto createDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanSfcPrintDto>> GetPagedListAsync(PlanSfcPrintPagedQueryDto pagedQueryDto);

    }
}
