using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.RotorHandle
{
    /// <summary>
    /// 转子线交互服务
    /// </summary>
    public class RotorServer : IRotorService
    {
        private readonly HttpClient _httpClient;
        private readonly RotorOption _options;
        public RotorServer(HttpClient httpClient, IOptions<RotorOption> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }
        
        public async Task<bool> WorkOrderStart(string workOrderCode)
        {
            var httpResponse = await _httpClient.GetAsync($"{_options.StartOrderRoute}{workOrderCode}");

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> WorkOrderStop(string workOrderCode)
        {
            var httpResponse = await _httpClient.GetAsync($"{_options.StopOrderRoute}{workOrderCode}");

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> WorkOrderSync(RotorWorkOrderSync rotorWorkOrder)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync<RotorWorkOrderSync>(_options.CreateOrderRoute, rotorWorkOrder);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }
    }
}
