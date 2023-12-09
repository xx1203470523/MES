using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.WhWarehouseShelf
{
    /// <summary>
    /// 数据实体（货架）   
    /// wh_warehouse_shelf
    /// @author zsj
    /// @date 2023-11-30 07:52:12
    /// </summary>
    public class WhWarehouseShelfEntity : BaseEntity
    {
        /// <summary>
        /// 货架编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 货架名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 仓库id
        /// </summary>
        public long WarehouseId { get; set; }

       /// <summary>
        /// 库区id
        /// </summary>
        public long WarehouseRegionId { get; set; }

       /// <summary>
        /// 库位行列
        /// </summary>
        public int Column { get; set; }

       /// <summary>
        /// 库位列数
        /// </summary>
        public int Row { get; set; }

       /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
