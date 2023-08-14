namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 根据SFC查询参数
    /// </summary>
    public class ManuSfcProduceBySfcQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }


    /// <summary>
    /// 条码生产信息（物理删除） 根据SFCs查询参数
    /// </summary>
    public class ManuSfcProduceBySfcsQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }
}
