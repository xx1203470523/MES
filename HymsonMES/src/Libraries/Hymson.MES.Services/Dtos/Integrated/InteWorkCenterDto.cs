using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 工作中心表Dto
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public record InteWorkCenterDto : BaseEntityDto
    {
        /// <summary>
        ///  
        /// </summary>
        public long Id { get; set; }

        /// <summary>   
        /// 站点Id 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工作中心代码 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工作中心名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型(工厂/车间/产线) 
        /// </summary>
        public WorkCenterTypeEnum? Type { get; set; }

        /// <summary>
        /// 数据来源 
        /// </summary>
        public WorkCenterSourceEnum? Source { get; set; }

        /// <summary>
        /// 状态 
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否混线 
        /// </summary>
        public bool IsMixLine { get; set; }

        /// <summary>
        /// 线体编码，条码生成使用
        /// </summary>
        public string? LineCoding { get; set; }
        /// <summary>
        /// 说明 
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人 
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人 
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间 
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除 
        /// </summary>
        public byte IsDeleted { get; set; }
    }

    /// <summary>
    /// 工作中心关联工作中心
    /// </summary>
    public record InteWorkCenterRelationDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父级工作中心id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 子集工作中心id
        /// </summary>
        public long SubWorkCenterId { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 工作中心关联资源
    /// </summary>
    public record InteWorkCenterResourceRelationDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工作中心id
        /// </summary>
        public long WorkCenterId { get; set; }


        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

    }

    /// <summary>
    /// 工作中心表新增Dto
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public record InteWorkCenterCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 工作中心代码 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工作中心名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型(工厂/车间/产线) 
        /// </summary>
        public WorkCenterTypeEnum? Type { get; set; }

        /// <summary>
        /// 数据来源 
        /// </summary>
        public WorkCenterSourceEnum? Source { get; set; }

        /// <summary>
        /// 是否混线 
        /// </summary>
        public bool IsMixLine { get; set; }

        /// <summary>
        /// 说明 
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 线体编码 条码规则生成会使用到
        /// </summary>
        public string? LineCoding { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public List<long>? WorkCenterIds { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public List<long>? ResourceIds { get; set; }
    }

    /// <summary>
    /// 工作中心表更新Dto
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public record InteWorkCenterModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工作中心名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型(工厂/车间/产线) 
        /// </summary>
        public WorkCenterTypeEnum? Type { get; set; }

        /// <summary>
        /// 数据来源 
        /// </summary>
        public WorkCenterSourceEnum? Source { get; set; }

        /// <summary>
        /// 是否混线 
        /// </summary>
        public bool? IsMixLine { get; set; }
        /// <summary>
        /// 线体编码 条码规则生成会使用到
        /// </summary>
        public string? LineCoding { get; set; }
        /// <summary>
        /// 说明 
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public List<long>? WorkCenterIds { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public List<long>? ResourceIds { get; set; }
    }

    /// <summary>
    /// 工作中心表分页Dto
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class InteWorkCenterPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工作中心代码 
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工作中心名称 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 类型(工厂/车间/产线) 
        /// </summary>
        public WorkCenterTypeEnum? Type { get; set; }

        /// <summary>
        /// 数据来源 
        /// </summary>
        public WorkCenterSourceEnum? Source { get; set; }

        /// <summary>
        /// 状态 
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 根据类型查询列表
    /// </summary>
    public class QueryInteWorkCenterByTypeAndParentIdDto
    {
        /// <summary>
        /// 类型(工厂/车间/产线) 
        /// </summary>
        public WorkCenterTypeEnum? Type { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public long? ParentId { get; set; }
    }
}
