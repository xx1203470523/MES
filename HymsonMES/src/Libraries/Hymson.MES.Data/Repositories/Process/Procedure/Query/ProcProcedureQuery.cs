namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序表 查询参数
    /// </summary>
    public class ProcProcedureQuery
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 所属站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 工序编码列表
        /// </summary>
        public IEnumerable<string>? Codes { get; set; }

        /// <summary>
        /// 资源类型组
        /// </summary>
        public IEnumerable<long>? ResourceTypeIds {  get; set; }
    }


    /// <summary>
    /// 根据资源获取工序 查询参数
    /// </summary>
    public class ProcProdureByResourceIdQuery
    {
        /// <summary>
        ///资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 所属站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
