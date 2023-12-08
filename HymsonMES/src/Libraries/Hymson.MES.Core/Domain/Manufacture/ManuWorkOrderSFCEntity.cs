using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（工单条码记录表）   
    /// manu_workorder_sfc
    /// @author Czhipu
    /// @date 2023-12-08 11:05:03
    /// </summary>
    public class ManuWorkOrderSFCEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 状态: 1：排队中 2：活动中 3：完成-在制 4：完成 5：锁定 6：报废 7：删除
        /// </summary>
        public SfcStatusEnum Status { get; set; }


    }
}
