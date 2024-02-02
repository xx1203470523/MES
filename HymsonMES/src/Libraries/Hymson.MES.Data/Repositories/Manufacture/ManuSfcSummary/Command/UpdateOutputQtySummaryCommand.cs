using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 合格产出更新
    /// </summary>
    public class UpdateOutputQtySummaryCommand : UpdateCommand
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产出数量
        /// </summary>
        public decimal OutputQty { get; set; }


        /// <summary>
        /// 是否复判
        /// </summary>
        public bool IsJudgment { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndOn { get; set; }
    }

    /// <summary>
    /// 不合格出站更新
    /// </summary>
    public class MultiUpdateSummaryUnqualifiedCommand : UpdateCommand
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产出数量
        /// </summary>
        public decimal UnqualifiedQty { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndOn { get; set; }
    }

    /// <summary>
    /// 复判不合格
    /// </summary>
    public class MultiUpdateSummaryReJudgmentUnqualifiedCommand : UpdateCommand
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产出数量
        /// </summary>
        public decimal UnqualifiedQty { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime JudgmentOn { get; set; }
    }

    /// <summary>
    /// 复判合格
    /// </summary>
    public class MultiUpdateSummaryReJudgmentQualifiedCommand : UpdateCommand
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime JudgmentOn { get; set; }
    }

}
