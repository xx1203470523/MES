using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents;
using Hymson.MES.CoreServices.Services.Manufacture.ManuSfcStep;

namespace Hymson.MES.BackgroundServices.EventHandling.ManufactureHandling.ManuSfcStepHandling
{
    /// <summary>
    /// 步骤表新增
    /// </summary>
    public  class ManuSfcStepHander : IIntegrationEventHandler<ManuSfcStepEvent>
    {
        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IManuSfcStepServices _manuSfcStepServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messagePushService"></param>
        public ManuSfcStepHander(IManuSfcStepServices  manuSfcStepServices)
        {
            _manuSfcStepServices = manuSfcStepServices;
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task Handle(ManuSfcStepEvent @event)
        {
            await _manuSfcStepServices.AddAsync(@event);
        }
    }
}
