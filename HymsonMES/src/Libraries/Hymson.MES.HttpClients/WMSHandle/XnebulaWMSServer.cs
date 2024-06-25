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
    public class XnebulaWMSServer : IXnebulaWMSServer
    {
        private readonly HttpClient _httpClient;
        private readonly XnebulaWMSOptions _options;
        public XnebulaWMSServer(HttpClient httpClient,IOptions<XnebulaWMSOptions> options)
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
            
            await HandleResponse(httpResponse).ConfigureAwait(false);

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
            await HandleResponse(httpResponse).ConfigureAwait(false);
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

            await HandleResponse(httpResponse).ConfigureAwait(false);
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

            await HandleResponse(httpResponse).ConfigureAwait(false);
            return httpResponse.IsSuccessStatusCode;
        }

        private static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new CustomerValidationException(response.StatusCode.ToString(), content);
                }

                throw new NotFoundException(response.StatusCode.ToString(), content);
            }
        }

        //async Task<(string base64Str, bool result)> ILabelPrintRequest.PreviewFromImageBase64Async(PreviewRequest previewRequest)
        //{
        //    string api = "api/LabelPrint/preview";
        //    var httpResponseMessage = await _httpClient.PostAsJsonAsync<PreviewRequest>(api, previewRequest);

        //    if (httpResponseMessage.IsSuccessStatusCode)
        //    {
        //        using var contentStream =
        //            await httpResponseMessage.Content.ReadAsStreamAsync();

        //        var r = await System.Text.Json.JsonSerializer.DeserializeAsync
        //            <PrintResponse>(contentStream);
        //        return (base64Str: r.Data, result: r.Success);
        //    }
        //    return ("调用失败", false);
        //}

        //async Task<(string msg, bool result)> ILabelPrintRequest.PrintAsync(PrintRequest printRequest, bool ShowDialog)
        //{
        //    string api = "api/LabelPrint/print";
        //    var httpResponseMessage = await _httpClient.PostAsJsonAsync<PrintRequest>(api, printRequest);

        //    if (httpResponseMessage.IsSuccessStatusCode)
        //    {
        //        using var contentStream =
        //            await httpResponseMessage.Content.ReadAsStreamAsync();

        //        var r = await System.Text.Json.JsonSerializer.DeserializeAsync
        //            <PrintResponse>(contentStream);
        //        return (msg: r.Message, result: r.Success);
        //    }
        //    return ("调用失败", false);
        //}

    }
}
