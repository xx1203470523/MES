using Hymson.MES.HttpClients.Requests.WMS;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// WMS交互服务接口
    /// </summary>
    public interface IWMSApiClient
    {
        /// <summary>
        /// 回调（来料IQC）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> IQCReceiptCallBackAsync(IQCReceiptResultDto request);

        /// <summary>
        /// 回调（退料IQC）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> IQCReturnCallBackAsync(IQCReturnResultDto request);

        /// <summary>
        /// 生产入库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> WarehousingEntryRequestAsync(WarehousingEntryDto request);
    }
}
