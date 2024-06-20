namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 角色、人员资质认证 查询参数
    /// </summary>
    public class InteQualificationAuthenticationDetailsQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资质认证Id
        /// </summary>
        public long? AuthenticationId { get; set; }
    }
}
