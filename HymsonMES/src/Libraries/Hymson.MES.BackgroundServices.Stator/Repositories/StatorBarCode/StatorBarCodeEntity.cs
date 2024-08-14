namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 定子条码表
    /// </summary>
    public class StatorBarCodeEntity
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 内定子Id
        /// </summary>
        public long InnerId { get; set; }

        /// <summary>
        /// 内定子编码
        /// </summary>
        public string InnerBarCode { get; set; }

        /// <summary>
        /// 外定子编码
        /// </summary>
        public string OuterBarCode { get; set; }

        /// <summary>
        /// BusBar编码
        /// </summary>
        public string BusBarCode { get; set; }

        /// <summary>
        /// 槽底纸编码
        /// </summary>
        public string PaperBottomLotBarcode { get; set; }

        /// <summary>
        /// 槽盖纸编码
        /// </summary>
        public string PaperTopLotBarcode { get; set; }

        /// <summary>
        /// 成品码
        /// </summary>
        public string ProductionCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

    }
}
