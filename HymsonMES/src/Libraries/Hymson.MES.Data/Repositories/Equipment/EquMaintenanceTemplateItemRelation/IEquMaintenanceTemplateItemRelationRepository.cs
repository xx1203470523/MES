/*
 *creator: Karl
 *
 *describe: 设备点检模板与项目关系仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:39
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquMaintenanceTemplateItemRelation
{
    /// <summary>
    /// 设备点检模板与项目关系仓储接口
    /// </summary>
    public interface IEquMaintenanceTemplateItemRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquMaintenanceTemplateItemRelationEntity EquMaintenanceTemplateItemRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquMaintenanceTemplateItemRelationEntity> EquMaintenanceTemplateItemRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquMaintenanceTemplateItemRelationEntity EquMaintenanceTemplateItemRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquMaintenanceTemplateItemRelationEntity> EquMaintenanceTemplateItemRelationEntitys);

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
        Task<int> DeleteByMaintenanceTemplateIdsAsync(IEnumerable<long> MaintenanceTemplateIds);

        /// <summary> 
        /// 批量删除（物理删除）
        /// </summary>  
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesByTemplateIdAndItemIdsAsync(GetByTemplateIdAndItemIdQuery param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquMaintenanceTemplateItemRelationEntity> GetByIdAsync(long id);


        /// <summary>
        /// 根据ID获取数据（组合）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenanceTemplateItemRelationEntity>> GetByTemplateIdAndItemIdSqlAsync(GetByTemplateIdAndItemIdQuery param);


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenanceTemplateItemRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenanceTemplateItemRelationEntity>> GetEquMaintenanceTemplateItemRelationEntitiesAsync(EquMaintenanceTemplateItemRelationQuery EquMaintenanceTemplateItemRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquMaintenanceTemplateItemRelationEntity>> GetPagedInfoAsync(EquMaintenanceTemplateItemRelationPagedQuery EquMaintenanceTemplateItemRelationPagedQuery);
        #endregion
    }
}
