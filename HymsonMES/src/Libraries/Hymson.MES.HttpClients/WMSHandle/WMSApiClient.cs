using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests;
using Hymson.MES.HttpClients.Requests.WMS;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;
using Hymson.MES.HttpClients.Responses.NioWms;
using Hymson.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        public async Task<BaseResponse> IQCReceiptCallBackAsync(IQCReceiptResultDto dto)
        {
            var responseDto = new BaseResponse { Code = -1, Message = "未知的错误" };

            _logger.LogDebug($"来料IQC -> Request: {dto.ToSerialize()}");

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.IQCReceipt.Route, dto);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"来料IQC -> Response: {jsonResponse}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var result = jsonResponse.ToDeserialize<BaseResponse>();
                if (result != null)
                {
                    responseDto.Code = result.Code;
                    responseDto.Message = result.Message;
                }
            }

            return responseDto;
        }

        /// <summary>
        /// 入库申请单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse> WarehousingEntryRequestAsync(WarehousingEntryDto request)
        {
            _logger.LogDebug($"退料入库 -> Request: {request.ToSerialize()}");

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

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"退料入库 -> Response: {jsonResponse}");

            return await httpResponse.Content.ReadFromJsonAsync<BaseResponse>();
        }

        /// <summary>
        /// 出库申请单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse> WarehousingDeliveryRequestAsync(DeliveryDto request)
        {
            _logger.LogDebug($"领料出库 -> Request: {request.ToSerialize()}");

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

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"领料出库 -> Response: {jsonResponse}");

            return await httpResponse.Content.ReadFromJsonAsync<BaseResponse>();
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

        public async Task<BaseResponse> ProductReceiptRequestAsync(ProductReceiptRequestDto request)
        {
            _logger.LogDebug($"成品入库 -> Request: {request.ToSerialize()}");

            ProductReceiptRequest materialReturnRequest = new()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Details = request.Details,
                Type = _options.Value.ProductReceipt.Type,
                WarehouseCode = request.WarehouseCode //_options.Value.ProductReceipt.WarehouseCode
            };

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.ProductReceipt.Route, materialReturnRequest);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"成品入库 -> Response: {jsonResponse}");

            return await httpResponse.Content.ReadFromJsonAsync<BaseResponse>();
        }

        /// <summary>
        /// NIO合作伙伴精益与库存信息
        /// </summary>
        /// <returns></returns>
        public async Task<NioStockInfoResponse?> NioStockInfoAsync(List<StockMesNIODto> request)
        {
            _logger.LogDebug($"NIO合作伙伴精益与库存信息 -> Request: {request.ToSerialize()}");

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.NioStockInfo.Route, request);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"NIO合作伙伴精益与库存信息 -> Response: {jsonResponse}");

            return await httpResponse.Content.ReadFromJsonAsync<NioStockInfoResponse>();
        }

        /// <summary>
        /// 关键下级键
        /// </summary>
        /// <returns></returns>
        public async Task<NioKeyItemInfoResponse?> NioKeyItemInfoAsync(List<StockMesNIODto> request)
        {
            _logger.LogDebug($"NIO合作伙伴精益与库存信息 -> Request: {request.ToSerialize()}");

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.NioKeyItemInfo.Route, request);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"NIO合作伙伴精益与库存信息 -> Response: {jsonResponse}");

            return await httpResponse.Content.ReadFromJsonAsync<NioKeyItemInfoResponse>();
        }

        /// <summary>
        /// 实际交付情况
        /// </summary>
        /// <returns></returns>
        public async Task<NioWmsActualDeliveryResponse?> NioActualDeliveryAsync(StockMesDataDto request)
        {
            _logger.LogDebug($"实际交付情况 -> Request: {request.ToSerialize()}");

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.NioActualDelivery.Route, request);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"实际交付情况 -> Response: {jsonResponse}");

            return await httpResponse.Content.ReadFromJsonAsync<NioWmsActualDeliveryResponse>();
        }

    }
}
