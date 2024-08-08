namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 
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
        /// 铜线ID_1
        /// </summary>
        public long WireID_1 { get; set; }

        /// <summary>
        /// 铜线编码_1
        /// </summary>
        public string WireBarCode_1 { get; set; }

        /// <summary>
        /// 铜线ID_2
        /// </summary>
        public long WireID_2 { get; set; }

        /// <summary>
        /// 铜线编码_2
        /// </summary>
        public string WireBarCode_2 { get; set; }

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
