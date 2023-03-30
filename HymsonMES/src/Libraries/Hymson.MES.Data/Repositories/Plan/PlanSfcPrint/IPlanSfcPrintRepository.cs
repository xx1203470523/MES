/*
 *creator: Karl
 *
 *describe: 条码打印仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
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
    /// 条码打印仓储接口
    /// </summary>
    public interface IPlanSfcPrintRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcInfoEntity planSfcInfoEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcInfoEntity planSfcInfoEntity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanSfcPrintView> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfoEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planSfcInfoPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanSfcPrintView>> GetPagedInfoAsync(PlanSfcPrintPagedQuery planSfcInfoPagedQuery);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="SFC"></param>
        /// <returns></returns>
        Task<ManuSfcInfoEntity> GetBySFCAsync(string SFC);

        /// <summary>
        /// 获取条码数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSfcInfoEntity> GetPlanSfcInfoAsync(PlanSfcPrintQuery query);
    }
}
