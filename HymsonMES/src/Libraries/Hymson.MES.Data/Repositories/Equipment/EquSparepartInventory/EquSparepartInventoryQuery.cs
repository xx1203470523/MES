/*
 *creator: Karl
 *
 *describe: 备件库存 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */

namespace Hymson.MES.Data.Repositories.EquSparepartInventory
{
    /// <summary>
    /// 备件库存 查询参数
    /// </summary>
    public class EquSparepartInventoryQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }


        /// <summary>
        /// 备件Id equ_sparepartId
        /// </summary>
        public long? SparepartId { get; set; } 

        /// <summary>
        /// 备件Id equ_sparepartId
        /// </summary>
        public IEnumerable<long>? SparepartIds { get; set; }
    }
}
