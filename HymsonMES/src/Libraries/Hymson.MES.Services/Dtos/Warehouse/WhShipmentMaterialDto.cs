using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhShipmentBarcode;

namespace Hymson.MES.Services.Dtos.WhShipmentMaterial
{
    /// <summary>
    /// 出货单物料详情新增/更新Dto
    /// </summary>
    public record WhShipmentMaterialSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 出货单Id
        /// </summary>
        public long ShipmentId { get; set; }

       /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 出货数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 出货单物料详情Dto
    /// </summary>
    public record WhShipmentMaterialDto : BaseEntityDto
    {
       /// <summary>
        /// 出货单Id
        /// </summary>
        public long ShipmentId { get; set; }

       /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 出货数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public IList<WhShipmentBarcodeDto> Barcods { get; set; }    

    }

    /// <summary>
    /// 出货单物料详情分页Dto
    /// </summary>
    public class WhShipmentMaterialPagedQueryDto : PagerInfo { }

}
