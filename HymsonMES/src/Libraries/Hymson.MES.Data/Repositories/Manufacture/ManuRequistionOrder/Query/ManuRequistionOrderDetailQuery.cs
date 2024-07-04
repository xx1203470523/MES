/*
 *creator: Karl
 *
 *describe: 生产领料单明细 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-04 02:34:40
 */

namespace Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder
{
    /// <summary>
    /// 生产领料单明细 查询参数
    /// </summary>
    public class ManuRequistionOrderDetailQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 工单编码
        /// </summary>
        public long[] RequistionOrderIds { get; set; }
    }
}
