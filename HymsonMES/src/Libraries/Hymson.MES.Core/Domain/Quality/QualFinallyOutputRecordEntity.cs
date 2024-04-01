using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（成品条码产出记录(FQC生成使用)）   
    /// qual_finally_output_record
    /// @author xiaofei
    /// @date 2024-03-29 03:06:15
    /// </summary>
    public class QualFinallyOutputRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 条码类型(1-托盘 2-栈板 3-SFC 4-箱)
        /// </summary>
        public FQCLotUnitEnum CodeType { get; set; }

        /// <summary>
        /// 是否已生成过检验单(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum IsGenerated { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
