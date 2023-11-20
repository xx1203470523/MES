using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuSfcProduceVehiclePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        ///产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[]? SfcArray { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 载具Id
        /// </summary>
        public long? VehicleId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum? IsScrap { get; set; }
    }
}
