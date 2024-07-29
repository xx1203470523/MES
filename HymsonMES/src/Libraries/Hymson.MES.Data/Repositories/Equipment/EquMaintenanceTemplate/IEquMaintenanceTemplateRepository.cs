/*
 *creator: Karl
 *
 *describe: 设备保养模板仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquMaintenanceTemplate
{
    /// <summary>
    /// 设备保养模板仓储接口
    /// </summary>
    public interface IEquMaintenanceTemplateRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquMaintenanceTemplateEntity EquMaintenanceTemplateEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquMaintenanceTemplateEntity> EquMaintenanceTemplateEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquMaintenanceTemplateEntity EquMaintenanceTemplateEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="EquMaintenanceTemplateEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquMaintenanceTemplateEntity> EquMaintenanceTemplateEntitys);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquMaintenanceTemplateEntity> GetByIdAsync(long id);


        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquMaintenanceTemplateEntity> GetByCodeAsync(EquMaintenanceTemplateQuery param);


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenanceTemplateEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="EquMaintenanceTemplateQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenanceTemplateEntity>> GetEquMaintenanceTemplateEntitiesAsync(EquMaintenanceTemplateQuery EquMaintenanceTemplateQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenanceTemplatePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquMaintenanceTemplateEntity>> GetPagedInfoAsync(EquMaintenanceTemplatePagedQuery EquMaintenanceTemplatePagedQuery);
        #endregion
    }
}
