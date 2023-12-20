using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhWarehouseRegion;

namespace Hymson.MES.Services.Services.WhWarehouseRegion
{
    /// <summary>
    /// 服务接口（库区）
    /// </summary>
    public interface IWhWarehouseRegionService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(WhWarehouseRegionSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(WhWarehouseRegionModifyDto modifyDto);

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
        Task<WhWarehouseRegionDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhWarehouseRegionDto>> GetPagedListAsync(WhWarehouseRegionPagedQueryDto pagedQueryDto);

    }
}