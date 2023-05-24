using Hymson.MES.HttpClients.Requests.Print;

using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// FastReport 打印请求
    /// </summary>
    public class FastReportPrintRequest : ILabelPrintRequest
    {
        private readonly HttpClient _httpClient;
        public FastReportPrintRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(string msg, bool result)> UploadTemplateAsync(string url, string templateName)
        {
            string api = $"api/LabelPrint/uploadtemplate?url={url}&templateName={templateName}";
            var httpResponseMessage = await _httpClient.GetAsync(api);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                try
                {

                    var r = await System.Text.Json.JsonSerializer.DeserializeAsync
                    <PrintResponse>(contentStream);
                    return (msg: r.Message, result: r.Success);
                }
                catch (Exception ex)
                {
                    return (msg: ex.Message, result: false);
                }
            }
            else
            {
                return (msg: "调用失败", result: false);
            }
        }

        async Task<(string base64Str, bool result)> ILabelPrintRequest.PreviewFromImageBase64Async(PrintRequest printRequest)
        {
            string api = "api/LabelPrint/printview";
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<PrintRequest>(api, printRequest);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
               
                try
                {

                    var r = await System.Text.Json.JsonSerializer.DeserializeAsync
                    <PrintResponse>(contentStream);
                    return (base64Str: r.Data, result: r.Success);
                   
                    
                   // dynamic dynParam = JsonConvert.DeserializeObject(Convert.ToString(r));
                   // return (base64Str: dynParam.Item1,result: dynParam.Item2);
                   
                }
                catch (Exception)
                {
                   
                    throw;
                }
                
            }
            return ("调用失败",false);
        }

        async Task<(string msg, bool result)> ILabelPrintRequest.PrintAsync(PrintRequest printRequest, bool ShowDialog)
        {
            string api = "api/LabelPrint/print";
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<PrintRequest>(api, printRequest);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var r = await System.Text.Json.JsonSerializer.DeserializeAsync
                    <PrintResponse>(contentStream);
                return (msg: r.Message, result: r.Success);
            }
            return ("调用失败", false);
        }
    }
}
