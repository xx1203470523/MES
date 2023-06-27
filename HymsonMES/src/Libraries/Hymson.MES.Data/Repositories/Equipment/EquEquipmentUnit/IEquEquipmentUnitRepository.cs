using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquEquipmentUnitRepository
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentUnitEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentUnitEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentUnitEntity> GetByIdAsync(long id);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentUnitEntity>> GetPagedListAsync(EquEquipmentUnitPagedQuery pagedQuery);
        
    }
}
