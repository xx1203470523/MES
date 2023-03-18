/*
 *creator: Karl
 *
 *describe: 条码消耗表（bom消耗）    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:40:30
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码消耗表（bom消耗），数据实体对象   
    /// manu_sfc_consume
    /// @author zhaoqing
    /// @date 2023-03-18 05:40:30
    /// </summary>
    public class ManuSfcConsumeEntity : BaseEntity
    {
        /// <summary>
        /// 条码信息
        /// </summary>
        public long Sfc { get; set; }

       /// <summary>
        /// 产品id
        /// </summary>
        public string ProductId { get; set; }

       /// <summary>
        /// 扣料工序id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 扣料资源id
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 扣料设备id
        /// </summary>
        public long? EquipmentId { get; set; }

       /// <summary>
        /// 扣料上料点id
        /// </summary>
        public long? FeedingPointId { get; set; }

       /// <summary>
        /// 消耗条码
        /// </summary>
        public string ConsumeBarCode { get; set; }

       /// <summary>
        /// 消耗产品id
        /// </summary>
        public string ConsumeProductId { get; set; }

       /// <summary>
        /// 消耗数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
