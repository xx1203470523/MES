/*
 *creator: Karl
 *
 *describe: 容器条码表 查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器条码表 查询参数
    /// </summary>
    public partial class ManuContainerBarcodeQuery
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string>? BarCodes { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 容器条码表 查询参数
    /// </summary>
    public partial class ManuContainerIdQuery
    {
        /// <summary>
        /// 容器Id
        /// </summary>
        public long? ContainerId { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }
}
