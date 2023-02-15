using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 不合格代码组Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record QualUnqualifiedGroupDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格组
        /// </summary>
        public string UnqualifiedGroup { get; set; }

        /// <summary>
        /// 不合格组名称
        /// </summary>
        public string UnqualifiedGroupName { get; set; }

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
    /// 不合格代码组新增Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record QualUnqualifiedGroupCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格组
        /// </summary>
        public string UnqualifiedGroup { get; set; }

        /// <summary>
        /// 不合格组名称
        /// </summary>
        public string UnqualifiedGroupName { get; set; }

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
    /// 不合格代码组更新Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record QualUnqualifiedGroupModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格组
        /// </summary>
        public string UnqualifiedGroup { get; set; }

        /// <summary>
        /// 不合格组名称
        /// </summary>
        public string UnqualifiedGroupName { get; set; }

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
    /// 不合格代码组查询Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedGroupPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
    }
}
