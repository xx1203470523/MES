using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquipmentUnit.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquipmentUnitRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquipmentUnitEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquipmentUnitEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long[] idsArr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquipmentUnitEntity> GetByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquipmentUnitEntity>> GetEntitiesAsync(EquipmentUnitQuery equipmentUnitQuery);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquipmentUnitEntity>> GetPagedInfoAsync(EquipmentUnitPagedQuery equipmentUnitPagedQuery);
    }
}
