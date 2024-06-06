using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuSfcProduceVehicleView
    {
        /// <summary>
        ///  唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 载具Id
        /// </summary>
        public long? VehicleId { get; set; }
        
        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 条码Id
        /// </summary>
        public long? SFCId { get; set; }
    }
}
