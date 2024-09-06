namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class StatorBarCodeQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码（铜线）
        /// </summary>
        public IEnumerable<string>? WireBarCodes { get; set; }

        /// <summary>
        /// Id（内定子）
        /// </summary>
        public IEnumerable<string>? InnerIds { get; set; }

        /// <summary>
        /// 条码（内定子）
        /// </summary>
        public IEnumerable<string>? InnerBarCodes { get; set; }

        /// <summary>
        /// 条码（外定子）
        /// </summary>
        public IEnumerable<string>? OuterBarCodes { get; set; }

        /// <summary>
        /// 条码（BusBar）
        /// </summary>
        public IEnumerable<string>? BusBarCodes { get; set; }

        /// <summary>
        /// 条码（成品码）
        /// </summary>
        public IEnumerable<string>? ProductionCodes { get; set; }

    }

    /// <summary>
    /// 查询参数
    /// </summary>
    public class StatorSpecifiedColumnQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

    }

}
