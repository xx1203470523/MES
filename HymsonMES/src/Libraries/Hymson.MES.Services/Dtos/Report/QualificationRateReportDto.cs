using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.QualificationRateReport
{
    /// <summary>
    /// 合格率报表Dto
    /// </summary>
    public record QualificationRateReportDto : BaseEntityDto
    {
       /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

       /// <summary>
        /// 产品信息
        /// </summary>
        public string MaterialName { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public string ProcedureName { get; set; }

       /// <summary>
        /// 开始时间
        /// </summary>
        public string StartOn { get; set; }

       /// <summary>
        /// 结束时间
        /// </summary>
        public string EndOn { get; set; }

       /// <summary>
        /// 合格数
        /// </summary>
        public decimal QualifiedQuantity { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal UnQualifiedQuantity { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        public decimal QualifiedRate { get; set; }
    }

    /// <summary>
    /// 合格率报表分页Dto
    /// </summary>
    public class QualificationRateReportPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set;}

        /// <summary>
        /// 工序Ids
        /// </summary>
        public long[]? ProcedureIds { get; set;}

        /// <summary>
        /// 查询日期类型（日月年）
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime[]? Date { get; set;}
    }

}
