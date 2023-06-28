namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 多条码
    /// </summary>
    public class MultiSFCBo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> SFCs { get; set; } = new List<string>();
    }
}
    