/*
 *creator: Karl
 *
 *describe: 工单变更改记录表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-30 03:46:15
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单变更改记录表仓储接口
    /// </summary>
    public interface IPlanWorkOrderStatusRecordRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderStatusRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(PlanWorkOrderStatusRecordEntity planWorkOrderStatusRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderStatusRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<PlanWorkOrderStatusRecordEntity> planWorkOrderStatusRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderStatusRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanWorkOrderStatusRecordEntity planWorkOrderStatusRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="planWorkOrderStatusRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<PlanWorkOrderStatusRecordEntity> planWorkOrderStatusRecordEntitys);

        /// <summary>
        /// 删除
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
        Task<PlanWorkOrderStatusRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderStatusRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="planWorkOrderStatusRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderStatusRecordEntity>> GetPlanWorkOrderStatusRecordEntitiesAsync(PlanWorkOrderStatusRecordQuery planWorkOrderStatusRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderStatusRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderStatusRecordEntity>> GetPagedInfoAsync(PlanWorkOrderStatusRecordPagedQuery planWorkOrderStatusRecordPagedQuery);
    }
}
