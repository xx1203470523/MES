using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using OfficeOpenXml.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 物料维护Dto
    /// </summary>
    public record ProcMaterialDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属物料组ID
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 物料组编码
        /// </summary>
        public string MaterialGroupCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public MaterialOriginEnum? Origin { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        public MaterialBuyTypeEnum? BuyType { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// BomID
        /// </summary>
        public long? BomId { get; set; }

        /// <summary>
        /// 批次大小
        /// </summary>
        public decimal Batch { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        public int? PackageNum { get; set; }

        /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// 内/外序列号
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 验证掩码组
        /// </summary>
        public string? ValidationMaskGroup { get; set; }

        /// <summary>
        /// 基于时间(字典定义)
        /// </summary>
        public MaterialBaseTimeEnum? BaseTime { get; set; }

        /// <summary>
        /// 消耗公差
        /// </summary>
        public int? ConsumptionTolerance { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 修改人
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

        /// <summary>
        /// 消耗系数
        /// </summary>
        public decimal? ConsumeRatio { get; set; }

        /// <summary>
        /// 掩码规则ID
        /// </summary>
        public long? MaskCodeId { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public int? ValidTime { get; set; }

        /// <summary>
        /// 数量限制
        /// </summary>
        public MaterialQuantityLimitEnum? QuantityLimit { get; set; }
    }


    /// <summary>
    /// 物料维护新增Dto
    /// </summary>
    public record ProcMaterialCreateDto : BaseEntityDto
    {
        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; } = 0;

        /// <summary>
        /// 物料编码
        /// </summary>
        [Required(ErrorMessage = "物料编码不能为空")]
        public string MaterialCode { get; set; } = "";

        /// <summary>
        /// 物料名称
        /// </summary>
        [Required(ErrorMessage = "物料名称不能为空")]
        public string MaterialName { get; set; } = "";

        /// <summary>
        /// 来源
        /// </summary>
        public MaterialOriginEnum? Origin { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string? Specifications { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        public MaterialBuyTypeEnum? BuyType { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// BomID
        /// </summary>
        public long? BomId { get; set; }

        /// <summary>
        /// 批次大小
        /// </summary>
        public decimal Batch { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        public int? PackageNum { get; set; }

        /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// 内/外序列号
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 验证掩码组
        /// </summary>
        public string? ValidationMaskGroup { get; set; }

        /// <summary>
        /// 基于时间(字典定义)
        /// </summary>
        public MaterialBaseTimeEnum? BaseTime { get; set; }

        /// <summary>
        /// 消耗公差
        /// </summary>
        public int? ConsumptionTolerance { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool IsDefaultVersion { get; set; } = false;

        /// <summary>
        /// 消耗系数
        /// </summary>
        public decimal? ConsumeRatio { get; set; }

        /// <summary>
        /// 掩码规则ID
        /// </summary>
        public long? MaskCodeId { get; set; }

        /// <summary>
        /// 产品型号 条码规则 生成条码使用
        /// </summary>
        public string? ProductModel { get; set; }

        /// <summary>
        /// 原材料类型
        /// </summary>
        public MaterialTypeEnum? MaterialType { get; set; }


        /// 有效时间
        /// </summary>
        public int? ValidTime { get; set; }

        /// <summary>


        /// <summary>
        /// 数量限制
        /// </summary>
        public MaterialQuantityLimitEnum? QuantityLimit { get; set; }

        /// <summary>
        /// 替代品集合
        /// </summary>
        public List<ProcMaterialReplaceDto>? DynamicList { get; set; } = new List<ProcMaterialReplaceDto>();

        /// <summary>
        /// 
        /// </summary>
        public List<ProcMaterialSupplierRelationCreateDto>? MaterialSupplierList { get; set; } = new List<ProcMaterialSupplierRelationCreateDto>();

    }

    /// <summary>
    /// 物料维护表查询对象
    /// </summary>
    public record ProcMaterialReplaceDto : BaseEntityDto
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
    }

    /// <summary>
    /// 物料维护更新Dto
    /// </summary>
    public record ProcMaterialModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属物料组ID
        /// </summary>
        public long? GroupId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public MaterialOriginEnum? Origin { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string? Specifications { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        public MaterialBuyTypeEnum? BuyType { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// BomID
        /// </summary>
        public long? BomId { get; set; }

        /// <summary>
        /// 批次大小
        /// </summary>
        public decimal Batch { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        public int? PackageNum { get; set; }

        /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// 内/外序列号
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 验证掩码组
        /// </summary>
        public string? ValidationMaskGroup { get; set; }

        /// <summary>
        /// 基于时间(字典定义)
        /// </summary>
        public MaterialBaseTimeEnum? BaseTime { get; set; }

        /// <summary>
        /// 消耗公差
        /// </summary>
        public int? ConsumptionTolerance { get; set; }

        /// <summary>
        /// 消耗系数
        /// </summary>
        public decimal? ConsumeRatio { get; set; }

        /// <summary>
        /// 掩码规则ID
        /// </summary>
        public long? MaskCodeId { get; set; }

        /// <summary>
        /// 产品型号 条码规则 生成条码使用
        /// </summary>
        public string? ProductModel { get; set; }

        /// <summary>
        /// 原材料类型
        /// </summary>
        public MaterialTypeEnum? MaterialType { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public int? ValidTime { get; set; }

        /// <summary>
        /// 数量限制
        /// </summary>
        public MaterialQuantityLimitEnum? QuantityLimit { get; set; }

        /// <summary>
        /// 替代品集合
        /// </summary>
        public List<ProcMaterialReplaceDto>? DynamicList { get; set; } = new List<ProcMaterialReplaceDto>();

        /// <summary>
        /// 供应商
        /// </summary>
        public List<ProcMaterialSupplierRelationCreateDto>? MaterialSupplierList { get; set; } = new List<ProcMaterialSupplierRelationCreateDto>();
    }

    /// <summary>
    /// 物料维护分页Dto
    /// </summary>
    public class ProcMaterialPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料组ID
        /// </summary>
        public long? GroupId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; } = "";

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; } = "";

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public MaterialOriginEnum? Origin { get; set; }

        /// <summary>
        /// 采购类型 数组
        /// </summary>
        public MaterialBuyTypeEnum[]? BuyTypes { get; set; }

        /// <summary>
        /// 物料组编码
        /// </summary>
        public string? MaterialGroupCode {  get; set; }
        /// <summary>
        /// 有效时间
        /// </summary>
        public int? ValidTime { get; set; }
    }

    /// <summary>
    /// 物料维护 视图
    /// </summary>
    public record ProcMaterialViewDto : ProcMaterialDto
    {
        /// <summary>
        /// 描述 :所属物料组
        /// 空值 : false  
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// 描述 :编码 (工艺路线)
        /// 空值 : true  
        /// </summary>
        public string? ProcessRouteCode { get; set; }

        /// <summary>
        /// 描述 :名称 (工艺路线)
        /// 空值 : true  
        /// </summary>
        public string? ProcessRouteName { get; set; }

        /// <summary>
        /// 描述 :版本 (工艺路线) 
        /// 空值 : true  
        /// </summary>
        public string? ProcessRouteVersion { get; set; }

        /// <summary>
        /// 描述 :编码（工序Bom）
        /// 空值 : true  
        /// </summary>
        public string? BomCode { get; set; }

        /// <summary>
        /// 描述 :名称（Bom）
        /// 空值 : true  
        /// </summary>
        public string? BomName { get; set; }

        /// <summary>
        /// 描述 :版本（Bom）
        /// 空值 : true  
        /// </summary>
        public string? BomVersion { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }
        /// <summary>
        /// 产品型号 条码规则 生成条码使用
        /// </summary>
        public string? ProductModel { get; set; }

        /// <summary>
        /// 原材料类型
        /// </summary>
        public MaterialTypeEnum? MaterialType { get; set; }
        /// <summary>
        /// 替代料
        /// </summary>
        public List<ProcMaterialReplaceViewDto> ReplaceMaterialList { get; set; } = new List<ProcMaterialReplaceViewDto>();

    }

    /// <summary>
    /// 替换物料 视图
    /// </summary>
    public record ProcMaterialReplaceViewDto : BaseEntityDto
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
    }

    /// <summary>
    /// 物料关联供应商 视图
    /// </summary>
    public record ProcMaterialSupplierViewDto : BaseEntityDto
    {
        //
        // 摘要:
        //     唯一标识
        public long Id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 描述 :供应商编码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述 :供应商名称 
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 物料维护更新Dto
    /// </summary>
    public record ProcMaterialChangeStatusDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 需要变更为的状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

    }

    public class ProcMaterialExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    /// <summary>
    /// 物料导入模板
    /// </summary>
    public record ProcMaterialImportDto : BaseExcelDto
    {
        /// <summary>
        /// 编码（物料）
        /// </summary>
        [EpplusTableColumn(Header = "物料编码(必填)", Order = 1)]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 名称（物料）
        /// </summary>
        [EpplusTableColumn(Header = "物料名称(必填)", Order = 2)]
        public string MaterialName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [EpplusTableColumn(Header = "物料版本(必填)", Order = 3)]
        public string Version { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EpplusTableColumn(Header = "备注", Order = 4)]
        public string? Remark { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        [EpplusTableColumn(Header = "采购类型(必填)", Order = 5)]
        public MaterialBuyTypeEnum? BuyType { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        [EpplusTableColumn(Header = "工艺路线编码", Order = 6)]
        public string? ProcessRouteCode { get; set; }

        /// <summary>
        /// Bom
        /// </summary>
        [EpplusTableColumn(Header = "Bom编码", Order = 7)]
        public string? BomCode { get; set; }

        /// <summary>
        /// 批次大小
        /// </summary>
        [EpplusTableColumn(Header = "批次大小(必填)", Order = 8)]
        public decimal Batch { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        [EpplusTableColumn(Header = "标包数量", Order = 9)]
        public int? PackageNum { get; set; }

        /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        [EpplusTableColumn(Header = "计量单位", Order = 10)]
        public string? Unit { get; set; }

        /// <summary>
        /// 内/外序列号
        /// </summary>
        [EpplusTableColumn(Header = "数据收集方式(必填)", Order = 11)]
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 基于时间(字典定义)
        /// </summary>
        [EpplusTableColumn(Header = "基于时间", Order = 12)]
        public MaterialBaseTimeEnum? BaseTime { get; set; }

        /// <summary>
        /// 消耗公差
        /// </summary>
        [EpplusTableColumn(Header = "消耗公差", Order = 13)]
        public int? ConsumptionTolerance { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        [EpplusTableColumn(Header = "是否默认版本(必填)", Order = 14)]
        public YesOrNoEnum? DefaultVersion { get; set; }

        /// <summary>
        /// 消耗系数
        /// </summary>
        [EpplusTableColumn(Header = "消耗系数", Order = 15)]
        public decimal? ConsumeRatio { get; set; }

        /// <summary>
        /// 验证掩码组
        /// </summary>
        [EpplusTableColumn(Header = "验证掩码组", Order = 16)]
        public string? MaskCode { get; set; }

        /// <summary>
        /// 有效天数
        /// </summary>
        [EpplusTableColumn(Header = "有效天数", Order = 17)]
        public string? ValidTime { get; set; }

    }

    /// <summary>
    /// 物料导出模板
    /// </summary>
    public record ProcMaterialExportDto : BaseExcelDto
    {
        /// <summary>
        /// 编码（物料）
        /// </summary>
        [EpplusTableColumn(Header = "物料编码(必填)", Order = 1)]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 名称（物料）
        /// </summary>
        [EpplusTableColumn(Header = "物料名称(必填)", Order = 2)]
        public string MaterialName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [EpplusTableColumn(Header = "物料来源", Order = 3)]
        public MaterialOriginEnum? Origin { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [EpplusTableColumn(Header = "物料版本(必填)", Order = 4)]
        public string Version { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EpplusTableColumn(Header = "备注", Order = 5)]
        public string? Remark { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        [EpplusTableColumn(Header = "采购类型(必填)", Order = 6)]
        public MaterialBuyTypeEnum? BuyType { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        [EpplusTableColumn(Header = "工艺路线编码", Order = 7)]
        public string? ProcessRouteCode { get; set; }

        /// <summary>
        /// Bom
        /// </summary>
        [EpplusTableColumn(Header = "Bom编码", Order = 8)]
        public string? BomCode { get; set; }

        /// <summary>
        /// 批次大小
        /// </summary>
        [EpplusTableColumn(Header = "批次大小(必填)", Order = 9)]
        public decimal Batch { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        [EpplusTableColumn(Header = "标包数量", Order = 10)]
        public int? PackageNum { get; set; }

        /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        [EpplusTableColumn(Header = "计量单位", Order = 11)]
        public string? Unit { get; set; }

        /// <summary>
        /// 内/外序列号
        /// </summary>
        [EpplusTableColumn(Header = "数据收集方式(必填)", Order = 12)]
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 基于时间(字典定义)
        /// </summary>
        [EpplusTableColumn(Header = "基于时间", Order = 14)]
        public MaterialBaseTimeEnum? BaseTime { get; set; }

        /// <summary>
        /// 消耗公差
        /// </summary>
        [EpplusTableColumn(Header = "消耗公差", Order = 15)]
        public int? ConsumptionTolerance { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        [EpplusTableColumn(Header = "是否默认版本(必填)", Order = 16)]
        public YesOrNoEnum? DefaultVersion { get; set; }

        /// <summary>
        /// 消耗系数
        /// </summary>
        [EpplusTableColumn(Header = "消耗系数", Order = 17)]
        public decimal? ConsumeRatio { get; set; }

        /// <summary>
        /// 验证掩码组
        /// </summary>
        [EpplusTableColumn(Header = "验证掩码组", Order = 18)]
        public string? MaskCode { get; set; }

    }

}
