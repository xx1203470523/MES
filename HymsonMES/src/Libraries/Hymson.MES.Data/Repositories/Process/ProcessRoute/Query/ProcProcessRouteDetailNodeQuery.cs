namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线工序节点明细表 查询参数
    /// </summary>
    public class ProcProcessRouteDetailNodeQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序ID（前工序ID）
        /// </summary>
        public long ProcedureId { get; set; }
    }

    /// <summary>
    /// 工艺路线工序节点明细表 查询参数
    /// </summary>
    public class ProcProcessRouteDetailNodesQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序ID（前工序ID）
        /// </summary>
        public IEnumerable<long> ProcedureIds { get; set; }
    }
}
