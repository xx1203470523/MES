using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务接口（字段分配管理）
    /// </summary>
    public interface IInteBusinessFieldDistributeService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<long> CreateAsync(InteBusinessFieldDistributeSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(InteBusinessFieldDistributeSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteBusinessFieldDistributeDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteBusinessFieldDistributeDto>> GetPagedListAsync(InteBusinessFieldDistributePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据Id查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<BusinessFieldViewDto>> getBusinessFieldDistributeDetailsByIdAsync(long id);
    }
}