/*
 *creator: Karl
 *
 *describe: 生产领料单明细    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-05 03:48:53
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 生产领料单明细，数据实体对象   
    /// manu_requistion_order_detail
    /// @author zhaoqing
    /// @date 2023-07-05 03:48:53
    /// </summary>
    public class ManuRequistionOrderDetailEntity : BaseEntity
    {
        /// <summary>
        /// 所属领料单Id
        /// </summary>
        public long RequistionOrderId { get; set; }

       /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

       /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

       /// <summary>
        /// 物料批次
        /// </summary>
        public string Batch { get; set; }

       /// <summary>
        /// 领料数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 仓库id
        /// </summary>
        public long? WarehouseId { get; set; }

       /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

       /// <summary>
        /// 物料的有效期（过期时间)
        /// </summary>
        public DateTime ExpirationDate { get; set; }

       /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
