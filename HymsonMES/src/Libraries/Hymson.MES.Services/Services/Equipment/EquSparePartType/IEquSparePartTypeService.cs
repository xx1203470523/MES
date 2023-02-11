using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquSparePartType
{
    /// <summary>
    /// 备件类型 service接口
    /// </summary>
    public interface IEquSparePartTypeService
    {
        /// <summary>
        /// 添加（备件类型）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task CreateEquSparePartTypeAsync(EquSparePartTypeCreateDto createDto);

        /// <summary>
        /// 修改（备件类型）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task ModifyEquSparePartTypeAsync(EquSparePartTypeModifyDto modifyDto);

        /// <summary>
        /// 删除（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquSparePartTypeAsync(long id);

        /// <summary>
        /// 批量删除（备件类型）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesEquSparePartTypeAsync(long[] idsArr);

        /// <summary>
        /// 分页查询列表（备件类型）
        /// </summary>
        /// <param name="equSparePartTypePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartTypeDto>> GetPageListAsync(EquSparePartTypePagedQueryDto equSparePartTypePagedQueryDto);

        /// <summary>
        /// 查询详情（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSparePartTypeDto> QueryEquSparePartTypeByIdAsync(long id);
    }
}
