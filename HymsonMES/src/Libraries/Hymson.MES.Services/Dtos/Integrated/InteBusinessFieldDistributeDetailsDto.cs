using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 字段分配管理详情新增/更新Dto
    /// </summary>
    public record InteBusinessFieldDistributeDetailsSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 字段分配管理id
        /// </summary>
        public long BusinessFieldFistributeid { get; set; }

       /// <summary>
        /// 字段id
        /// </summary>
        public long BusinessFieldId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 是否必填 1是 2、否
        /// </summary>
        public bool? IsRequired { get; set; }

       
    }

    /// <summary>
    /// 字段分配管理详情Dto
    /// </summary>
    public record InteBusinessFieldDistributeDetailsDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 字段分配管理id
        /// </summary>
        public long BusinessFieldFistributeid { get; set; }

       /// <summary>
        /// 字段id
        /// </summary>
        public long BusinessFieldId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 是否必填 1是 2、否
        /// </summary>
        public bool? IsRequired { get; set; }

       
    }

    /// <summary>
    /// 字段定义掩码规则View
    /// </summary>
    public record BusinessFieldViewDto : InteBusinessFieldDistributeDetailsDto
    {
        public string BusinessFieldCode { get; set; } = "";

        public string BusinessFieldName { get; set; } = "";
    }

    /// <summary>
    /// 字段分配管理详情分页Dto
    /// </summary>
    public class InteBusinessFieldDistributeDetailsPagedQueryDto : PagerInfo { }

}
