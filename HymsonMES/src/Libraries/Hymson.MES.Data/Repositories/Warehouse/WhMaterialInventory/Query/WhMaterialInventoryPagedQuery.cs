/*
 *creator: Karl
 *
 *describe: 物料库存 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string? Batch { get; set; }
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
        /// 状态
        /// </summary>
        public WhMaterialInventoryStatusEnum? Status { get; set; }
    }
}
