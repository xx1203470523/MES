namespace Hymson.MES.CoreServices.Bos.Common
{
    /*
    /// <summary>
    /// 条码树
    /// </summary>
    public class FromSFCBo : NodeSFCBo
    {
        /// <summary>
        /// 条码ID（来源）
        /// </summary>
        public IEnumerable<long> SourceIds { get; set; }

    }

    /// <summary>
    /// 条码树
    /// </summary>
    public class ToSFCBo : NodeSFCBo
    {
        /// <summary>
        /// 条码ID（去向）
        /// </summary>
        public IEnumerable<long> DestinationIds { get; set; }

    }
    */

    /// <summary>
    /// 条码树
    /// </summary>
    public class NodeSFCBo
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 条码ID集合（来源）
        /// </summary>
        public IEnumerable<long> SourceIds { get; set; }

        /// <summary>
        /// 条码ID集合（去向）
        /// </summary>
        public IEnumerable<long> DestinationIds { get; set; }

    }

}
