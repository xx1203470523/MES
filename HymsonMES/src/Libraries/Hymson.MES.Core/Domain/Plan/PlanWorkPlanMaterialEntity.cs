using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 数据实体对象（生产计划物料）
    /// @author Czhipu
    /// @date 2024-06-16
    /// </summary>
    public partial class PlanWorkPlanMaterialEntity : BaseEntity
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
        /// 生产计划产品ID
        /// </summary>
        public long WorkPlanProductId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料编号 
        /// </summary>
        public string MaterialCode { get; set; } = "";

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; } = "";

        /// <summary>
        /// BomId
        /// </summary>
        public long? BomId { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 损耗
        /// </summary>
        public decimal Loss { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
