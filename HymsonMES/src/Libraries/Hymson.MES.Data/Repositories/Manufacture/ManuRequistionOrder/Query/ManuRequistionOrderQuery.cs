/*
 *creator: Karl
 *
 *describe: 生产领料单 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-04 02:34:15
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产领料单 查询参数
    /// </summary>
    public class ManuRequistionOrderQuery
    {
        // <summary>
        /// 领料单据号
        /// </summary>
        public string ReqOrderCode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }
    }
}
