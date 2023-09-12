using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Command
{
    /// <summary>
    /// 出站更新
    /// </summary>
    public class MultiUpdateSummaryOutStationCommand : UpdateCommand
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
        /// 结束时间
        /// </summary>
        public DateTime EndOn { get; set; }
    }
}
