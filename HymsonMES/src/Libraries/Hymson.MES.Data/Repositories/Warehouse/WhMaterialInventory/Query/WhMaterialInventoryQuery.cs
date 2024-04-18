/*
 *creator: Karl
 *
 *describe: 物料库存 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query
{
    /// <summary>
    /// 物料库存 查询参数
    /// </summary>
    public class WhMaterialInventoryQuery
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料条码列表
        /// </summary>
        public IEnumerable<string>? MaterialBarCodes { get; set; }
    }
}
