namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 查询参数
    /// </summary>
    public class ManuSfcProduceQuery
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
