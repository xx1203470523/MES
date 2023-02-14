using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup
{
    /// <summary>
    /// 设备组仓储接口
    /// </summary>
    public interface IEquEquipmentGroupRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentGroupEntity entity);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentGroupEntity entity);
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(long[] idsArr);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentGroupEntity> GetByIdAsync(long id);
        
        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equEquipmentGroupQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentGroupEntity>> GetEntitiesAsync(EquEquipmentGroupQuery equEquipmentGroupQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equEquipmentGroupPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentGroupEntity>> GetPagedListAsync(EquEquipmentGroupPagedQuery equEquipmentGroupPagedQuery);
    }
}
