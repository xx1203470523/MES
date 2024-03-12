using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（OQC检验单）   
    /// qual_oqc_order
    /// @author xiaofei
    /// @date 2024-03-04 10:53:43
    /// </summary>
    public class QualOqcOrderEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// OQC检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// 出货单明细Id
        /// </summary>
        public long ShipmentMaterialId { get; set; }

        /// <summary>
        /// 出货数量
        /// </summary>
        public decimal ShipmentQty { get; set; }

        /// <summary>
        /// 接收水准
        /// </summary>
        public int AcceptanceLevel {  get; set; }

        /// <summary>
        /// 状态(1-待检验 2-检验中 3-已检验 4-已关闭)
        /// </summary>
        public InspectionStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
