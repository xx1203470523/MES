using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Plan
{
    /// <summary>
    /// 工单信息表Dto
    /// </summary>
    public record PlanWorkOrderDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long? ProductBOMId { get; set; }

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
    /// 工单信息表新增Dto
    /// </summary>
    public record PlanWorkOrderCreateDto : BaseEntityDto
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
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long? ProductBOMId { get; set; }

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
        public PlanWorkOrderStatusEnum Status { get; set; } = PlanWorkOrderStatusEnum.NotStarted;

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
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 工单信息表更新Dto
    /// </summary>
    public record PlanWorkOrderModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long? ProductBOMId { get; set; }

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
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 工单信息表分页Dto
    /// </summary>
    public class PlanWorkOrderPagedQueryDto : PagerInfo
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
        /// 领料状态 1：未领料；2：已领料；3：部分领料
        /// </summary>
        public PlanWorkOrderPickStatusEnum? PickStatus { get; set; }

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

        /// <summary>
        /// 同步单号
        /// </summary>
        public string? PickCode { get; set; }

        /// <summary>
        /// 线体ID
        /// </summary>
        public long? LineId { get; set; }

    }

    /// <summary>
    /// 工单信息表Dto
    /// </summary>
    public record PlanWorkOrderDetailViewDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 派工单数量
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


        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string? MaterialUnit { get; set; }

        /// <summary>
        /// bom编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// bom版本
        /// </summary>
        public string BomVersion { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string ProcessRouteCode { get; set; }
        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string ProcessRouteVersion { get; set; }

        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 汇总数量：按照所有仓库汇总所有入库状态下数量
        /// </summary>
        public decimal SumQty { get; set; }

        /// <summary>
        /// 入库数量：按照入库的仓库分别计算该仓库的数量，入库状态排除：取消申请的【待检验仓入库数量】
        /// </summary>
        public decimal ToBeTestQty { get; set; }

        /// <summary>
        /// 入库数量：按照入库的仓库分别计算该仓库的数量，入库状态排除：取消申请的【成品仓入库数量】
        /// </summary>
        public decimal FinishQty { get; set; }

        /// <summary>
        /// 入库数量：按照入库的仓库分别计算该仓库的数量，入库状态排除：取消申请的【不良品仓入库数量】
        /// </summary>
        public decimal BadQty { get; set; }
    }

    /// <summary>
    /// 工单信息表Dto
    /// </summary>
    public record PlanWorkOrderListDetailViewDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long? ProductBOMId { get; set; }

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
        /// 领料状态 0：未领料；1：已领料；2：部分领料
        /// </summary>
        public PlanWorkOrderPickStatusEnum PickStatus { get; set; }

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
        /// 下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// bom编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// bom版本
        /// </summary>
        public string BomVersion { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string ProcessRouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string ProcessRouteName { get; set; }

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string ProcessRouteVersion { get; set; }

        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal InputQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStart { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? RealEnd { get; set; }
    }

    /// <summary>
    /// 更改工单状态
    /// </summary>
    public record PlanWorkOrderChangeStatusDto
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工单状态
        /// 需要改变为什么状态
        /// </summary>
        public PlanWorkOrderStatusEnum Status { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public record PlanWorkOrderLockedDto
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 是否锁定
        /// 需要改变为 锁定/解锁
        /// 20230609 克明又要求改成原来的：使用工单状态保存是否锁定状态，不使用isLocked来保存了,
        /// 但是这个传值参数先使用着
        /// </summary>
        public YesOrNoEnum IsLocked { get; set; }
    }

    /// <summary>
    /// 更改工单计划数
    /// </summary>
    public class EditPlanWorkOrderDto
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 修改的计划数量
        /// </summary>
        public decimal PlanQty {  get; set; }
    }
}
