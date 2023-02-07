using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquipmentUnitService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateEquipmentUnitAsync(EquipmentUnitCreateDto createDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyEquipmentUnitAsync(EquipmentUnitModifyDto modifyDto);

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
        Task<PagedInfo<EquipmentUnitDto>> GetListAsync(EquipmentUnitPagedQueryDto pagedQueryDto);
    }
}
