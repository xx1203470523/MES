using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;

namespace Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents
{
    /// <summary>
    /// 打印事件
    /// </summary>
    public record PrintIntegrationEvent : IntegrationEvent
    {
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 指定打印机
        /// </summary>
        public long? PrintId { get; set; }

        /// <summary>
        /// 指定模版Id
        /// </summary>
        public long? PrintTemplateId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<LabelTemplateBarCodeDto> BarCodes { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }
    }
}
