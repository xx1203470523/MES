/*
 *creator: Karl
 *
 *describe: 工单激活日志（日志中心）仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-12 11:17:05
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活日志（日志中心）仓储接口
    /// </summary>
    public interface IPlanWorkOrderBindRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderBindRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(PlanWorkOrderBindRecordEntity planWorkOrderBindRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderBindRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<PlanWorkOrderBindRecordEntity> planWorkOrderBindRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderBindRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanWorkOrderBindRecordEntity planWorkOrderBindRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="planWorkOrderBindRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<PlanWorkOrderBindRecordEntity> planWorkOrderBindRecordEntitys);

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
        Task<PlanWorkOrderBindRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderBindRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="planWorkOrderBindRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderBindRecordEntity>> GetPlanWorkOrderBindRecordEntitiesAsync(PlanWorkOrderBindRecordQuery planWorkOrderBindRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderBindRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderBindRecordEntity>> GetPagedInfoAsync(PlanWorkOrderBindRecordPagedQuery planWorkOrderBindRecordPagedQuery);
        #endregion
    }
}
