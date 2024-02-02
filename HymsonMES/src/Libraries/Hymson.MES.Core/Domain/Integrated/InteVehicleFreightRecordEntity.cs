using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 载具装载记录，数据实体对象   
    /// inte_vehicle_freight_record
    /// @author wxk
    /// @date 2023-07-24 04:45:45
    /// </summary>
    public class InteVehicleFreightRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具编码
        /// </summary>
        public long VehicleId { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public long LocationId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 状态;0-绑定 1-解绑
        /// </summary>
        public int OperateType { get; set; }


       
    }
}
