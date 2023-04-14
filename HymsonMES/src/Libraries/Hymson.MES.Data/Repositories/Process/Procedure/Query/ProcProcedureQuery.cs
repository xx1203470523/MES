namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序表 查询参数
    /// </summary>
    public class ProcProcedureQuery
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 所属站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
