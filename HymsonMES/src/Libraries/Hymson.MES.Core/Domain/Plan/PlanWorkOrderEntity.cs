using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 工单信息表，数据实体对象   
    /// plan_work_order
    /// @author Karl
    /// @date 2023-03-20 09:39:21
    /// </summary>
    public class PlanWorkOrderEntity : BaseEntity
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工作中心类型;和工作中心保持一致
        /// </summary>
        public WorkCenterTypeEnum? WorkCenterType { get; set; }

        /// <summary>
        /// 工作中心（车间或者线体）
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long ProductBOMId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum Status { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanEndTime { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 是否锁定   
        /// 废弃使用，将锁定状态放到工单状态里
        /// </summary>
        public YesOrNoEnum? IsLocked { get; set; }

        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public int? ValidTime { get; set; }

        /// <summary>
        /// 锁定状态时锁定前的工单状态
        /// </summary>
        public PlanWorkOrderStatusEnum? LockedStatus { get; set; }

        /// <summary>
        /// 生产计划id
        /// </summary>
        public long? WorkPlanId { get; set; }

    }
}
