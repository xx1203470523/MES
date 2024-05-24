namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM明细表 查询参数
    /// </summary>
    public class ProcBomDetailQuery
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public IEnumerable<long> ProcedureIds { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public IEnumerable<long> MaterialIds { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }
    }


    /// <summary>
    /// BOM明细表 查询参数
    /// </summary>
    public class ProcBomDetailByBomIdAndProcedureIdQuery 
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        ///BomID
        /// </summary>
        public long BomId { get; set; }

    }
}
