using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    public class MaterialScrappingInfoCommand
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>

        public string MaterialName { get; set; }

        /// <summary>
        /// 操作类型（报废/取消报废）
        /// </summary>
        public ScrapOperateTypeEnum OperationType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        ///// <summary>
        ///// 工序名称
        ///// </summary>
        //public string ProcedureName { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }


        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQuantity { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public decimal Batch { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        //public long[]? UnqualifiedGroupIds { get; set; }

        /// <summary>
        /// 不合格代码ID
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 报废类型
        /// </summary>
        public InventoryScrapTypeEnum ScrapType { get; set; }
    }

    public class MaterialCancelScrappingInfoCommand
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>

        public string? MaterialName { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQuantity { get; set; }

        /// <summary>
        /// 物料报废ID
        /// </summary>
        public long InventoryScrapId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }
        
        /// <summary>
        /// 批次
        /// </summary>
        public decimal? Batch { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }
    }
}
