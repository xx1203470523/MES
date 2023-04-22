using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using MySql.Data.MySqlClient;

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
        /// 批量修改设备的设备组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateEquipmentGroupIdAsync(UpdateEquipmentGroupIdCommand command);

        /// <summary>
        /// 清空设备的设备组
        /// </summary>
        /// <param name="equipmentGroupId"></param>
        /// <returns></returns>
        Task<int> ClearEquipmentGroupIdAsync(long equipmentGroupId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentGroupId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetByGroupIdAsync(long equipmentGroupId);

        /// <summary>
        /// 根据设备编码查询实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetByEquipmentCodeAsync(EntityByCodeQuery query);

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
