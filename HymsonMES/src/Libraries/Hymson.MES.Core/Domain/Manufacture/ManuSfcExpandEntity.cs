using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码扩展信息，数据实体对象   
    /// manu_sfc_expand
    /// @author wangkeming
    /// @date 2023-03-25 04:17:40
    /// </summary>
    public class ManuSfcExpandEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public long SfcInfoId { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 是否在用
        /// </summary>
        public bool IsUsed { get; set; }
    }
}
