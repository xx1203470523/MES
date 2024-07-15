using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Events.Quality
{
    /// <summary>
    ///
    /// </summary>
    public record FQCOrderAutoCreateIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserName { get; set; } = "";

        public IEnumerable<FQCOrderAutoCreateIntegration>? RecordDetails { get; set; }
    }

    public record FQCOrderAutoCreateIntegration
    {
        /// <summary>
        /// FinallyOutputRecord Id
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 条码类型(1-托盘 2-栈板 3-SFC 4-箱)
        /// </summary>
        public FQCLotUnitEnum CodeType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
