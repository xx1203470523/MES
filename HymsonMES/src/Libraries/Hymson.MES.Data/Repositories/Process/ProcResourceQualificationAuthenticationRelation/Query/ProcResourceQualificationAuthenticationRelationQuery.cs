namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 资源资质认证（物理删除） 查询参数
    /// </summary>
    public class ProcResourceQualificationAuthenticationRelationQuery
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 资质认证Id列表
        /// </summary>
        public IEnumerable<long>? QualificationAuthenticationIds { get; set; }
    }
}
