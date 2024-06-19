/*
 *creator: Karl
 *
 *describe: 设备点检模板    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Services.Dtos.Qual;

namespace Hymson.MES.Services.Dtos.EquSpotcheckTemplate
{
    /// <summary>
    /// 设备点检模板Dto
    /// </summary>
    public record EquSpotcheckTemplateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点检模板编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述
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
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }


    /// <summary>
    /// 设备点检模板新增Dto
    /// </summary>
    public record EquSpotcheckTemplateCreateDto : BaseEntityDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点检模板编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 模板与项目
        /// </summary>
        public IEnumerable<EquSpotcheckTemplateItemRelationDto> relationDto { get; set; }

        /// <summary>
        /// 模板与设备组
        /// </summary>
        public IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationDto> groupRelationDto { get; set; }

    }

    /// <summary>
    /// 设备点检模板更新Dto
    /// </summary>
    public record EquSpotcheckTemplateModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点检模板编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = "";


        /// <summary>
        /// 模板与项目
        /// </summary>
        public IEnumerable<EquSpotcheckTemplateItemRelationDto> relationDto { get; set; }

        /// <summary>
        /// 模板与设备组
        /// </summary>
        public IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationDto> groupRelationDto { get; set; }
    }

    /// <summary>
    /// 设备点检模板分页Dto
    /// </summary>
    public class EquSpotcheckTemplatePagedQueryDto : PagerInfo
    {

        /// <summary>
        /// 点检模板编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 点检模板名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 设备组编码
        /// </summary>
        public string? EquipmentGroupCode { get; set; }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string? EquipmentGroupName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public record EquSpotcheckTemplateDeleteDto
    {
        /// <summary>
        /// 要删除的组
        /// </summary>
        public IEnumerable<long> Ids { get; set; }
    }

    #region 关系子表
    /// <summary>
    /// 设备点检模板与项目关系Dto
    /// </summary>
    public record EquSpotcheckTemplateItemRelationDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点检项目ID;equ_spotcheck_item的Id
        /// </summary>
        public long SpotCheckItemId { get; set; }

        /// <summary>
        /// 点检模板
        /// </summary>
        public long SpotCheckTemplateId { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 规格值（规格中心）
        /// </summary>
        public decimal? Center { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }
    }


    /// <summary>
    /// 设备点检模板与设备组关系Dto
    /// </summary>
    public record EquSpotcheckTemplateEquipmentGroupRelationDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备组ID;equ_equipment_group的Id
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 设备组ID;equ_equipment_group的Id
        /// </summary>
        public long SpotCheckTemplateId { get; set; }
    }


    /// <summary>
    /// 设备点检模板与项目关系Dto
    /// </summary>
    public record GetSpotcheckItemRelationListDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点检项目ID;equ_spotcheck_item的Id
        /// </summary>
        public long SpotCheckItemId { get; set; }

        /// <summary>
        /// 点检模板ID
        /// </summary>
        public long SpotCheckTemplateId { get; set; }

        /// <summary>
        /// 点检项目Code
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string? Components { get; set; }
        /// <summary>
        /// 点检方式
        /// </summary>
        public EquSpotcheckItemMethodEnum? CheckType { get; set; }

        /// <summary>
        /// 点检方法
        /// </summary>
        public string? CheckMethod { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public EquSpotcheckDataTypeEnum? DataType { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 规格值（规格中心）
        /// </summary>
        public decimal? Center { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }
    }


    /// <summary>
    /// 设备点检模板与项目关系Dto
    /// </summary>
    public record QuerySpotcheckEquipmentGroupRelationListDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点检模板ID
        /// </summary>
        public long SpotCheckTemplateId { get; set; }

        /// <summary>
        /// 设备组ID;equ_equipment_group的Id
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 设备组编码
        /// </summary>
        public string? EquipmentGroupCode { get; set; }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string? EquipmentGroupName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }
    }


    /// <summary>
    /// 设备点检模板与项目关系 查询参数
    /// </summary> 
    public record GetEquSpotcheckTemplateItemRelationDto
    {
        /// <summary>
        /// 模板IDs
        /// </summary>
        public IEnumerable<long> SpotCheckTemplateIds { get; set; }

    }
    #endregion
}
