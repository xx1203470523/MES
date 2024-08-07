using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhWareHouse;
using Hymson.MES.Core.Domain.WhWarehouseRegion;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.WhWareHouse;

namespace Hymson.MES.Services.Services.WhWareHouse
{
    /// <summary>
    /// 服务接口（仓库）
    /// </summary>
    public interface IWhWarehouseService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(WhWarehouseSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(WhWarehouseModifyDto modifyDto);

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
        Task<WhWarehouseDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhWarehouseDto>> GetPagedListAsync(WhWarehousePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhWarehouseDto>> GetPagedListCopyAsync(WhWarehousePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询所有仓库信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> GetWarehouseListAsync();
    }
}