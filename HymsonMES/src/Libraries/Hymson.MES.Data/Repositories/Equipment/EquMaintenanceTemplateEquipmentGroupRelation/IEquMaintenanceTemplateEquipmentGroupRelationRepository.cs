/*
 *creator: Karl
 *
 *describe: 设备点检模板与设备组关系仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:22
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquMaintenanceTemplateEquipmentGroupRelation
{
    /// <summary>
    /// 设备点检模板与设备组关系仓储接口
    /// </summary>
    public interface IEquMaintenanceTemplateEquipmentGroupRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquMaintenanceTemplateEquipmentGroupRelationEntity EquMaintenanceTemplateEquipmentGroupRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquMaintenanceTemplateEquipmentGroupRelationEntity> EquMaintenanceTemplateEquipmentGroupRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquMaintenanceTemplateEquipmentGroupRelationEntity EquMaintenanceTemplateEquipmentGroupRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquMaintenanceTemplateEquipmentGroupRelationEntity> EquMaintenanceTemplateEquipmentGroupRelationEntitys);

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
        Task<int> DeletesByMaintenanceTemplateIdsAsync(IEnumerable<long> MaintenanceTemplateIds);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        Task<int> DeletesByTemplateIdAndGroupIdsAsync(GetByTemplateIdAndGroupIdQuery MaintenanceTemplateIds);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquMaintenanceTemplateEquipmentGroupRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据IDs批量获取数据(组合)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        Task<IEnumerable<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetByTemplateIdAndGroupIdAsync(GetByTemplateIdAndGroupIdQuery param);


        /// <summary>
        /// 根据GroupId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        Task<IEnumerable<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetByGroupIdAsync(IEnumerable<long> groupIdSql);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetEquMaintenanceTemplateEquipmentGroupRelationEntitiesAsync(EquMaintenanceTemplateEquipmentGroupRelationQuery EquMaintenanceTemplateEquipmentGroupRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetPagedInfoAsync(EquMaintenanceTemplateEquipmentGroupRelationPagedQuery EquMaintenanceTemplateEquipmentGroupRelationPagedQuery);
        #endregion
    }
}
