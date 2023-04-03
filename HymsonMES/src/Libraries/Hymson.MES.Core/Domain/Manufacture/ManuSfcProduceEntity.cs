/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除）    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除），数据实体对象   
    /// manu_sfc_produce
    /// @author zhaoqing
    /// @date 2023-03-18 05:37:27
    /// </summary>
    public class ManuSfcProduceEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 条码数据id
        /// </summary>
        public long BarCodeInfoId { get; set; }

       /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

       /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

       /// <summary>
        /// BOMId
        /// </summary>
        public long BOMId { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 设备Id
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

       /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcProduceStatusEnum Status { get; set; }

       /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public int? Lock { get; set; }

       /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }

       /// <summary>
        /// 是否可疑
        /// </summary>
        public bool? IsSuspicious { get; set; }

       /// <summary>
        /// 复投次数;复投次数
        /// </summary>
        public int RepeatedCount { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum? IsScrap { get; set; }
    }
}
