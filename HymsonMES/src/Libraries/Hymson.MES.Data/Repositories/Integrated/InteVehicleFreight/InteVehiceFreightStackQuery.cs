/*
 *creator: Karl
 *
 *describe: 二维载具条码明细 查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-19 08:14:38
 */

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 二维载具条码明细 查询参数
    /// </summary>
    public class InteVehiceFreightStackQuery
    {
        /// <summary>
        /// 载具Id
        /// </summary>
        public long VehicleId { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
    /// <summary>
    /// 二维载具条码明细 查询参数
    /// </summary>
    public class InteVehiceFreightStackQueryByLocation
    {
        /// <summary>
        /// 位置Id
        /// </summary>
        public long LocationId { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
