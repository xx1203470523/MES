using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents
{
    /// <summary>
    /// 新增步骤表实体
    /// </summary>
    public record ManuSfcStepEvent : IntegrationEvent
    {
        public ManuSfcStepEntity manuSfcStep { get; set; }
    }
}
