using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（OQC检验单检验样本记录）   
    /// qual_oqc_order_sample
    /// @author xiaofei
    /// @date 2024-03-04 10:59:22
    /// </summary>
    public class QualOqcOrderSampleEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// 检验类型Id
        /// </summary>
        public long OQCOrderTypeId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
