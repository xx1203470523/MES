using Hymson.Infrastructure;

namespace Hymson.MES.SystemServices.Dtos.Plan
{
    /// <summary>
    /// 订单同步Dto
    /// </summary>
    public record PlanWorkOrderDto : BaseEntityDto
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanEndTime { get; set; }
    }
}
