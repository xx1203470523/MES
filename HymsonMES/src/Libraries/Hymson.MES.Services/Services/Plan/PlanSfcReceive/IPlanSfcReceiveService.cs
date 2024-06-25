/*
 *creator: Karl
 *
 *describe: 条码接收    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码接收 service接口
    /// </summary>
    public interface IPlanSfcReceiveService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planSfcInfoCreateDto"></param>
        /// <returns></returns>
        Task CreatePlanSfcInfoAsync(PlanSfcReceiveCreateDto planSfcInfoCreateDto);

        /// <summary>
        /// 条码接收扫码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PlanSfcReceiveSfcDto> PlanSfcReceiveScanCodeAsync(PlanSfcReceiveScanCodeDto param);

        /// <summary>
        /// 条码接收扫码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<PlanSfcReceiveSfcDto>> PlanSfcReceiveScanListAsync(PlanSfcReceiveScanListDto param);
    }
}
