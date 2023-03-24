/*
 *creator: Karl
 *
 *describe: 条码接收    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码接收 service接口
    /// </summary>
    public interface IPlanSfcReceiveService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="planSfcInfoPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanSfcReceiveDto>> GetPageListAsync(PlanSfcReceivePagedQueryDto planSfcInfoPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planSfcInfoDto"></param>
        /// <returns></returns>
        Task CreatePlanSfcInfoAsync(PlanSfcReceiveCreateDto planSfcInfoCreateDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesPlanSfcInfoAsync(long[] idsArr);
    }
}
