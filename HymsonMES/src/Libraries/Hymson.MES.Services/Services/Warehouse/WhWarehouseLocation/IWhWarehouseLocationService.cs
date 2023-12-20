using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhWarehouseLocation;

namespace Hymson.MES.Services.Services.WhWarehouseLocation
{
    /// <summary>
    /// 服务接口（库位）
    /// </summary>
    public interface IWhWarehouseLocationService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(WhWarehouseLocationSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(WhWarehouseLocationModifyDto modifyDto);

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
        Task<WhWarehouseLocationDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhWarehouseLocationDto>> GetPagedListAsync(WhWarehouseLocationPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<WhWarehouseLocationDto>> GetListAsync(WhWarehouseLocationQueryDto queryDto);

    }
}