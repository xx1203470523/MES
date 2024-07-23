using Hymson.MES.HttpClients.Requests;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using MailKit.Net.Smtp;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// Xnebula 仓库请求
    /// </summary>
    public class XnebulaWMSApiClient : IXnebulaWMSApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly XnebulaWMSOption _options;
        public XnebulaWMSApiClient(HttpClient httpClient,IOptions<XnebulaWMSOption> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<bool> MaterialPickingRequestAsync(MaterialPickingRequestDto request)
        {
            
            MaterialPickingRequest materialPickingRequest = new MaterialPickingRequest()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Details = request.details,
                Type = _options.Delivery.Type,
                WarehouseCode = _options.Delivery.WarehouseCode
            };
           
            var httpResponse = await _httpClient.PostAsJsonAsync<MaterialPickingRequest>(_options.Delivery.RoutePath, materialPickingRequest);
            
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        

        public async Task<bool> MaterialPickingCancelAsync(MaterialPickingCancelDto request)
        {

            MaterialPickingCancel materialPickingCancel = new MaterialPickingCancel()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Type = _options.Delivery.Type,
                WarehouseCode = _options.Delivery.WarehouseCode
            };
            var httpResponse = await _httpClient.PostAsJsonAsync<MaterialPickingCancel>(_options.Delivery.RoutePath, materialPickingCancel);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> MaterialReturnRequestAsync(MaterialReturnRequestDto request)
        {
            MaterialReturnRequest materialReturnRequest = new MaterialReturnRequest()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Details = request.Details,
                Type = _options.Receipt.Type,
                WarehouseCode = _options.Receipt.WarehouseCode
            };

            var httpResponse = await _httpClient.PostAsJsonAsync<MaterialReturnRequest>(_options.Delivery.RoutePath, materialReturnRequest);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> MaterialReturnCancelAsync(MaterialReturnCancelDto request)
        {
            MaterialReturnCancel materialReturnCancel = new MaterialReturnCancel()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Type = _options.Delivery.Type,
                WarehouseCode = _options.Delivery.WarehouseCode
            };
            var httpResponse = await _httpClient.PostAsJsonAsync<MaterialReturnCancel>(_options.Delivery.RoutePath, materialReturnCancel);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> ProductReceiptCancelAsync(ProductReceiptCancelDto request)
        {
            ProductReceiptCancel productReceiptCancel = new ProductReceiptCancel()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Type = _options.ProductReceiptCancel.Type,
                WarehouseCode = _options.ProductReceiptCancel.WarehouseCode
            };
            var httpResponse = await _httpClient.PostAsJsonAsync<ProductReceiptCancel>(_options.ProductReceiptCancel.RoutePath, productReceiptCancel);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> ProductReceiptRequestAsync(ProductReceiptRequestDto request)
        {
            ProductReceiptRequest materialReturnRequest = new ProductReceiptRequest()
            {
                SendOn = request.SendOn,
                SyncCode = request.SyncCode,
                Details = request.Details,
                Type = _options.ProductReceiptOptions.Type,
                WarehouseCode = _options.ProductReceiptOptions.WarehouseCode
            };

            var httpResponse = await _httpClient.PostAsJsonAsync<ProductReceiptRequest>(_options.ProductReceiptOptions.RoutePath, materialReturnRequest);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }
    }
}
