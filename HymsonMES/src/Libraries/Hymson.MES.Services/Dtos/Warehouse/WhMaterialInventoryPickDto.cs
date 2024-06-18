using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Warehouse
{
    /// <summary>
    /// 生产领料
    /// </summary>
    public record WhMaterialInventoryPickDto : BaseEntityDto
    {
        /// <summary>
        /// ERP领料单据号
        /// </summary>
        public string ERPRequisitionOrder { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 领料信息
        /// </summary>
        public List<ProductionPickMaterialDto> ReceiveMaterials { get; set; }
    }

    public class ProductionPickMaterialDto
    {
        /// <summary>
        /// 领料仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal ReceivedQty { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string MaterialBatch { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 物料的有效期（截止日期）
        /// </summary>
        public DateTime ExpirationDate { get; set; }
    }
}
