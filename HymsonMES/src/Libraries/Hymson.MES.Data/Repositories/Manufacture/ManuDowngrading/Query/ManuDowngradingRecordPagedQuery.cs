/*
 *creator: Karl
 *
 *describe: 降级品录入记录 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:49
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级品录入记录 分页参数
    /// </summary>
    public class ManuDowngradingRecordPagedQuery : PagerInfo
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 品级
        /// </summary>
        public string? Grade { get; set; }
    }
}
