/*
 *creator: Karl
 *
 *describe: 工单激活（物理删除）仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-12 11:14:23
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活（物理删除）仓储接口
    /// </summary>
    public interface IPlanWorkOrderBindRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderBindEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(PlanWorkOrderBindEntity planWorkOrderBindEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderBindEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<PlanWorkOrderBindEntity> planWorkOrderBindEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderBindEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanWorkOrderBindEntity planWorkOrderBindEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="planWorkOrderBindEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<PlanWorkOrderBindEntity> planWorkOrderBindEntitys);

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
        Task<PlanWorkOrderBindEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderBindEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="planWorkOrderBindQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderBindEntity>> GetPlanWorkOrderBindEntitiesAsync(PlanWorkOrderBindQuery planWorkOrderBindQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderBindPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderBindEntity>> GetPagedInfoAsync(PlanWorkOrderBindPagedQuery planWorkOrderBindPagedQuery);
        #endregion

        /// <summary>
        /// 根据资源ID批量删除（真删除）
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<int> DeletesTrueByResourceIdAsync(long resourceId);

        /// <summary>
        /// 根据资源ID和工单Ids批量删除（真删除）
        /// </summary>
        /// <param name="comm"></param>
        /// <returns></returns>
        Task<int> DeletesTrueByResourceIdAndWorkOrderIdsAsync(DeleteplanWorkOrderBindCommand comm);
    }
}
