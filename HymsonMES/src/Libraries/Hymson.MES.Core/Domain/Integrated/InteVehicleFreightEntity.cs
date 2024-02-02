using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 载具装载，数据实体对象   
    /// inte_vehicle_freight
    /// @author Karl
    /// @date 2023-07-18 09:52:16
    /// </summary>
    public class InteVehicleFreightEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具id
        /// </summary>
        public long VehicleId { get; set; }

        /// <summary>
        /// 已存放的数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// x位置
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// y位置
        /// </summary>
        public int Row { get; set; }

       /// <summary>
       /// 位置号 
       /// </summary>
        public string? Location { get; set; }


       /// <summary>
        /// 状态;0-禁用 1-启用
        /// </summary>
        public bool? Status { get; set; }

       
    }
}
