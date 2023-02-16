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
        /// 
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
        public string Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string Degree { get; set; }

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
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }
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
