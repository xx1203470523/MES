namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 铜线条码表
    /// </summary>
    public class WireBarCodeEntity
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 铜线Id
        /// </summary>
        public long WireId { get; set; }

        /// <summary>
        /// 铜线编码
        /// </summary>
        public string WireBarCode { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

    }
}
