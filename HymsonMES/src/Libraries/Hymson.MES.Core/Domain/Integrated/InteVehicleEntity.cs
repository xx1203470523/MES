using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 载具注册表，数据实体对象   
    /// inte_vehicle
    /// @author Karl
    /// @date 2023-07-14 10:03:53
    /// </summary>
    public class InteVehicleEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 托盘编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 托盘名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 载具类型id
        /// </summary>
        public long VehicleTypeId { get; set; }

       /// <summary>
        /// 存放位置
        /// </summary>
        public string Position { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

       
    }
}
