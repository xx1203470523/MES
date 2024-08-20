namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 工序资质认证（物理删除） 查询参数
    /// </summary>
    public class ProcProcedureQualificationAuthenticationRelationQuery
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资质认证Id列表
        /// </summary>
        public IEnumerable<long>? QualificationAuthenticationIds { get; set; }
    }
}
