/*
 *creator: Karl
 *
 *describe: 载具装载    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-07-18 09:52:16
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

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
        /// 位置号
        /// </summary>
        public string? Location { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 状态;0-禁用 1-启用
        /// </summary>
        public bool? Status { get; set; }

       
    }
}
