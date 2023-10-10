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
        /// 条码集合（不要使用这个对象）
        /// </summary>
        [Obsolete("这个对象已作废，请在各自作业里面定义作业所需的参数", false)]
        public IEnumerable<string> SFCs { get; set; } = new List<string>();
    }
}
