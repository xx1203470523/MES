/*
 *creator: Karl
 *
 *describe: 环境检验单    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.QualEnvOrder
{
    /// <summary>
    /// 环境检验单Dto
    /// </summary>
    public record QualEnvOrderDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// 环境检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 工作中心Code
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心Name
        /// </summary>
        public string? WorkCenterName { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序Code
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public string? ProcedureName { get; set; }

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
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 环境检验单新增Dto
    /// </summary>
    public record QualEnvOrderCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 环境检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 是否触发异常
        /// </summary>
        public bool? IsAbnormal { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// 环境检验单更新Dto
    /// </summary>
    public record QualEnvOrderModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 环境检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 是否触发异常
        /// </summary>
        public bool? IsAbnormal { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }



    }

    /// <summary>
    /// 环境检验单分页Dto
    /// </summary>
    public class QualEnvOrderPagedQueryDto : PagerInfo
    {

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工作中心Code
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心Name
        /// </summary>
        public string? WorkCenterName { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工序Code
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序Name
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 检验日期
        /// </summary>
        public DateTime[]? InspectionDate { get; set; }
    }
}
