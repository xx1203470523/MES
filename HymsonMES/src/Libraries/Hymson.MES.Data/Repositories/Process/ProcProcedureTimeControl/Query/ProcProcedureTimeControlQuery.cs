namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 跨工序时间管控 查询参数
    /// </summary>
    public class ProcProcedureTimeControlQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 起始工序Id
        /// </summary>
        public long? FromProcedureId { get; set; }

        /// <summary>
        /// 到达工序Id
        /// </summary>
        public long? ToProcedureId { get; set; }
    }
}
