using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 载具类型维护，数据实体对象   
    /// inte_vehicle_type
    /// @author Karl
    /// @date 2023-07-12 10:37:17
    /// </summary>
    public class InteVehicleTypeEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 类型编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }

       /// <summary>
        /// 列
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 单元数量
        /// </summary>
        public int CellQty { get; set; }
        
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

       
    }
}
