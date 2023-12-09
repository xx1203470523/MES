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
        public IEnumerable<string>  Sfcs { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long? ProcedureId { get; set; }
    }
}
