using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码信息表，数据实体对象   
    /// manu_sfc_info
    /// @author zhaoqing
    /// @date 2023-03-18 05:40:43
    /// </summary>
    public class ManuSfcInfoEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public long SfcId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// Bom id
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        public bool IsUsed { get; set; }
    }
}
