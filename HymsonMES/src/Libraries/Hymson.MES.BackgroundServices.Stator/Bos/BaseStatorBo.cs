using Hymson.Utils;

namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 核心服务层基类
    /// </summary>
    public class BaseStatorBo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string User { get; set; } = "";

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();

        /// <summary>
        /// 产线 ID
        /// </summary>
        public long WorkLineId { get; set; }

        /// <summary>
        /// 工单 ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品 ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// BOM ID
        /// </summary>
        public long ProductBOMId { get; set; }

        /// <summary>
        /// 工艺路线 ID
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序 ID
        /// </summary>
        public long ProcedureId { get; set; }

    }

}
