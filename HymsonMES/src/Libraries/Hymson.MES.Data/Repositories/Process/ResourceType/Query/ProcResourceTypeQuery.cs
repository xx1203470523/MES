namespace Hymson.MES.Data.Repositories.Process.ResourceType
{
    public class ProcResourceTypeQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 资源类型列表
        /// </summary>
        public string[]? ResTypes { get; set; }
    }
}
