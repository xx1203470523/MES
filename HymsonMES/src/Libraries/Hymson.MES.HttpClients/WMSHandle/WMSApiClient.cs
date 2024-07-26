using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests;
using Hymson.MES.HttpClients.Requests.WMS;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;
using Hymson.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI.Common;
using Polly.Caching;
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
        private readonly ILogger<WMSOptions> _logger;
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
        /// <param name="logger"></param>
        /// <param name="options"></param>
        /// <param name="httpClient"></param>
        public WMSApiClient(ILogger<WMSOptions> logger, IOptions<WMSOptions> options, HttpClient httpClient)
        {
            _logger = logger;
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
            _logger.LogDebug($"IQCReceiptCallBackAsync -> Request: {dto.ToSerialize()}");

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.IQCReceiptRoute, dto);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"IQCReceiptCallBackAsync -> Response: {jsonResponse}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var result = jsonResponse.ToDeserialize<ResultDto>();
                return result?.Code == 0;
            }

            return false;
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

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.Receipt.Route, materialReturnRequest);

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
            DeliveryRequest deliveryRequest = new()
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

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.Delivery.Route, deliveryRequest);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }




        public async Task<bool> MaterialPickingRequestAsync(MaterialPickingRequestDto request)
        {
            MaterialPickingRequest materialPickingRequest = new()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Details = request.details,
                Type = _options.Value.Delivery.Type,
                WarehouseCode = _options.Value.Delivery.WarehouseCode
            };

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.Delivery.Route, materialPickingRequest);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> MaterialPickingCancelAsync(MaterialPickingCancelDto request)
        {

            MaterialPickingCancel materialPickingCancel = new()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Type = _options.Value.Delivery.Type,
                WarehouseCode = _options.Value.Delivery.WarehouseCode
            };
            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.Delivery.Route, materialPickingCancel);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> MaterialReturnRequestAsync(MaterialReturnRequestDto request)
        {
            MaterialReturnRequest materialReturnRequest = new()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Details = request.Details,
                Type = _options.Value.Receipt.Type,
                WarehouseCode = _options.Value.Receipt.WarehouseCode
            };

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.Delivery.Route, materialReturnRequest);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> MaterialReturnCancelAsync(MaterialReturnCancelDto request)
        {
            MaterialReturnCancel materialReturnCancel = new()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Type = _options.Value.Delivery.Type,
                WarehouseCode = _options.Value.Delivery.WarehouseCode
            };
            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.Delivery.Route, materialReturnCancel);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> ProductReceiptCancelAsync(ProductReceiptCancelDto request)
        {
            ProductReceiptCancel productReceiptCancel = new()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Type = _options.Value.ProductReceiptCancel.Type,
                WarehouseCode = _options.Value.ProductReceiptCancel.WarehouseCode
            };
            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.ProductReceiptCancel.Route, productReceiptCancel);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> ProductReceiptRequestAsync(ProductReceiptRequestDto request)
        {
            ProductReceiptRequest materialReturnRequest = new()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Details = request.Details,
                Type = _options.Value.ProductReceipt.Type,
                WarehouseCode = _options.Value.ProductReceipt.WarehouseCode
            };

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.ProductReceipt.Route, materialReturnRequest);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }



    }
}
