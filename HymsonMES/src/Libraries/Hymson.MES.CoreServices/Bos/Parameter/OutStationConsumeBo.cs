namespace Hymson.MES.CoreServices.Bos.Parameter
{
    /// <summary>
    /// 消耗
    /// </summary>
    public record OutStationConsumeBo
    {
        /// <summary>
        /// 消耗条码的物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 消耗条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 消耗数量
        /// </summary>
        public decimal? ConsumeQty { get; set; }
    }
}
