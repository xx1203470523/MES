/*
 *creator: Karl
 *
 *describe: 设备点检计划与设备关系仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-20 03:51:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquMaintenancePlanEquipmentRelation
{
    /// <summary>
    /// 设备点检计划与设备关系仓储接口
    /// </summary>
    public interface IEquMaintenancePlanEquipmentRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquMaintenancePlanEquipmentRelationEntity EquMaintenancePlanEquipmentRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquMaintenancePlanEquipmentRelationEntity> EquMaintenancePlanEquipmentRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquMaintenancePlanEquipmentRelationEntity EquMaintenancePlanEquipmentRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquMaintenancePlanEquipmentRelationEntity> EquMaintenancePlanEquipmentRelationEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> PhysicalDeletesAsync(IEnumerable<long> MaintenancePlanIds);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquMaintenancePlanEquipmentRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenancePlanEquipmentRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据MaintenancePlanId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenancePlanEquipmentRelationEntity>> GetByMaintenancePlanIdsAsync(long MaintenancePlanId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenancePlanEquipmentRelationEntity>> GetEquMaintenancePlanEquipmentRelationEntitiesAsync(EquMaintenancePlanEquipmentRelationQuery EquMaintenancePlanEquipmentRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquMaintenancePlanEquipmentRelationEntity>> GetPagedInfoAsync(EquMaintenancePlanEquipmentRelationPagedQuery EquMaintenancePlanEquipmentRelationPagedQuery);
        #endregion
    }
}
