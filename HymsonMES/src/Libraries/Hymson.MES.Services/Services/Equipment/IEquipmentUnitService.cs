using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquipmentUnitService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquipmentUnitDto>> GetListAsync(EquipmentUnitPagedQueryDto equipmentUnitPagedQueryDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitDto"></param>
        /// <returns></returns>
        Task CreateEquipmentUnitAsync(EquipmentUnitDto equipmentUnitDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitDto"></param>
        /// <returns></returns>
        Task ModifyEquipmentUnitAsync(EquipmentUnitDto equipmentUnitDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquipmentUnitAsync(long id);
    }
}
