namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 条码树
    /// </summary>
    public class NodeSourceBo
    {
        /// <summary>
        /// 条码ID
        /// </summary>
        public long Id { get; set; }

        /*
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }
        */

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
        /// 条码ID集合（来源/去向）
        /// </summary>
        public List<NodeSourceBo> Children { get; set; } = new();

    }

}
