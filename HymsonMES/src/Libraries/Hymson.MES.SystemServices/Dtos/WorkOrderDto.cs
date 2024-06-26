using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using System.Text.Json.Serialization;

namespace Hymson.MES.SystemServices.Dtos
{
   
    //public record WorkOrder
    //{
    //    /// <summary>
    //    /// 派工单列表
    //    /// </summary>
    //    public IEnumerable<WorkOrderItem> WorkOrderItems { get; set; }
    //}
    /// <summary>
    /// 转子产线工单同步
    /// </summary>
    public record RotorWorkOrder
    {
        /// <summary>
        /// 工单/排程号
        /// </summary>
        /// 
        [JsonPropertyName("workNo")]
        public string OrderCode { get; set; }

        /// <summary>
        /// 生产计划编码
        /// </summary>
        /// 
        [JsonPropertyName("orderNo")]
        public string PlanWorkOrder { get; set; }
        /// <summary>
        /// 型号号+_+版本号
        /// </summary>
        /// 
        [JsonPropertyName("productTypeNO")]
        public string productTypeNO { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        /// 
        [JsonPropertyName("itemNo")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 工作中心（车间或者线体）
        /// </summary>
        /// 
        [JsonPropertyName("workTotal")]
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        ///
        [JsonPropertyName("workTotal")]
        public decimal Qty { get; set; }


        /// <summary>
        /// 计划开始时间
        /// </summary>
        /// 
        [JsonPropertyName("planTime")]
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        /// 
        [JsonPropertyName("endTime")]
        public DateTime? PlanEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("enable")]
        public bool Enable { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonPropertyName("versionNo")]
        public string VersionNo { get; set; } = "1";

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
}
