/*
 *creator: Karl
 *
 *describe: 生产汇总表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  chenjianxiong
 *build datetime: 2023-06-15 10:37:18
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 生产汇总表，数据实体对象   
    /// manu_sfc_summary
    /// @author chenjianxiong
    /// @date 2023-06-15 10:37:18
    /// </summary>
    public class ManuSfcSummaryEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 当前工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 投入时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 产出时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 复投次数
        /// </summary>
        public int RepeatedCount { get; set; } = 0;

        /// <summary>
        /// 产出数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 复投时NG次数
        /// </summary>
        public int NgNum { get; set; } = 0;

        /// <summary>
        /// 第一次的品质状态;1 第一次合格，0 第一次不合格
        /// </summary>
        public int? FirstQualityStatus { get; set; }

        /// <summary>
        /// 最终品质状态;1 合格，0 不合格
        /// </summary>
        public int? QualityStatus { get; set; }
    }
}
