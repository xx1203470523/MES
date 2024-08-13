namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class StatorWireRelationQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// Id（铜线）
        /// </summary>
        public IEnumerable<string>? WireIds { get; set; }

        /// <summary>
        /// 条码（铜线）
        /// </summary>
        public IEnumerable<string>? WireBarCodes { get; set; }

    }
}
