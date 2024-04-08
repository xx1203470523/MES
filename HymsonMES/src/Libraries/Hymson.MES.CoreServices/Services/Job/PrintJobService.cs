using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hymson.MES.CoreServices.Services.Job
{
    [Job("条码打印", JobTypeEnum.Standard)]
    public class PrintJobService : IJobService
    {
        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance1> _eventBus;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventBus"></param>
        public PrintJobService(
            IMasterDataService masterDataService,
          IEventBus<EventBusInstance1> eventBus)
        {
            _eventBus = eventBus;
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }
            var barCodes = new List<LabelTemplateBarCodeDto>();

            foreach (var item in sfcProduceEntities)
            {
                barCodes.Add(new LabelTemplateBarCodeDto
                {
                    BarCode = item.SFC,
                    MateriaId = item.ProductId
                });
            }

            return new PrintIntegrationEvent
            {
                SiteId = commonBo.SiteId,
                ResourceId = commonBo.ResourceId,
                ProcedureId = commonBo.ProcedureId,
                BarCodes = barCodes,
                UserName = commonBo.UserName
            };
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            if (obj is not PrintIntegrationEvent data) return null;
            _eventBus.PublishDelay(data, 1);
            return null;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
