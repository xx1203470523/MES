using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Plan
{
    /// <summary>
    /// Dto（生产计划）
    /// </summary>
    public record PlanWorkPlanDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 生产计划单号
        /// </summary>
        public string PlanCode { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

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
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// BOM编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// BOM名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long? BomId { get; set; }

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
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 保存对象（生产计划）
    /// </summary>
    public record PlanWorkPlanSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 生产计划Id
        /// </summary>
        public long WorkPlanId { get; set; }

        /// <summary>
        /// 工单明细
        /// </summary>
        public IEnumerable<PlanWorkPlanDetailSaveDto>? Details { get; set; }

    }

    /// <summary>
    /// 保存对象（生产计划）
    /// </summary>
    public record PlanWorkPlanDetailSaveDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanEndTime { get; set; }

    }

    /// <summary>
    /// 分页Dto（生产计划）
    /// </summary>
    public class PlanWorkPlanPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public IEnumerable<PlanWorkOrderStatusEnum>? InStatus { get; set; }

        /// <summary>
        /// 计划开始时间  时间范围  数组
        /// </summary>
        public DateTime[]? PlanStartTime { get; set; }

        /// <summary>
        /// 查询状态集合
        /// </summary>
        public IEnumerable<PlanWorkOrderStatusEnum>? Statuss { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }
    }

}
