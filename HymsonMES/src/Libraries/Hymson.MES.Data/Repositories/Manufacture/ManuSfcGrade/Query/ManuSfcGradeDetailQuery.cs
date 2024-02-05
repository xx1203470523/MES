/*
 *creator: Karl
 *
 *describe: 条码档位明细表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-27 01:54:27
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码档位明细表 查询参数
    /// </summary>
    public class ManuSfcGradeDetailQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }

    public class ManuSfcGradeDetailByGradeIdQuery 
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 档位Id
        /// </summary>
        public long GadeId { get; set; }
    }
}
