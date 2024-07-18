using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests;
using Hymson.MES.HttpClients.Requests.Rotor;
using Hymson.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.RotorHandle
{
    /// <summary>
    /// 转子线交互服务
    /// </summary>
    public class RotorApiClient : IRotorApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 
        /// </summary>
        private readonly RotorOption _options;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RotorApiClient(HttpClient httpClient, IOptions<RotorOption> options,
            ISysConfigRepository sysConfigRepository)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _sysConfigRepository = sysConfigRepository;
        }

        /// <summary>
        /// 初始化URL和TOKEN
        /// </summary>
        /// <returns></returns>
        private async Task InitAsync()
        {
            var tokenList = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.RotorLmsToken });
            if (tokenList == null || !tokenList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", SysConfigEnum.NioMaterial.ToString());
            }
            _httpClient.DefaultRequestHeaders.Add("SYSTOKEN", tokenList.ElementAt(0).Value);

            var urlList = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.RotorLmsUrl });
            if (urlList == null || !urlList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", SysConfigEnum.NioMaterial.ToString());
            }
            _httpClient.BaseAddress = new Uri(urlList.ElementAt(0).Value);
        }

        public Task<bool> ProcedureLineSync(IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetails)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 工单激活
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        public async Task<bool> WorkOrderStartAsync(string workOrderCode)
        {
            await InitAsync();

            var httpResponse = await _httpClient.GetAsync($"{_options.SetWorkOrderStatusRoute}?WorkNo={workOrderCode}&status=R");
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 工单取消激活
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        public async Task<bool> WorkOrderStopAsync(string workOrderCode)
        {
            await InitAsync();

            var httpResponse = await _httpClient.GetAsync($"{_options.SetWorkOrderStatusRoute}?WorkNo={workOrderCode}&status=T");
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 工单同步
        /// </summary>
        /// <param name="rotorWorkOrder"></param>
        /// <returns></returns>
        public async Task<bool> WorkOrderAsync(RotorWorkOrderRequest param)
        {
            await InitAsync();

            var httpResponse = await _httpClient.PostAsJsonAsync<RotorWorkOrderRequest>(_options.CreateOrderRoute, param);
            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 物料同步
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> MaterialAsync(IEnumerable<RotorMaterialRequest> list)
        {
            await InitAsync();

            int num = 0;
            foreach (var rotor in list)
            {
                var httpResponse = await _httpClient.PostAsJsonAsync(_options.MaterialSyncRoute, rotor);
                await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
                ++num;
            }

            return num;
        }

        /// <summary>
        /// 工序同步
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> ProcedureAsync(IEnumerable<ProcProcedureEntity> list)
        {
            throw new NotImplementedException();
        }
    }
}
