using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests;
using Hymson.MES.HttpClients.Requests.ERP;
using Hymson.MES.HttpClients.Responses.NioErp;
using Hymson.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// 交互服务（ERP）
    /// </summary>
    public class ERPApiClient : IERPApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<ERPOptions> _logger;
        /// <summary>
        /// 
        /// </summary>
        private readonly IOptions<ERPOptions> _options;
        /// <summary>
        /// 
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        /// <param name="httpClient"></param>
        public ERPApiClient(ILogger<ERPOptions> logger, IOptions<ERPOptions> options, HttpClient httpClient)
        {
            _logger = logger;
            _options = options;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_options.Value.BaseAddressUri);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.Value.SysToken);
        }


        /// <summary>
        /// 切换生产计划启用状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseERPResponse> EnabledPlanAsync(PlanRequestDto request)
        {
            _logger.LogDebug($"切换生产计划启用状态 -> Request: {request.ToSerialize()}");
            var responseDto = new BaseERPResponse { Status = false, Message = "未知的错误" };

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.EnabledPlanRoute, request);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"切换生产计划启用状态 -> Response: {jsonResponse}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var result = jsonResponse.ToDeserialize<BaseERPResponse>();
                if (result != null)
                {
                    responseDto.Status = result.Status;
                    responseDto.Message = result.Message;
                }
            }

            return responseDto;
        }

        /// <summary>
        /// 查询NIO需要的物料信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NioErpResponse> MaterailQueryAsync(MaterialRequest request)
        {
            var responseDto = new NioErpResponse { Status = false, Message = "未知的错误" };

            _logger.LogDebug($"查询NIO需要的物料信息 -> Request: {request.ToSerialize()}");

            var httpResponse = await _httpClient.PostAsJsonAsync(_options.Value.MaterialNioRoute, request);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug($"查询NIO需要的物料信息 -> Response: {jsonResponse}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var result = jsonResponse.ToDeserialize<NioErpResponse>();
                if (result != null)
                {
                    responseDto.Status = result.Status;
                    responseDto.Message = result.Message;
                }
            }

            return responseDto;
        }
    }
}
