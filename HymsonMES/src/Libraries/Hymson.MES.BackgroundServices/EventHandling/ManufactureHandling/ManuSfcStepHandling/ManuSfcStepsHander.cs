using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents;
using Hymson.MES.CoreServices.Services.Manufacture.ManuSfcStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.EventHandling.ManufactureHandling.ManuSfcStepHandling
{
    /// <summary>
    /// 批量新增步骤表
    /// </summary>
    public class ManuSfcStepsHander : IIntegrationEventHandler<ManuSfcStepsEvent>
    {
        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IManuSfcStepServices _manuSfcStepServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messagePushService"></param>
        public ManuSfcStepsHander(IManuSfcStepServices manuSfcStepServices)
        {
            _manuSfcStepServices = manuSfcStepServices;
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task Handle(ManuSfcStepsEvent @event)
        {
            await _manuSfcStepServices.AddRangeAsync(@event);
        }
    }
}
