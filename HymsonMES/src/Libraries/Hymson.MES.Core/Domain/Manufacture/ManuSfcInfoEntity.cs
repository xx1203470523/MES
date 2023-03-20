/*
 *creator: Karl
 *
 *describe: 条码信息表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:40:43
 */
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
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 状态;1：在制；2：完成；3：已入库；4：报废；
        /// </summary>
        public bool? Status { get; set; }

       /// <summary>
        /// 是否在用
        /// </summary>
        public long? IsUse { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
