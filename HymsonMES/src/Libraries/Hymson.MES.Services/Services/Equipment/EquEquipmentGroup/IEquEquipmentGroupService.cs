using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.EquEquipmentGroup
{
    /// <summary>
    /// 设备组 service接口
    /// </summary>
    public interface IEquEquipmentGroupService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquEquipmentGroupCreateDto createDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquEquipmentGroupModifyDto modifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentGroupListDto>> GetPagedListAsync(EquEquipmentGroupPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentGroupDto> GetDetailAsync(EquEquipmentGroupQueryDto query);
    }
}
