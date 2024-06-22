using Hymson.MES.HttpClients.Requests;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// Xnebula 仓库请求
    /// </summary>
    public class XnebulaWMSServer : IWMSServer
    {
        private readonly HttpClient _httpClient;
        private readonly WMSOptions _options;
        public XnebulaWMSServer(HttpClient httpClient,IOptions<WMSOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<(string msg,bool result)> MaterialPickingRequestAsync(MaterialPickingRequestDto request)
        {
            
            MaterialPickingRequest materialPickingRequest = new MaterialPickingRequest()
            {
                SendOn = request.sendOn,
                SyncCode = request.syncCode,
                Details = request.details,
                Type = _options.Delivery.Type,
                WarehouseCode = _options.Delivery.WarehouseCode
            };
           
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<MaterialPickingRequest>(_options.Delivery.RoutePath, materialPickingRequest);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //using var contentStream =
                //    await httpResponseMessage.Content.ReadAsStreamAsync();

                //var r = await System.Text.Json.JsonSerializer.DeserializeAsync
                //    <PrintResponse>(contentStream);
                //return (base64Str: r.Data, result: r.Success);
                return ("调用成功", true);
            }
            return ("调用失败", false);
        }

        

        public Task<bool> MaterialPickingCancelAsync(MaterialPickingCancelDto request)
        {
            throw new NotImplementedException();
        }

        public Task MaterialReturnRequestAsync(MaterialReturnRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MaterialReturnCancelAsync(MaterialReturnCancelDto request)
        {
            throw new NotImplementedException();
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
