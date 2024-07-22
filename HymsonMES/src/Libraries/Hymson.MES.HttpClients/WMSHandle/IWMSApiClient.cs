using Hymson.MES.HttpClients.Requests.WMS;

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
        Task<bool> IQCReceiptCallBackAsync(IQCReceiptRequestDto request);

        /// <summary>
        /// 回调（退料IQC）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> IQCReturnCallBackAsync(IQCReturnRequestDto request);

    }
}
