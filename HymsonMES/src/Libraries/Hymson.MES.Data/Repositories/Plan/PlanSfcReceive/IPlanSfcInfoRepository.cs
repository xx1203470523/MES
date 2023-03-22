/*
 *creator: Karl
 *
 *describe: 条码接收仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码接收仓储接口
    /// </summary>
    public interface IPlanSfcInfoRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(PlanSfcInfoView planSfcInfoEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planSfcInfoEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<PlanSfcInfoView> planSfcInfoEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanSfcInfoView planSfcInfoEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="planSfcInfoEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<PlanSfcInfoView> planSfcInfoEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanSfcInfoView> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanSfcInfoView>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="planSfcInfoQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanSfcInfoView>> GetPlanSfcInfoEntitiesAsync(PlanSfcInfoQuery planSfcInfoQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planSfcInfoPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanSfcInfoView>> GetPagedInfoAsync(PlanSfcInfoPagedQuery planSfcInfoPagedQuery);
    }
}
