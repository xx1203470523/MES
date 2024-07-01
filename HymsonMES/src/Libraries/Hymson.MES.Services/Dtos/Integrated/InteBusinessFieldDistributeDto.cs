using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 字段分配管理新增/更新Dto
    /// </summary>
    public record InteBusinessFieldDistributeSaveDto : BaseEntityDto
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
        /// 类型 1、装配，2、不合格，3、包装
        /// </summary>
        public FieldAssignmentTypeEnum Type { get; set; }

       /// <summary>
        /// 分配类型编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段定义分配详情数据
        /// </summary>
        public IList<InteBusinessFieldDistributeDetailsCreateDto>? InteBusinessFieldDistributeDetailList { get; set; }
    }
    /// <summary>
    /// 字段定义列表数据新增Dto
    /// </summary>
    public record InteBusinessFieldDistributeDetailsCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// 字段定义Id
        /// </summary>
        public long BusinessFieldId { get; set; }

        /// <summary>
        /// 字段分配管理id 
        /// </summary>
        public long BusinessFieldFistributeid { get; set; }

    }
    /// <summary>
    /// 字段分配管理Dto
    /// </summary>
    public record InteBusinessFieldDistributeDto : BaseEntityDto
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
        /// 类型 1、装配，2、不合格，3、包装
        /// </summary>
        public FieldAssignmentTypeEnum Type { get; set; }

       /// <summary>
        /// 分配类型编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 字段分配管理分页Dto
    /// </summary>
    public class InteBusinessFieldDistributePagedQueryDto : PagerInfo
    {

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 开始时间  数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 类型 1、装配，2、不合格，3、包装
        /// </summary>
        public FieldAssignmentTypeEnum? Type { get; set; }
    }

}
