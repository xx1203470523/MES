using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
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
        /// 判断是否存在（编码）
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        Task<bool> IsCodeExistsAsync(string equipmentCode);

        /// <summary>
        /// 根据名称读取数据
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
         Task<EquEquipmentGroupEntity> GetByNameAsync(string equipmentCode);

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
        /// 分页查询
        /// </summary>
        /// <param name="equEquipmentGroupPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentGroupEntity>> GetPagedListAsync(EquEquipmentGroupPagedQuery equEquipmentGroupPagedQuery);
    }
}
