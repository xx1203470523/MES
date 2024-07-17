using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 工具类型管理新增/更新Dto
    /// </summary>
    public record EquToolsTypeSaveDto : BaseEntityDto
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
        /// 状态 1、启用  0、禁用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 额定寿命
        /// </summary>
        public decimal RatedLife { get; set; }

        ///// <summary>
        ///// 额定寿命单位
        ///// </summary>
        //public string RatedLifeUnit { get; set; } = "";

        /// <summary>
        /// 是否校准 1、是 2、否
        /// </summary>
        public YesOrNoEnum IsCalibrated { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        public decimal? CalibrationCycle { get; set; }

        /// <summary>
        /// 校准周期单位
        /// </summary>
        public ToolingTypeEnum? CalibrationCycleUnit { get; set; }

        /// <summary>
        /// 是否所有设备都可用 1、是 2、否
        /// </summary>
        public bool IsAllEquipmentUsed { get; set; }

        /// <summary>
        /// 是否物料都可用 1、是 2、否
        /// </summary>
        public bool IsAllMaterialUsed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 关联设备组
        /// </summary>
        public IEnumerable<long>? EquipmentGroupIds { get; set; }

        /// <summary>
        /// 关联物料
        /// </summary>
        public IEnumerable<long>? MaterialIdIds { get; set; }
    }

    /// <summary>
    /// 工具类型管理Dto
    /// </summary>
    public record EquToolsTypeDto : BaseEntityDto
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
        /// 额定寿命
        /// </summary>
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 额定寿命单位
        /// </summary>
        public string RatedLifeUnit { get; set; }

        /// <summary>
        /// 是否校准 1、是 2、否
        /// </summary>
        public YesOrNoEnum? IsCalibrated { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        public decimal? CalibrationCycle { get; set; }

        /// <summary>
        /// 校准周期单位
        /// </summary>
        public ToolingTypeEnum? CalibrationCycleUnit { get; set; }

        /// <summary>
        /// 是否所有设备都可用 1、是 2、否
        /// </summary>
        public bool IsAllEquipmentUsed { get; set; } = false;

        /// <summary>
        /// 是否物料都可用 1、是 2、否
        /// </summary>
        public bool IsAllMaterialUsed { get; set; } = false;

        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 工具类型管理分页Dto
    /// </summary>
    public class EquToolsTypePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工具类型编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 更新时间  时间范围  数组
        /// </summary>
        public DateTime[]? UpdatedOn { get; set; }

        /// <summary>
        ///创建时间  时间范围  数组
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }

    public class EquToolsTypeCofigEquipmentGroupDto
    {
        /// <summary>
        /// 是否所有设备都可用 1、是 2、否
        /// </summary>
        public bool IsAllEquipmentUsed { get; set; }

        public IEnumerable<EquToolsTypeEquipmentGroupRelationDto> ToolsTypes { get; set; }
    }

    public class EquToolsTypeEquipmentGroupRelationDto
    {
        /// <summary>
        /// 工具类型 的id
        /// </summary>
        public long? ToolTypeId { get; set; }

        /// <summary>
        /// 设备组 的id
        /// </summary>
        public long? EquipmentGroupId { get; set; }
    }

    public class EquToolsTypeCofigMaterialDto
    {
        /// <summary>
        /// 是否物料都可用 1、是 2、否
        /// </summary>
        public bool IsAllMaterialUsed { get; set; } = false;

        public IEnumerable<EquToolsTypeMaterialRelationDto> ToolsTypes { get; set; }
    }

    public class EquToolsTypeMaterialRelationDto
    {
        /// <summary>
        /// 工具类型 的id
        /// </summary>
        public long? ToolTypeId { get; set; }

        /// <summary>
        /// 设备组 的id
        /// </summary>
        public long? MaterialId { get; set; }
    }

    /// <summary>
    /// 工具类型查询Dto
    /// </summary>
    public class EquToolsTypeQueryDto
    {
        /// <summary>
        /// 工具类型Id
        /// </summary>
        public long Id { get; set; }=0L;
    }


    /// <summary>
    /// 工具类型导入模板
    /// </summary>
    public record EquToolingTypeExcelDto : BaseExcelDto
    {
        /// <summary>
        /// 工具类型编码
        /// </summary>
        [EpplusTableColumn(Header = "工具类型编码(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 工具类型名称
        /// </summary>
        [EpplusTableColumn(Header = "工具类型名称(必填)", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        [EpplusTableColumn(Header = "状态(必填 已启用,已禁用)", Order = 3)]
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 额定使用寿命
        /// </summary>
        [EpplusTableColumn(Header = "额定使用寿命(必填,数字)", Order = 4)]
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 是否校准 1、是 2、否
        /// </summary>
        [EpplusTableColumn(Header = "需要校准(必填是，否)", Order = 5)]
        public YesOrNoEnum IsCalibrated { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        [EpplusTableColumn(Header = "校准周期", Order = 6)]
        public decimal? CalibrationCycle { get; set; }

        /// <summary>
        /// 校准周期单位
        /// </summary>
        [EpplusTableColumn(Header = "校准周期单位（天，周，月）", Order = 7)]
        public ToolingTypeEnum? CalibrationCycleUnit { get; set; }

        /// <summary>
        /// 所有设备组均可使用 1、是 2、否
        /// </summary>
        [EpplusTableColumn(Header = "所有设备组均可使用(是,否)", Order = 8)]
        public TrueOrFalseEnum IsAllEquipmentUsed { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 可用设备组
        /// </summary>
        [EpplusTableColumn(Header = "可用设备组(设备组编码用逗号隔开)", Order = 9)]
        public string EqumentGroupCodes { get; set; } = "";

        /// <summary>
        /// 是否物料组均可使用 1、是 2、否
        /// </summary>
        [EpplusTableColumn(Header = "所有物料组均可使用(是,否)", Order = 10)]
        public TrueOrFalseEnum IsAllMaterialUsed { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 可用物料
        /// </summary>
        [EpplusTableColumn(Header = "可用物料(物料编码用逗号隔开)", Order = 11)]
        public string MaterialCodes { get; set; } = "";

    }
}
