using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquConsumable
{
    /// <summary>
    /// 业务接口层（工装注册） 
    /// </summary>
    public interface IEquConsumableService
    {
        /// <summary>
        /// 添加（工装注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquConsumableCreateDto createDto);

        /// <summary>
        /// 修改（工装注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquConsumableModifyDto modifyDto);

        /// <summary>
        /// 删除（工装注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除（工装注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 分页查询列表（工装注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquConsumableDto>> GetPagedListAsync(EquConsumablePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询详情（工装注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquConsumableDto> GetDetailAsync(long id);
    }
}
