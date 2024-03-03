/*
 *creator: Karl
 *
 *describe: 操作面板    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:05:24
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 操作面板Dto
    /// </summary>
    public record ManuFacePlateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型;1、生产过站；2、在制品维修
        /// </summary>
        public ManuFacePlateTypeEnum Type { get; set; }

        /// <summary>
        /// 状态;1、新建、2、启用、3、保留、4、废除；
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 会话时间（分钟）
        /// </summary>
        public int ConversationTime { get; set; }

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
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }


    /// <summary>
    /// 操作面板新增Dto
    /// </summary>
    public record ManuFacePlateCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型;1、生产过站；2、在制品维修
        /// </summary>
        public ManuFacePlateTypeEnum Type { get; set; }

        /// <summary>
        /// 会话时间（分钟）
        /// </summary>
        public int ConversationTime { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string? ContainerCode { get; set; }
    }

    /// <summary>
    /// 操作面板更新Dto
    /// </summary>
    public record ManuFacePlateModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型;1、生产过站；2、在制品维修
        /// </summary>
        public ManuFacePlateTypeEnum Type { get; set; }

        /// <summary>
        /// 会话时间（分钟）
        /// </summary>
        public int ConversationTime { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string? ContainerCode { get; set; }
    }

    /// <summary>
    /// 操作面板分页Dto
    /// </summary>
    public class ManuFacePlatePagedQueryDto : PagerInfo
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
        /// 类型
        /// </summary>
        public ManuFacePlateTypeEnum? Type { get; set; }

        /// <summary>
        /// 状态;0-新建 1-启用 2-保留3-废弃
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }

    /// <summary>
    /// 面板查询DTO
    /// </summary>
    public class ManuFacePlateQueryDto
    {
        /// <summary>
        /// 面板信息
        /// </summary>
        public ManuFacePlateDto FacePlate { get; set; }

        /// <summary>
        /// 面板Production
        /// </summary>
        public ManuFacePlateProductionDto FacePlateProduction { get; set; }

        /// <summary>
        /// 在制品维修
        /// </summary>
        public ManuFacePlateRepairDto FacePlateRepair { get; set; }

        /// <summary>
        /// 容器包装配置信息
        /// </summary>
        public ManuFacePlateContainerPackDto FacePlateContainerPack { get; set; }

        /// <summary>
        /// 按钮信息
        /// </summary>
        public ManuFacePlateButtonDto[] ManuFacePlateButtons { get; set; }
    }

    /// <summary>
    /// 面板新增Dto
    /// </summary>
    public record AddManuFacePlateDto : BaseEntityDto
    {
        /// <summary>
        /// 面板信息
        /// </summary>
        public ManuFacePlateCreateDto FacePlate { get; set; }

        /// <summary>
        /// 面板Production
        /// </summary>
        public ManuFacePlateProductionCreateDto FacePlateProduction { get; set; }

        /// <summary>
        /// 面板Repair
        /// </summary>
        public ManuFacePlateRepairCreateDto FacePlateRepair { get; set; }

        /// <summary>
        /// 容器包装配置信息
        /// </summary>
        public ManuFacePlateContainerPackCreateDto FacePlateContainerPack { get; set; }

        /// <summary>
        /// 按钮配置信息
        /// </summary>
        public List<ManuFacePlateButtonCreateDto>? FacePlateButtonList { get; set; }
    }

    /// <summary>
    /// 面板修改Dto
    /// </summary>
    public record UpdateManuFacePlateDto : BaseEntityDto
    {
        /// <summary>
        /// 面板信息
        /// </summary>
        public ManuFacePlateModifyDto FacePlate { get; set; }

        /// <summary>
        /// 面板Production
        /// </summary>
        public ManuFacePlateProductionModifyDto FacePlateProduction { get; set; }

        /// <summary>
        /// 面板Repair
        /// </summary>
        public ManuFacePlateRepairModifyDto FacePlateRepair { get; set; }

        /// <summary>
        /// 容器包装配置信息
        /// </summary>
        public ManuFacePlateContainerPackModifyDto FacePlateContainerPack { get; set; }

        /// <summary>
        /// 按钮配置信息
        /// </summary>
        public List<ManuFacePlateButtonModifyDto>? FacePlateButtonList { get; set; }
    }

}
