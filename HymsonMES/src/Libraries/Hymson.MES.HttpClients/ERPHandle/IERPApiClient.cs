using Hymson.MES.HttpClients.Requests.ERP;
using Hymson.MES.HttpClients.Responses.NioErp;

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

        /// <summary>
        /// 查询NIO需要的物料信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<NioErpResponse> MaterailQueryAsync(MaterialRequest request);
    }
}
