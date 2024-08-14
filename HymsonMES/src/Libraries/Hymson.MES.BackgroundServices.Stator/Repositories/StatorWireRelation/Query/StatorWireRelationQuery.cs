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
        /// Id（内定子）
        /// </summary>
        public IEnumerable<string>? InnerIds { get; set; }

        /// <summary>
        /// Id（铜线）
        /// </summary>
        public IEnumerable<string>? WireIds { get; set; }

    }
}
