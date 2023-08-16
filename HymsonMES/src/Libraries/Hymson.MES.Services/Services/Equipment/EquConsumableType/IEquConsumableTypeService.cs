using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquSparePartType
{
    /// <summary>
    /// 业务接口层（工装类型）
    /// </summary>
    public interface IEquConsumableTypeService
    {
        /// <summary>
        /// 添加（工装类型）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquConsumableTypeSaveDto createDto);

        /// <summary>
        /// 修改（工装类型）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquConsumableTypeSaveDto modifyDto);

        /// <summary>
        /// 删除（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除（工装类型）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 分页查询列表（工装类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquConsumableTypeDto>> GetPagedListAsync(EquConsumableTypePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询详情（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquConsumableTypeDto> GetDetailAsync(long id);
    }
}
