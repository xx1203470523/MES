using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BadRecordReportDto : PagerInfo
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 计划开始时间  数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuProductBadRecordReportViewDto
    {
        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 汇总数量
        /// </summary>
        //public decimal Num { get; set; }
        public int Num { get; set; }

        /// <summary>
        /// 描述 :不合格代码 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 描述 :不合格代码名称 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedCodeName { get; set; }
    }

    /// <summary>
    /// 不良报告日志 分页参数
    /// </summary>
    public partial class ManuProductBadRecordLogReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 计划开始时间  数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResourceCode { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum? UnqualifiedType { get; set; }

        /// <summary>
        /// 不良记录状态
        /// </summary>
        public ProductBadRecordStatusEnum? BadRecordStatus { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial record ManuProductBadRecordLogReportViewDto : BaseEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 流出工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 实际NG工序
        /// </summary>
        public string FactProcdedureCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum UnqualifiedType { get; set; }

        /// <summary>
        /// 不良状态
        /// </summary>
        public ProductBadRecordStatusEnum BadRecordStatus { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public record ManuProductBadRecordLogReportRequestDto
    {
        /// <summary>
        /// 不良记录Id
        /// </summary>
        public long BadRecordId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public record ManuProductBadRecordLogReportResponseDto : BaseEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }


}
