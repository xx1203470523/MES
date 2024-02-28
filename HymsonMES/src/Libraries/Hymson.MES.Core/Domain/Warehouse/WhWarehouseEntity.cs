using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.WhWareHouse
{
    /// <summary>
    /// 数据实体（仓库）   
    /// wh_warehouse
    /// @author zsj
    /// @date 2023-11-28 10:29:43
    /// </summary>
    public class WhWarehouseEntity : BaseEntity
    {
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态
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
