using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 降级品明细报表Dto
    /// </summary>
    public record ManuDowngradingDetailReportDto : BaseEntityDto
    {
        /// <summary>
        /// 工作中心Code
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 降级编码
        /// </summary>
        public string? DowngradingCode { get; set; }

        /// <summary>
        /// 降级名称
        /// </summary>
        public string? DowngradingName { get; set; }

        /// <summary>
        /// 降级描述
        /// </summary>
        public string? DowngradingRemark { get; set; }

        /// <summary>
        /// 录入人员
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum OrderType { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }


    }

    /// <summary>
    /// 降级品明细报表分页Dto
    /// </summary>
    public class ManuDowngradingDetailReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工作中心Id
        /// </summary>
        public string? WorkCenterId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public string? OrderId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public string? ProductId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public string? ProcedureId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 降级等级
        /// </summary>
        public string? DowngradingCode { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

    }

}
