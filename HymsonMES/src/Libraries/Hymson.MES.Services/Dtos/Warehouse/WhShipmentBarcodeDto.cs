using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.WhShipmentBarcode
{
    /// <summary>
    /// 出货单条码表新增/更新Dto
    /// </summary>
    public record WhShipmentBarcodeSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 出货单详情 Id
        /// </summary>
        public long ShipmentDetailId { get; set; }

       /// <summary>
        /// 出货条码
        /// </summary>
        public string BarCode { get; set; }

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
    /// 出货单条码表Dto
    /// </summary>
    public record WhShipmentBarcodeDto : BaseEntityDto
    {
       /// <summary>
        /// 出货单详情 Id
        /// </summary>
        public long ShipmentDetailId { get; set; }

       /// <summary>
        /// 出货条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }

    /// <summary>
    /// 出货单条码表分页Dto
    /// </summary>
    public class WhShipmentBarcodePagedQueryDto : PagerInfo { }

}
