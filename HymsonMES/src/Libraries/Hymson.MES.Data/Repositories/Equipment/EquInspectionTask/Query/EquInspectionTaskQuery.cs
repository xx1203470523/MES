namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 点检任务 查询参数
    /// </summary>
    public class EquInspectionTaskQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 点检任务编码
        /// </summary>
        public string? Code { get; set; }
    }
}
