using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquEquipmentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(long[] idsArr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(string equipmentCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetByGroupIdAsync(long groupId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentView> GetViewByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetByEquipmentCodeAsync(string equipmentCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetBaseListAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetEntitiesAsync(EquEquipmentQuery equipmentQuery);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentEntity>> GetPagedListAsync(EquEquipmentPagedQuery equipmentPagedQuery);
    }
}
