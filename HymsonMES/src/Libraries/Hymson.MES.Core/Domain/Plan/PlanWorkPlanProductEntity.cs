using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 数据实体对象（生产计划产品）
    /// @author Czhipu
    /// @date 2024-06-16
    /// </summary>
    public partial class PlanWorkPlanProductEntity : BaseEntity
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 生产计划ID
        /// </summary>
        public long WorkPlanId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// BOM编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// BOM版本
        /// </summary>
        public string BomVersion { get; set; }

        /// <summary>
        /// bomId
        /// </summary>
        public long BomId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
