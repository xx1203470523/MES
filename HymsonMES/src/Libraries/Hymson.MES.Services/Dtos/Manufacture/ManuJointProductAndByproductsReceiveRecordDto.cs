using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 联副产品收货新增/更新Dto
    /// </summary>
    public record ManuJointProductAndByproductsReceiveRecordSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 工单 plan_work_order的id
        /// </summary>
        public long WorkOrderid { get; set; }

        /// <summary>
        /// 产品id  proc_material 的id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 存储地方wh_warehouse 的id
        /// </summary>
        public long? WarehouseId { get; set; }
    }

    /// <summary>
    /// 联副产品收货Dto
    /// </summary>
    public record ManuJointProductAndByproductsReceiveRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }
       
    }

   /// <summary>
   /// 联副产品查询返回信息
   /// </summary>
    public class ManuJointProductAndByproductsReceiveRecordResult
    {

        /// <summary>
        /// 物料/版本
        /// </summary>
        public string ProductCodeVersion {  get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 联产品信息
        /// </summary>
        public List<JointProductResult> JointProductList { get; set;}

        /// <summary>
        /// 副产品信息
        /// </summary>
        public List<ByproductsResult> ByproductsList { get; set;}
    }
    /// <summary>
    /// 联产品信息
    /// </summary>
    public class JointProductResult
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set;}

        /// <summary>
        /// 物料/版本
        /// </summary>
        public string ProductCodeVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 已收货数量
        /// </summary>
        public decimal ReceivedQty {  get; set; }

        /// <summary>
        /// 待收货数量
        /// </summary>
        public decimal QuantityToBeReceived { get; set; }
    }
    /// <summary>
    /// 副产品信息
    /// </summary>
    public class ByproductsResult
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 物料/版本
        /// </summary>
        public string ProductCodeVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName { get; set; }
        
        /// <summary>
        /// 存储地点
        /// </summary>
        public string StorageLocation { get; set; }

        /// <summary>
        /// 已收货数量
        /// </summary>
        public decimal ReceivedQty { get; set; }

        /// <summary>
        /// 待收货数量
        /// </summary>
        public decimal QuantityToBeReceived { get; set; }
    }

    public class ManuJointProductAndByproductsReceiveRecordSaveResultDto
    {
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductCodeVersion { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC {  get; set; }
    }
    /// <summary>
    /// 联副产品收货分页Dto
    /// </summary>
    public class ManuJointProductAndByproductsReceiveRecordPagedQueryDto : PagerInfo { }

}
