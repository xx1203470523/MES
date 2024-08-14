using Hymson.MES.BackgroundServices.NIO.Services.ERP;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.NIO.Jobs.ERP
{
    /// <summary>
    /// NIO合作伙伴精益与库存信息
    /// </summary>
    [DisallowConcurrentExecution]
    internal class ProductionCapacityJob : IJob
    {
        private readonly ILogger<ProductionCapacityJob> _logger;
        private readonly IErpDataPushService _erpDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="buzDataPushService"></param>
        public ProductionCapacityJob(ILogger<ProductionCapacityJob> logger, IErpDataPushService erpDataPushService)
        {
            _logger = logger;
            _erpDataPushService = erpDataPushService;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _erpDataPushService.NioStockInfoAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "扫描推送数据 -> NIO合作伙伴精益与库存信息");
            }
        }
    }
}
