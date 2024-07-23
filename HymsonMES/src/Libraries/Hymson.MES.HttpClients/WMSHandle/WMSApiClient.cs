using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests.WMS;
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

    }
}
