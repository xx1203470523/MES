namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM明细表 查询参数
    /// </summary>
    public class ProcBomDetailQuery
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

    }
}
