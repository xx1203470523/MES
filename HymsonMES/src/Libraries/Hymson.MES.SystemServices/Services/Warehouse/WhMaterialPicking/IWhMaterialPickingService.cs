using Hymson.MES.SystemServices.Dtos.Warehouse;

namespace Hymson.MES.SystemServices.Services.Warehouse.WhMaterialPicking
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWhMaterialPickingService
    {
        /// <summary>
        /// 退料确认
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<string> MaterialPickingReceiveAsync(WhMaterialPickingReceiveDto param);

        /// <summary>
        /// 领料单接收（不校验物料明细）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<string> MaterialPickingReceiveWithoutDetailAsync(WhMaterialPickingReceiveDto param);

    }
}
