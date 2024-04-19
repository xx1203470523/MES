/*
 *creator: Karl
 *
 *describe: 物料台账    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-13 10:03:29
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

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
        /// 批次
        /// </summary>
        public decimal Batch { get; set; }

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
        /// 批次
        /// </summary>
        public decimal Batch { get; set; }

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
        /// 批次
        /// </summary>
        public decimal Batch { get; set; }

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
        /// 批次
        /// </summary>
        public decimal? Batch { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; } = "";

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; } 
    }
}
