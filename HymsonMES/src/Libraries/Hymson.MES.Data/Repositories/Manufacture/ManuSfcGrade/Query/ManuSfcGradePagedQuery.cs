/*
 *creator: Karl
 *
 *describe: 条码档位表 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-27 01:54:16
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码档位表 分页参数
    /// </summary>
    public class ManuSfcGradePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }
}
