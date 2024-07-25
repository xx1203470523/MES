using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests.WMS;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// WMS交互服务
    /// </summary>
    public class WMSApiClient : IWMSApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IOptions<WMSOptions> _options;
        /// <summary>
        /// 
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpClient"></param>
        public WMSApiClient(IOptions<WMSOptions> options, HttpClient httpClient)
        {
            _options = options;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_options.Value.BaseAddressUri);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.Value.SysToken);
        }

        /// <summary>
        /// 回调（来料IQC）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> IQCReceiptCallBackAsync(IQCReceiptResultDto dto)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.IQCReceiptRoute, dto);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 回调（退料IQC）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> IQCReturnCallBackAsync(IQCReturnResultDto dto)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.IQCReturnRoute, dto);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 入库申请单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> WarehousingEntryRequestAsync(WarehousingEntryDto request)
        {
            WarehousingEntryRequest materialReturnRequest = new WarehousingEntryRequest()
            {
                Type = request.Type,
                WarehouseCode = request.WarehouseCode,
                SyncCode = request.SyncCode,
                SendOn = request.SendOn,
                SupplierCode = request.SupplierCode,
                CustomerCode = request.CustomerCode,
                PurchaseType = request.PurchaseType,
                InboundCategory = request.InboundCategory,
                IsAutoExecute = request.IsAutoExecute,
                CreatedBy = request.CreatedBy,
                Remark = request.Remark,
                Details = request.Details
            };

            var httpResponse = await _httpClient.PostAsJsonAsync<WarehousingEntryRequest>(_options.Value.Receipt.RoutePath, materialReturnRequest);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 出库申请单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> WarehousingDeliveryRequestAsync(DeliveryDto request)
        {
            DeliveryRequest deliveryRequest = new DeliveryRequest()
            {
                Type = request.Type,
                WarehouseCode = request.WarehouseCode,
                SyncCode = request.SyncCode,
                SendOn = request.SendOn,
                SupplierCode = request.SupplierCode,
                CustomerCode = request.CustomerCode,
                StockOutCategory = request.StockOutCategory,
                IsAutoExecute = request.IsAutoExecute,
                CreatedBy = request.CreatedBy,
                Remark = request.Remark,
                Details = request.Details
            };

            var httpResponse = await _httpClient.PostAsJsonAsync<DeliveryRequest>(_options.Value.Delivery.RoutePath, deliveryRequest);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

    }
}
