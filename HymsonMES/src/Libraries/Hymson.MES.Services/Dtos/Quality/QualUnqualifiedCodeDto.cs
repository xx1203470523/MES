using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 不合格代码Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record QualUnqualifiedCodeDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 描述 :类型 
        /// 空值 : true  
        /// </summary>
        public QualUnqualifiedCodeTypeEnum Type { get; set; }

        /// <summary>
        /// 描述 :等级 
        /// 空值 : true  
        /// </summary>
        public QualUnqualifiedCodeDegreeEnum Degree { get; set; }

        /// <summary>
        /// 不合格工艺路线（所属工艺路线ID）
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 拦截工序Id
        /// </summary>
        public long? InterceptProcedureId { get; set; }

        /// <summary>
        /// 拦截工序编码
        /// </summary>
        public string? InterceptProcedureCode { get; set; }

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
        public DateTime? UpdatedOn { get; set; }


    }

    /// <summary>
    /// 
    /// </summary>
    public record UnqualifiedCodeGroupRelationDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格代码组id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }

        /// <summary>
        /// 不合格组
        /// </summary>
        public string UnqualifiedGroup { get; set; }

        /// <summary>
        /// 不合格组名称
        /// </summary>
        public string UnqualifiedGroupName { get; set; }
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
    /// 不合格代码新增Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record QualUnqualifiedCodeCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        ///// <summary>
        ///// 描述 :状态 
        ///// 空值 : true  
        ///// </summary>

        /// <summary>
        /// 类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum? Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public QualUnqualifiedCodeDegreeEnum? Degree { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 不合格代码组
        /// </summary>
        public long[]? UnqualifiedGroupIds { get; set; }
    }

    /// <summary>
    /// 不合格代码更新Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record QualUnqualifiedCodeModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }


        ///// <summary>
        ///// 描述 :状态 
        ///// 空值 : true  
        ///// </summary>

        /// <summary>
        /// 类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum? Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public QualUnqualifiedCodeDegreeEnum? Degree { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }


        /// <summary>
        /// 不合格代码组
        /// </summary>
        public long[]? UnqualifiedGroupIds { get; set; }
    }

    /// <summary>
    /// 不合格代码分页Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string? UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum? Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public QualUnqualifiedCodeDegreeEnum? Degree { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? OrUnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string? OrUnqualifiedCodeName { get; set; }

        /// <summary>
        /// 不合格代码组id
        /// </summary>
        public long? QualUnqualifiedGroupId { get; set; }
    }
}
