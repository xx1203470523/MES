/*
 *creator: Karl
 *
 *describe: 备件库存    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquSparepartInventory
{
    /// <summary>
    /// 备件库存，数据实体对象   
    /// equ_sparepart_inventory
    /// @author pengxin
    /// @date 2024-06-12 10:15:26
    /// </summary>
    public class EquSparepartInventoryEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 备件Id equ_sparepartId
        /// </summary>
        public long SparepartId { get; set; }

       /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
