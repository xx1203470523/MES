namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线工序节点关系明细表(前节点多条就存多条) 查询参数
    /// </summary>
    public class ProcProcessRouteDetailLinkQuery
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序ID（前工序ID）
        /// </summary>
        public long ProcedureId { get; set; }
    }
}
