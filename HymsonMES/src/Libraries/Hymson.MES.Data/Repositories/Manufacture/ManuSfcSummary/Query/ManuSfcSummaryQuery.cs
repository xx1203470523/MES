/*
 *creator: Karl
 *
 *describe: 生产汇总表 查询类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-06-15 10:37:18
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产汇总表 查询参数
    /// </summary>
    public class ManuSfcSummaryQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string[] SFCS { get;set; }
    }
}
