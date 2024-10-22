namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线表 查询参数
    /// </summary>
    public class ProcProcessRouteQuery
    {
        /// <summary>
        /// id集合
        /// </summary>
        public long[] Ids { get; set; }

        /// <summary>
        /// 状态列表
        /// </summary>
        public int[] StatusArr { get; set; }
        /// <summary>
        /// 工艺路线代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        public string Version { get; set; }

        public long Id { get; set; } = 0;
    }
}
