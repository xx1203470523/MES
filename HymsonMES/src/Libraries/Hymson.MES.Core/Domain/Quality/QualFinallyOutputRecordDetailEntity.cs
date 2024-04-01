using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（成品条码产出记录明细(FQC生成使用)）   
    /// qual_finally_output_record_detail
    /// @author xiaofei
    /// @date 2024-03-29 03:06:28
    /// </summary>
    public class QualFinallyOutputRecordDetailEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产出记录Id
        /// </summary>
        public long OutputRecordId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
