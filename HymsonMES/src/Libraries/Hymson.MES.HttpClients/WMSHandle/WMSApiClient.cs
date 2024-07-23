using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests;
using Hymson.MES.HttpClients.Requests.WMS;
using Hymson.MES.HttpClients.Responses.Rotor;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> IQCReceiptCallBackAsync(IQCReceiptRequestDto request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(_options.CreateOrderRoute, request);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            httpResponse.EnsureSuccessStatusCode();

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            return true;
        }

        /// <summary>
        /// 回调（退料IQC）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> IQCReturnCallBackAsync(IQCReturnRequestDto request)
        {
            // TODO
            await Task.CompletedTask;
            return false;
        }

    }
}
