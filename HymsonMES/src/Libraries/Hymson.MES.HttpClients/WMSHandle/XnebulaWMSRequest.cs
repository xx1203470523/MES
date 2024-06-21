using Hymson.MES.HttpClients.Requests.Print;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// Xnebula 仓库请求
    /// </summary>
    public class XnebulaWMSRequest : IWMSRequest
    {
        private readonly HttpClient _httpClient;
        private readonly WMSOptions _options;
        public XnebulaWMSRequest(HttpClient httpClient,IOptions<WMSOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<(string msg,bool result)> MaterialPickingRequestAsync(MaterialPickingRequest request)
        {
            string api = "Delivery/create";
            if(!string.IsNullOrEmpty(_options.MaterialPickingRequestUrl.Trim()))
            {
                api = _options.MaterialPickingRequestUrl.Trim();
            }
            request.warehouseCode = _options.WarehouseCode;
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<MaterialPickingRequest>(api, request);

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

        public Task MaterialPickingCancelAsync(MaterialPickingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task MaterialReturnAsync(MaterialReturnRequest request)
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
