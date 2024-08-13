using Hymson.MES.HttpClients.Requests;
using Hymson.MES.HttpClients.Requests.WMS;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;
using Hymson.MES.HttpClients.Responses.NioWms;

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
        Task<BaseResponse> IQCReceiptCallBackAsync(IQCReceiptResultDto request);

        /// <summary>
        /// 生产入库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse> WarehousingEntryRequestAsync(WarehousingEntryDto request);

        /// <summary>
        /// 出库单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse> WarehousingDeliveryRequestAsync(DeliveryDto request);



        /// <summary>
        /// 领料申请，包含工单借料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialPickingRequestAsync(MaterialPickingRequestDto request);

        /// <summary>
        /// 领料取消
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialPickingCancelAsync(MaterialPickingCancelDto request);

        /// <summary>
        /// 退料申请，请求发送成功之后，库存即刻扣除，后续WMS反馈有问题时候再加回来
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialReturnRequestAsync(MaterialReturnRequestDto request);
        /// <summary>
        /// 取消退料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialReturnCancelAsync(MaterialReturnCancelDto request);

        /// <summary>
        /// 入库申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse> ProductReceiptRequestAsync(ProductReceiptRequestDto request);

        /// <summary>
        /// 取消入库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ProductReceiptCancelAsync(ProductReceiptCancelDto request);

        /// <summary>
        /// NIO合作伙伴精益与库存信息
        /// </summary>
        /// <returns></returns>
        Task<NioStockInfoResponse> NioStockInfoAsync(List<StockMesNIODto> request);

        /// <summary>
        /// 关键下级键
        /// </summary>
        /// <returns></returns>
        Task<NioKeyItemInfoResponse> NioKeyItemInfoAsync(List<StockMesNIODto> request);

        /// <summary>
        /// 实际交付情况
        /// </summary>
        /// <returns></returns>
        Task<NioWmsActualDeliveryResponse?> NioActualDeliveryAsync(StockMesDataDto request);
    }
}
