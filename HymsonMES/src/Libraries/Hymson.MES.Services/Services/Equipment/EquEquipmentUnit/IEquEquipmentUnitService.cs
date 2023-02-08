using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquEquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquEquipmentUnitService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateEquipmentUnitAsync(EquEquipmentUnitCreateDto createDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyEquipmentUnitAsync(EquEquipmentUnitModifyDto modifyDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeleteEquipmentUnitAsync(long[] idsArr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentUnitDto>> GetPagedListAsync(EquEquipmentUnitPagedQueryDto pagedQueryDto);
    }
}
