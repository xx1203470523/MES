using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhWarehouseShelf;

namespace Hymson.MES.Services.Services.WhWarehouseShelf
{
    /// <summary>
    /// 服务接口（货架）
    /// </summary>
    public interface IWhWarehouseShelfService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(WhWarehouseShelfSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(WhWarehouseShelfSaveDto saveDto);

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
        Task<WhWarehouseShelfDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhWarehouseShelfDto>> GetPagedListAsync(WhWarehouseShelfPagedQueryDto pagedQueryDto);

    }
}