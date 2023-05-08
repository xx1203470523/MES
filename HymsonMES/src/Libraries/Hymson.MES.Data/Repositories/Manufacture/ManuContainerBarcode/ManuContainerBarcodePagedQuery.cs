/*
 *creator: Karl
 *
 *describe: 容器条码表 分页查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器条码表 分页参数
    /// </summary>
    public class ManuContainerBarcodePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 容器条码
        /// </summary>
        public string? BarCode { get; set; }
        /// <summary>
        /// 包装等级
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 产品编码 对应物料表编码
        /// </summary>
        public string? ProductCode { get; set; }
        /// <summary>
        /// 产品名称 对应物料表名称
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 包装等级
        /// </summary>
        public int? PackLevel { get; set; }
    }
}
