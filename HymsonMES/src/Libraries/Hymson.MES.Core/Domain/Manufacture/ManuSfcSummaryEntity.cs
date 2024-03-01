using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（条码工序生产汇总表）   
    /// manu_sfc_summary
    /// @author wangkeming
    /// @date 2023-09-08 05:07:06
    /// </summary>
    public class ManuSfcSummaryEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 工单号
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 产品信息
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

       /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartOn { get; set; }

       /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndOn { get; set; }

        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal? InvestQty { get; set; }

        /// <summary>
        /// 产出数量
        /// </summary>
        public decimal? OutputQty { get; set; }

       /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal? UnqualifiedQty { get; set; }

       /// <summary>
        /// 复投次数
        /// </summary>
        public int? RepeatedCount { get; set; }

       /// <summary>
        /// 是否判定 0、否 1、 是
        /// </summary>
        public bool? IsJudgment { get; set; }

       /// <summary>
        /// 复判时间
        /// </summary>
        public DateTime? JudgmentOn { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedOn { get; set; }
    }
}
