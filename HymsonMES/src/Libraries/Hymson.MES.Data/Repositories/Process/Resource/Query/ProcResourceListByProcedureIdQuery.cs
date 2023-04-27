namespace Hymson.MES.Data.Repositories.Process.Resource
{
    /// <summary>
    /// 资源维护表查询对象
    /// </summary>
    public class ProcResourceListByProcedureIdQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
