using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquSparePartType
{
    /// <summary>
    /// 业务接口层（备件类型） 
    /// </summary>
    public interface IEquSparePartTypeService
    {
        /// <summary>
        /// 添加（备件类型）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquSparePartTypeSaveDto createDto);

        /// <summary>
        /// 修改（备件类型）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquSparePartTypeSaveDto modifyDto);

        /// <summary>
        /// 删除（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除（备件类型）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 分页查询列表（备件类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartTypeDto>> GetPagedListAsync(EquSparePartTypePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询详情（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSparePartTypeDto> GetDetailAsync(long id);
    }
}
