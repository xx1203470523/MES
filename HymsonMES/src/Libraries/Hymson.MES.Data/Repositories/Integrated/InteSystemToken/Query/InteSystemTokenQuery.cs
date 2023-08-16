/*
 *creator: Karl
 *
 *describe: 系统Token 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 系统Token 查询参数
    /// </summary>
    public class InteSystemTokenQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SystemCode { get; set; }
    }
}
