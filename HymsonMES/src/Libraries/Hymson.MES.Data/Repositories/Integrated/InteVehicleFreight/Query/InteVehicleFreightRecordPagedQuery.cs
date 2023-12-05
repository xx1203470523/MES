using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具装载记录 分页参数
    /// </summary>
    public class InteVehicleFreightRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 托盘Id
        /// </summary>
        public long? VehicleId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 计划开始时间  数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }
    }
}
