using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// Marking执行表新增/更新Dto
    /// </summary>
    public record ManuSfcMarkingExecuteSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// Marking信息表Id
        /// </summary>
        public long SfcMarkingId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 发现不良工序
        /// </summary>
        public long FoundBadProcedureId { get; set; }

       /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedCodeId { get; set; }

       /// <summary>
        /// 应拦截工序
        /// </summary>
        public long ShouldInterceptProcedureId { get; set; }

       /// <summary>
        /// Marking类型(1-拦截 2-只标记)
        /// </summary>
        public bool MarkingType { get; set; }

       /// <summary>
        /// 备注
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
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// Marking执行表Dto
    /// </summary>
    public record ManuSfcMarkingExecuteDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// Marking信息表Id
        /// </summary>
        public long SfcMarkingId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 发现不良工序
        /// </summary>
        public long FoundBadProcedureId { get; set; }

       /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedCodeId { get; set; }

       /// <summary>
        /// 应拦截工序
        /// </summary>
        public long ShouldInterceptProcedureId { get; set; }

       /// <summary>
        /// Marking类型(1-拦截 2-只标记)
        /// </summary>
        public bool MarkingType { get; set; }

       /// <summary>
        /// 备注
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
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// Marking执行表分页Dto
    /// </summary>
    public class ManuSfcMarkingExecutePagedQueryDto : PagerInfo { }

}
