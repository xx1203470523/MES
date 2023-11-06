/*
 *creator: Karl
 *
 *describe: 客户维护 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-11 09:33:26
 */

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 客户维护 查询参数
    /// </summary>
    public class InteCustomQuery
    {
        /// <summary>
        /// 所属站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 客户编码列表
        /// </summary>
        public string[]? Codes { get; set; }
    }
}
