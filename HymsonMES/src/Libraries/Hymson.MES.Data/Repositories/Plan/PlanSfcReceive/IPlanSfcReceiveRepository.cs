/*
 *creator: Karl
 *
 *describe: 条码接收仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码接收仓储接口
    /// </summary>
    public interface IPlanSfcReceiveRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planSfcInfoPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanSfcReceiveView>> GetPagedInfoAsync(PlanSfcReceivePagedQuery planSfcInfoPagedQuery);

        /// <summary>
        /// 获取条码数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSfcInfoEntity> GetPlanSfcInfoAsync(PlanSfcReceiveQuery query);
    }
}
