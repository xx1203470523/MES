/*
 *creator: Karl
 *
 *describe: 设备保养计划仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-20 04:05:45
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquMaintenancePlan
{
    /// <summary>
    /// 设备保养计划仓储接口
    /// </summary>
    public interface IEquMaintenancePlanRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenancePlanEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquMaintenancePlanEntity EquMaintenancePlanEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenancePlanEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquMaintenancePlanEntity> EquMaintenancePlanEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenancePlanEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquMaintenancePlanEntity EquMaintenancePlanEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="EquMaintenancePlanEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquMaintenancePlanEntity> EquMaintenancePlanEntitys);

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
        Task<EquMaintenancePlanEntity> GetByIdAsync(long id);


        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        Task<EquMaintenancePlanEntity> GetByCodeAsync(EquMaintenancePlanQuery param);


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenancePlanEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="EquMaintenancePlanQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquMaintenancePlanEntity>> GetEquMaintenancePlanEntitiesAsync(EquMaintenancePlanQuery EquMaintenancePlanQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenancePlanPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquMaintenancePlanEntity>> GetPagedInfoAsync(EquMaintenancePlanPagedQuery EquMaintenancePlanPagedQuery);
        #endregion
    }
}
