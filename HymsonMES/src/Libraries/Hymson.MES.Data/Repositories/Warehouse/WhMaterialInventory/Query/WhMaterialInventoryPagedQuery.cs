/*
 *creator: Karl
 *
 *describe: 物料库存 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query
{
    /// <summary>
    /// 物料库存 分页参数
    /// </summary>
    public class WhMaterialInventoryPagedQuery : PagerInfo
    {
        /// <summary>
        /// 批次
        /// </summary>
        public decimal? Batch { get; set; } = 0;
        /// <summary>
        /// 物料条码
        /// </summary>
        public string? MaterialBarCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public WhMaterialInventoryStatusEnum? Status { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        #region 添加 库存修改功能时添加 karl
        /// <summary>
        /// 接收时间  时间范围  数组
        /// </summary>
        public DateTime[]? CreatedOnRange { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public MaterialInventorySourceEnum[]? Sources { get; set; }
        #endregion

    }
}
