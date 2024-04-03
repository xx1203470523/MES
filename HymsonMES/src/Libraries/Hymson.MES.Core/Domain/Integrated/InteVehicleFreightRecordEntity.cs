/*
 *creator: Karl
 *
 *describe: 载具装载记录    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-07-24 04:45:45
 */
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

        /// <summary>
        /// 产品信息
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工作中心id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }
    }
}
