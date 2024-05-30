using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class WorkOrderStepControlView:BaseEntity
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 条码下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal UnQualifiedQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderStepControlOptimizeView : BaseEntity
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 条码下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal UnQualifiedQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
