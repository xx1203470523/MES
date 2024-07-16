/*
 *creator: Karl
 *
 *describe: 物料库存    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Warehouse;

namespace Hymson.MES.Services.Dtos.Warehouse
{
    /// <summary>
    /// 物料信息Dto
    /// </summary>
    public record ProcMaterialInfoViewDto : BaseEntityDto
    {
        /// <summary>
        /// 物料信息
        /// </summary>
        public ProcMaterialInfoView MaterialInfo { get; set; }

        /// <summary>
        /// 供应商信息
        /// </summary>
        public IEnumerable<WhSupplierInfoView> SupplierInfo { get; set; }

    }

    public record WhMaterialInventoryPageListViewDto : BaseEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 物料组ID
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
        /// 物料单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string? Batch { get; set; }
        /// <summary>
        /// 工单
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 数量（剩余）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public MaterialInventorySourceEnum Source { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 是否删除 
        /// </summary>
        public long IsDeleted { get; set; }

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

    }



    /// <summary>
    /// 物料库存Dto
    /// </summary>
    public record WhMaterialInventoryDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string? Batch { get; set; }

        /// <summary>
        /// 数量（剩余）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 接收数量 (一开始的数量)
        /// </summary>
        public decimal ReceivedQty { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

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
    /// 物料库存新增Dto
    /// </summary>
    public record WhMaterialInventoryCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public decimal? Batch { get; set; } = 0;

        /// <summary>
        /// 数量（剩余）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

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
    /// 物料库存批量新增Dto
    /// </summary>
    public record WhMaterialInventoryListCreateDto : BaseEntityDto
    {

        /// <summary>
        /// 来源
        /// </summary>
        public MaterialInventorySourceEnum Source { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public long MaterialId { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string? Batch { get; set; }
        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal QuantityResidue { get; set; }

        ///// <summary>
        ///// 供应商ID
        ///// </summary>
        public long SupplierId { get; set; }


        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 转类型
        /// </summary>
        public WhMaterialInventoryTypeEnum Type { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

    }

    /// <summary>
    /// 物料库存更新Dto
    /// </summary>
    public record WhMaterialInventoryModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public decimal? Batch { get; set; } = 0;

        /// <summary>
        /// 数量（剩余）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

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
    /// 物料库存分页Dto
    /// </summary>
    public class WhMaterialInventoryPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 批次
        /// </summary>
        public decimal? Batch { get; set; } = 0;
        /// <summary>
        /// 物料条码
        /// </summary>
        public string? MaterialBarCode { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 物料条码(多个)
        /// </summary>
        public IEnumerable<string>? MaterialBarCodes { get; set; }
        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; } = "";
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialName { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public WhMaterialInventoryStatusEnum? Status { get; set; }

        /// <summary>
        /// 状态s
        /// </summary>
        public IEnumerable<WhMaterialInventoryStatusEnum>? Statuss { get; set; }


    #region 添加 库存修改功能时添加 karl
    /// <summary>
    /// 接收时间  时间范围  数组
    /// </summary>
    public DateTime[]? CreatedOnRange { get; set; }

        #endregion
    }

    public record WhMaterialInventoryDetailDto : WhMaterialInventoryDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
    }

    /// <summary>
    /// 修改外部来源的库存
    /// </summary>
    public class OutsideWhMaterialInventoryModifyDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 现有剩余的数量
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string? Batch { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }
    }


    /// <summary>
    /// 条码拆分
    /// </summary>
    public record MaterialBarCodeSplitAdjustDto : BaseEntityDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 待拆分的数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 物料合并
    /// </summary>
    public record MaterialBarCodeMergeAdjust : BaseEntityDto
    {
        /// <summary>
        /// 待合并的条码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }

        /// <summary>
        /// 指定合条码
        /// </summary>
        public string? MergeSFC { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }
}
