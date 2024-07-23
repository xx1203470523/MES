using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests.WMS;
using Microsoft.Extensions.Options;
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
        private readonly HttpClient _httpClient;
        private readonly WMSOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="options"></param>
        public WMSApiClient(HttpClient httpClient, IOptions<WMSOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        /// <summary>
        /// 回调（来料IQC）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> IQCReceiptCallBackAsync(IQCReceiptResultDto dto)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(_options.IQCReceiptRoute, dto);
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
            var httpResponse = await _httpClient.PostAsJsonAsync(_options.IQCReturnRoute, dto);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

    }
}
