using Hymson.MES.HttpClients.Requests.Print;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Hymson.MES.HttpClients
{
    public class FastReportPrintService : ILabelPrintService
    {
        private readonly HttpClient _httpClient;
        public FastReportPrintService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       

        async Task<(string base64Str, bool result)> ILabelPrintService.PreviewFromImageBase64Async(PrintRequest printRequest)
        {
            string api = "api/LabelPrint/printview";
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<PrintRequest>(api, printRequest);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync
                    <(string base64Str, bool result)>(contentStream);
            }
            return ("调用失败",false);
        }

        async Task<(string msg, bool result)> ILabelPrintService.PrintAsync(PrintRequest printRequest, bool ShowDialog)
        {
            string api = "api/LabelPrint/print";
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<PrintRequest>(api, printRequest);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync
                    <(string msg, bool result)>(contentStream);
            }
            return ("调用失败", false);
        }
    }
}
