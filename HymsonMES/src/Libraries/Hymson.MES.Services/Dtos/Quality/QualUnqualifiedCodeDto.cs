using Hymson.Infrastructure;

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
        /// 状态
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public short Degree { get; set; }

        /// <summary>
        /// 不合格工艺路线（所属工艺路线ID）
        /// </summary>
        public string ProcessRouteId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

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

        /// <summary>
        /// 不合代码关联不合格代码组
        /// </summary>
        public List<UnqualifiedCodeGroupRelationDto> UnqualifiedCodeGroupRelationList { get; set; }
    }

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

        // <summary>
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

        /// <summary>
        /// 类型
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public short Degree { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 不合格代码组
        /// </summary>
        public List<long> UnqualifiedGroupIds { get; set; }
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

        /// <summary>
        /// 状态
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public short Degree { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 不合格代码组
        /// </summary>
        public long[] UnqualifiedGroupIds { get; set; }
    }

    /// <summary>
    /// 不合格代码分页Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodePagedQueryDto : PagerInfo
    {
        // <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public short? Type { get; set; }
    }
}
