using Hymson.MES.HttpClients.Requests.ERP;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// 交互服务接口（ERP）
    /// </summary>
    public interface IERPApiClient
    {
        /// <summary>
        /// 切换生产计划启用状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseERPResponse> EnabledPlanAsync(PlanRequestDto request);

    }
}
