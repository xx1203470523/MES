using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly RotorOptions _options;
        public RotorServer(HttpClient httpClient, IOptions<RotorOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }
        
        public Task<bool> WorkOrderStart(string workOrderCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WorkOrderStop(string workOrderCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WorkOrderSync(RotorWorkOrderSync rotorWorkOrder)
        {
            throw new NotImplementedException();
        }
    }
}
