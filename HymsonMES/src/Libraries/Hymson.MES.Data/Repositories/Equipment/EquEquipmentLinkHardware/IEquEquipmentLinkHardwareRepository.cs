using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkHardware.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquEquipmentLinkHardwareRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentLinkHardwareEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<EquEquipmentLinkHardwareEntity> entitys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentLinkHardwareEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<EquEquipmentLinkHardwareEntity> entitys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(IEnumerable<long> idsArr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long equipmentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] equipmentIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentLinkHardwareEntity> GetByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hardwareCode"></param>
        /// <param name="hardwareType"></param>
        /// <returns></returns>
        Task<EquEquipmentLinkHardwareEntity> GetByHardwareCodeAsync(string hardwareCode, string hardwareType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentLinkHardwareEntity>> GetListAsync(long equipmentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentLinkHardwareEntity>> GetEntitiesAsync(EquEquipmentLinkHardwareQuery query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentLinkHardwareEntity>> GetPagedListAsync(EquEquipmentLinkHardwarePagedQuery pagedQuery);
    }
}
