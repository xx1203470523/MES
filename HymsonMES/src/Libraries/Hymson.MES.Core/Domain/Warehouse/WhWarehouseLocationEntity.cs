using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.WhWarehouseLocation
{
    /// <summary>
    /// 数据实体（库位）   
    /// wh_warehouse_location
    /// @author zsj
    /// @date 2023-11-30 07:52:28
    /// </summary>
    public class WhWarehouseLocationEntity : BaseEntity
    {
        /// <summary>
        /// 货架id
        /// </summary>
        public long WarehouseShelfId { get; set; }

       /// <summary>
        /// 库位编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 方式;1、自动生成  2、自定义 3、指定
        /// </summary>
        public WhWarehouseLocationTypeEnum Type { get; set; }

       /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
