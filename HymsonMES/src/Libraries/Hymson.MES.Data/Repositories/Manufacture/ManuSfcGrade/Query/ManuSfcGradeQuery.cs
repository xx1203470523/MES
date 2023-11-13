/*
 *creator: Karl
 *
 *describe: 条码档位表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-27 01:54:16
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码档位表 查询参数
    /// </summary>
    public class ManuSfcGradeQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }
    }
}
