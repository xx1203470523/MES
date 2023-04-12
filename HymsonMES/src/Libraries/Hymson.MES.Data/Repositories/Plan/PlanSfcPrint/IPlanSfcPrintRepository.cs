/*
 *creator: Karl
 *
 *describe: 条码打印仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码打印仓储接口
    /// </summary>
    public interface IPlanSfcPrintRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planSfcInfoPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanSfcPrintView>> GetPagedInfoAsync(PlanSfcPrintPagedQuery planSfcInfoPagedQuery);
    }
}
