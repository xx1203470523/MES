using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 二维载具条码明细，数据实体对象   
    /// inte_vehice_freight_stack
    /// @author wxk
    /// @date 2023-07-19 08:14:38
    /// </summary>
    public class InteVehicleFreightStackEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具位置id
        /// </summary>
        public long LocationId { get; set; }
        /// <summary>
        /// 载具Id
        /// </summary>
        public long VehicleId { get; set; }

        /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

       
    }
}
