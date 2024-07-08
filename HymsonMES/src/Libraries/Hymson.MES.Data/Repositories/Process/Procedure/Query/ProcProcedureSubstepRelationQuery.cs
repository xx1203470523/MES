namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 工序配置子步骤表 查询参数
    /// </summary>
    public class ProcProcedureSubstepRelationQuery
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 子步骤Id列表
        /// </summary>
        public IEnumerable<long>? ProcedureSubstepIds { get; set; }
    }
}
