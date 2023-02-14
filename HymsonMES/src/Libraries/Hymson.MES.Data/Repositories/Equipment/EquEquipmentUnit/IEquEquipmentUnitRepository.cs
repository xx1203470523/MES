using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquEquipmentUnitRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentUnitEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentUnitEntity equipmentUnitEntity);

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
        Task<EquEquipmentUnitEntity> GetByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentUnitEntity>> GetPagedListAsync(EquEquipmentUnitPagedQuery pagedQuery);
        
    }
}
