using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Services.Dtos.Marking
{
    /// <summary>
    /// Marking拦截汇总表Dto
    /// </summary>
    public record MarkingRecordReportDto : BaseEntityDto
    {


       /// <summary>
        /// 产品序列码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 条码所对应的资源
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 工单对应的工作中心
        /// </summary>
        public string? WorkCenterName { get; set; }

        /// <summary>
        /// 工单名称
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// 发现工序名称
        /// </summary>
        public string? FindProcedureName { get; set; }

        /// <summary>
        /// 设定拦截工序名称
        /// </summary>
        public string? AppointInterceptProcedureName { get; set; }

        /// <summary>
        /// 实际拦截工序名称
        /// </summary>
        public string? InterceptProcedureName { get; set; }

        /// <summary>
        /// 拦截时间
        /// </summary>
        public DateTime? InterceptOn { get; set; }

        /// <summary>
        /// 拦截设备名称
        /// </summary>
        public string? InterceptEquipmentName { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string? UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 不合格状态
        /// </summary>
        public string? UnqualifiedStatus { get; set; }

        /// <summary>
        /// 不合格类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum? UnqualifiedType { get; set; }

        /// <summary>
        /// 产品条码数量
        /// </summary>
        public string? Qty { get; set; }

        /// <summary>
        /// Marking录入人员
        /// </summary>
        public string? MarkingCreatedBy { get; set; }

        /// <summary>
        /// Marking录入时间
        /// </summary>
        public DateTime? MarkingCreatedOn { get; set; }

        /// <summary>
        /// Marking关闭人员
        /// </summary>
        public string? MarkingClosedBy { get; set; }

        /// <summary>
        /// Marking关闭时间
        /// </summary>
        public DateTime? MarkingClosedOn { get; set; }



    }

    /// <summary>
    /// Marking拦截汇总表分页Dto
    /// </summary>
    public class MarkingInterceptReportPagedQueryDto : PagerInfo {

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 发现工序Id
        /// </summary>
        public long? FindProcedureId { get; set; }

        /// <summary>
        /// 拦截工序Id
        /// </summary>
        public long? AppointInterceptProcedureId { get; set; }

        /// <summary>
        /// 实际拦截工序Id
        /// </summary>
        public long? InterceptProcedureId { get; set; }

        /// <summary>
        /// 拦截设备Id
        /// </summary>
        public long? InterceptEquipmentId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格状态
        /// </summary>
        public string? UnqualifiedStatus { get; set; }

        /// <summary>
        /// Marking录入时间
        /// </summary>
        public DateTime[]? MarkingCreatedOn { get; set; }

        /// <summary>
        /// Marking录入时间
        /// </summary>
        public DateTime[]? MarkingCloseOn { get; set; }

        /// <summary>
        /// Marking拦截时间
        /// </summary>
        public DateTime[]? InterceptOn { get; set; }    


        /// <summary>
        /// 产品序列码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 录入人员
        /// </summary>
        public string? MarkingCreatedBy { get; set; }

        /// <summary>
        /// 关闭人员
        /// </summary>
        public string? MarkingClosedBy { get; set; }

    }

}
