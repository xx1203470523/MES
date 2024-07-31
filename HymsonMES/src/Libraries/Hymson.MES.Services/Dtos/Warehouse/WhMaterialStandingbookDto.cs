/*
 *creator: Karl
 *
 *describe: 物料台账    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-13 10:03:29
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Warehouse
{
    /// <summary>
    /// 物料台账Dto
    /// </summary>
    public record WhMaterialStandingbookDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称（D）
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本（D）
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? Batch { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 单位;来自物料模块的计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 流转类型;物料接收/物料退料/物料加载
        /// </summary>
        public WhMaterialInventoryTypeEnum Type { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public MaterialInventorySourceEnum Source { get; set; }

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
        /// 是否删除;删除时赋值为主键
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }
    }


    /// <summary>
    /// 物料台账新增Dto
    /// </summary>
    public record WhMaterialStandingbookCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称（D）
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本（D）
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? Batch { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 单位;来自物料模块的计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 流转类型;物料接收/物料退料/物料加载
        /// </summary>
        public WhMaterialInventoryTypeEnum Type { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public MaterialInventorySourceEnum Source { get; set; }

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
        /// 是否删除;删除时赋值为主键
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }

    /// <summary>
    /// 物料台账更新Dto
    /// </summary>
    public record WhMaterialStandingbookModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称（D）
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本（D）
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? Batch { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 单位;来自物料模块的计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 流转类型;物料接收/物料退料/物料加载
        /// </summary>
        public WhMaterialInventoryTypeEnum Type { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public MaterialInventorySourceEnum Source { get; set; }

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
        /// 是否删除;删除时赋值为主键
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }

    /// <summary>
    /// 物料台账分页Dto
    /// </summary>
    public class WhMaterialStandingbookPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string? MaterialBarCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? Batch { get; set; } = "";

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; } = "";

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; } 
    }

    public class WhMaterialStandingbookExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    /// <summary>
    /// 物料台账Dto
    /// </summary>
    public record WhMaterialStandingbookExportDto : BaseExcelDto
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        [EpplusTableColumn(Header = "物料条码", Order = 1)]
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        [EpplusTableColumn(Header = "物料批次", Order = 2)]
        public string? Batch { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        [EpplusTableColumn(Header = "数量", Order = 3)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 单位;来自物料模块的计量单位
        /// </summary>
        [EpplusTableColumn(Header = "单位", Order = 4)]
        public string Unit { get; set; }

        /// <summary>
        /// 物料名称（D）
        /// </summary>
        [EpplusTableColumn(Header = "物料名称", Order = 5)]
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 6)]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本（D）
        /// </summary>
        [EpplusTableColumn(Header = "版本", Order = 7)]
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 流转类型;物料接收/物料退料/物料加载
        /// </summary>
        [EpplusTableColumn(Header = "流转类型", Order = 8)]
        public WhMaterialInventoryTypeEnum Type { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        [EpplusTableColumn(Header = "来源", Order = 9)]
        public MaterialInventorySourceEnum Source { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        [EpplusTableColumn(Header = "来源供应商编码", Order = 10)]
        public string SupplierCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EpplusTableColumn(Header = "操作人", Order = 11)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EpplusTableColumn(Header = "接收时间", Order = 12)]
        public DateTime CreatedOn { get; set; }  
    }
}
