/*
 *creator: Karl
 *
 *describe: 降级录入 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:26
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级录入 查询参数
    /// </summary>
    public class ManuDowngradingQuery
    {
    }

    /// <summary>
    /// 降级录入 查询参数
    /// </summary>
    public class ManuDowngradingBySfcsQuery
    {
        public long SiteId { get; set; }

        public string[] Sfcs { get; set; }
    }

    /// <summary>
    /// 降级录入 查询参数
    /// </summary>
    public class ManuDowngradingBySFCsQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }

}
