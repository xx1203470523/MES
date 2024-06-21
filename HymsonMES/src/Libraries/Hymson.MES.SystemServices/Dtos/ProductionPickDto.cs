using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 生产领料
    /// </summary>
    public record ProductionPickDto : BaseEntityDto
    {
        /// <summary>
        /// WMS领料出货单据号
        /// </summary>
        public string WMSRequisitionOrder { get; set; }

        /// <summary>
        /// MES领料申请单号，格式：派工单code_领料申请单Id
        /// </summary>
        public string RequistionId { get; set; }

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
        public string? MaterialVersion { get; set; }

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
