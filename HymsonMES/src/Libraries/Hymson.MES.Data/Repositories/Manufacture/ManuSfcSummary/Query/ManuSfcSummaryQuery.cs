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
        /// 工序Id集合
        /// </summary>
        public long[]? ProcedureIds { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备Ids
        /// </summary>
        public long[]? EquipmentIds { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string[] SFCS { get; set; }

        /// <summary>
        /// 最终状态
        /// </summary>
        public int? QualityStatus { get; set; }

        /// <summary>
        /// 是否补料
        /// </summary>
        public int? IsReplenish { get; set; }

        /// <summary>
        /// 第一次合格状态
        /// </summary>
        public int? FirstQualityStatus { get; set; }

        /// <summary>
        ///生产开始时间
        ///CreatedOn
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        ///生产结束时间
        ///CreatedOn
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
