using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// Marking信息表新增Dto
    /// </summary>
    public record ManuSfcMarkingSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 发现不良工序
        /// </summary>
        public long FoundBadOperationId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 应拦截工序
        /// </summary>
        public long InterceptProcedureId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    public class MarkingInfoDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Marking信息表Id
        /// </summary>
        public long SfcMarkingId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedName { get; set; }

        /// <summary>
        /// 拦截工序编码
        /// </summary>
        public string InterceptProcedureCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 资源代码
        /// </summary>
        public string ResourceCode { get; set; }
    }

    /// <summary>
    /// Marking信息表Dto
    /// </summary>
    public record ManuSfcMarkingDto : BaseEntityDto
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
        /// 状态(0-关闭 1-开启)
        /// </summary>
        public MarkingStatusEnum Status { get; set; }

        /// <summary>
        /// Marking类型(1-拦截 2-标记)
        /// </summary>
        public MarkingTypeEnum MarkingType { get; set; }

        /// <summary>
        /// 来源(1-直接录入 2-继承)
        /// </summary>
        public MarkingSourceTypeEnum SourceType { get; set; }

        /// <summary>
        /// 父级条码
        /// </summary>
        public string ParentSFC { get; set; }

        /// <summary>
        /// 原始条码
        /// </summary>
        public string OriginalSFC { get; set; }

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
    /// Marking信息表分页Dto
    /// </summary>
    public class ManuSfcMarkingPagedQueryDto : PagerInfo { }

}
