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
        /// 因为用Status，添加表时审核平台不允许，因此改名为SFCStatus，等同于步骤表的Status
        /// </summary>
        public SfcStatusEnum SFCStatus { get; set; }


    }
}
