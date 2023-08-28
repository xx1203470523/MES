/*
 *creator: Karl
 *
 *describe: 物料维护    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-08 04:47:44
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

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
        public int Batch { get; set; }

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
        public int Batch { get; set; }

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
        /// 替代品集合
        /// </summary>
        public List<ProcMaterialReplaceDto>? DynamicList { get; set; } = new List<ProcMaterialReplaceDto>();

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
        public int Batch { get; set; }

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

}
