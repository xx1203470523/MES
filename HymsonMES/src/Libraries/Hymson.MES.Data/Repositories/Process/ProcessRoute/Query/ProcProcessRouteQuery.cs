/*
 *creator: Karl
 *
 *describe: 工艺路线表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
 */

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
        public string[] StatusArr { get; set; }
        /// <summary>
        /// 工艺路线代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
