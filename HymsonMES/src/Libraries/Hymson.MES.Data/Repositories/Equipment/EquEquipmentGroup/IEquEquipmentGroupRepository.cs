using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
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
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentGroupEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentGroupEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentGroupEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentGroupEntity>> GetByCodeOrNameAsync(EntityByCodeQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentGroupEntity>> GetPagedListAsync(EquEquipmentGroupPagedQuery pagedQuery);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="equipmentGroupQuery"></param>
        /// <returns></returns>
         Task<IEnumerable<EquEquipmentGroupEntity>> GetEntitiesAsync(EquEquipmentGroupQuery equipmentGroupQuery);
    }
}
