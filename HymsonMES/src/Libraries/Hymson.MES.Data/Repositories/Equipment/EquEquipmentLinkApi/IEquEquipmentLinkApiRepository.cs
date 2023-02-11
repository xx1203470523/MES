using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquEquipmentLinkApiRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentLinkApiEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<EquEquipmentLinkApiEntity> entitys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentLinkApiEntity equipmentUnitEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<EquEquipmentLinkApiEntity> entitys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(IEnumerable<long> idsArr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(long[] equipmentIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentLinkApiEntity> GetByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        Task<EquEquipmentLinkApiEntity> GetByEquipmentIdAsync(long equipmentId, string apiType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentLinkApiEntity>> GetListAsync(long equipmentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentLinkApiEntity>> GetEntitiesAsync(EquEquipmentLinkApiQuery query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentLinkApiEntity>> GetPagedListAsync(EquEquipmentLinkApiPagedQuery pagedQuery);
    }
}
