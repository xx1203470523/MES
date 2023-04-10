/*
 *creator: Karl
 *
 *describe: 工单激活记录仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-30 02:42:18
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
    /// 工单激活记录仓储接口
    /// </summary>
    public interface IPlanWorkOrderActivationRecordRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderActivationRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(PlanWorkOrderActivationRecordEntity planWorkOrderActivationRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderActivationRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<PlanWorkOrderActivationRecordEntity> planWorkOrderActivationRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderActivationRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanWorkOrderActivationRecordEntity planWorkOrderActivationRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="planWorkOrderActivationRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<PlanWorkOrderActivationRecordEntity> planWorkOrderActivationRecordEntitys);

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
        Task<PlanWorkOrderActivationRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderActivationRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="planWorkOrderActivationRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderActivationRecordEntity>> GetPlanWorkOrderActivationRecordEntitiesAsync(PlanWorkOrderActivationRecordQuery planWorkOrderActivationRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderActivationRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderActivationRecordEntity>> GetPagedInfoAsync(PlanWorkOrderActivationRecordPagedQuery planWorkOrderActivationRecordPagedQuery);
    }
}