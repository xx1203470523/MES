using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents
{
    /// <summary>
    /// 批量新增步骤表实体
    /// </summary>
    public record ManuSfcStepsEvent : IntegrationEvent
    {
        public IEnumerable<ManuSfcStepEntity> manuSfcStepEntities { get; set; }
    }
}
