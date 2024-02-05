using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.WhWarehouseRegion
{
    /// <summary>
    /// 数据实体（库区）   
    /// wh_warehouse_region
    /// @author zsj
    /// @date 2023-11-30 04:17:35
    /// </summary>
    public class WhWarehouseRegionEntity : BaseEntity
    {
        /// <summary>
        /// 库区编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 库区名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 仓库id
        /// </summary>
        public long WarehouseId { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 状态;1、启用  2未启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
