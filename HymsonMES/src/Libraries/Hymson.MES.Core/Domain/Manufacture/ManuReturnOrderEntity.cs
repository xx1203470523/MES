using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（生产退料单）   
    /// manu_return_order
    /// @author wxk
    /// @date 2024-06-22 02:24:51
    /// </summary>
    public class ManuReturnOrderEntity : BaseEntity
    {
        /// <summary>
        /// 退料单据号
        /// </summary>
        public string ReqOrderCode { get; set; }

        /// <summary>
        /// 工单id，工单借料情况下，接收物料的工单
        /// </summary>
        public string TargetWorkOrderCode { get; set; }

        /// <summary>
        /// 退料单类型0:工单退料 1:工单借料
        /// </summary>
        public ManuReturnTypeEnum Type { get; set; }

        /// <summary>
        /// 状态0:审批中，1：审批失败，2：审批成功3.已退料
        /// </summary>
        public WhWarehouseReturnStatusEnum Status { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工单id，待退料的工单
        /// </summary>
        public long SourceWorkOrderId { get; set; }

        /// <summary>
        /// 工单id，待退料的工单
        /// </summary>
        public string SourceWorkOrderCode { get; set; }


    }
}
