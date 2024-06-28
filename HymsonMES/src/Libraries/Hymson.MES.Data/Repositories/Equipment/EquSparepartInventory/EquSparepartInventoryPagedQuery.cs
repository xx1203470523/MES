/*
 *creator: Karl
 *
 *describe: 备件库存 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquSparepartInventory
{
    /// <summary>
    /// 备件库存 分页参数
    /// </summary>
    public class EquSparepartInventoryPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string? SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? SparepartName { get; set; }
        /// <summary>
        /// 备件编码
        /// </summary>
        public string? SparePartsGroupCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string? Specifications { get; set; } 
    }
}
