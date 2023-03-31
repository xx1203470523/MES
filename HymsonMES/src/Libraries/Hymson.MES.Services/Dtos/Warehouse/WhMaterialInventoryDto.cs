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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ///// <summary>
        ///// 供应商ID
        ///// </summary>
        //public long SupplierId { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        ///// <summary>
        ///// 物料ID
        ///// </summary>
        //public long MaterialId { get; set; }

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
        public string Batch { get; set; }

        /// <summary>
        /// 数量（剩余）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public int Source { get; set; }

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
        public string Batch { get; set; }

        /// <summary>
        /// 数量（剩余）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public int Source { get; set; }

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
        public string Batch { get; set; }

        /// <summary>
        /// 数量（剩余）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public bool Source { get; set; }

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
        public WhMaterialInventorySourceEnum Source { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }
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
        public DateTime? DueDate { get; set; } = DateTime.Now.AddMonths(1);

        /// <summary>
        /// 供应商编码
        /// </summary>
        // public string SupplierCode { get; set; }
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
        public string Batch { get; set; }

        /// <summary>
        /// 数量（剩余）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public bool Source { get; set; }

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
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }


        /// <summary>
        /// 批次
        /// </summary>
        public string? Batch { get; set; }
        /// <summary>
        /// 物料条码
        /// </summary>
        public string? MaterialBarCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; } = "";
        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

    }
}
