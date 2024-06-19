using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（联副产品收货）   
    /// manu_joint_product_and_byproducts_receive_record
    /// @author User
    /// @date 2024-06-05 02:15:16
    /// </summary>
    public class ManuJointProductAndByproductsReceiveRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工单 plan_work_order的id
        /// </summary>
        public long WorkOrderid { get; set; }

       /// <summary>
        /// 产品id  proc_material 的id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 存储地方wh_warehouse 的id
        /// </summary>
        public long? WarehouseId { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
