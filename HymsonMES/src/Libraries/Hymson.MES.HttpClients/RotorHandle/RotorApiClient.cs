using Hymson.MES.Core.Domain.Process;
using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests;
using Hymson.MES.HttpClients.Requests.Rotor;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        private readonly HttpClient _httpClient;
        private readonly RotorOption _options;
        public RotorApiClient(HttpClient httpClient, IOptions<RotorOption> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
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
        public async Task<bool> WorkOrderStart(string workOrderCode)
        {
            var httpResponse = await _httpClient.GetAsync($"{_options.SetWorkOrderStatusRoute}?WorkNo={workOrderCode}&status=R");

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 工单取消激活
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        public async Task<bool> WorkOrderStop(string workOrderCode)
        {
            var httpResponse = await _httpClient.GetAsync($"{_options.SetWorkOrderStatusRoute}?WorkNo={workOrderCode}&status=S");

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 工单同步
        /// </summary>
        /// <param name="rotorWorkOrder"></param>
        /// <returns></returns>
        public async Task<bool> WorkOrderSync(RotorWorkOrderSync rotorWorkOrder)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync<RotorWorkOrderSync>(_options.CreateOrderRoute, rotorWorkOrder);

            await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        /// <summary>
        /// 物料同步
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> MaterialSync(IEnumerable<ProcMaterialEntity> list)
        {
            List<RotorMaterialSync> rotorList = Change(list);

            foreach (var rotor in rotorList)
            {
                var httpResponse = await _httpClient.PostAsJsonAsync(_options.MaterialSyncRoute, rotor);
                await CommonHttpClient.HandleResponse(httpResponse).ConfigureAwait(false);
            }

            return 0;

            List<RotorMaterialSync> Change(IEnumerable<ProcMaterialEntity> list)
            {
                return list.Select(m => new RotorMaterialSync()
                {
                    MaterialCode = m.MaterialCode,
                    MaterialName = m.MaterialName,
                    MatSpecification = m.Specifications ?? "",
                    MaterialType = m.BuyType == null ? "1" : ((int)m.BuyType!).ToString(),
                    MaterialVersion = m.Version ?? "",
                    MaterialUnit = m.Unit ?? "",
                    BarcodeType = "",
                    BatchCodeRegular = "",
                    EffectiveDays = m.ValidTime,
                    WarnAmount = null,
                    Enable = true
                }).ToList();
            }
        }

        /// <summary>
        /// 工序同步
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> ProcedureSync(IEnumerable<ProcProcedureEntity> list)
        {
            throw new NotImplementedException();
        }
    }
}
