/*
 *creator: Karl
 *
 *describe: 发布记录表    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-12-19 10:03:09
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 发布记录表Dto
    /// </summary>
    public record SysReleaseRecordDto : BaseEntityDto
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
        /// 备件编码
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 时间时间
        /// </summary>
        public DateTime? RealTime { get; set; }

        /// <summary>
        /// 状态;1、预留 2、发布
        /// </summary>
        public SysReleaseRecordStatusEnum Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 发布内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 环境类型;1、正式 2、测试
        /// </summary>
        public EnvironmentTypeEnum? EnvironmentType { get; set; }

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
        public long IsDeleted { get; set; }


    }


    /// <summary>
    /// 发布记录表新增Dto
    /// </summary>
    public record SysReleaseRecordCreateDto : BaseEntityDto
    {


        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public string PlanTime { get; set; }

        /// <summary>
        /// 发布内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 环境类型;1、正式 2、测试
        /// </summary>
        public EnvironmentTypeEnum EnvironmentType { get; set; }



    }

    /// <summary>
    /// 发布记录表更新Dto
    /// </summary>
    public record SysReleaseRecordModifyDto : BaseEntityDto
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public string? PlanTime { get; set; }

        /// <summary>
        /// 发布内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 环境类型;1、正式 2、测试
        /// </summary>
        public EnvironmentTypeEnum? EnvironmentType { get; set; }

        /// <summary>
        /// 状态;1、预留 2、发布
        /// </summary>
        public SysReleaseRecordStatusEnum? Status { get; set; }
    }

    /// <summary>
    /// 发布记录表分页Dto
    /// </summary>
    public class SysReleaseRecordPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        ///发布时间 范围 
        /// </summary>
        public DateTime[]? RealTime { get; set; }

        /// <summary>
        /// 状态;1、预留 2、发布
        /// </summary>
        public SysReleaseRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 环境类型;1、正式 2、测试
        /// </summary>
        public EnvironmentTypeEnum? EnvironmentType { get; set; }
    }
}
